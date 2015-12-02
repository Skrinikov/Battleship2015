using System;
using System.Windows;

public class BattleshipAI
{

    private int difficulty;
    private int[,] computerShipBoard;
    private int[,] computerHitsBoard;
    Point lastHit;

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

        bool hit = false;
        Point pos;


        
        for (int i = 0; i < 10 || hit == !true; i++)
        {

            for (int j = (i + 5) % 10; j < 10; j++)
            {

                if (computerHitsBoard[i, j] == 0)
                {

                    pos = new Point(i, j);
                    lastHit = new Point(i, j);
                    return pos;

                }

            }

        }
        //2nd pattern
        for (int i = 3; i < 10 || hit == !true; i++)
        {

            for (int j = (i + 5) % 10; j < 10; j++)
            {

                if (computerHitsBoard[i, j] == 0)
                {

                    pos = new Point(i, j);
                    lastHit = new Point(i, j);
                    return pos;

                }

            }

        }
        //Third Pattern.
        for (int i = 1; i < 10 || hit == !true; i++)
        {

            for (int j = (i + 5) % 10; j < 10; j++)
            {

                if (computerHitsBoard[i, j] == 0)
                {

                    pos = new Point(i, j);
                    lastHit = new Point(i, j);
                    return pos;

                }

            }

        }

        pos = new Point(5, 5);
        return pos;

    }

    private bool isPosLegal(int xy)
    {

        int x = xy / 10;
        int y = xy % 10;

        if (x > 9 || y > 9)
            return false;
        return true;

    }

}
