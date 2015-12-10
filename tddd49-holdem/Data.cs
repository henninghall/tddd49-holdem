using System.Collections.Generic;
using System.ComponentModel;

namespace tddd49_holdem
{
    /// <summary>
    /// Notifies the client that a binding property has changed.
    /// This updates the GUI when a property is changed. 
    /// </summary>
    // From: http://stackoverflow.com/questions/1315621/implementing-inotifypropertychanged-does-a-better-way-exist
    public class Data : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

       
    }
}