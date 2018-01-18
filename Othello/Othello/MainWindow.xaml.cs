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
        private int[,] board = new int[8, 8];
        OthelloIA gameController = new OthelloIA();
        private bool whiteTurn = false;

        public const int WHITE = 0;
        public const int BLACK = 1;
        public const int EMPTY = -1;
        #endregion

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();

            NewGame();
        }
        #endregion

        private void NewGame()
        {
            RefreshBoard(gameController.GetBoard());
        }

        private void RefreshBoard(int[,] newBoard)
        {
            board = newBoard; // Passage par refèrence des tableaux... pas sûr
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Button btn = (Button)this.FindName($"Button{i}_{j}");
                    if (board[i, j] != EMPTY)
                    {
                        if (board[i, j] == WHITE) btn.Content = CreateBtnImage(WHITE);
                        else if (board[i, j] == BLACK) btn.Content = CreateBtnImage(BLACK);
                    }
                    else
                    {
                        btn.Content = CreateBtnImage(EMPTY);
                    }
                }
            }
        }

        private void ButtonMouseEnter(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;
            int col = (int)Char.GetNumericValue(btn.Name[6]);
            int row = (int)Char.GetNumericValue(btn.Name[8]);
            bool isPlayable = gameController.isPlayable(col, row, whiteTurn);

            if (isPlayable)
            {
                btn.Content = (whiteTurn) ? CreateBtnImage(WHITE) : CreateBtnImage(BLACK);
            }
        }
        private void ButtonMouseLeave(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;
            int row = (int)Char.GetNumericValue(btn.Name[8]);
            int col = (int)Char.GetNumericValue(btn.Name[6]);

            if (board[col, row] == EMPTY) btn.Content = CreateBtnImage(EMPTY);
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int row = (int)Char.GetNumericValue(btn.Name[8]);
            int col = (int)Char.GetNumericValue(btn.Name[6]);
            bool isPlayable = gameController.isPlayable(col, row, whiteTurn);

            if (isPlayable)
            {
                if (gameController.PlayMove(col, row, whiteTurn)) {
                    whiteTurn = (whiteTurn) ? false : true;
                    RefreshBoard(gameController.GetBoard());
                }
            }
        }

        private Image CreateBtnImage(int player)
        {
            Image tmp;
            switch (player)
            {
                case WHITE:
                    tmp = new Image
                    {
                        Source = new BitmapImage(new Uri("pack://application:,,,/Othello;component/whiteBtn.png")),
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    break;
                case BLACK:
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
