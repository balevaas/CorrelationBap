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
        private int _pointNumber;
        private DateTime[] _dates;
        private decimal[] _pollution;

        public string CityName
        {
            get => _cityName;
            set
            {
                _cityName = value;
                OnPropertyChanged();
            }
        }
        public int PointNumber
        {
            get => _pointNumber;
            set
            {
                _pointNumber = value;
                OnPropertyChanged();
            }
        }
        public DateTime[] Dates
        {
            get => _dates;
            set
            {
                _dates = value;
                OnPropertyChanged();
            }
        }

        public decimal[] Pollution
        {
            get => _pollution;
            set
            {
                _pollution = value;
                OnPropertyChanged();
            }
        }

        //public event EventHandler DataChanged;

        //protected virtual void OnDataChanged()
        //{
        //    DataChanged?.Invoke(this, EventArgs.Empty);
        //}
    }
}
