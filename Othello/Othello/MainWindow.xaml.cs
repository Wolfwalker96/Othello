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
using System.Collections.ObjectModel;
using Participants.JeanbourquinSantos;
using System.Windows.Threading;

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
        public int[] scores = new int[2];
        public int[] timers = new int[2];
        private DateTime previousTime;

        private DispatcherTimer timer = new DispatcherTimer();

        public const int WHITE = 0;
        public const int BLACK = 1;
        public const int EMPTY = -1;
        #endregion

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            NewGame();
        }
        #endregion

        private void NewGame()
        {
            RefreshUI(gameController.GetBoard());
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e) {
            if (previousTime == null) previousTime = DateTime.Now;
            DateTime currentTimer = DateTime.Now;
            timers[(whiteTurn) ? WHITE : BLACK] += currentTimer.Millisecond - previousTime.Millisecond;
        }

        private void RefreshUI(int[,] newBoard)
        {
            board = newBoard; // Passage par refèrence des tableaux... pas sûr
            scores[WHITE] = 0;
            scores[BLACK] = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Button btn = (Button)this.FindName($"Button{i}_{j}");
                    if (board[i, j] != EMPTY)
                    {
                        if (board[i, j] == WHITE)
                        {
                            btn.Content = CreateBtnImage(WHITE);
                            scores[WHITE]++;
                        }
                        else if (board[i, j] == BLACK) {
                            btn.Content = CreateBtnImage(BLACK);
                            scores[BLACK]++;
                        }
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
                    RefreshUI(gameController.GetBoard());
                }
            }
        }

        /// <summary>
        /// Get the score of a color
        /// </summary>
        /// <param name="color">Color to score (0:black, 1:white)</param>
        /// <returns>Score</returns>
        private int GetGenericScore(int color)
        {
            int score = 0;
            foreach (int jet in board)
            {
                if (jet == color) score++;
            }
            return score;
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

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double squareSize = Math.Min(((FrameworkElement)this.Content).ActualWidth / 5*2, ((FrameworkElement)this.Content).ActualHeight);
            Board.Height = squareSize;
            Board.Width = squareSize;
        }
    }
}
