using System;
using System.Collections.Generic;
using System.Text;

namespace SettlementBoardGame
{
    class Board
    {
        public Board()
        {
            InitializeBoard();
        }

        public Tile[] tiles { get; private set; }
        public Vertex[] vertices { get; private set; }


        private void InitializeBoard()
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
            x = -edgeLength * sine60;
            y = edgeLength / 2;
            var pt0 = new Vertex((int)x, (int)y);

            // Next top center point
            x = 0;
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

            // Generate the list of tiles.
            for (int i = 0; i < 19; i++)
            {
                // Generate the points for a given tile and add the points to the master point list.
                for(int j = 0; j < 6; j++)
                {
                    //vertices[(i*6)+j] = new Vertex(x, y);
                    
                }
                
            }
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
