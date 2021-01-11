using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace SettlementBoardGameGUI
{
    class Board
    {
        public Board(Point centerpoint)
        {
            InitializeBoard(centerpoint.X, centerpoint.Y);
        }

        public Tile[] tiles { get; private set; }
        public List<Vertex> vertices { get; private set; }


        private void InitializeBoard(double screenCenterX, double screenCenterY)
        {
            double x, y, xOffset, yOffset, edgeLength;
            double sine60 = Math.Sin(60 / 180 * Math.PI);
            double cosine60 = Math.Cos(60 / 180 * Math.PI);

            x = 0;
            y = 0;
            xOffset = 0;
            yOffset = 0;
            edgeLength = 10;

            // First create the center tile.
            // Create top left point from center of tile
            x = screenCenterX - edgeLength * sine60;
            y = screenCenterY - edgeLength / 2;
            vertices.Add(new Vertex(x, y));

            // Next top center point
            x = screenCenterX;
            y = screenCenterY - (edgeLength * cosine60 + (edgeLength / 2));
            vertices.Add(new Vertex(x, y));

            // Next top right point
            x = screenCenterX + edgeLength * sine60;
            y = screenCenterY - edgeLength / 2;
            vertices.Add(new Vertex(x, y));

            // Next bottom right point
            x = screenCenterX + edgeLength * sine60;
            y = screenCenterY + edgeLength / 2;
            vertices.Add(new Vertex(x, y));

            // Next bottom center point
            x = screenCenterX;
            y = screenCenterY + (edgeLength * cosine60 + (edgeLength / 2));
            vertices.Add(new Vertex(x, y));

            // Next bottom left point
            x = screenCenterX - (edgeLength * sine60);
            y = screenCenterY + (edgeLength / 2);
            vertices.Add(new Vertex(x, y));

            //var ed = new Edge(ref vertices[0], ref vertices[1]);

            //var tile0 = new Tile(new Edge(ref vertices[0], ref vertices[1]))

            // Now make the next tile.


        }

        private Tile CreateTile(double edgeLength, int centerpointX, int centerpointY)
        {
            double x, y;
            double sine60 = Math.Sin(60 / 180 * Math.PI);
            double cosine60 = Math.Cos(60 / 180 * Math.PI);

            // Create top left point from center of tile
            x = centerpointX - edgeLength * sine60;
            y = centerpointY + edgeLength / 2;
            var pt0 = new Vertex((int)x, (int)y);

            // Next top center point
            x = centerpointX;
            y = edgeLength * cosine60 + (edgeLength / 2);
            var pt1 = new Vertex((int)x, (int)y);

            // Next top right point
            x = -pt0.x; //edgeLength * sine60
            y = pt0.y; //edgeLength / 2
            var pt2 = new Vertex((int)x, (int)y);

            // Next bottom right point
            x = pt2.x; //edgeLength * sine60
            y = -pt2.y; //edgeLength / 2
            var pt3 = new Vertex((int)x, (int)y);

            // Next bottom center point
            x = 0;
            y = -pt1.y; //-edgeLength * cosine60 + (edgeLength / 2);
            var pt4 = new Vertex((int)x, (int)y);

            // Next bottom left point
            x = pt0.x; //-edgeLength * sine60
            y = -pt3.y; //edgeLength / 2
            var pt5 = new Vertex((int)x, (int)y);

            return null;
        }
    }
}
