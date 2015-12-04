using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BattleShip
{
    class BattleshipGame
    {

        private int difficulty;
        private int[,] playerShipBoard;
        private int[,] playerHitsBoard;
        private int[,] computerHitsBoard;
        private int[,] computerShipBoard;
        BattleshipAI ai;

        public BattleshipGame()
        {

            difficulty = 0;

        }

        public BattleshipGame(int difficulty, int[,] playerShipBoard)
        {

            this.difficulty = difficulty;
            this.playerShipBoard = playerShipBoard;
            playerHitsBoard = new int[10, 10];


            ai = new BattleshipAI(difficulty);

            computerShipBoard = ai.GetShipBoard();
            computerHitsBoard = ai.GetHitsBoard();

        }

        public int MoveByPlayer(Point pos)
        {

            int y = (int)pos.X /40 -1;
            int x = (int)pos.Y /40 -1;

            if (computerShipBoard[x, y] > 0)
            {
                playerHitsBoard[x, y] = 1;
                computerShipBoard[x, y] *= -1;
                return 1;
            }
            else if (computerShipBoard[x, y] == 0)
            {
                playerHitsBoard[x, y] = -1;
                computerShipBoard[x, y] = -1;
                return 0;
            }
            else
                return -1;

        }

        public Point MoveByComputer()
        {

            Point pos = ai.MakeComputerMove();
            int x = (int)pos.X;
            int y = (int)pos.Y;

            if (playerShipBoard[x, y] == 1)
            {
                computerHitsBoard[x, y] = 1;
                
            }
            else
            {
                computerHitsBoard[x, y] = -1;
            }

            return pos;

        }

        public bool DidPlayerWin()
        {
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    if (computerShipBoard[i, j] == 1)
                        return false;

            return true;
        }

        public bool DidComputerWin()
        {
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    if (playerShipBoard[i, j] == 1)
                        return false;

            return true;
        }

    }
}
