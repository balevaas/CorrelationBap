using BaseData.Context;
using BaseView.DatasDTO;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseView.ViewModel
{
    public class AnalysisViewModel : BaseViewModel
    {
        private readonly DataContext _model;

        //private SaveDatas _save;
        private string _name;
        public string CityName
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(CityName));
            }
        }
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
            //CityName = SelectDatas.First().CityName;

        }

        public void SaveDatas(ObservableCollection<SaveDatas> datas)
        {
            SelectDatas = datas;
            OnPropertyChanged(nameof(SelectDatas));
            CityName = SelectDatas.First().CityName;
            OnPropertyChanged(nameof(CityName));
            //CityName = datas.First().CityName;
            //OnPropertyChanged(nameof(CityName));
            //Debug.WriteLine(CityName);
        }
    }
}
