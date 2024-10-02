using BaseData.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using ViewModelBase;
using ViewModelBase.Commands.AsyncCommand;

namespace BaseViewModel
{
    public class StationVm : ViewModel
    {
        private readonly DataContext _model; // связываемся с моделью и бд
        public StationVm(DataContext model)
        {
            _model = model;
            // Получение данных из таблицы Monitorings, заполнение таблицы
            Stations = new(_model.Stations.Select(m =>
            new DTO()
            {
                ID = m.ID,
                Name = m.Name,
                Location = m.Location,
                Latitude = m.Latitude,
                Longitude = m.Longitude,
                Height = m.Height
            }));
        }

        public ObservableCollection<DTO> Stations { get; }

    }
}
