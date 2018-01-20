using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IPlayable;

// Numero : 10
/*
 * DLL -> BureauClassic -> Biblio de classe -> .NET 4.5.2
 */

namespace Participants.JeanbourquinSantos
{
    class OthelloIABoard10 : IPlayable.IPlayable
    {

        private int[,] board;
        public const int BOARD_SIZE = 8;

        public const int WHITE = 0;
        public const int BLACK = 1;
        public const int EMPTY = -1;

        public OthelloIABoard10()
        {
            board = new int[BOARD_SIZE, BOARD_SIZE];

            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    board[i, j] = EMPTY;
                }
            }

            board[3, 4] = 1;
            board[3, 3] = 0;
            board[4, 3] = 1;
            board[4, 4] = 0;
        }
        public int GetBlackScore()
        {
            return GetGenericScore(BLACK);
        }

        public int GetWhiteScore()
        {
            return GetGenericScore(WHITE);
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

        private int Eval(int[,] board, int color)
        {
            int evalVal = 0;
            int[,] val = {
                { 99, -8,   8,  6,  6,  8, -8,   99},
                { -8, -24, -4, -3, -3, -4, -24, -8},
                {  8, -4,   7,  4,  4,  7, -4,   8},
                {  6, -3,   4,  0,  0,  4, -3,   6},
                {  6, -3,   4,  0,  0,  4, -3,   6},
                {  8, -4,   7,  4,  4,  7, -4,   8},
                { -8, -24, -4, -3, -3, -4, -24, -8},
                { 99, -8,   8,  6,  6,  8, -8,   99}
            };

            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    if (board[i, j] == color)
                    {
                        evalVal += val[i, j];
                    }
                }
            }
            int other = 1;
            if (color == 1)
                other = 0;
            evalVal *= 4;
            evalVal += GetGenericScore(color);
            evalVal /= GetGenericScore(other);
            return evalVal;
        }
        public int[,] GetBoard()
        {
            return board;
        }

        public string GetName()
        {
            return "Jeanbourquin & Santos";
        }

        public Tuple<int, int> GetNextMove(int[,] board, int level, bool isWhiteTurn)
        {
            // IA Core
            Tuple<int, int, int> nextMove = Alphabeta(board, 5, 1, int.MinValue, isWhiteTurn);
            return new Tuple<int, int>(nextMove.Item2, nextMove.Item3);
        }

        //<val, col, line>
        private Tuple<int, int, int> Alphabeta(int[,] root, int depth, int minOrMax, int parentValue, bool isWhite)
        {

            if (depth == 0 || Final(root, isWhite))
            {
                return new Tuple<int, int, int>(Eval(root, ColorVal(isWhite)), -1, -1);
            }
            int optVal = minOrMax * int.MinValue;
            int col = -1;
            int line = -1;

            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    if (IsPlayable(root, i, j, isWhite))
                    {
                        int[,] newBoard = (int[,])root.Clone();
                        PlayMove(newBoard, i, j, isWhite);

                        Tuple<int, int, int> vals = Alphabeta(newBoard, depth - 1, -minOrMax, optVal, !isWhite);
                        if (vals.Item1 * minOrMax > optVal * minOrMax)
                        {
                            optVal = vals.Item1;
                            col = i;
                            line = j;
                            if (optVal * minOrMax > parentValue * minOrMax)
                            {
                                break;
                            }
                        }
                    }
                }

                if (optVal * minOrMax > parentValue * minOrMax)
                {
                    break;
                }
            }
            return new Tuple<int, int, int>(optVal, col, line);
        }

        private bool Final(int[,] board, bool isWhite)
        {
            bool isFinal = true;
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    if (IsPlayable(board, j, i, isWhite))
                    {
                        isFinal = false;
                    }
                }
            }

            return isFinal;
        }

        private int ColorVal(bool isWhite)
        {
            return isWhite ? WHITE : BLACK;
        }

        public bool PlayMove(int col, int line, bool isWhite)
        {
            return this.PlayMove(board, col, line, isWhite);
        }

        public bool PlayMove(int[,] board, int col, int line, bool isWhite)
        {
            // Update Board
            if (IsPlayable(board, col, line, isWhite))
            {
                board[col, line] = ColorVal(isWhite);
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        if (j != 0 || i != 0)
                        {
                            int iTemp = col + i;
                            int jTemp = line + j;
                            List<Tuple<int, int>> toCapture = new List<Tuple<int, int>>();
                            while (iTemp > -1 && iTemp < BOARD_SIZE && jTemp > -1 && jTemp < BOARD_SIZE &&
                                board[iTemp, jTemp] != (ColorVal(isWhite)) &&
                                board[iTemp, jTemp] != EMPTY)
                            {
                                toCapture.Add(new Tuple<int, int>(iTemp, jTemp));
                                iTemp += i;
                                jTemp += j;
                            }
                            if (iTemp > -1 && iTemp < BOARD_SIZE && jTemp > -1 && jTemp < BOARD_SIZE && board[iTemp, jTemp] == ColorVal(isWhite))
                            {
                                foreach (Tuple<int, int> zone in toCapture)
                                {
                                    board[zone.Item1, zone.Item2] = ColorVal(isWhite);
                                }
                            }
                        }
                    }
                }
                return true;
            }
            return false;
        }

        public bool IsPlayable(int col, int line, bool isWhite)
        {
            return this.IsPlayable(board, col, line, isWhite);
        }

        public bool IsPlayable(int[,] board, int col, int line, bool isWhite)
        {
            // Check if the move is legit
            // Neighbour check
            bool isValid = false;
            int color = ColorVal(isWhite);

            if (board[col, line] == EMPTY)
            {
                for (int i = -1; i < 2; i++)
                { // take the neighbors in the line
                    for (int j = -1; j < 2; j++)
                    { // take the neighbors in the column
                        if (i != 0 || j != 0) // i and j mustn't be the origin
                        {
                            int iTemp = col + i;  // Calculate board position
                            int jTemp = line + j; // Calculate board position
                            if (iTemp > -1 && iTemp < BOARD_SIZE && jTemp > -1 && jTemp < BOARD_SIZE) // Checks if the postions existe
                            {
                                if (board[iTemp, jTemp] != color && board[iTemp, jTemp] != -1)
                                {
                                    while (iTemp > -1 && iTemp < BOARD_SIZE && jTemp > -1 && jTemp < BOARD_SIZE) // Explore the paths from neighbor to edge
                                    {
                                        if (board[iTemp, jTemp] == EMPTY)
                                        {
                                            break;
                                        }

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
            }
            return isValid;
        }
    }
}
