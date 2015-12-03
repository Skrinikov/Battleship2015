using System;
using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

/**
* A version of  the Battleship game with a space theme.
*
* @author Uen Yi (Cindy) Hung
* @Version 30/11/2015
*/
namespace BattleShip
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // VARIALBES
        int a = 0;
        int b = 0;
        int counter = 0;
        int mode = 0;
        int moveCounter = 0;
        int playerWins = 0;
        int playerLoses = 0;
        BattleshipGame game;
        bool canPlace = true;
        Point pos;
        String playerName = null;

        Image[] pieces;
        Image[] piecesFlipped;
        Image[] set;
        Image[] moves = new Image[200];
        String[] letterPos;
        String[] numberPos;
        int[,] player = new int[10, 10];
        int[,] computer = new int[10, 10];

        public MainWindow()
        {
            InitializeComponent();
            initializers();
        }

        public void initializers()
        {
            pieces = new Image[7] { piece1, piece2, piece3, piece4, piece5, piece6, piece7 };
            piecesFlipped = new Image[7] { piece1f, piece2f, piece3f, piece4f, piece5f, piece6f, piece7f };
            set = new Image[7] { ship1, ship2, ship3, ship4, ship5, ship6, ship7 };
            letterPos = new String[10] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
            numberPos = new String[10] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
            for (int i = 0; i < player.GetLength(0); i++)
                for (int j = 0; j < player.GetLength(1); j++)
                {
                    player[i, j] = 0;
                    computer[i, j] = 0;
                }
            loadRecord();
        }

        //MENU ITEMS
        private void exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void newGame_Click(object sender, RoutedEventArgs e)
        {
            menuNewGame.IsEnabled = false;
            menuReset.IsEnabled = true;
            playerBoardCanvas.IsEnabled = true;
            pcBoardCanvas.IsEnabled = false;
            resetBtn.Visibility = Visibility.Visible;
            resetBtn_click(sender, e);
            moveCounter = 0;
            playerBoardCanvas.Children.RemoveRange(7, playerBoardCanvas.Children.Count - 7);
            pcBoardCanvas.Children.RemoveRange(0, pcBoardCanvas.Children.Count);
        }

        private void menuMode_Click(object sender, RoutedEventArgs e)
        {
            if (nameInputTxt.Text.Length > 2)
            {
                welcomeGrid.Visibility = Visibility.Hidden;
                menuReset.IsEnabled = true;
                playerName = (playerName != null) ? playerName : nameInputTxt.Text;
                playerNameLbl.Content = "•.• " + playerName + " •.•";
                playerNameRecordLbl.Content = "Player: " + playerName;
                searchPlayer();
                playerWinRecordLbl.Content = "Wins: " + playerWins;
                playerLossRecordLbl.Content = "Loses: " + playerLoses;

                if (((MenuItem)sender).Tag.Equals("easyMode"))
                {
                    mode = 0;
                    menuEasy.IsEnabled = false;
                    menuNormal.IsEnabled = true;
                    menuHard.IsEnabled = true;
                }
                else if (((MenuItem)sender).Tag.Equals("normalMode"))
                {
                    mode = 1;
                    menuNormal.IsEnabled = false;
                    menuEasy.IsEnabled = true;
                    menuHard.IsEnabled = true;
                }
                else
                {
                    mode = 2;
                    menuNormal.IsEnabled = true;
                    menuEasy.IsEnabled = true;
                    menuHard.IsEnabled = false;
                }

                resetBtn_click(sender, e);
            }
            else
                MessageBox.Show("Player name must be at least 3 characters long.", "Error", MessageBoxButton.OK);
        }

        private void credit_Click(object sender, RoutedEventArgs e)
        {
            creditGrid.Visibility = Visibility.Visible;
            howToPlayGrid.Visibility = Visibility.Hidden;
            scoreBoardGrid.Visibility = Visibility.Hidden;
        }

        private void creditOk_Click(object sender, RoutedEventArgs e)
        {
            creditGrid.Visibility = Visibility.Hidden;
        }

        private void howToPlay_Click(object sender, RoutedEventArgs e)
        {
            howToPlayGrid.Visibility = Visibility.Visible;
            creditGrid.Visibility = Visibility.Hidden;
            scoreBoardGrid.Visibility = Visibility.Hidden;
        }

        private void howToPlayOk_Click(object sender, RoutedEventArgs e)
        {
            howToPlayGrid.Visibility = Visibility.Hidden;
        }

        private void menuScore_Click(object sender, RoutedEventArgs e)
        {
            scoreBoardGrid.Visibility = Visibility.Visible;
            howToPlayGrid.Visibility = Visibility.Hidden;
            creditGrid.Visibility = Visibility.Hidden;

        }

        private void scoreOkBtn_Click(object sender, RoutedEventArgs e)
        {
            scoreBoardGrid.Visibility = Visibility.Hidden;
        }

        // WELCOME PAGE/GRID
        private void welcomeModeBtn_click(object sender, RoutedEventArgs e)
        {
            if (nameInputTxt.Text.Length > 2)
            {
                welcomeGrid.Visibility = Visibility.Hidden;
                menuReset.IsEnabled = true;
                resetBtn_click(sender, e);
                playerName = nameInputTxt.Text;
                playerNameLbl.Content = "•.• " + playerName + " •.•";
                playerNameRecordLbl.Content = "Player: " + playerName;
                searchPlayer();
                playerWinRecordLbl.Content = "Wins: " + playerWins;
                playerLossRecordLbl.Content = "Loses: " + playerLoses;
                ;

                if (((Button)sender).Tag.Equals("easyMode"))
                    mode = 0;
                else if (((Button)sender).Tag.Equals("normalMode"))
                    mode = 1;
                else
                    mode = 2;
            }
            else
                MessageBox.Show("Player name must be at least 3 characters long.", "Error", MessageBoxButton.OK);
        }

        private void nameInput_click(object sender, MouseButtonEventArgs e)
        {
            nameInputTxt.Text = "";
            easyModeBtn.IsEnabled = true;
            normalModeBtn.IsEnabled = true;
            hardModeBtn.IsEnabled = true;
            menuEasy.IsEnabled = true;
            menuNormal.IsEnabled = true;
            menuHard.IsEnabled = true;
        }

        // GAME VISUAL
        private void playerCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            pos = Mouse.GetPosition((Canvas)sender);

            // To make sure the ships will not be on the first row and column
            // which are for grid identification.
            pos.X = (pos.X < 40) ? 40 : pos.X;
            pos.Y = (pos.Y < 40) ? 40 : pos.Y;

            // Converting to number of "block" instead of using a range of pixels.
            pos.X = (((int)pos.X) / 40) * 40.0;
            pos.Y = (((int)pos.Y) / 40) * 40.0;

            // To make sure the ships do no go over the board.
            if (counter < pieces.Length)
            {
                if (pieces[counter].Tag.Equals("normal"))
                    pos.X = (pos.X >= 440 - pieces[counter].Width) ? 440 - pieces[counter].Width : pos.X;
                else
                    pos.Y = (pos.Y >= 440 - pieces[counter].Height) ? 440 - pieces[counter].Height : pos.Y;

                moveImage(pieces[counter]);
            }

            // For programmer: making sure the numbers are right.
            Console.WriteLine(pos.X + " " + pos.Y);
        }

        private void moveImage(Image img)
        {
            img.Visibility = Visibility.Visible;
            set[counter].Visibility = Visibility.Hidden;
            Canvas.SetTop(img, pos.Y);
            Canvas.SetLeft(img, pos.X);
        }

        private void playerBoardCanvas_LeftClick(object sender, MouseButtonEventArgs e)
        {
            // If player can still play pieces.
            if (counter < pieces.Length)
            {
                a = ((int)pos.Y / 40) - 1;
                b = ((int)pos.X / 40) - 1;
                canPlace = true;

                // Check if the 'grid blocks' are already taken or not.
                for (int i = 0; i < pieces[counter].Width / 40; i++)
                    for (int j = 0; j < pieces[counter].Height / 40; j++)
                    {
                        if (player[a + j, b + i] > 0)
                        {
                            canPlace = false;
                            SystemSounds.Beep.Play();
                            break;
                        }
                    }

                // If 'grid blocks' are not taken, then proceed inputing 1s in the allocate spaces.
                if (canPlace)
                {
                    for (int i = 0; i < pieces[counter].Width / 40; i++)
                        for (int j = 0; j < pieces[counter].Height / 40; j++)
                        {
                            if (counter > 4)
                                player[a + j, b + i] = 2;
                            else
                                player[a + j, b + i] = 1;
                        }
                    counter++;
                }
            }
            // For programmer:
            // Nothing to do with the game itself, just making sure array positions are allocated properly.
            for (int i = 0; i < player.GetLength(0); i++)
                for (int j = 0; j < player.GetLength(1); j++)
                {
                    if (j % 10 == 0)
                        Console.WriteLine();
                    Console.Write(player[i, j]);
                }

            if (counter >= pieces.Length)
            {
                startBtn.Visibility = Visibility.Visible;
                playerBoardCanvas.IsEnabled = false;
                pcBoardCanvas.IsEnabled = true;
            }
        }

        private void playerBoardCanvas_RightClick(object sender, MouseButtonEventArgs e)
        {
            Image temp;

            if (counter < pieces.Length)
            {
                pieces[counter].Visibility = Visibility.Hidden;
                temp = pieces[counter];
                pieces[counter] = piecesFlipped[counter];
                piecesFlipped[counter] = temp;
            }
        }

        private void resetBtn_click(object sender, RoutedEventArgs e)
        {
            startBtn.Visibility = Visibility.Hidden;
            playerBoardCanvas.IsEnabled = true;
            pcBoardCanvas.IsEnabled = false;

            for (counter = 0; counter < pieces.Length; counter++)
            {
                pieces[counter].Visibility = Visibility.Hidden;
                set[counter].Visibility = Visibility.Visible;
            }

            pieces = new Image[7] { piece1, piece2, piece3, piece4, piece5, piece6, piece7 };
            piecesFlipped = new Image[7] { piece1f, piece2f, piece3f, piece4f, piece5f, piece6f, piece7f };

            counter = 0;

            for (int i = 0; i < player.GetLength(0); i++)
                for (int j = 0; j < player.GetLength(1); j++)
                {
                    player[i, j] = 0;
                    // TO BE DECIDED MAYBE MAYBE NOT
                    //computer[i, j] = 0;
                }
        }

        private void startBtn_click(object sender, RoutedEventArgs e)
        {
            resetBtn.Visibility = Visibility.Hidden;
            startBtn.Visibility = Visibility.Hidden;
            menuReset.IsEnabled = false;
            menuNewGame.IsEnabled = true;
            game = new BattleshipGame(mode, player);
        }

        private void pcBoardCanvas_Click(object sender, MouseButtonEventArgs e)
        {
            if (counter >= pieces.Length)
            {
                pos = e.GetPosition((Canvas)sender);

                // Not on first column or beyond the 11th column.
                if (pos.X >= 40 && pos.X <= 440)
                    // Not on first row or beyond the 11th row.
                    if (pos.Y >= 40 && pos.Y <= 440)
                    {
                        // Converting to number of "block" instead of using a range of pixels.
                        pos.X = (((int)pos.X) / 40) * 40.0;
                        pos.Y = (((int)pos.Y) / 40) * 40.0;


                        // Prepare the image
                        moves[moveCounter] = new Image();
                        moves[moveCounter].Width = 40;
                        moves[moveCounter].Height = 40;

                        if (game.MoveByPlayer(pos) > -1)
                        {
                            if (game.MoveByPlayer(pos) == 1)
                                moves[moveCounter].Source = ((Image)this.FindResource("hitImg")).Source;
                            else
                                moves[moveCounter].Source = ((Image)this.FindResource("missImg")).Source;

                            pcBoardCanvas.Children.Add(moves[moveCounter]);
                            Canvas.SetTop(moves[moveCounter], pos.Y);
                            Canvas.SetLeft(moves[moveCounter], pos.X);
                            moveCounter++;


                            // Computer's turn
                            moves[moveCounter] = new Image();
                            moves[moveCounter].Width = 40;
                            moves[moveCounter].Height = 40;

                            pos = game.MoveByComputer();
                            if (player[(int)pos.Y, (int)pos.X] > 0)
                                moves[moveCounter].Source = ((Image)this.FindResource("hitImg")).Source;
                            else
                                moves[moveCounter].Source = ((Image)this.FindResource("missImg")).Source;

                            // Convert back to pixels
                            playerBoardCanvas.Children.Add(moves[moveCounter]);
                            pos.X = (pos.X + 1) * 40;
                            pos.Y = (pos.Y + 1) * 40;
                            Canvas.SetTop(moves[moveCounter], pos.Y);
                            Canvas.SetLeft(moves[moveCounter], pos.X);
                        }



                        //CHECK IF ANYONE WON
                        //IF WON
                    }
            }
        }

        // OTHER
        private void loadRecord()
        {
            String record;
            String[] recordArray;
            if (File.Exists("record.txt"))
            {
                StreamReader sr = new StreamReader("record.txt");

                do
                {
                    record = sr.ReadLine();
                    if (record != null && record.Split(',').Length == 3)
                    {
                        recordArray = record.Split(',');
                        scoreRecordTxtB.Text += String.Format("Player: {0,-20} Wins: {1,-5} Loses: {2,-5} \n", recordArray[0], recordArray[1], recordArray[2]);
                    }
                } while (record != null);
            }
        }

        private void searchPlayer()
        {
            String record;
            String[] recordArray;
            if (File.Exists("record.txt"))
            {
                StreamReader sr = new StreamReader("record.txt");

                do
                {
                    record = sr.ReadLine();
                    if (record != null && record.Split(',').Length == 3)
                    {
                        recordArray = record.Split(',');
                        if (playerName.Equals(recordArray[0]))
                        {
                            playerWins = int.Parse(recordArray[1]);
                            playerLoses = int.Parse(recordArray[2]);
                            break;
                        }
                    }
                } while (record != null);
            }
        }

    } // End of partial class.
} // THE END.