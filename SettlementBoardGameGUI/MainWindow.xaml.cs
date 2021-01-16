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
        public MainWindow()
        {
            InitializeComponent();
            drawBoard();
        }
        public void drawBoard()
        {
            var board = new Board(new Point(gameCanvas.Width / 2, gameCanvas.Height / 2));
            gameCanvas.Children.Clear();
            List<Edge> roads = new List<Edge>();
            List<Vertex> vertices = new List<Vertex>();

            foreach (var tile in board.tiles)
            {
                var col = Colors.Tan;
                if(tile.resource == ResourceType.Lumber)
                {
                    col = Colors.Green;
                }
                else if(tile.resource == ResourceType.Brick)
                {
                    col = Colors.DarkRed;
                }
                else if (tile.resource == ResourceType.Wool)
                {
                    col = Colors.Gray;
                }
                else if (tile.resource == ResourceType.Grain)
                {
                    col = Colors.Goldenrod;
                }
                PointCollection pc = new PointCollection();
                foreach (var pt in tile.vertices)
                {
                    pc.Add(new Point(pt.x, pt.y));
                }
                drawPolygon(pc, col);
            }

            foreach(var road in board.edges)
            {
                drawRoad(new Point(road.point0.x, road.point0.y), new Point(road.point1.x, road.point1.y));
            }

            foreach (var vertex in board.vertices)
            {
                drawVertex(new Point(vertex.x, vertex.y));
            }
        }
        public void drawPolygon(PointCollection points, Color col)
        {
            Polygon hex = new Polygon();
            hex.Points = points;
            hex.Fill = new SolidColorBrush(col);
            gameCanvas.Children.Add(hex);
            // TODO: Add events for clicking/hovering over polygon.
        }
        public void drawRoad(Point p1, Point p2)
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
            polygon.Fill = new SolidColorBrush(Colors.Yellow);
            gameCanvas.Children.Add(polygon);


        }

        private void drawVertex(Point p0)
        {
            int radius = 12;
            var elip = new Ellipse() { Height = radius, Width = radius };
            elip.Fill = new SolidColorBrush(Colors.Black);
            Canvas.SetLeft(elip, p0.X - radius / 2);
            Canvas.SetTop(elip, p0.Y - radius / 2);
            gameCanvas.Children.Add(elip);
            
        }

        private void onRoadLeftClick(object sender, MouseButtonEventArgs e)
        {
            var road = sender as Rectangle;
            road.Fill = new SolidColorBrush(Colors.Red);
        }

        private void onVertexLeftClick(object sender, MouseButtonEventArgs e)
        {
            var vertex = sender as Ellipse;
            vertex.Fill = new SolidColorBrush(Colors.Red);
        }

        private void pressMeButton_Click(object sender, RoutedEventArgs e)
        {

            
        }
    }
}
