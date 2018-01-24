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
using System.ComponentModel;

namespace Othello
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Members
        private int[,] board = new int[8, 8];
        OthelloIA gameController = new OthelloIA();
        private bool whiteTurn = false;
        public bool IsPlaying = false;
        private double[] timers = new double[2];
        private DateTime previousTime;

        private DispatcherTimer timer;

        public const int WHITE = 0;
        public const int BLACK = 1;
        public const int EMPTY = -1;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public string TimerWhite
        {
            get { return TimeSpan.FromMilliseconds(timers[WHITE]).ToString(@"mm\:ss"); }
        }
        public string TimerBlack
        {
            get { return TimeSpan.FromMilliseconds(timers[BLACK]).ToString(@"mm\:ss"); }
        }

        public int ScoreWhite
        {
            get { return gameController.GetWhiteScore(); }
        }

        public int ScoreBlack
        {
            get { return gameController.GetBlackScore(); }
        }

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();
            SetTimers();
            NewGame();

            this.DataContext = this;

        }

        private void SetTimers()
        {
            timer = new DispatcherTimer();

            timer.Tick += new EventHandler(Timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
        }
        #endregion

        private void NewGame()
        {
            timer.Stop();
            timers[WHITE] = 0;
            timers[BLACK] = 0;
            gameController.NewGame();
            RefreshUI(gameController.GetBoard());
            timer.Start();
            IsPlaying = true;
        }

        private void EndGame()
        {
            IsPlaying = false;
            string message = "Draw";
            if (ScoreBlack > ScoreWhite)
            {
                message = "Kim won";
            }
            else if(ScoreWhite > ScoreBlack)
            {
                message = "Trump won";
            }

            MessageBoxResult answer = MessageBox.Show($"{message}\nWant to replay ?","End of game",MessageBoxButton.YesNo);
            if (answer == MessageBoxResult.Yes)
            {
                this.NewGame();
            }
            else {
                this.Close();
            }
        }
        private void Timer_Tick(object sender, EventArgs e) {
            if (IsPlaying)
            {
                if(previousTime == DateTime.MinValue) previousTime = DateTime.Now;
                DateTime currentTime = DateTime.Now;
                timers[(whiteTurn) ? WHITE : BLACK] += currentTime.Subtract(previousTime).TotalMilliseconds;
                previousTime = currentTime;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TimerWhite"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TimerBlack"));
            }
            else {
                previousTime = DateTime.Now;
            }
        }

        private void RefreshUI(int[,] newBoard)
        {
            board = newBoard; // Passage par refèrence des tableaux... pas sûr
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
                        }
                        else if (board[i, j] == BLACK) {
                            btn.Content = CreateBtnImage(BLACK);
                        }
                    }
                    else
                    {
                        btn.Content = CreateBtnImage(EMPTY);
                    }
                }
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ScoreWhite"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ScoreBlack"));

        }

        private void ButtonMouseEnter(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;
            int col = (int)Char.GetNumericValue(btn.Name[6]);
            int row = (int)Char.GetNumericValue(btn.Name[8]);
            bool isPlayable = gameController.IsPlayable(col, row, whiteTurn);

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
            bool isPlayable = gameController.IsPlayable(col, row, whiteTurn);

            if (isPlayable)
            {
                if (gameController.PlayMove(col, row, whiteTurn)) {
                    whiteTurn = (whiteTurn) ? false : true;
                    RefreshUI(gameController.GetBoard());
                }

                //first pass
                if (!gameController.CanPlay(whiteTurn))
                {                    
                    whiteTurn = (whiteTurn) ? false : true;

                    //second pass = endgame
                    if (!gameController.CanPlay(whiteTurn))
                    {
                        EndGame();
                    }
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("WhiteScore"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BlackScore"));
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
