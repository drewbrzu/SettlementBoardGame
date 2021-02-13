using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SettlementBoardGameGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Game myGame;
        Dictionary<int, Color> playerColors;
        public MainWindow()
        {
            InitializeComponent();
            myGame = new Game(new Point(gameCanvas.Width / 2, gameCanvas.Height / 2), 3, 0);
            
            playerColors = new Dictionary<int, Color>();
            // TODO: Make player colors match on ID, maybe add way to pick color, or make it random. For now just assume playerId goes from 1 - 4 (max of 4 players).
            playerColors.Add(0, Colors.DarkGray);
            playerColors.Add(1, Colors.Red);
            playerColors.Add(2, Colors.Blue);
            playerColors.Add(3, Colors.Green);
            playerColors.Add(4, Colors.Orange);

            drawBoard();
        }
        public void drawBoard()
        {
            //board = new Game(new Point(gameCanvas.Width / 2, gameCanvas.Height / 2), 2, 0);
            var board = myGame.gameBoard;
            gameCanvas.Children.Clear();

            for (int t = 0; t < board.tiles.Count; t++)
            {
                var col = Colors.Tan;
                if(board.tiles[t].resource == ResourceType.Lumber)
                {
                    col = Colors.Green;
                }
                else if(board.tiles[t].resource == ResourceType.Brick)
                {
                    col = Colors.DarkRed;
                }
                else if (board.tiles[t].resource == ResourceType.Wool)
                {
                    col = Colors.Gray;
                }
                else if (board.tiles[t].resource == ResourceType.Grain)
                {
                    col = Colors.Goldenrod;
                }
                PointCollection pc = new PointCollection();
                foreach (var pt in board.tiles[t].vertices)
                {
                    pc.Add(new Point(pt.x, pt.y));
                }
                var centerpoint = new Point(board.tiles[t].centerpointX, board.tiles[t].centerpointY);
                var rand = new Random();
                drawHexagon(pc, col, t, centerpoint, rand.Next(1, 13));
            }

            for(int r = 0; r < board.edges.Count; r++)
            {
                drawRoad(new Point(board.edges[r].point0.x, board.edges[r].point0.y), new Point(board.edges[r].point1.x, board.edges[r].point1.y), r, board.edges[r].outsideEdge);
            }

            for(int v = 0; v < board.vertices.Count; v++)
            {
                drawVertex(new Point(board.vertices[v].x, board.vertices[v].y), v, playerColors[board.vertices[v].ownedBy]);
            }
        }
        public void drawHexagon(PointCollection points, Color color, int id, Point centerpoint, int rollNumber)
        {
            Polygon hex = new Polygon();
            hex.Points = points;
            hex.Fill = new SolidColorBrush(color);
            gameCanvas.Children.Add(hex);

            // Draw circle underneath number:
            int circleRadius = 12;
            Ellipse circle = new Ellipse() { Height = circleRadius * 2, Width = circleRadius * 2};
            circle.Fill = new SolidColorBrush(Colors.White);
            Canvas.SetLeft(circle, centerpoint.X - circleRadius);
            Canvas.SetTop(circle, centerpoint.Y - circleRadius);
            gameCanvas.Children.Add(circle);

            var numText = new TextBlock() { Text = rollNumber.ToString(), FontSize = 14, FontWeight = FontWeights.Bold, Padding = new Thickness(7, 2, 7, 0)};
            Canvas.SetLeft(numText, centerpoint.X - circleRadius);
            Canvas.SetTop(numText, centerpoint.Y - circleRadius);
            gameCanvas.Children.Add(numText);

            // TODO: Add events for clicking/hovering over polygon.
            // Events to handle mouse click
            hex.MouseLeftButtonDown += delegate (object sender, MouseButtonEventArgs e) { onTileLeftClick(sender, e, id); };
        }
        public void drawRoad(Point p1, Point p2, int id, bool outsideEdge)
        {
            var rectWidth = 10;
            var xDiff = p2.X - p1.X;
            var yDiff = p2.Y - p1.Y;
            var distanceBetweenPoints = Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
            var angleRotation = Math.Asin(yDiff / distanceBetweenPoints);
            var xDisp = Math.Sin(angleRotation) * rectWidth / 2;
            var yDisp = Math.Cos(angleRotation) * rectWidth / 2;
            var slope = (yDiff / xDiff);
            if(xDiff < 0)
            {
                xDisp = -xDisp;
            }

            var poly0 = new Point(p1.X - xDisp, p1.Y + yDisp);
            var poly1 = new Point(p2.X - xDisp, p2.Y + yDisp);
            var poly2 = new Point(p2.X + xDisp, p2.Y - yDisp); 
            var poly3 = new Point(p1.X + xDisp, p1.Y - yDisp);

            var polygon = new Polygon();
            polygon.Points = new PointCollection() { poly0, poly1, poly2, poly3 };
            polygon.Fill = new SolidColorBrush(Colors.Gray);
            gameCanvas.Children.Add(polygon);

            // Events to handle mouse click
            polygon.MouseLeftButtonDown += delegate (object sender, MouseButtonEventArgs e) { onRoadLeftClick(sender, e, id, outsideEdge); };
        }

        private void drawVertex(Point p0, int id, Color color)
        {
            int radius = 12;
            var elip = new Ellipse() { Height = radius, Width = radius };
            elip.Fill = new SolidColorBrush(color);
            Canvas.SetLeft(elip, p0.X - radius / 2);
            Canvas.SetTop(elip, p0.Y - radius / 2);
            gameCanvas.Children.Add(elip);

            // Events to handle mouse click
            elip.MouseLeftButtonDown += delegate (object sender, MouseButtonEventArgs e) { onVertexLeftClick(sender, e, id); };
            
        }


        private void loadPlayerScreen()
        {
            int lumber = 0, brick = 0, grain = 0, wool = 0, ore = 0;
            // load information for current player
            foreach(var card in myGame.currentPlayer().resourceCards)
            {
                switch (card)
                {
                    case ResourceType.Lumber:
                        lumber++;
                        break;
                    case ResourceType.Brick:
                        brick++;
                        break;
                    case ResourceType.Grain:
                        grain++;
                        break;
                    case ResourceType.Wool:
                        wool++;
                        break;
                    case ResourceType.Ore:
                        ore++;
                        break;
                    default:
                        break;
                }
            }
            var devcardList = myGame.currentPlayer().developmentCards;

            lumberCountTextBlock.Text = lumber.ToString();
            brickCountTextBlock.Text = brick.ToString();
            grainCountTextBlock.Text = grain.ToString();
            woolCountTextBlock.Text = wool.ToString();
            oreCountTextBlock.Text = ore.ToString();
            devcardCountTextBlock.Text = devcardList.Count.ToString();
        }

        private void clearPlayerScreen()
        {


            lumberCountTextBlock.Text = "0";
            brickCountTextBlock.Text = "0";
            grainCountTextBlock.Text = "0";
            woolCountTextBlock.Text = "0";
            oreCountTextBlock.Text = "0";
            devcardCountTextBlock.Text = "0";
        }

        private void onRoadLeftClick(object sender, MouseButtonEventArgs e, int itemId, bool outsideEdge)
        {
            var road = sender as Polygon;
            //road.Fill = new SolidColorBrush(Colors.Red);
            MessageBox.Show("You clicked road id: " + itemId + ". Outside Edge: " + outsideEdge);
        }

        private void onVertexLeftClick(object sender, MouseButtonEventArgs e, int itemId)
        {
            var vertex = sender as Ellipse;
            //vertex.Fill = new SolidColorBrush(Colors.Red);
            MessageBox.Show("You clicked vertex id: " + itemId);
            if (!myGame.buildSettlement(itemId))
            {
                MessageBox.Show("You don't have enough resources to build that.");
                return;
            }
            loadPlayerScreen();
            drawBoard();
        }

        private void onTileLeftClick(object sender, MouseButtonEventArgs e, int itemId)
        {
            var tile = sender as Polygon;
            MessageBox.Show("You clicked tile id: " + itemId);
        }

        private void pressMeButton_Click(object sender, RoutedEventArgs e)
        {
            loadPlayerScreen();
            
        }

        private void rollDiceButton_Click(object sender, RoutedEventArgs e)
        {
            var roll = myGame.rollDice();
            MessageBox.Show("You rolled a " + roll.ToString());
        }

        private void endTurnButton_Click(object sender, RoutedEventArgs e)
        {
            myGame.nextTurn();

            // Hide player information shown at bottom of screen.
            clearPlayerScreen();

            // Show pop up telling next player to sit down.
            MessageBox.Show(myGame.currentPlayer().playerName + " it is your turn.");

            // Update screen for next player.
            loadPlayerScreen();
        }

        private void buildButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
