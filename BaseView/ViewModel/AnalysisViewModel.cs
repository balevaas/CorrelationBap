using BaseData.Context;
using BaseView.DatasDTO;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace BaseView.ViewModel
{
    public class AnalysisViewModel : BaseViewModel
    {
        private readonly DataContext _model;

        private SaveDatas _save;
        private SelectViewModel _selectViewModel;
        //private string _name;
        //public string CityName
        //{
        //    get => _name;
        //    set
        //    {
        //        _name = value;
        //        OnPropertyChanged(nameof(CityName));
        //    }
        //}

        private DateTime[] dates;
        public DateTime[] Dates
        {
            get => dates;
            set
            {
                dates = value; OnPropertyChanged(nameof(Dates));
            }
        }

        
        private decimal[] pollutions;
        public decimal[] Pollutions
        {
            get => pollutions;
            set
            {
                pollutions = value; 
                //OnPropertyChanged(nameof(Pollutions));
            }
        }

        public int[] wind = { 1, 7, 2, 4 };
        private ObservableCollection<SaveDatas> select;
        public ObservableCollection<SaveDatas> SelectDatas
        {
            get => select;
            set
            {
                select = value;
                OnPropertyChanged(nameof(SelectDatas));
            }
        }


        public AnalysisViewModel()
        {

        }

        //public double result { get; set; }
        public AnalysisViewModel(DataContext model)
        {
            _model = model;
            //result = CalculateCorrelation();
            //OnPropertyChanged(nameof(result));

        }

        public void SaveDatas(ObservableCollection<SaveDatas> datas)
        {
            SelectDatas = datas;
            Dates = SelectDatas.First().Dates;
            Debug.WriteLine(Dates[0]);
            Pollutions = SelectDatas.First().Pollution;
        }

        public double CalculateCorrelation()
        {
            if (Pollutions == null) return 0;
            else if (Pollutions.Length != wind.Length) throw new ArgumentException("Arrays must be of equal length.");
            else
            {
                decimal count = wind.Length;
                int n = wind.Length;

                decimal sumWind = wind.Sum();
                decimal sumPol = Pollutions.Sum();
                decimal sumWindPol = wind.Zip(Pollutions, (x, y) => x * y).Sum();
                decimal sumWindSquare = wind.Sum(x => x * x);
                decimal sumPolSquare = Pollutions.Sum(x => x * x);
                double correaltion = (double)(n * sumWindSquare - sumWind * sumPol)
                    / Math.Sqrt((double)((n * sumWindSquare - sumWind * sumWind) * (n * sumPolSquare - sumPol * sumPol)));
                return correaltion;
            }
        }
    }
}
