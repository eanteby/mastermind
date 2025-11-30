using MastermindSystem;

namespace MastermindMAUI;

public partial class Main : ContentPage
{
    Game activegame;
    List<Button> lstbtn;
    List<Label> lstlbl;
    List<Game> lstgame = new() { new Game(), new Game(), new Game() };
    public Main()
	{
		InitializeComponent();
        Game1Rb.BindingContext = lstgame[0];
        Game2Rb.BindingContext = lstgame[1];
        Game3Rb.BindingContext = lstgame[2];
        activegame = lstgame[0];
        this.BindingContext = activegame;
        btnNewGame.Clicked += BtnNewGame_Clicked;
        btnCheckCode.Clicked += BtnCheckCode_Clicked;
        btnHelp.Clicked += BtnHelp_Clicked;
        lstbtn = new()
            {
                btncode1, btncode2, btncode3, btncode4,
                btn1, btn2, btn3, btn4,
                btn5, btn6, btn7, btn8,
                btn9, btn10, btn11, btn12,
                btn13, btn14, btn15, btn16,
                btn17, btn18, btn19, btn20,
                btn21, btn22, btn23, btn24,
                btn25, btn26, btn27, btn28,
                btn29, btn30, btn31, btn32,
                btn33, btn34, btn35, btn36,
                btn37, btn38, btn39, btn40
            };
        lstlbl = new()
            {
                lbl1, lbl2, lbl3, lbl4,
                lbl5, lbl6, lbl7, lbl8,
                lbl9, lbl10, lbl11, lbl12,
                lbl13, lbl14, lbl15, lbl16,
                lbl17, lbl18, lbl19, lbl20,
                lbl21, lbl22, lbl23, lbl24,
                lbl25, lbl26, lbl27, lbl28 ,
                lbl29, lbl30, lbl31, lbl32,
                lbl33, lbl34, lbl35, lbl36,
                lbl37, lbl38, lbl39, lbl40
            };

        lstbtn.ForEach(b =>
        {
            b.Clicked += B_Clicked;
            b.BindingContext = activegame.Spots[lstbtn.IndexOf(b)];
            b.SetBinding(Button.BackgroundColorProperty, "MauiSpotColor");
            b.SetBinding(Button.TextProperty, "Text");
        });

        lstlbl.ForEach(l =>
        {

            l.BindingContext = activegame.FeedbackSpots[lstlbl.IndexOf(l)];
            l.SetBinding(Button.BackgroundColorProperty, "MauiFeedbackSpotColor");
        });

        foreach (Button b in ColorGrid)
        {
            b.Clicked += B_Clicked1;

        }

        this.BindingContext = activegame;
        lblMessage.SetBinding(Label.TextProperty, "Comment");

    }

    private Game.SpotColorEnum GetColor(Button b)
    {
        if (b == btnPink)
            return Game.SpotColorEnum.Pink;
        else if (b == btnYellow)
            return Game.SpotColorEnum.Yellow;
        else if (b == btnGreen)
            return Game.SpotColorEnum.Green;
        else if (b == btnBlue)
            return Game.SpotColorEnum.Blue;
        else if (b == btnPurple)
            return Game.SpotColorEnum.Purple;
        else if (b == btnBlack)
            return Game.SpotColorEnum.Black;
        else
            return Game.SpotColorEnum.Default;
    }



    private void BtnCheckCode_Clicked(object? sender, EventArgs e)
    {
        activegame.CheckCode();
        activegame.NextTurn();
    }

    private void B_Clicked(object? sender, EventArgs e)
    {
        activegame.GuessColor(lstbtn.IndexOf((Button)sender));
    }

    private void B_Clicked1(object? sender, EventArgs e)
    {
        if (sender is Button button)
        {
            activegame.ChosenColor = GetColor((Button)sender);
        }
    }

    private void BtnNewGame_Clicked(object? sender, EventArgs e)
    {
        activegame.StartGame(optBeginner.IsChecked);
    }

    private void BtnHelp_Clicked(object? sender, EventArgs e)
    {
        DisplayAlert("Game Instructions", activegame.Instructions, "OK");
    }

    private void GameRb_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        RadioButton rb = (RadioButton)sender;
        if (rb.IsChecked && rb.BindingContext != null)
        {
            activegame = (Game)rb.BindingContext;
            this.BindingContext = activegame;

            lstbtn.ForEach(b =>
            {
                int index = lstbtn.IndexOf(b);
                b.BindingContext = activegame.Spots[index];
            });

            lstlbl.ForEach(l =>
            {
                int index = lstlbl.IndexOf(l);
                l.BindingContext = activegame.FeedbackSpots[index];
            });
        }

    }
}