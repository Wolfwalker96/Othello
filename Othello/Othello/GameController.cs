using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello
{
    public enum GameColor { NONE, BLACK, WHITE };

    public delegate void GameEventListener(GameController controller, GameColor currentColor);
    public class GameController
    {
        private GameColor currentColor;
        private List<GameEventListener> gameEventListeners = new List<GameEventListener>();

        public GameColor[][] GameBoard { get; private set; }

        public GameController() {
            NextColor();
        }

        public void Play(int x, int y) {
            if (GameBoard[x][y] == GameColor.NONE) {
                GameBoard[x][y] = currentColor;
                NextColor();
                Notify();
            }
        }

        public void NextColor() {
            if (currentColor == GameColor.BLACK)
            {
                currentColor = GameColor.WHITE;
            }
            else {
                currentColor = GameColor.BLACK;
            }
        }

        public void AddGameEventListener(GameEventListener listener) {
            gameEventListeners.Add(listener);
        }

        private void Notify() {
            foreach (GameEventListener listener in gameEventListeners) {
                listener(this, currentColor);
            }
        }
    }
}
