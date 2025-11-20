using Microsoft.Maui.Graphics;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using static MastermindSystem.Game;

namespace MastermindSystem
{
    public class Spot : INotifyPropertyChanged
    {
        Game.SpotColorEnum _backcolor;
        private string _text = "";
        public int Id {  get; set; }
        public Game.SpotColorEnum BackColor
        {
            get => _backcolor;
            set
            {
                _backcolor = value;
                this.InvokePropertyChanged();
                this.InvokePropertyChanged("MauiSpotColor");
                this.InvokePropertyChanged("SystemSpotColor");
            }
        }

        public System.Drawing.Color SystemSpotColor
        {
            get
            {
                return this.BackColor switch
                {
                    SpotColorEnum.Blue => System.Drawing.Color.DeepSkyBlue,
                    SpotColorEnum.Pink => System.Drawing.Color.DeepPink,
                    SpotColorEnum.Yellow => System.Drawing.Color.Yellow,
                    SpotColorEnum.Green => System.Drawing.Color.LimeGreen,
                    SpotColorEnum.Purple => System.Drawing.Color.Purple,
                    SpotColorEnum.Black => System.Drawing.Color.Black,
                    SpotColorEnum.Default => System.Drawing.Color.White,
                    _ => System.Drawing.Color.Transparent,  // or Default color
                };
            }
        }

        public Microsoft.Maui.Graphics.Color MauiSpotColor
        {
            get
            {
                return this.BackColor switch
                {
                    Game.SpotColorEnum.Blue => Colors.DeepSkyBlue,
                    Game.SpotColorEnum.Pink => Colors.DeepPink,
                    Game.SpotColorEnum.Yellow => Colors.Yellow,
                    Game.SpotColorEnum.Green => Colors.LimeGreen,
                    Game.SpotColorEnum.Purple => Colors.Purple,
                    Game.SpotColorEnum.Black => Colors.Black,
                    Game.SpotColorEnum.Default => Colors.White,
                    _ => Colors.Transparent // Default color
                };
            }
        }

        public String Text 
        {
            get => _text;
            set
            {
                _text = value;
                this.InvokePropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void InvokePropertyChanged([CallerMemberName] string propertyname = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

    }
}
