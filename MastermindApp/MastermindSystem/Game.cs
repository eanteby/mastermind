using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace MastermindSystem
{
    public class Game : INotifyPropertyChanged
    {
        public enum GameStatusEnum { NotStarted, Playing, Won, Lost }
        public GameStatusEnum gamestatus = GameStatusEnum.NotStarted;

        int _numguess = 0;
        private List<List<Spot>> _rows = new();
        private List<Spot> _coderow = new();
        private List<Spot> _activerow = new();
        private List<FeedbackSpot> _activefeedbackspots = new();
        private List<List<FeedbackSpot>> _feedbackrows = new();
        private String _comment;

        public Game()
        {

            for (int i = 0; i < 44; i++)
            {
                Spot s = new();
                this.Spots.Add(s);
                s.Id = i;
            }

            for (int i = 1; i < 11; i++)
            {
                Rows.Add(Spots.GetRange(i * 4, 4)); // Get 4 spots for each list
            }

            for (int i = 0; i < 41; i++)
            {
                this.FeedbackSpots.Add(new FeedbackSpot());
            }

            for (int i = 0; i < 10; i++)
            {
                FeedbackRows.Add(FeedbackSpots.GetRange(i * 4, 4)); // Get 4 spots for each list
            }

            this.CodeRow = new() { this.Spots[0], this.Spots[1], this.Spots[2], this.Spots[3] };
            this.CodeRow.ForEach(b => b.Text = "?");


        }
        public List<Spot> Spots { get; private set; } = new();
        public List<FeedbackSpot> FeedbackSpots { get; private set; } = new();

        public List<List<Spot>> Rows { get; set; } = new();

        public List<List<FeedbackSpot>> FeedbackRows { get; set; } = new();

        public List<String> lstcomments { get; set; } = new() { "You got this!", "You can do it!", "Keep trying!", "Think hard!", "Don't give up!" };

        public Color Color { get; set; } = new();

        public Microsoft.Maui.Graphics.Color ColorMaui {  get; set; }= new();

        public Color CorrectColorBackColor { get; set; } = System.Drawing.Color.Gainsboro;
        public Color CorrectPostionBackColor { get; set; } = System.Drawing.Color.Black;
        public Color DefaultBackColor {get; set; } = System.Drawing.Color.White;
        public List<Color> Colors { get; set; }

        public String Instructions { get; private set; } = "The goal of Mastermind is to crack the 4 color code generated at the start of the game. " +
                "In the beginner level, the code cannot contain each color more than once, while in the advanced level, a single color can show up multiple times in the code. " +
                "Each turn, 4 colors should be chosen by clicking the buttons in the highest empty row. " +
                "When check code is clicked, a black label will appear for each correct color that is in the correct position and a white label will appear for each correct color in the wrong position. " +
                "Players will have a maximum of 10 tries to guess the code. Get cracking! ";


        //public int NumCorrectPosition { get; set; } = new();
        //public int NumCorrectColor { get; set; } = new();
        public List<Spot> ActiveRow 
        {
            get => _activerow;
            set { _activerow = Rows[NumGuess]; }
        }
        public List<FeedbackSpot> ActiveFeedbackRow
        {
            get => _activefeedbackspots ;
            set { _activefeedbackspots = FeedbackRows[NumGuess]; }
        }

        public List<Spot> CodeRow { get; set; } = new();
        public List<Color> ColorCode { get; set; } = new();

        public event PropertyChangedEventHandler? PropertyChanged;

        public GameStatusEnum GameStatus
        {
            get => gamestatus;
            private set
            {
                gamestatus = value;
                this.InvokePropertyChanged("Comment");
                this.InvokePropertyChanged();
            }
        }

        public int NumGuess
        {
            get => _numguess;
            private set
            {
                _numguess = value;
                this.InvokePropertyChanged("Comment");
            }
        }

        public bool IsBeginner { get; set; }

        public void StartGame(bool beginner = true)
        {
            ClearBoard();
            this.gamestatus = GameStatusEnum.Playing;
            NumGuess = 0;
            ActiveRow = new();
            ActiveFeedbackRow = new();
            IsBeginner = beginner;
            CodeRow.ForEach(l => { l.BackColor = this.CorrectPostionBackColor; l.Text = "?"; });
            GenerateCode();
        }

        private void ClearBoard()
        {
            Spots.ForEach(s => s.BackColor = DefaultBackColor);
            FeedbackSpots.ForEach(s => s.LabelBackColor = DefaultBackColor);
        }
        private void GenerateCode()
        {
            ColorCode = new();
            Random rnd = new();
            this.Colors = new() { System.Drawing.Color.DeepSkyBlue, System.Drawing.Color.DeepPink, System.Drawing.Color.Yellow, System.Drawing.Color.LimeGreen, System.Drawing.Color.Purple, System.Drawing.Color.Black };
            for (int i = 0; i < 4; i++)
            {
                Color newcolor = Colors[rnd.Next(Colors.Count())];
                if (IsBeginner)
                {
                    newcolor = Colors.Where(c => !ColorCode.Contains(c)).ToList()[rnd.Next(Colors.Count(c => !ColorCode.Contains(c)))];
                }
                ColorCode.Add(newcolor);
            }
        }
        public void GuessColor(int num)
        {
            Spot spot = this.Spots[num];
            if (gamestatus == GameStatusEnum.Playing)
            {
                if (ActiveRow.Exists(b => b.Id == num))
                {
                    spot.BackColor = this.Color;
                    this.InvokePropertyChanged("BackColor");
                }
            }
        }

        public void CheckCode()
        {
            if (gamestatus == GameStatusEnum.Playing) 
            {
                if (ActiveRow.Count(s => s.BackColor == this.DefaultBackColor) == 0)
                {
                    AddFeedbackLabels();
                }
            }
        }

        private void AddFeedbackLabels()
        {
            int correctguesses = 0;
            List<Color> lstguessedcolors = new();
            List<Spot> lstguessedspots = new();
            //insert black label for every correct color in correct place
            this.ActiveRow.Where(s => s.BackColor.Name == (this.ColorCode[this.ActiveRow.IndexOf(s)]).Name).ToList().ForEach(s =>
            {
                FeedbackSpot? emptyspot = ActiveFeedbackRow.FirstOrDefault(s => s.LabelBackColor == this.DefaultBackColor);
                if (emptyspot != null)
                {
                    emptyspot.LabelBackColor = this.CorrectPostionBackColor;
                    InvokePropertyChanged("LabelBackColor");
                }
                lstguessedspots.Add(s);
                lstguessedcolors.Add(s.BackColor);
                correctguesses += 1;
            });
            //check if win
            if (correctguesses == 4)
            {
                gamestatus = GameStatusEnum.Won;
                ShowAnswer();
            }
            else
            {
                //insert white label for every correct color in incorrect place
                //this.NumCorrectColor = 0;
                this.ActiveRow.ForEach(s =>
                {
                    if ((this.ColorCode.Count(c => c == s.BackColor) > lstguessedcolors.Count(c => c == s.BackColor)) && lstguessedspots.Contains(s) == false)
                    {
                        FeedbackSpot? emptyspot = ActiveFeedbackRow.FirstOrDefault(s => s.LabelBackColor == this.DefaultBackColor);
                        if (emptyspot != null)
                        {
                            emptyspot.LabelBackColor = this.CorrectColorBackColor;
                            InvokePropertyChanged("LabelBackColor");
                        }
                        //this.NumCorrectColor += 1;
                        lstguessedcolors.Add(s.BackColor);
                        lstguessedspots.Add(s);
                    }
                });

                //move on to next turn
                NumGuess = NumGuess + 1;
                if (NumGuess > 9)
                {
                    gamestatus = GameStatusEnum.Lost;
                    ShowAnswer();
                }
                else
                {
                    this.ActiveRow = Rows[Rows.IndexOf(this.ActiveRow) + 1];
                    this.ActiveFeedbackRow = FeedbackRows[FeedbackRows.IndexOf(this.ActiveFeedbackRow) + 1];
                }
            }

        }

        private void ShowAnswer()
        {
            CodeRow.ForEach(l =>
            {
                if (gamestatus == GameStatusEnum.Won)
                {
                    l.BackColor = ColorCode[CodeRow.IndexOf(l)];
                    l.Text = "!";
                    InvokePropertyChanged("Backcolor");
                    InvokePropertyChanged("Text");
                    InvokePropertyChanged("Comment");
                }
                else if (gamestatus == GameStatusEnum.Lost)
                {
                    l.BackColor = ColorCode[CodeRow.IndexOf(l)];
                    InvokePropertyChanged("Backcolor");
                    InvokePropertyChanged("Comment");
                }
            });
        }

        public String Comment 
        {
            get
            {
                Random rnd = new();
                switch (gamestatus)
                {
                    case GameStatusEnum.NotStarted:
                        _comment = "Select a level and click new game to start. Click the orange ? for game instructions.";
                        break;
                    case GameStatusEnum.Playing:
                        _comment = ((10 - NumGuess) < 10 ? lstcomments[rnd.Next(lstcomments.Count())] + " " : "") + (10 - NumGuess) + " guesses left!";
                        break;
                    case GameStatusEnum.Won:
                        _comment = "Awesome job! You cracked the code! Select a level and click new game to play again.";
                        break;
                    case GameStatusEnum.Lost:
                        _comment = "Game over. Better luck next time! Select a level and click new game to play again.";
                        break;
                }
                return _comment;
            }
            set
            {
                _comment = value;
            }
        }
        public Microsoft.Maui.Graphics.Color ConvertToMauiColor(System.Drawing.Color systemColor)
        {
            float red = systemColor.R / 255f;
            float green = systemColor.G / 255f;
            float blue = systemColor.B / 255f;
            float alpha = systemColor.A / 255f;

            return new Microsoft.Maui.Graphics.Color(red, green, blue, alpha);
        }

        private void InvokePropertyChanged([CallerMemberName] string propertyname = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
    }
}
