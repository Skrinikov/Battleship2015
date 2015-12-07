using System;
using System.Windows;

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

    /** Randomly assigns the ships through the board. 
     * 
     */
    private void initializeBoards()
    {


        computerShipBoard = new int[10,10];
        computerHitsBoard = new int[10,10];
        int boatSize = 4;
        int direction;
        int x, y;
        bool isLegalLocation = true;
        bool firstPass = false;

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

                        if (computerShipBoard[x,y + j] != 0)
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
                                computerShipBoard[x,y + j] = 1;
                            else
                                computerShipBoard[x, y + j] = 2;
                            
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

                        if (computerShipBoard[x+j,y] != 0)
                        {
                            isLegalLocation = false;
                            break;
                        }
                    }
                    if (isLegalLocation)
                    {

                        for (int j = 0; j < boatSize; j++)
                        {
                            if(boatSize != 1)
                                computerShipBoard[x + j, y] = 1;
                            else
                                computerShipBoard[x + j, y] = 2;

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

    public Point MakeComputerMove()
    {

        if (computerMovesNumber > 99)
            throw new ArgumentOutOfRangeException("Maximum amount of moves reached.");

        computerMovesNumber++;

        if (difficulty == 0)
            return Easy();
        else if (difficulty == 1)
            return Medium();
        else
            return Hard();


    }


    private Point Easy()
    {

        int x = r.Next(0, 10);
        int y = r.Next(0, 10);


        Point pos = new Point(x,y);
        pos.X = x;
        pos.Y = y;

        if (computerHitsBoard[x, y] != 0)
            pos = Easy();

        return pos;

        
    }

    private Point Medium()
    {

        if (r.Next(0, 2) == 1)
            return Hard();
        else
            return Easy();

    }

    private Point Hard()
    {

        Point pos;


        if (computerHitsBoard[(int)lastHit.X, (int)lastHit.Y] == 1)
        {

            Point temp = shipDestroyer();

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
            Easy();
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

                    if (computerHitsBoard[(k+3)%10, j] == 0)
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
            Easy();
            
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

                    if (computerHitsBoard[(k+1)%10, j] == 0)
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
        return Easy();
        
    }

    private bool isPosLegal(int xy)
    {

        int x = xy / 10;
        int y = xy % 10;

        if (x > 9 || y > 9)
            return false;
        return true;

    }


    private Point shipDestroyer()
    {

        int x;
        int y;

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

            //Vertical UP
            if (tempHit == 1) {

                try
                {
                    if (computerHitsBoard[x + i, y] == -1)
                    {

                        directionChosen = false;
                        return shipDestroyer();

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
                    return shipDestroyer();
                }
            
            }
            //Vertical down
            else if (tempHit == 2)
            {
                try
                {
                    if (computerHitsBoard[x - i , y] == -1)
                    {

                        directionChosen = false;
                        return shipDestroyer();

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
                    return shipDestroyer();
                }
            

            }
            //Horizontal Left
            else if (tempHit == 0)
            {
                try
                {
                    if (computerHitsBoard[x, y + i ] == -1)
                    {

                        directionChosen = false;
                        return shipDestroyer();

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
                    return shipDestroyer();
                }
            

            }
            //Horizontal right.
            else if (tempHit == 3)
            {

                try
                {
                    if (computerHitsBoard[x, y - i ] == -1)
                    {

                        directionChosen = false;
                        return shipDestroyer();

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
                    return shipDestroyer();
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
