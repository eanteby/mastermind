using MastermindSystem;
namespace MastermindApp
{
    public partial class frmMastermind : Form
    {
        Game game = new();
        List<Label> lstlbl;
        List<Button> lstbutton;
        public frmMastermind() 
        {
            InitializeComponent();

            lstbutton = new()
            {
                btnCode1, btnCode2, btnCode3, btnCode4,
                button1, button2, button3, button4,
                button5, button6, button7, button8,
                button9, button10, button11, button12,
                button13, button14, button15, button16,
                button17, button18, button19, button20,
                button21, button22, button23, button24,
                button25, button26, button27, button28,
                button29, button30, button31, button32,
                button33, button34, button35, button36,
                button37, button38, button39, button40
            };
            
            lstlbl = new()
            {
                label1, label2, label3, label4,
                label5, label6, label7, label8,
                label9, label10, label11, label12,
                label13, label14, label15, label16,
                label17, label18, label19, label20,
                label21, label22, label23, label24,
                label25, label26, label27, label28,
                label29, label30, label31, label32,
                label33, label34, label35, label36,
                label37, label38, label39, label40
            };
            
            lstbutton.ForEach(b =>
            {
                Spot spot = game.Spots[lstbutton.IndexOf(b)];
                b.Click += B_Click;
                b.DataBindings.Add("BackColor", spot, "SystemSpotColor");
                b.DataBindings.Add("Text", spot, "Text");
            });

            lstlbl.ForEach(l =>
            {

                FeedbackSpot feedbackspot = game.FeedbackSpots[lstlbl.IndexOf(l)];
                l.DataBindings.Add("BackColor", feedbackspot, "SystemFeedbackSpotColor");
            });

            btnCheckCode.Click += BtnCheckCode_Click;
            btnNewGame.Click += BtnNewGame_Click;
            btnHelp.Click += BtnHelp_Click;
            txtMessage.DataBindings.Add("Text", game, "Comment");

            foreach (Button b in tblColors.Controls)
            {
                b.Click += B_Click1;
            }

            btnBlack.Tag = Game.SpotColorEnum.Black;
            btnBlue.Tag = Game.SpotColorEnum.Blue;
            btnGreen.Tag = Game.SpotColorEnum.Green;
            btnPink.Tag = Game.SpotColorEnum.Pink;
            btnPurple.Tag = Game.SpotColorEnum.Purple;
            btnYellow.Tag = Game.SpotColorEnum.Yellow;
        }
        private void BtnNewGame_Click(object? sender, EventArgs e)
        {
            game.StartGame(optBeginner.Checked);
        }

        private void BtnCheckCode_Click(object? sender, EventArgs e)
        {
            game.CheckCode();
            game.NextTurn();
        }

        private void B_Click1(object? sender, EventArgs e)
        {
            if (sender != null && sender is Button)
            {
                game.ChosenColor = (Game.SpotColorEnum)((Button)sender).Tag;
            }
        }

        private void B_Click(object? sender, EventArgs e)
        {
            game.GuessColor(lstbutton.IndexOf((Button)sender));
        }
        private void BtnHelp_Click(object? sender, EventArgs e)
        {
            MessageBox.Show(game.Instructions, "Game Instructions");
        }

    }
}