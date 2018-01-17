using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Numero : 10
/*
 * DLL -> BureauClassic -> Biblio de classe -> .NET 4.5.2
 */

namespace Participants.JeanbourquinSantos
{
    class OthelloIA
    {
        private int[,] board;
        public const int BOARD_SIZE = 8;
        public const int WHITE = 0;
        public const int BLACK = 1;
        public const int EMPTY = -1;

        public OthelloIA() {
            board = new int[BOARD_SIZE, BOARD_SIZE];
            for (int i = 0; i < BOARD_SIZE; i++) {
                for (int j = 0; j < BOARD_SIZE; j++) {
                    board[i, j] = -1;
                }
            }

            board[3, 4] = 0;
            board[3, 3] = 1;
            board[4, 3] = 0;
            board[4, 4] = 1;
        }

        public int GetBlackScore() {
            return GetGenericScore(0);
        }

        public int GetWhiteScore() {
            return GetGenericScore(1);
        }

        /// <summary>
        /// Get the score of a color
        /// </summary>
        /// <param name="color">Color to score (0:black, 1:white)</param>
        /// <returns>Score</returns>
        private int GetGenericScore(int color) {
            int score = 0;
            foreach (int jet in board) {
                if (jet == color) score++;
            }
            return score;
        }

        public int[,] GetBoard() {
            return board;
        }

        public string GetName() {
            return "Jeanbourquin & Santos";
        }

        public Tuple<int, int> GetNextMove(int[,] board, int level, bool isWhiteTurn) {
            // IA Core
            return new Tuple<int, int>(-1, -1);
        }

        //<val, col, line>
        private Tuple<int, int, int> Alphabeta(int[,] root, int depth, int minOrMax, int parentValue, bool isWhite) {

            if(depth == 0 || Final(root, isWhite))
            {
                return new Tuple<int, int, int>(-1, -1, -1);
            }
            return new Tuple<int, int, int>(-1, -1, -1);
        }

        private bool Final(int [,] board, bool isWhite)
        {
            for (int i = 0; i< BOARD_SIZE; i++) {
                for (int j = 0; j < BOARD_SIZE; i++) {
                    if (isPlayable(j, i, isWhite)){
                        return false;
                    }
                }
            }

            return true;

        }

        private int ColorVal(bool isWhite) {
            return isWhite ? WHITE : BLACK;
        }

        public bool PlayMove(int col, int line, bool isWhite) {
            // Update Board
            if (isPlayable(col, line, isWhite))
            {
                board[line, col] = ColorVal(isWhite);
                return true;
            }
            return false;
        }

        public bool isPlayable(int col, int line, bool isWhite) {
            // Check if the move is legit
            // Neighbour check
            bool isValid = false;
            int color = ColorVal(isWhite);
            for (int i = - 1; i < 2; i++) { // take the neighbors in the line
                for (int j = - 1; j < 2; j++) { // take the neighbors in the column
                    if (i != 0 || j != 0) // i and j mustn't be the origin
                    {
                        int iTemp = line + i;  // Calculate board position
                        int jTemp = col + j; // Calculate board position
                        if (iTemp > -1 && iTemp < BOARD_SIZE && jTemp > -1 && jTemp < BOARD_SIZE) // Checks if the postions existe
                        {
                            if (board[iTemp, jTemp] != color && board[iTemp, jTemp] != -1) // Check if
                            {
                                while (iTemp > -1 && iTemp < BOARD_SIZE && jTemp > -1 && jTemp < BOARD_SIZE) // Explore the paths from neighbor to edge
                                {
                                    // Checks
                                    if (board[iTemp, jTemp] == color)
                                    {
                                        isValid = true;
                                    }
                                    // End Checks
                                    iTemp += i;
                                    jTemp += j;
                                }
                            }
                        }
                    }
                }
            }
            return isValid;
        }
    }
}
