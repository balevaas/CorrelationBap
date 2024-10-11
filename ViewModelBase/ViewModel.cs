﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ViewModelBase
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        
        /// <summary>
        /// Событие, возникающее при изменении ВСЕХ свойств, ВЫБРАННЫХ разработчиком.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Вызывает событие PropertyChanged
        /// </summary>
        /// <param name="propertyName">Определяет какое именно свойство вызвало событие;
        /// по умолчанию null - определит самостоятельно с помощью [CallerMemberName].</param>
        protected virtual void OnPropertyChanged(
            [CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName));

        /// <summary>
        /// Помощник для сеттера свойств, к-ые инкапсулируют поля с событиями
        /// [set => Set(ref _field, value);]
        /// </summary>
        /// <typeparam name="T">тип свойства</typeparam>
        /// <param name="field">ссылка на икапсулированное поле</param>
        /// <param name="value">новое значение</param>
        /// <param name="propertyName">имя свойства, по умолчанию null - определит самостоятельно</param>
        /// <returns>true/false, если свойство (/не)изменилось</returns>
        protected virtual bool Set<T>(ref T field, T value,
            [CallerMemberName] string? propertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
