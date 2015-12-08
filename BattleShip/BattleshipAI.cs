using System;
using System.Windows;


/**
* A version of  the Battleship game with a space theme instance of AI.
*
* @author Danieil Skrinikov with a continuous code breaking by Uen Yi (Cindy) Hung
* @Version 12/08/2015
*/
public class BattleshipAI
{

    private int difficulty;
    private int timesLooped = 0;
    private int pattern = 1;
    private int computerMovesNumber = 0;
    private int destoryDirection;
    private int[,] computerShipBoard;
    private int[,] computerHitsBoard;
    private int[] directions;
    private int tempHit;
    private Point lastHit;
    private Point direction;

    private bool firstPattern = true;
    private bool directionChosen = false;

    Random r = new Random();


    /** No parameter constructor sets the difficulty to easy by default.
     * 
     */
    public BattleshipAI()
    {
        this.difficulty = 0;
        initializeBoards();

    }

    /* Overloaded difficulty constructor. Sets the difficulty to what the player wanted to have.
     */
    public BattleshipAI(int difficulty)
    {

        this.difficulty = difficulty;
        initializeBoards();

    }

    /** Randomly puts 5 ships and 2 decoys on the computer ship board which will be used for each game instance. 
     *  After this method is executed the computerShipBoard is guaranteed to have 14 '1' and 2 '2' which represent ships and decoys.
     * 
     */
    private void initializeBoards()
    {


        computerShipBoard = new int[10, 10];
        computerHitsBoard = new int[10, 10];
        int boatSize = 4;
        int direction;
        int x, y;
        bool isLegalLocation = true;
        bool firstPass = false;
        int[] computerShips = { 7, 6, 5, 4, 3 };

        //1x4
        //1x3
        //1x3
        //1x2
        //1x2
        //1x1
        //1x1

        for (int i = 0; i < 7; i++)
        {

            direction = r.Next(0, 2);
            isLegalLocation = true;

            if (direction == 0)
            {
                x = r.Next(0, 9);
                y = r.Next(0, 9 - boatSize);

                for (int j = 0; j < boatSize; j++)
                {

                    if (computerShipBoard[x, y + j] != 0)
                    {
                        isLegalLocation = false;
                        break;
                    }
                }
                if (isLegalLocation)
                {

                    for (int j = 0; j < boatSize; j++)
                    {
                        if (boatSize != 1)
                            computerShipBoard[x, y + j] = computerShips[i];
                        else
                            computerShipBoard[x, y + j] = 1;

                    }

                }
                else
                    i--;

            }
            else
            {

                x = r.Next(0, 9 - boatSize);
                y = r.Next(0, 9);

                for (int j = 0; j < boatSize; j++)
                {

                    if (computerShipBoard[x + j, y] != 0)
                    {
                        isLegalLocation = false;
                        break;
                    }
                }
                if (isLegalLocation)
                {

                    for (int j = 0; j < boatSize; j++)
                    {
                        if (boatSize != 1)
                            computerShipBoard[x + j, y] = computerShips[i];
                        else
                            computerShipBoard[x + j, y] = 1;

                    }

                }
                else
                {
                    i--;
                    continue;
                }

            }//End of outer if else.

            if (firstPass)
            {
                firstPass = false;
            }
            else
            {
                firstPass = true;
                boatSize--;
            }



        }//End of for.
    }//End of Initialize boards.


    /** Returns the ship board of a player fully set up.
     */
    public int[,] GetShipBoard()
    {
        return computerShipBoard;
    }


    /** Returns the hits board of the computer.
     */
    public int[,] GetHitsBoard()
    {
        return computerHitsBoard;
    }
    /* This method is the hub between different difficulties. When called will check for the difficulty of the current AI instance and then redirect to it accordinly.
     * This methodalso checks the amount of turns the computer has already played. If the computer played has played more than a 100 turns there will be no free squares for the easy AI
     * to make it's move. Therefore it will create an infinite recursion which will result in a stack overflow. This method prevents that.
     * 
     * Throws ArgumentOutOfRangeException
     *                      If the computer has made more than 99 turns.
     */
    public Point MakeComputerMove(bool isBoatDestroyed)
    {

        if (computerMovesNumber > 99)
            throw new ArgumentOutOfRangeException("Maximum amount of moves reached.");

        computerMovesNumber++;

        if (difficulty == 0)
            return Easy(isBoatDestroyed);
        else if (difficulty == 1)
            return Medium(isBoatDestroyed);
        else
            return Hard(isBoatDestroyed);


    }

    /*  Easy AI. Will Randomly select a coordinate and then look if the computer has already hit that position before. 
     * If it did already hit this position before it will call itself recursively until it finds an un used value. 
     *  
     */
    private Point Easy(bool isBoatDestroyed)
    {

        int x = r.Next(0, 10);
        int y = r.Next(0, 10);


        Point pos = new Point(x, y);
        pos.X = x;
        pos.Y = y;

        if (computerHitsBoard[x, y] != 0)
            pos = Easy(isBoatDestroyed);

        return pos;


    }

    /** Medium AI. Half the time calls the hard AI method half the time calls the medium method.
     */
    private Point Medium(bool isBoatDestroyed)
    {

        if (r.Next(0, 2) == 1)
            return Hard(isBoatDestroyed);
        else
            return Easy(isBoatDestroyed);

    }


    /** Hard AI. First checks if the last position that the computer shot at hit a boat. If it did. Calls the shipDestroyer method. If the shipDestroyer method returns a vaslue Hard ai will return the value.
     *  If the hard ai returns a Point(-1,-1) it will continue to do its pattern. The hard AI has 3 different patterns. He will first complete the central pattern randomly then will go for it's second pattern and so forth.
     *  If everything misses. It will look randomly for a sqaure to destroy.
     * 
     */
    private Point Hard(bool isBoatDestroyed)
    {

        Point pos;


        if (computerHitsBoard[(int)lastHit.X, (int)lastHit.Y] == 1)
        {

            Point temp = shipDestroyer(isBoatDestroyed);

            if (temp.X != -1 && temp.Y != -1)
                return temp;
        }


        if (pattern == 1)
        {

            int[] possibleValues = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            for (int i = 0; i < 10 && timesLooped < 20; i++)
            {
                timesLooped++;
                int k = r.Next(0, 10);

                if (possibleValues[k] != k)
                {
                    i--;
                    timesLooped--;
                    continue;
                }
                else
                {
                    possibleValues[k] = -1;
                }

                if (r.Next(0, 2) == 1)
                {

                    int j = k;

                    if (computerHitsBoard[k, j] == 0)
                    {

                        pos = new Point(k, j);
                        lastHit = new Point(k, j);
                        destoryDirection = 4;
                        return pos;
                    }
                    j = (j + 5) % 10;
                    if (computerHitsBoard[k, j] == 0)
                    {

                        pos = new Point(k, j);
                        lastHit = new Point(k, j);
                        destoryDirection = 4;
                        return pos;
                    }
                    timesLooped--;

                }
                else
                {
                    int j = k;

                    j = (j + 5) % 10;
                    if (computerHitsBoard[k, j] == 0)
                    {

                        pos = new Point(k, j);
                        lastHit = new Point(k, j);
                        destoryDirection = 4;
                        return pos;
                    }

                    j = (j + 5) % 10;
                    if (computerHitsBoard[k, j] == 0)
                    {

                        pos = new Point(k, j);
                        lastHit = new Point(k, j);
                        destoryDirection = 4;
                        return pos;
                    }
                    timesLooped--;

                }

            }//End of for
            pattern++;
            Easy(isBoatDestroyed);
        }//End of pattern == 1
        else if (pattern == 2)
        {
            timesLooped = 0;
            int[] possibleValues = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            for (int i = 0; i < 10 && timesLooped < 20; i++)
            {
                timesLooped++;
                int k = r.Next(0, 10);
                if (possibleValues[k] != k)
                {
                    i--;
                    timesLooped--;
                    continue;
                }
                else
                {
                    possibleValues[k] = -1;
                }

                if (r.Next(0, 2) == 1)
                {

                    int j = k;

                    if (computerHitsBoard[(k + 3) % 10, j] == 0)
                    {

                        pos = new Point((k + 3) % 10, j);
                        lastHit = new Point((k + 3) % 10, j);
                        destoryDirection = 4;
                        return pos;
                    }
                    j = (j + 5) % 10;
                    if (computerHitsBoard[(k + 3) % 10, j] == 0)
                    {

                        pos = new Point((k + 3) % 10, j);
                        lastHit = new Point((k + 3) % 10, j);
                        destoryDirection = 4;
                        return pos;
                    }

                }
                else
                {
                    int j = k;

                    j = (j + 5) % 10;
                    if (computerHitsBoard[(k + 3) % 10, j] == 0)
                    {

                        pos = new Point((k + 3) % 10, j);
                        lastHit = new Point((k + 3) % 10, j);
                        destoryDirection = 4;
                        return pos;
                    }

                    j = (j + 5) % 10;
                    if (computerHitsBoard[(k + 3) % 10, j] == 0)
                    {

                        pos = new Point((k + 3) % 10, j);
                        lastHit = new Point((k + 3) % 10, j);
                        destoryDirection = 4;
                        return pos;
                    }

                }

            }//End of for    

            pattern++;
            Easy(isBoatDestroyed);

        }//End of pattern == 2

        else if (pattern == 3)
        {
            timesLooped = 0;
            int[] possibleValues = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            for (int i = 0; i < 10 && timesLooped < 20; i++)
            {
                timesLooped++;
                int k = r.Next(0, 10);
                if (possibleValues[k] != k)
                {
                    i--;
                    timesLooped--;
                    continue;
                }
                else
                {
                    possibleValues[k] = -1;
                }

                if (r.Next(0, 2) == 1)
                {

                    int j = k;

                    if (computerHitsBoard[(k + 1) % 10, j] == 0)
                    {

                        pos = new Point((k + 1) % 10, j);
                        lastHit = new Point((k + 1) % 10, j);
                        return pos;
                    }
                    j = (j + 5) % 10;
                    if (computerHitsBoard[(k + 1) % 10, j] == 0)
                    {

                        pos = new Point((k + 1) % 10, j);
                        lastHit = new Point((k + 1) % 10, j);
                        return pos;
                    }

                }
                else
                {
                    int j = k;

                    j = (j + 5) % 10;
                    if (computerHitsBoard[(k + 1) % 10, j] == 0)
                    {

                        pos = new Point((k + 1) % 10, j);
                        lastHit = new Point((k + 1) % 10, j);
                        return pos;
                    }

                    j = (j + 5) % 10;
                    if (computerHitsBoard[(k + 1) % 10, j] == 0)
                    {

                        pos = new Point((k + 1) % 10, j);
                        lastHit = new Point((k + 1) % 10, j);
                        return pos;
                    }

                }

            }//End of for    

            pattern++;

        }
        return Easy(isBoatDestroyed);

    }

    private bool isPosLegal(int xy)
    {

        int x = xy / 10;
        int y = xy % 10;

        if (x > 9 || y > 9)
            return false;
        return true;

    }

    /** Once the hard Ai finds a position which hit a ship it will randomly look for the 4 directions for the ship. If a direction is found it will shoot in this direction until all directions are not hit
     *  or until the boat is destroyed.
     */
    private Point shipDestroyer(bool isBoatDestroyed)
    {

        int x;
        int y;


        Console.WriteLine("pc");
        for (int i = 0; i < 10; i++)
            for (int j = 0; j < 10; j++)
            {
                if (j % 10 == 0)
                    Console.WriteLine();
                Console.Write(computerHitsBoard[i, j]);
            }
        Console.WriteLine();


        if (firstPattern)
        {
            firstPattern = false;

            directions = new int[4];
            directions[0] = 0;
            directions[1] = 1;
            directions[2] = 2;
            directions[3] = 3;

        }

        while (!directionChosen)
        {

            tempHit = r.Next(0, 4);

            //Checks if all values are -1. If so breaks from the loop in order to prevent infinite loop.
            if (directions[0] == -1 && directions[1] == -1 && directions[2] == -1 && directions[3] == -1)
            {
                tempHit = -2;
                break;
            }

            if (directions[tempHit] != -1)
            {
                directionChosen = true;
                tempHit = directions[tempHit];
                directions[tempHit] = -1;
            }

        }

        for (int i = 1; i < 11; i++)
        {

            x = (int)lastHit.X;
            y = (int)lastHit.Y;

            if (isBoatDestroyed)
            {
                firstPattern = true;
                break;
            }

            //Vertical UP
            if (tempHit == 1)
            {

                try
                {
                    if (computerHitsBoard[x + i, y] == -1)
                    {

                        directionChosen = false;
                        return shipDestroyer(isBoatDestroyed);

                    }
                    else if (computerHitsBoard[x + i, y] == 0)
                    {

                        Point p = new Point(x + i, y);
                        return p;

                    }
                    else
                        continue;
                }
                catch (IndexOutOfRangeException e)
                {
                    directionChosen = false;
                    return shipDestroyer(isBoatDestroyed);
                }

            }
            //Vertical down
            else if (tempHit == 2)
            {
                try
                {
                    if (computerHitsBoard[x - i, y] == -1)
                    {

                        directionChosen = false;
                        return shipDestroyer(isBoatDestroyed);

                    }
                    else if (computerHitsBoard[x - i, y] == 0)
                    {

                        Point p = new Point(x - i, y);
                        return p;

                    }
                    else
                        continue;
                }
                catch (IndexOutOfRangeException e)
                {
                    directionChosen = false;
                    return shipDestroyer(isBoatDestroyed);
                }


            }
            //Horizontal Left
            else if (tempHit == 0)
            {
                try
                {
                    if (computerHitsBoard[x, y + i] == -1)
                    {

                        directionChosen = false;
                        return shipDestroyer(isBoatDestroyed);

                    }
                    else if (computerHitsBoard[x, y + i] == 0)
                    {

                        Point p = new Point(x, y + i);
                        return p;

                    }
                    else
                        continue;
                }
                catch (IndexOutOfRangeException e)
                {
                    directionChosen = false;
                    return shipDestroyer(isBoatDestroyed);
                }


            }
            //Horizontal right.
            else if (tempHit == 3)
            {

                try
                {
                    if (computerHitsBoard[x, y - i] == -1)
                    {

                        directionChosen = false;
                        return shipDestroyer(isBoatDestroyed);

                    }
                    else if (computerHitsBoard[x, y - i] == 0)
                    {

                        Point p = new Point(x, y - i);
                        return p;

                    }
                    else
                        continue;
                }
                catch (IndexOutOfRangeException e)
                {
                    directionChosen = false;
                    return shipDestroyer(isBoatDestroyed);
                }

            }
            else
            {

                firstPattern = true;

            }



        }

        return new Point(-1, -1);


    }


}