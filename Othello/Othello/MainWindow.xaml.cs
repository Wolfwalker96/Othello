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

        private void ButtonMouseEnter(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;
            OthelloIA othelloTruc = new OthelloIA();

            btn.Content = Convert.ToString(othelloTruc.isPlayable((int)Char.GetNumericValue(btn.Name[6]), (int)Char.GetNumericValue(btn.Name[8]), isPlayer1Turn));

        }
        private void ButtonMouseLeave(object sender, MouseEventArgs e)
        {
          //  (sender as Button).Content = "Not Hover";
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            (sender as Button).Content = "Clickity";
        }
    }
}
