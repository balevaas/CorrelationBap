using BaseData.EntityFramework.Context;
using System.Collections.ObjectModel;
using ViewModelBase;
using ViewModelBase.Commands.QuickCommand;

namespace BaseViewModel
{
    public class PollutionVm : ViewModel
    {
        private readonly DataContext _model;

        public ObservableCollection<string> StationName { get; private set; }
        public string NameS;

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
            NameS = name;
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
        public ObservableCollection<int> IDPoint { get; private set; }
        public int PointID;
        private void SelectPoints(int id)
        {
            IDPoint = new ObservableCollection<int>(_model.Points.Where(p => p.ID == id).Select(p => p.ID));
            PointID = IDPoint.ElementAt(0);
            
            Year = new List<int> { 2021, 2022 };
            OnPropertyChanged(nameof(Year));
        }
    }
}
