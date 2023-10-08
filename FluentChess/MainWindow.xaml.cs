using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FluentChess
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public List<ChessSquare> Chessboard { get; set; }
        public Color WhiteColor { get; set; } = Colors.White;
        public Color BlackColor { get; set; } = Colors.DimGray;
        public MainWindow()
        {
            this.InitializeComponent();
            InitializeChessboard();
            //DataContext = this;
        }

        private void InitializeChessboard()
        {
            Chessboard = new List<ChessSquare>();

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    var color = (row + col) % 2 == 0 ? WhiteColor : BlackColor;
                    var rank = 8 - row; // Rank (row) number
                    var file = (char)('a' + col); // File (column) letter
                    var text = $"{file}{rank}";
                    Chessboard.Add(new ChessSquare { Color = new SolidColorBrush(color), Text = text });
                }
            }
        }

        private void ValidateButton_Click(object sender, RoutedEventArgs e)
        {
            string userInput = InputTextBox.Text.Trim().ToLower();

            // Check if the input follows the format "a1" through "h8"
            if (userInput.Length == 2 && userInput[0] >= 'a' && userInput[0] <= 'h' && userInput[1] >= '1' && userInput[1] <= '8')
            {
                char file = userInput[0];
                int rank = int.Parse(userInput[1].ToString());
                int col = file - 'a';
                int row = 8 - rank;

                // Check if the cell exists
                if (row >= 0 && row < 8 && col >= 0 && col < 8)
                {
                    var cell = Chessboard[row * 8 + col];
                    string message = $"Cell {userInput} exists and is {GetColorName(cell.Color)}.";

                    // Display the result or take further action
                    // For demonstration purposes, let's display a message box
                    // Replace this with your desired logic
                    ShowMessage(message);
                }
                else
                {
                    // Cell is out of bounds
                    ShowMessage("Cell is out of bounds.");
                }
            }
            else
            {
                // Invalid input format
                ShowMessage("Invalid input format. Use format like 'h6'.");
            }
        }
        private string GetColorName(Brush colorBrush)
        {
            if (colorBrush is SolidColorBrush solidColorBrush)
            {
                Color color = solidColorBrush.Color;

                if (color == WhiteColor)
                {
                    return "white";
                }
                else if (color == BlackColor)
                {
                    return "black";
                }
            }

            return "unknown";
        }

        private void Cell_Click(object sender, RoutedEventArgs e)
        {
            var cell = (sender as FrameworkElement)?.DataContext as ChessSquare;
            if (cell != null)
            {
                InputTextBox.Text = cell.Text;
            }
        }

        private async void ShowMessage(string message)
        {
            var dialog = new Windows.UI.Popups.MessageDialog(message);
            await dialog.ShowAsync();
        }
    }
    public class ChessSquare
    {
        public SolidColorBrush Color { get; set; }
        public string Text { get; set; }
    }
}
