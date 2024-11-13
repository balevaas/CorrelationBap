using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseView.DatasDTO
{
    public class SaveDatas : BaseViewModel
    {
        private string _cityName;
        public string CityName
        {
            get => _cityName;
            set
            {
                _cityName = value;
                OnPropertyChanged(nameof(CityName));
            }
        }
        private int _pointNumber;
        public int PointNumber
        {
            get => _pointNumber;
            set
            {
                _pointNumber = value;
                OnPropertyChanged(nameof(PointNumber));
            }
        }
        private DateTime[] _dates;
        public DateTime[] Dates
        {
            get => _dates;
            set
            {
                _dates = value;
                OnPropertyChanged(nameof(Dates));
            }
        }
        private decimal[] pollution;
        public decimal[] Pollution
        {
            get => pollution;
            set
            {
                pollution = value;
                OnPropertyChanged(nameof(Pollution));
            }
        }
    }
}
