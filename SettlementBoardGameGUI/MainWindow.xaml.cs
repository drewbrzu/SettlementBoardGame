using System;
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
            //gameCanvas.Children.Clear();
            //double centerpointX = gameCanvas.Width / 2;
            //double centerpointY = gameCanvas.Height / 2;
            //drawHex(centerpointX, centerpointY);
            var board = new Board(new Point(gameCanvas.Width / 2, gameCanvas.Height / 2));
            gameCanvas.Children.Clear();
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

                for (int i = 0; i < 5; i++)
                {
                    drawRoad(pc[i], pc[i + 1]);
                }

                //foreach (var pt in pc)
                //{

                //}
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
            var rectWidth = 6;

            var distanceBetweenPoints = Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
            var angleRotation = Math.Asin((Math.Abs(p2.Y) - p1.Y) / distanceBetweenPoints) * (180 / Math.PI);
            if (p2.X - p1.X < 0)
            {
                angleRotation = -angleRotation + 180;
            }
            var rect = new Rectangle() { Height = rectWidth, Width = distanceBetweenPoints };
            var rot = new RotateTransform(angleRotation);
            rect.Fill = new SolidColorBrush(Colors.Yellow);
            // Figure out where to place the rectangle top left corner
            var point1DistanceFromOrigin = Math.Sqrt(Math.Pow(p1.X, 2) + Math.Pow(p1.Y, 2));
            var rectXOffset = p1.X / point1DistanceFromOrigin * (point1DistanceFromOrigin);
            var rectYOffset = Math.Cos((angleRotation - 90) * (Math.PI / 180)) * (rectWidth / 2);
            Canvas.SetLeft(rect, p1.X );
            Canvas.SetTop(rect, p1.Y * 1.1);
            // Rotate the rectangle
            rect.RenderTransform = rot;
            gameCanvas.Children.Add(rect);

            var el1 = new Ellipse() { Height = 10, Width = 10 };
            var el2 = new Ellipse() { Height = 10, Width = 10 };
            el1.Fill = new SolidColorBrush(Colors.Orange);
            el2.Fill = new SolidColorBrush(Colors.MediumPurple);
            Canvas.SetLeft(el1, p1.X);
            Canvas.SetTop(el1, p1.Y);
            gameCanvas.Children.Add(el1);
            Canvas.SetLeft(el2, p2.X);
            Canvas.SetTop(el2, p2.Y);
            gameCanvas.Children.Add(el2);


            //rect.MouseLeftButtonDown += onRoadLeftClick;
        }

        private void drawHex(double centerpointX, double centerpointY)
        {
            Polygon hexagon = new Polygon();
            PointCollection hexPts = new PointCollection();
            double edgeLength = 60;
            double pointDiameter = 10;
            
            double x, y;
            double sine60 = Math.Sin((60.0 / 180.0) * Math.PI);
            double cosine60 = Math.Cos((60.0 / 180.0) * Math.PI);

            // Create top left point from center of tile
            x = centerpointX - edgeLength * sine60;
            y = centerpointY - edgeLength / 2;
            hexPts.Add(new Point(x, y));

            // Next top center point
            x = centerpointX;
            y = centerpointY - (edgeLength * cosine60 + (edgeLength / 2));
            hexPts.Add(new Point(x, y));

            // Next top right point
            x = centerpointX + edgeLength * sine60;
            y = centerpointY - edgeLength / 2;
            hexPts.Add(new Point(x, y));

            // Next bottom right point
            x = centerpointX + edgeLength * sine60;
            y = centerpointY + edgeLength / 2;
            hexPts.Add(new Point(x, y));

            // Next bottom center point
            x = centerpointX;
            y = centerpointY + (edgeLength * cosine60 + (edgeLength / 2));
            hexPts.Add(new Point(x, y));

            // Next bottom left point
            x = centerpointX - (edgeLength * sine60);
            y = centerpointY + (edgeLength / 2);
            hexPts.Add(new Point(x, y));

            hexPts.Add(new Point(hexPts[0].X, hexPts[0].Y));

            hexagon.Points = hexPts;
            hexagon.Fill = new SolidColorBrush(Colors.Green);

            gameCanvas.Children.Add(hexagon);

            //for(int i = 0; i < hexPts.Count(); i++)
            //{
            //    var rect = new Rectangle() { Height = 5, Width = edgeLength };
            //    var rot = new RotateTransform(60 * i - 30);
            //    rect.Fill = new SolidColorBrush(Colors.Yellow);
            //    rect.MouseLeftButtonDown += onRoadLeftClick;
            //    Canvas.SetLeft(rect, hexPts[i].X - (centerpointX - hexPts[i].X) * 0.1);
            //    Canvas.SetTop(rect, hexPts[i].Y - (centerpointY - hexPts[i].Y) * 0.1);
            //    rect.RenderTransform = rot;
            //    gameCanvas.Children.Add(rect);

            //    var elipse = new Ellipse() { Height = pointDiameter, Width = pointDiameter };
            //    elipse.MouseLeftButtonDown += onVertexLeftClick;
            //    elipse.Fill = new SolidColorBrush(Colors.Black);
            //    Canvas.SetLeft(elipse, hexPts[i].X - (pointDiameter / 2));
            //    Canvas.SetTop(elipse, hexPts[i].Y - (pointDiameter / 2));
            //    gameCanvas.Children.Add(elipse);

            //}


            drawRoad(new Point(hexPts[0].X, hexPts[0].Y), new Point(hexPts[1].X, hexPts[1].Y));
            drawRoad(new Point(hexPts[1].X, hexPts[1].Y), new Point(hexPts[2].X, hexPts[2].Y));
            drawRoad(new Point(hexPts[2].X, hexPts[2].Y), new Point(hexPts[3].X, hexPts[3].Y));
            drawRoad(new Point(hexPts[3].X, hexPts[3].Y), new Point(hexPts[4].X, hexPts[4].Y));
            drawRoad(new Point(hexPts[4].X, hexPts[4].Y), new Point(hexPts[5].X, hexPts[5].Y));
            drawRoad(new Point(hexPts[5].X, hexPts[5].Y), new Point(hexPts[0].X, hexPts[0].Y));
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
