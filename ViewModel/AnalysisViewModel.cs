using BaseData.EntityFramework.Context;
using ViewModelBase;

namespace BaseViewModel
{
    public class AnalysisViewModel : ViewModel
    {
        private readonly DataContext _model;

        public int[] WindSpeed = { 1, 2, 3 };
        public static decimal[] Pollution;

        public string NameCity;

        public AnalysisViewModel(DataContext dataContext)
        {
            _model = dataContext;
            //NameCity = SelectDataViewModel.NameS;
        }

    }
}
