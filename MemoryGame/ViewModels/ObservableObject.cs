using System.ComponentModel;

namespace MemoryGame.ViewModels
{
    /// <summary>
    /// Observable object
    /// </summary>
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
