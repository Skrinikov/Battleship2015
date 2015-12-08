using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

/**
* A version of  the Battleship game with a space theme instance.
*
* @author Danieil Skrinikov with the help of Uen Yi (Cindy) Hung and Claudio Pietrantonio.
* @Version 12/08/2015
*/
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

        /** Two parameter constructor for a single AI game instance. 
         * 
         * @param difficulty
         *          Is the Difficulty on which the game will be played.
         * @param playerShipBoard
         *          Is the board in which the player has set up his ships.
         */
        public BattleshipGame(int difficulty, int[,] playerShipBoard)
        {

            this.difficulty = difficulty;
            this.playerShipBoard = playerShipBoard;
            playerHitsBoard = new int[10, 10];


            ai = new BattleshipAI(difficulty);

            computerShipBoard = ai.GetShipBoard();
            computerHitsBoard = ai.GetHitsBoard();

        }

        /// <summary>
        ///     Everytime the player makes a move this method looks if the Position of the move corresponds to a position of a ship inside the computerShipBoard array.
        ///     If it does corresponds to a ship tile. It will flip the ship tile to negative and assigns the lastShipHitByPlayer to the corresponding ship hit.
        /// </summary>
        /// <param name="pos">
        ///     pos: It is the position that the player chose to make his move on.
        /// </param>
        /// <returns>
        ///     returns 1  if it hit a boat,
        ///     returns 0  if it did not hit a board.
        ///     returns -1 if the position was already hit before.
        /// </returns>
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
        /// <summary>
        ///     Calls the ai MakeComputerMove method. Using its return it then looks if the computer move corresponds to a ship tile inside the player ships array. 
        ///         If it does: Flips the player ships array to negative and assigns a 1 into the corresponding position of the computerHitsArray.
        ///         If it does: Assigns -1 to the corresponding positin in the computerHitsArray.
        /// </summary>
        /// <returns>
        ///     The positiong in which the computer made its move.
        /// </returns>
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

        /// <summary>
        ///     Looks if the player won.
        /// </summary>
        /// <returns>
        ///     True if the player won.
        ///     False if the player did not win yet.
        /// </returns>
        public bool DidPlayerWin()
        {
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    if (computerShipBoard[i, j] > 1)
                        return false;

            return true;
        }

        /// <summary>
        ///     Looks if the Computer won.
        /// </summary>
        /// <returns>
        ///     True if the Computer won.
        ///     False if the Computer did not win yet.
        /// </returns>
        public bool DidComputerWin()
        {
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    if (playerShipBoard[i, j] > 1)
                        return false;

            return true;
        }

        /// <summary>
        ///     Cheks if the player sunk a boat.
        /// </summary>
        /// <returns>
        ///     True is the player sunk a boat.
        ///     False if the player did not sink a boat.
        /// </returns>
        public bool DidPlayerSinkABoat()
        {

            if (lastShipHitByPlayer == 1)
                return false;

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

        /// <summary>
        ///     Cheks if the computer sunk a boat.
        /// </summary>
        /// <returns>
        ///     True is the computer sunk a boat.
        ///     False if the computer did not sink a boat.
        /// </returns>
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
