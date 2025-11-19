using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MastermindSystem
{
    public class FeedbackSpot : INotifyPropertyChanged
    {

        System.Drawing.Color _labelbackcolor;
        public System.Drawing.Color LabelBackColor
        {
            get => _labelbackcolor;
            set
            {
                _labelbackcolor = value;
                this.InvokePropertyChanged();
                this.InvokePropertyChanged("BackColorMaui");
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        private void InvokePropertyChanged([CallerMemberName] string propertyname = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
    }
}
