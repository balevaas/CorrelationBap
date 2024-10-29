using BaseData.Context;
using BaseView.DatasDTO;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseView.ViewModel
{
    public class AnalysisViewModel : BaseViewModel
    {
        private readonly DataContext _model;
        //public decimal[] Pollutions { get; set; }
        //public SaveDatas saveDatas = new SaveDatas();

        private readonly SaveDatas _saves;
        public AnalysisViewModel(SaveDatas saves)
        {
            _saves = saves;
            //for(int i = 0; i < saveDatas.Pollutions.Length; i++)
            //{
            //    Debug.WriteLine(saveDatas.Pollutions[i]);
            //}
            Debug.WriteLine(CityName);
        }

        public string CityName => _saves.CityName;

        public void UpdateCityName()
        {
            OnPropertyChanged(nameof(CityName));
        }
    }
}
