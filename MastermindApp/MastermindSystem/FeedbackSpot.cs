using Microsoft.Maui.Graphics;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace MastermindSystem
{
    public class FeedbackSpot : INotifyPropertyChanged
    {

        Game.FeedbackSpotColorEnum _labelbackcolor;
        public Game.FeedbackSpotColorEnum LabelBackColor
        {
            get => _labelbackcolor;
            set
            {
                _labelbackcolor = value;
                this.InvokePropertyChanged();
                this.InvokePropertyChanged("MauiFeedbackSpotColor");
                this.InvokePropertyChanged("SystemFeedbackSpotColor");
            }
        }
        public System.Drawing.Color SystemFeedbackSpotColor
        {
            get
            {
                return this.LabelBackColor switch
                {
                    Game.FeedbackSpotColorEnum.Black => System.Drawing.Color.Black,
                    Game.FeedbackSpotColorEnum.White => System.Drawing.Color.Gainsboro,
                    Game.FeedbackSpotColorEnum.Default => System.Drawing.Color.White,
                    _ => System.Drawing.Color.Transparent
                };
            }
        }

        public Microsoft.Maui.Graphics.Color MauiFeedbackSpotColor
        {
            get
            {
                return this.LabelBackColor switch
                {
                    Game.FeedbackSpotColorEnum.Black => Microsoft.Maui.Graphics.Colors.Black,
                    Game.FeedbackSpotColorEnum.White => Microsoft.Maui.Graphics.Colors.Gainsboro,
                    Game.FeedbackSpotColorEnum.Default => Microsoft.Maui.Graphics.Colors.White,
                    _ => Microsoft.Maui.Graphics.Colors.Transparent
                };
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        private void InvokePropertyChanged([CallerMemberName] string propertyname = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
    }
}
