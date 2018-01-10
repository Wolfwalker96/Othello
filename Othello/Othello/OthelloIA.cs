using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Numero : 10

namespace Participants.JeanbourquinSantos
{
    class OthelloIA
    {
        private int[,] board;
        public const int BOARD_SIZE = 8;
        private int color;

        public OthelloIA() {
            board = new int[BOARD_SIZE, BOARD_SIZE];
            for (int i = 0; i < BOARD_SIZE; i++) {
                for (int j = 0; i < BOARD_SIZE; j++) {
                    board[i, j] = -1;
                }
            }
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
                board[line, col] = isWhite ? 1 : 0;
                return true;
            }
            return false;
        }

        public bool isPlayable(int col, int line, bool isWhite) {
            // Check if the move is legit
            // Neighbour check
            bool isValid = false;
            int color = (isWhite ? 1 : 0);
            for (int i = - 1; i < 2; i++) {
                for (int j = - 1; j < 2; j++) {
                    if (board[line + i, col + j] != color && board[i,j]!= -1 ) {
                        int iTemp = line + i;
                        int jTemp = col + j;
                        while (iTemp > -1 && iTemp < BOARD_SIZE && jTemp > -1 && jTemp < BOARD_SIZE) {
                            // Checks
                            if (board[iTemp, jTemp] == color) {
                                isValid = true;
                            }
                            // End Checks
                            iTemp +=  i;
                            jTemp +=  j;
                        }
                    }
                }
            }
            return isValid;
        }
    }
}
