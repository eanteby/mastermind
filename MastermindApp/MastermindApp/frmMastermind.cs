using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MastermindApp
{
    public partial class frmMastermind : Form
    {
        List<List<Button>> lstrows;
        List<Color> lstcolor; 
        List<String> lstcomments = new() {"You got this!", "You can do it!", "Keep trying!", "Think hard!", "Don't give up!" };
        List<Button> activerow;
        List<TableLayoutPanel> lsttbl;
        List<Color> colorcode;
        List<Label> lstlbl;
        TableLayoutPanel activetbl;
        int numturnsleft = 10;
        enum gamestatusenum {notstarted, playing, won, lost};
        gamestatusenum gamestatus = gamestatusenum.notstarted;
        public frmMastermind()
        {
            InitializeComponent();
            lsttbl = new() { tblRow1, tblRow2, tblRow3, tblRow4, tblRow5, tblRow6, tblRow7, tblRow8, tblRow9, tblRow10 };
            lstlbl = new() { lblCode1, lblCode2, lblCode3, lblCode4 };
            lstrows = new()
            {
                new(){button1, button2, button3, button4},
                new(){button5, button6, button7, button8},
                new(){button9, button10, button11, button12},
                new(){button13, button14, button15, button16},
                new(){button17, button18, button19, button20},
                new(){button21, button22, button23, button24},
                new(){button25, button26, button27, button28},
                new(){button29, button30, button31, button32},
                new(){button33, button34, button35, button36},
                new(){button37, button38, button39, button40},
            };
            lstrows.ForEach(r => r.ForEach(b => b.Click += B_Click));
            btnCheckCode.Click += BtnCheckCode_Click;
            btnNewGame.Click += BtnNewGame_Click;
            btnHelp.Click += BtnHelp_Click;
            Comment();
        }

        private void CheckCode()
        {
            int correctguesses = 0;
            List<Color> lstguessedcolors = new();
            //insert black label for every correct color in correct place
            activerow.Where(b => b.BackColor == (colorcode[activerow.IndexOf(b)])).ToList().ForEach(b => {
                CreateLabel(Color.Black);
                lstguessedcolors.Add(b.BackColor);
                correctguesses += 1;
            });
            //check if win
            if (correctguesses == 4)
            {
                gamestatus = gamestatusenum.won;
                ShowAnswer();
            }
            else
            {
                //insert white label for every correct color in incorrect place
                activerow.ForEach(b => {
                    if (colorcode.Count(c => c == b.BackColor) >= lstguessedcolors.Count(c => c == b.BackColor) + 1)
                    {
                        CreateLabel(Color.White);
                        lstguessedcolors.Add(b.BackColor);
                    }
                });

                //move on to next turn
                numturnsleft = 9 - lstrows.IndexOf(activerow);
                if (numturnsleft == 0)
                {
                    gamestatus = gamestatusenum.lost;
                    ShowAnswer();
                }
                else
                {
                    activerow = lstrows[lstrows.IndexOf(activerow) + 1];
                    activetbl = lsttbl[lsttbl.IndexOf(activetbl) + 1];
                }
            }
            Comment();
            
        }

        private void GuessColor(Button btn)
        {
            if (gamestatus == gamestatusenum.playing)
            {
                if (btn != null && activerow.Exists(b => b == btn))
                {
                    int btncolorindex = lstcolor.IndexOf(btn.BackColor);
                    btn.BackColor = btncolorindex < lstcolor.Count() - 1 ? lstcolor[btncolorindex + 1] : lstcolor[0];
                }
            }
        }

        private void StartGame()
        {
            gamestatus = gamestatusenum.playing;
            activerow = lstrows[0];
            activetbl = lsttbl[0];
            numturnsleft = 10;
            lsttbl.ForEach(t => t.Controls.Clear());
            foreach (Button b in tblBoard.Controls.OfType<Button>()){
                b.BackColor = Color.White;
            }
            lstlbl.ForEach(l => { l.BackColor = Color.White; l.ForeColor = Color.Black; l.Text = "?"; });
            Comment();
            GenerateCode();
        }

        private void GenerateCode()
        {
            lstcolor = new() { Color.DeepSkyBlue, Color.DeepPink, Color.Yellow, Color.LimeGreen, Color.Purple, Color.Black };
            Random rnd = new();
            colorcode = new();
            for (int i = 0; i < 4; i++)
            {
                Color newcolor = lstcolor[rnd.Next(lstcolor.Count())];
                if (optBeginner.Checked) {
                    newcolor = lstcolor.Where(c => !colorcode.Contains(c)).ToList()[rnd.Next(lstcolor.Where(c => !colorcode.Contains(c)).ToList().Count())];
                }
                colorcode.Add(newcolor);
            }
        }

        private void ShowAnswer()
        {
            lstlbl.ForEach(l =>
            {
                if (gamestatus == gamestatusenum.won) { 
                   l.BackColor = colorcode[lstlbl.IndexOf(l)]; 
                   l.Text = "!";
                }
                l.ForeColor = gamestatus == gamestatusenum.won ? Color.White : colorcode[lstlbl.IndexOf(l)];
            });
        }

        private void Comment()
        {
            string m = "";
            Random rnd = new();
            switch (gamestatus)
            {
                case gamestatusenum.notstarted:
                    m = "Select a level and click new game to start. Click the orange ? for game instructions.";
                    break;
                case gamestatusenum.playing:
                    m = (numturnsleft < 10 ? lstcomments[rnd.Next(lstcomments.Count())] + " " : "") + numturnsleft + " guesses left!";
                    break;
                case gamestatusenum.won:
                    m = "Awesome job! You cracked the code! Select a level and click new game to play again.";
                    break;
                case gamestatusenum.lost:
                    m = "Game over. Better luck next time! Select a level and click new game to play again.";
                    break;
            }
            txtMessage.Text = m.ToString();
        }
        private void CreateLabel(Color clr)
        {
            Label lbl = new() { BackColor = clr, BorderStyle = BorderStyle.FixedSingle, Dock = DockStyle.Fill };
            activetbl.Controls.Add(lbl);
        }
        private void BtnNewGame_Click(object? sender, EventArgs e)
        {
            StartGame();
        }

        private void BtnCheckCode_Click(object? sender, EventArgs e)
        {
            if (gamestatus == gamestatusenum.playing)
            {
                CheckCode();
            }         
        }

        private void B_Click(object? sender, EventArgs e)
        {
            if (sender is Button)
            {
                GuessColor((Button)sender);
            }
        }
        private void BtnHelp_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("The goal of Mastermind is to crack the 4 color code generated at the start of the game. " +
                "In the beginner level, the code cannot contain each color more than once, while in the advanced level, a single color can show up multiple times in the code. " +
                "Each turn, 4 colors should be chosen by clicking the buttons in the highest empty row. " +
                "When check code is clicked, a black label will appear for each correct color that is in the correct position and a white label will appear for each correct color in the wrong position. " +
                "Players will have a maximum of 10 tries to guess the code. Get cracking! ", 
                "Game Instructions");
        }
    }
}