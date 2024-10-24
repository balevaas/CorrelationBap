using BaseData.EntityFramework.Context;
using BaseViewModel.DatasDTO;
using System.Diagnostics;
using ViewModelBase;

namespace BaseViewModel
{
    public class AnalysisViewModel : ViewModel
    {
        private readonly DataContext _model;

        public int[] WindSpeed = { 1, 2, 3 };
        public static decimal[] Pollution;

        public string CityName;

        public AnalysisViewModel(DataContext dataContext)
        {
            _model = dataContext;
        }

        //private string receivedCityName;
        //public string ReceivedCityName
        //{
        //    get => receivedCityName;
        //    set
        //    {
        //        receivedCityName = value;
        //        OnPropertyChanged(nameof(ReceivedCityName));
        //    }
        //}

    }
}
