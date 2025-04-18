﻿using BaseView.DatasDTO;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot;
using WeatherDataParser;
using static ConnectionConfig.Strings;

namespace BaseView.Utilities
{
    public class DataPoints
    {
        public double Wind { get; set; }
        public double Pollution { get; set; }
        public DateTime Dates { get; set; }
    }
    public class Calculation
    {
        public Statistics statistics;
        public decimal[] Shtils;
        public SaveDatas saveDatas = new();

        private PlotModel _plotModel;
        public PlotModel PlotModels
        {
            get => _plotModel;
            set
            {
                _plotModel = value;
            }
        }

        public (double, double, double) CalculateCorrelation(int[] mas, int IdStation, int[] NumberMonth, decimal[] Pollution)
        {
            if (mas.Length == 1)
            {
                statistics = new Statistics(DateTime.Parse($"01.01.{mas[0]}"), DateTime.Parse($"31.12.{mas[0]}"), IdStation, GetConnectionStrings(Sqlite));
            }
            else if (mas.Length > 1)
            {
                int index = mas.Length - 1;
                statistics = new Statistics(DateTime.Parse($"01.01.{mas[0]}"), DateTime.Parse($"31.12.{mas[index]}"), IdStation, GetConnectionStrings(Sqlite));
            }

            var avg = statistics.GetWindPeriodicityChart(null, Microsoft.VisualBasic.DateInterval.Month);
            
;
            var st = GetShtilsForDates(avg, mas, NumberMonth);

            Shtils = st.Select(value => value * 100).ToArray();

            if (Pollution.Length != Shtils.Length) throw new ArgumentException("Массивы должны быть одинаковой длины");
            else
            {
                decimal count = Shtils.Length;
                int n = Shtils.Length;

                decimal sumWind = Shtils.Sum();
                decimal sumPol = Pollution.Sum();
                decimal sumWindPol = Shtils.Zip(Pollution, (x, y) => x * y).Sum();
                decimal sumWindSquare = Shtils.Sum(x => x * x);
                decimal sumPolSquare = Pollution.Sum(x => x * x);
                double correaltion = (double)(n * sumWindPol - sumWind * sumPol)
                    / Math.Sqrt((double)((n * sumWindSquare - sumWind * sumWind) * (n * sumPolSquare - sumPol * sumPol)));

                double slope = (double)(count * sumWindPol - sumWind * sumPol) / (double)(count * sumWindSquare - sumWind * sumWind);
                double intercept = (double)((double)sumPol - slope * (double)sumWind) / (double)count;
                return (correaltion, slope, intercept);
            }
        }

        public PlotModel LoadData(int[] NumberYear, decimal[] Pollution, string cityName, int pointId, string info, string rescorr)
        {
            PlotModels = new PlotModel
            {
                Title = info,
                Subtitle = rescorr,
                TitleFontSize = 30,
                TitleFont = "Bahnschrift SemiLight",
                TitleHorizontalAlignment = TitleHorizontalAlignment.CenteredWithinPlotArea,
                SubtitleFontSize = 22,
                SubtitleFont = "Bahnschrift SemiLight",
                PlotAreaBorderThickness = new OxyThickness(2),
                PlotAreaBorderColor = OxyColors.Gray
                
            };
            var series = new ScatterSeries();

            if (NumberYear.Length == 1)
            {
                series = new ScatterSeries { Title = $"Город - {cityName}, Пост - {pointId}", MarkerType = MarkerType.Circle };
            }
            else series = new ScatterSeries { Title = $"Город - {cityName}, Пост - {pointId}", MarkerType = MarkerType.Circle };

            List<DataPoints> dataPoints = [];

            for (int i = 0; i < Pollution.Length; i++)
            {
                dataPoints.Add(new DataPoints { Wind = (double)Shtils[i], Pollution = (double)Pollution[i]});
            }
            foreach (var point in dataPoints)
            {
                series.Points.Add(new ScatterPoint(point.Wind, point.Pollution));
            }

            PlotModels.Series.Add(series);

            //foreach (var point in dataPoints)
            //{
            //    var textAnnotation = new TextAnnotation
            //    {
            //        Text = $"{point.Dates.ToString("Y")}",
            //        TextPosition = new DataPoint(point.Wind, point.Pollution + 0.2),
            //        TextHorizontalAlignment = HorizontalAlignment.Center,
            //        TextVerticalAlignment = VerticalAlignment.Bottom
            //    };
            //    PlotModels.Annotations.Add(textAnnotation);
            //}
            double minWind, maxWind, minPollution, maxPollution;
            if (dataPoints.Count > 0)
            {
                minWind = dataPoints.Min(p => p.Wind);
                maxWind = dataPoints.Max(p => p.Wind);
                minPollution = dataPoints.Min(p => p.Pollution);
                maxPollution = dataPoints.Max(p => p.Pollution);
                PlotModels.Axes.Clear(); // Очистка существующих осей

                PlotModels.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Повторяемость штилей, %", Minimum = minWind - 1, Maximum = maxWind + 1 ,
                    TitleFont = "Bahnschrift SemiLight", TitleFontSize = 15});
                PlotModels.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Концентрация нг / м^3", Minimum = minPollution - 1, Maximum = maxPollution + 1,
                    TitleFont = "Bahnschrift SemiLight", TitleFontSize = 15
                });
            }

            double averageX = (double)Shtils.Average();
            double averageY = (double)Pollution.Average();
            double b = (Shtils.Zip((Pollution), (x, y) => ((double)x - averageX) * ((double)y - averageY)).Sum()) / (Shtils.Select(x => Math.Pow((double)x - averageX, 2)).Sum());
            double a = averageY - b * averageX;
            var lineSeries = new LineSeries
            {
                Title = "Линейная регрессия",
                StrokeThickness = 2,
                Color = OxyColors.Red
            };
            lineSeries.Points.Add(new DataPoint((double)Shtils.Min(), a + b * (double)Shtils.Min()));
            lineSeries.Points.Add(new DataPoint((double)Shtils.Max(), a + b * (double)Shtils.Max()));
            PlotModels.Series.Add(lineSeries);


            PlotModels.InvalidatePlot(true);
            return PlotModels;
        }
        private static decimal[] GetShtilsForDates(Dictionary<DateTime, decimal> avg, int[] mas, int[] NumberMonth)
        {
            List<(int, int)> listDate = [];
            foreach (var year in mas)
            {
                foreach (var month in NumberMonth)
                {
                    listDate.Add((year, month));
                }
            }

            // Создаем список для хранения списков значений
            List<List<decimal>> dates = [];

            foreach (var item in listDate)
            {
                // Получаем значения для текущего года и месяца
                var values = avg
                    .Where(y => y.Key.Year == item.Item1 && y.Key.Month == item.Item2)
                    .Select(m => m.Value)
                    .ToList();

                // Добавляем список значений в dates
                dates.Add(values);
            }
            return dates.SelectMany(x => x).ToArray();
        }               
    }
}
