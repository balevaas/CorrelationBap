using BaseData.Entities;
using BaseData.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Diagnostics;
using ViewModelBase;
using ViewModelBase.Commands.QuickCommand;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BaseViewModel
{
    public class PollutionVm : ViewModel
    {
        private readonly DataContext _model;

        public ObservableCollection<string> StationName { get; private set; }

        public PollutionVm(DataContext model)
        {
            _model = model;
            StationName = new ObservableCollection<string>(model.Stations.Select(s => s.Name));

            SelectStationID = new Command<string>(SelectStation);
            SelectPoint = new Command<int>(SelectPoints);
        }

        public ObservableCollection<int> StationID { get; private set; }
        public Command<string> SelectStationID { get; set; }
        public List<int> Points { get; private set; }
        private void SelectStation(string name)
        {
            StationID = new ObservableCollection<int>(_model.Stations.Where(p => p.Name == name).Select(p => p.ID));
            int id = StationID.ElementAt(0);
            Points = GetIDByStationID(id);
            OnPropertyChanged(nameof(Points));

        }
        public List<int> GetIDByStationID(int station)
        {
            var ID = _model.Points
                .Where(od => od.StationID == station)
                .Select(od => od.ID)
                .ToList();
            return ID; // Возвращаем найденный ID            
        }

        public List<int> Year { get; private set; } 
        public Command<int> SelectPoint;
        private void SelectPoints(int id)
        {
            Year = new List<int> { 2021, 2022 };
            OnPropertyChanged(nameof(Year));
        }
    }
}
