using BaseView.DatasDTO;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot;
using WeatherDataParser;
using static BaseView.ViewModel.CorrelationViewModel;
using static ConnectionConfig.Strings;

namespace BaseView.Utilities
{
    public class Calculation
    {
        public Statistics statistics;
        public decimal[] wind;
        public SaveDatas saveDatas = new SaveDatas();

        private PlotModel _plotModel;
        public PlotModel PlotModels
        {
            get => _plotModel;
            set
            {
                _plotModel = value;
            }
        }
        public class DataPoints
        {
            public double Wind { get; set; }
            public double Pollution { get; set; }
        }
        public (double, double, double) CalculateCorrelation(int[] mas, int IdStation, int[] NumberMonth, decimal[] Pollution)
        {
            if (mas.Length == 1) statistics = new Statistics(DateTime.Parse($"01.01.{mas[0]}"), DateTime.Parse($"31.12.{mas[0]}"), IdStation, GetConnectionStrings(Sqlite));
            else if (mas.Length > 1)
            {
                int index = mas.Length - 1;
                statistics = new Statistics(DateTime.Parse($"01.01.{mas[0]}"), DateTime.Parse($"31.12.{mas[index]}"), IdStation, GetConnectionStrings(Sqlite));
            }
            var avg = statistics.GetAveragesChart(Parameter.Wind_Speed, Microsoft.VisualBasic.DateInterval.Month);
            wind = avg.Where(x => NumberMonth.Contains(x.Key.Month)).ToDictionary().Values.ToArray();
            if (Pollution.Length != wind.Length) throw new ArgumentException("Массивы должны быть одинаковой длины");
            else
            {
                decimal count = wind.Length;
                int n = wind.Length;

                decimal sumWind = wind.Sum();
                decimal sumPol = Pollution.Sum();
                decimal sumWindPol = wind.Zip(Pollution, (x, y) => x * y).Sum();
                decimal sumWindSquare = wind.Sum(x => x * x);
                decimal sumPolSquare = Pollution.Sum(x => x * x);
                double correaltion = (double)(n * sumWindPol - sumWind * sumPol)
                    / Math.Sqrt((double)((n * sumWindSquare - sumWind * sumWind) * (n * sumPolSquare - sumPol * sumPol)));

                double slope = (double)(count * sumWindPol - sumWind * sumPol) / (double)(count * sumWindSquare - sumWind * sumWind);
                double intercept = (double)((double)sumPol - slope * (double)sumWind) / (double)count;
                return (correaltion, slope, intercept);
            }
        }       

        public PlotModel LoadData(int[] NumberYear, decimal[] Pollution)
        {
            PlotModels = new PlotModel { Title = "График корреляции" };
            var series = new ScatterSeries { Title = $"Город - {saveDatas.CityName}, Пост - {saveDatas.PointNumber}, {NumberYear[0]} год\" ", MarkerType = MarkerType.Circle };
            List<DataPoints> dataPoints = new List<DataPoints>();

            for (int i = 0; i < Pollution.Length; i++)
            {
                dataPoints.Add(new DataPoints { Wind = (double)wind[i], Pollution = (double)Pollution[i] });
            }
            foreach (var point in dataPoints)
            {
                series.Points.Add(new ScatterPoint(point.Wind, point.Pollution));
            }

            PlotModels.Series.Add(series);
            double minWind, maxWind, minPollution, maxPollution;
            if (dataPoints.Count > 0)
            {
                minWind = dataPoints.Min(p => p.Wind);
                maxWind = dataPoints.Max(p => p.Wind);
                minPollution = dataPoints.Min(p => p.Pollution);
                maxPollution = dataPoints.Max(p => p.Pollution);
                PlotModels.Axes.Clear(); // Очистка существующих осей

                PlotModels.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Ветер", Minimum = minWind - 1, Maximum = maxWind + 1 });
                PlotModels.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Концентрация нг / м^3", Minimum = minPollution - 1, Maximum = maxPollution + 1 });
            }

            double averageX = (double)wind.Average();
            double averageY = (double)Pollution.Average();
            double b = (wind.Zip((Pollution), (x, y) => ((double)x - averageX) * ((double)y - averageY)).Sum()) / (wind.Select(x => Math.Pow((double)x - averageX, 2)).Sum());
            double a = averageY - b * averageX;
            var lineSeries = new LineSeries
            {
                Title = "Линейная регрессия",
                StrokeThickness = 2,
                Color = OxyColors.Red
            };
            lineSeries.Points.Add(new DataPoint((double)wind.Min(), a + b * (double)wind.Min()));
            lineSeries.Points.Add(new DataPoint((double)wind.Max(), a + b * (double)wind.Max()));
            PlotModels.Series.Add(lineSeries);


            PlotModels.InvalidatePlot(true);
            return PlotModels;
        }
    }
}
