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
        private int[] computerShips = { 7, 6, 5, 4, 3 };
        private int[] playerShips = { 7, 6, 5, 4, 3 };
        private int lastShipHitByPlayer;
        private int lastShipHitByComputer;
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

            int y = (int)pos.X / 40 - 1;
            int x = (int)pos.Y / 40 - 1;

            if (computerShipBoard[x, y] > 0)
            {
                playerHitsBoard[x, y] = 1;
                lastShipHitByPlayer = computerShipBoard[x, y];
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

            Point pos = ai.MakeComputerMove(DidComputerSinkABoat());
            int x = (int)pos.X;
            int y = (int)pos.Y;

            if (playerShipBoard[x, y] > 0)
            {
                lastShipHitByComputer = playerShipBoard[x, y];
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
                    if (computerShipBoard[i, j] > 1)
                        return false;

            return true;
        }

        public bool DidComputerWin()
        {
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    if (playerShipBoard[i, j] > 1)
                        return false;

            return true;
        }

        public bool DidPlayerSinkABoat()
        {

            for (int i = 0; i < 5; i++)
                if (computerShips[i] == lastShipHitByPlayer * -1)
                    return false;

            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    if (computerShipBoard[i, j] == lastShipHitByPlayer)
                        return false;

            for (int i = 0; i < computerShips.Length; i++)
                if (computerShips[i] == lastShipHitByPlayer)
                    computerShips[i] *= -1;

            return true;
        }

        private bool DidComputerSinkABoat()
        {

            for (int i = 0; i < 5; i++)
                if (playerShips[i] == lastShipHitByComputer * -1)
                    return false;

            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    if (playerShipBoard[i, j] == lastShipHitByComputer)
                        return false;

            for (int i = 0; i < 5; i++)
                if (playerShips[i] == lastShipHitByComputer)
                    playerShips[i] *= -1;

            return true;
        }



    }
}
