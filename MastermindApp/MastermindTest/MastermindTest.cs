using MastermindSystem;
using Microsoft.Maui.Graphics;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
namespace MastermindTest;

public class MastermindTest
{
    [SetUp]
    public void Setup()
    {
        
    }

    [Test]
    public void TestStartGame()
    {
        Game game = new Game();
        game.StartGame();
        string msg = $"game status = {game.GameStatus.ToString()} num spots = {game.Spots.Count()} num feedback spots = {game.FeedbackSpots.Count()}. Active row is {game.Rows.IndexOf(game.ActiveRow)} and active feedback row is {game.FeedbackRows.IndexOf(game.ActiveFeedbackRow)}.";
        Assert.IsTrue(game.GameStatus == Game.GameStatusEnum.Playing && game.Spots.Count == 44 && game.FeedbackSpots.Count == 40, msg);
        TestContext.WriteLine(msg);
    }


    [TestCase(true)]
    [TestCase(false)]
    public void TestGenerateCode(bool isbeginner)
    {
        Game game = new();
        game.StartGame(isbeginner);
        string msg = $"Color Code has {game.ColorCode.Count()} colors: {game.ColorCode[0]}, {game.ColorCode[1]}, {game.ColorCode[2]}, and {game.ColorCode[3]} ";
        Assert.IsTrue(game.ColorCode.Count() == 4, msg);
        TestContext.WriteLine(msg);
    }

    [TestCase(5)]
    public void TakeGuess(int num)
    {
        Game game = new();
        game.StartGame();
        game.ChosenColor = Game.SpotColorEnum.Pink;
        game.GuessColor(num);
        string msg = $"Current color = {game.ChosenColor.ToString()}. Backcolor of current spot is {game.Spots[num].BackColor}";
        Assert.IsTrue(game.Spots[num].BackColor == game.ChosenColor, msg);
        TestContext.WriteLine(msg);
    }

    [TestCase(0, 1, 2, 3)]
    [TestCase(4, 2, 2, 3)]
    [TestCase(0, 1, 0, 5)]
    public void CheckCode(int clr1, int clr2, int clr3, int clr4)
    {
        Game game = new();
        game.StartGame(false);
        game.ChosenColor = game.Colors[clr1];
        game.GuessColor(game.ActiveRow[0].Id);
        game.ChosenColor = game.Colors[clr2];
        game.GuessColor(game.ActiveRow[1].Id);
        game.ChosenColor = game.Colors[clr3];
        game.GuessColor(game.ActiveRow[2].Id);
        game.ChosenColor = game.Colors[clr4];
        game.GuessColor(game.ActiveRow[3].Id);
        game.CheckCode();
        string msg = $"Color code: {game.ColorCode[0]}, {game.ColorCode[1]}, {game.ColorCode[2]}, and {game.ColorCode[3]}. ";
        msg += $"Guess: {game.Colors[clr1]}, {game.Colors[clr2]}, {game.Colors[clr3]}, and {game.Colors[clr4]}. Num Correct Color: {game.NumCorrectColor} Num Correct Position: {game.NumCorrectPosition}. " + Environment.NewLine;
        msg += $"Num White labels {game.ActiveFeedbackRow.Where(l => l.LabelBackColor == Game.FeedbackSpotColorEnum.White).Count()}. Num black labels {game.ActiveFeedbackRow.Where(l => l.LabelBackColor == Game.FeedbackSpotColorEnum.Black).Count()}." + Environment.NewLine;
        Assert.IsTrue(game.NumCorrectPosition == game.ActiveFeedbackRow.Where(l => l.LabelBackColor == Game.FeedbackSpotColorEnum.Black).Count() && game.NumCorrectColor == game.ActiveFeedbackRow.Where(l => l.LabelBackColor
        == Game.FeedbackSpotColorEnum.White).Count(), msg);
        TestContext.WriteLine(msg);
    }


}

 