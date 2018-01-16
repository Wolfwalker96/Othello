using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Participants.JeanbourquinSantos;

namespace Othello
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Members

        private Array[] boardValues;
        private bool isPlayer1Turn;

        #endregion
        private int[,] board = new int[8, 8];
        OthelloIA player1 = new OthelloIA();
        //IPlayable player1;
        //IPlayable player2;
        #region Constructor
        public MainWindow()
        {
            InitializeComponent();

            NewGame();
        }
        #endregion


        private void NewGame()
        {
            //TODO
        }

        private void RefreshBoard(int[,] newBoard)
        {
            board = newBoard; // Passage par refèrence des tableaux... pas sûr
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Button btn = (Button)this.FindName($"Button{i}_{j}");
                    if (board[i, j] != -1)
                    {
                        if (board[i, j] == 1) btn.Content = CreateBtnImage(1);
                        else if (board[i, j] == 0) btn.Content = CreateBtnImage(0);
                    }
                    else
                    {
                        btn.Content = CreateBtnImage(-1);
                    }
                }
            }
        }

        private void ButtonMouseEnter(object sender, MouseEventArgs e)
        {
            RefreshBoard(player1.GetBoard());
            Button btn = sender as Button;
            bool isPlayable = player1.isPlayable((int)Char.GetNumericValue(btn.Name[6]), (int)Char.GetNumericValue(btn.Name[8]), isPlayer1Turn);

            if (isPlayable)
            {
                btn.Content = (isPlayer1Turn) ? CreateBtnImage(1) : CreateBtnImage(0);
            }
        }
        private void ButtonMouseLeave(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;
            int row = (int)Char.GetNumericValue(btn.Name[6]);
            int col = (int)Char.GetNumericValue(btn.Name[8]);

            if (board[row, col] == -1) btn.Content = "";
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            (sender as Button).Content = "Clickity";
        }

        private Image CreateBtnImage(int player)
        {
            Image tmp;
            switch (player)
            {
                case 1:
                    tmp = new Image
                    {
                        Source = new BitmapImage(new Uri("pack://application:,,,/Othello;component/whiteBtn.png")),
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    break;
                case 0:
                    tmp = new Image
                    {
                        Source = new BitmapImage(new Uri("pack://application:,,,/Othello;component/blackBtn.png")),
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    break;
                default:
                    tmp = new Image();
                    break;
            }
            return tmp;
        }
    }
}
