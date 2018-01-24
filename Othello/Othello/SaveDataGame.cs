using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello
{
    [Serializable]
    class SaveDataGame
    {
        public int[,] board;
        public bool isWhite;
        public double[] timers;

        public SaveDataGame(int[,] board, double[] timers, bool isWhite) {
            this.board = board;
            this.timers = timers;
            this.isWhite = isWhite;
        }
    }
}
