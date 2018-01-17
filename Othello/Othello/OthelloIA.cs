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

        public OthelloIA() {
            board = new int[BOARD_SIZE, BOARD_SIZE];
            for (int i = 0; i < BOARD_SIZE; i++) {
                for (int j = 0; j < BOARD_SIZE; j++) {
                    board[i, j] = -1;
                }
            }

            board[3, 4] = 1;
            board[3, 3] = 0;
            board[4, 3] = 1;
            board[4, 4] = 0;
        }

        public int GetBlackScore() {
            return getGenericScore(0);
        }

        public int GetWhiteScore() {
            return getGenericScore(1);
        }

        /// <summary>
        /// Get the score of a color
        /// </summary>
        /// <param name="color">Color to score (0:black, 1:white)</param>
        /// <returns>Score</returns>
        private int getGenericScore(int color) {
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
            // Do someting I given up on you
            return "Jeanbourquin & Santos";
        }

        public Tuple<int, int> GetNextMove(int[,] board, int level, bool isWhiteTurn) {
            // IA Core
            return new Tuple<int, int>(-1, -1);
        }

        public bool PlayMove(int col, int line, bool isWhite) {
            // Update Board
            if (isPlayable(col, line, isWhite))
            {
                board[line, col] = isWhite ? 0 : 1;
                for (int i = -1; i < 2; i++) {
                    for (int j = -1; j < 2; j++) {
                        if (j != 0 && i != 0) {
                            int iTemp = col + i;
                            int jTemp = line + j;
                            List<Tuple<int, int>> toCapture = new List<Tuple<int, int>>();
                            while (iTemp>-1 && iTemp<BOARD_SIZE && jTemp > -1 && 
                                jTemp < BOARD_SIZE && 
                                board[iTemp,jTemp] != ((isWhite)?0:1) && 
                                board[iTemp, jTemp] != -1) {
                                toCapture.Add(new Tuple<int, int>(iTemp, jTemp));
                                iTemp += i;
                                jTemp += j;
                            }
                            if (board[iTemp, jTemp] == (isWhite ? 0 : 1)) {
                                foreach (Tuple<int, int> zone in toCapture) {
                                    board[zone.Item1, zone.Item2] = (isWhite ? 0 : 1);
                                }
                            }
                        }
                    }
                }
                return true;
            }
            return false;
        }

        public bool isPlayable(int col, int line, bool isWhite) {
            // Check if the move is legit
            // Neighbour check
            bool isValid = false;
            int color = (isWhite ? 1 : 0);
            for (int i = - 1; i < 2; i++) { // take the neighbors in the line
                for (int j = - 1; j < 2; j++) { // take the neighbors in the column
                    if (i != 0 || j != 0) // i and j mustn't be the origin
                    {
                        int iTemp = line + i;  // Calculate board position
                        int jTemp = col + j; // Calculate board position
                        if (iTemp > -1 && iTemp < BOARD_SIZE && jTemp > -1 && jTemp < BOARD_SIZE) // Checks if the postions existe
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
            return isValid;
        }
    }
}
