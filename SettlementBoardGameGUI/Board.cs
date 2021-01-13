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

        public List<Tile> tiles { get; private set; }
        public List<Vertex> vertices { get; private set; }


        private void InitializeBoard(double screenCenterX, double screenCenterY)
        {
            double x, y, xOffset, yOffset, edgeLength;
            double sine60 = Math.Sin(60.0 / 180.0 * Math.PI);
            double cosine60 = Math.Cos(60.0 / 180.0 * Math.PI);
            vertices = new List<Vertex>();
            tiles = new List<Tile>();

            x = 0;
            y = 0;
            xOffset = 0;
            yOffset = 0;
            edgeLength = 30;

            // First create the center tile.
            // Create top left point from center of tile
            x = screenCenterX - edgeLength * sine60;
            y = screenCenterY - edgeLength / 2;
            var pt0 = new Vertex(x, y);
            vertices.Add(pt0);

            // Next top center point
            x = screenCenterX;
            y = screenCenterY - (edgeLength * cosine60 + (edgeLength / 2));
            var pt1 = new Vertex(x, y);
            vertices.Add(pt1);

            // Next top right point
            x = screenCenterX + edgeLength * sine60;
            y = screenCenterY - edgeLength / 2;
            var pt2 = new Vertex(x, y);
            vertices.Add(pt2);

            // Next bottom right point
            x = screenCenterX + edgeLength * sine60;
            y = screenCenterY + edgeLength / 2;
            var pt3 = new Vertex(x, y);
            vertices.Add(pt3);

            // Next bottom center point
            x = screenCenterX;
            y = screenCenterY + (edgeLength * cosine60 + (edgeLength / 2));
            var pt4 = new Vertex(x, y);
            vertices.Add(pt4);

            // Next bottom left point
            x = screenCenterX - (edgeLength * sine60);
            y = screenCenterY + (edgeLength / 2);
            var pt5 = new Vertex(x, y);
            vertices.Add(pt5);

            var edge0 = new Edge(ref pt0, ref pt1);
            var edge1 = new Edge(ref pt1, ref pt2);
            var edge2 = new Edge(ref pt2, ref pt3);
            var edge3 = new Edge(ref pt3, ref pt4);
            var edge4 = new Edge(ref pt4, ref pt5);
            var edge5 = new Edge(ref pt5, ref pt0);

            var tile0 = new Tile(ref edge0, ref edge1, ref edge2, ref edge3, ref edge4, ref edge5, ResourceType.Desert, 0);

            tiles.Add(tile0);

            // Now make the 2nd tile.
            var pt1_0 = new Vertex(pt1.x, pt1.y);//new Vertex(120, 120);
            var pt1_1 = new Vertex(pt0.x, pt0.y);//new Vertex(135, 135);
            var pt1_2 = rotateByAngle(pt1_1, pt1_0, -120);
            var pt1_3 = rotateByAngle(pt1_2, pt1_1, -120);
            var pt1_4 = rotateByAngle(pt1_3, pt1_2, -120);
            var pt1_5 = rotateByAngle(pt1_4, pt1_3, -120);

            var edge1_0 = new Edge(ref pt1_0, ref pt1_1);
            var edge1_1 = new Edge(ref pt1_1, ref pt1_2);
            var edge1_2 = new Edge(ref pt1_2, ref pt1_3);
            var edge1_3 = new Edge(ref pt1_3, ref pt1_4);
            var edge1_4 = new Edge(ref pt1_4, ref pt1_5);
            var edge1_5 = new Edge(ref pt1_5, ref pt1_0);

            var tile1 = new Tile(ref edge1_0, ref edge1_1, ref edge1_2, ref edge1_3, ref edge1_4, ref edge1_5, ResourceType.Brick, 0);
            tiles.Add(tile1);

            // Now the 3rd tile.
            var pt2_0 = new Vertex(pt2.x, pt2.y);
            var pt2_1 = new Vertex(pt1.x, pt1.y);
            var pt2_2 = rotateByAngle(pt2_1, pt2_0, -120);
            var pt2_3 = rotateByAngle(pt2_2, pt2_1, -120);
            var pt2_4 = rotateByAngle(pt2_3, pt2_2, -120);
            var pt2_5 = rotateByAngle(pt2_4, pt2_3, -120);

            var edge2_0 = new Edge(ref pt2_0, ref pt2_1);
            var edge2_1 = new Edge(ref pt2_1, ref pt2_2);
            var edge2_2 = new Edge(ref pt2_2, ref pt2_3);
            var edge2_3 = new Edge(ref pt2_3, ref pt2_4);
            var edge2_4 = new Edge(ref pt2_4, ref pt2_5);
            var edge2_5 = new Edge(ref pt2_5, ref pt2_0);

            var tile2 = new Tile(ref edge2_0, ref edge2_1, ref edge2_2, ref edge2_3, ref edge2_4, ref edge2_5, ResourceType.Lumber, 0);
            tiles.Add(tile2);

            // Now the 4rd tile.
            var pt3_0 = new Vertex(pt3.x, pt3.y);
            var pt3_1 = new Vertex(pt2.x, pt2.y);
            var pt3_2 = rotateByAngle(pt3_1, pt3_0, -120);
            var pt3_3 = rotateByAngle(pt3_2, pt3_1, -120);
            var pt3_4 = rotateByAngle(pt3_3, pt3_2, -120);
            var pt3_5 = rotateByAngle(pt3_4, pt3_3, -120);

            var edge3_0 = new Edge(ref pt3_0, ref pt3_1);
            var edge3_1 = new Edge(ref pt3_1, ref pt3_2);
            var edge3_2 = new Edge(ref pt3_2, ref pt3_3);
            var edge3_3 = new Edge(ref pt3_3, ref pt3_4);
            var edge3_4 = new Edge(ref pt3_4, ref pt3_5);
            var edge3_5 = new Edge(ref pt3_5, ref pt3_0);

            var tile3 = new Tile(ref edge3_0, ref edge3_1, ref edge3_2, ref edge3_3, ref edge3_4, ref edge3_5, ResourceType.Wool, 0);
            tiles.Add(tile3);

            // Now the 5rd tile.
            var pt4_0 = new Vertex(pt4.x, pt4.y);
            var pt4_1 = new Vertex(pt3.x, pt3.y);
            var pt4_2 = rotateByAngle(pt4_1, pt4_0, -120);
            var pt4_3 = rotateByAngle(pt4_2, pt4_1, -120);
            var pt4_4 = rotateByAngle(pt4_3, pt4_2, -120);
            var pt4_5 = rotateByAngle(pt4_4, pt4_3, -120);

            var edge4_0 = new Edge(ref pt4_0, ref pt4_1);
            var edge4_1 = new Edge(ref pt4_1, ref pt4_2);
            var edge4_2 = new Edge(ref pt4_2, ref pt4_3);
            var edge4_3 = new Edge(ref pt4_3, ref pt4_4);
            var edge4_4 = new Edge(ref pt4_4, ref pt4_5);
            var edge4_5 = new Edge(ref pt4_5, ref pt4_0);

            var tile4 = new Tile(ref edge4_0, ref edge4_1, ref edge4_2, ref edge4_3, ref edge4_4, ref edge4_5, ResourceType.Grain, 0);
            tiles.Add(tile4);


        }

        /// <summary>
        /// Calculate location of new point by rotating pt1 around pt0 by some angle.
        /// </summary>
        /// <param name="pt0"></param>
        /// <param name="pt1"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        private Vertex rotateByAngle(Vertex pt0, Vertex pt1, double angle)
        {
            //  https://math.stackexchange.com/questions/1687901/how-to-rotate-a-line-segment-around-one-of-the-end-points
            var distanceBetweenPoints = Math.Sqrt(Math.Pow(pt1.x - pt0.x, 2) + Math.Pow(pt1.y - pt0.y, 2));
            var cosine = Math.Cos(angle * (Math.PI / 180));
            var sine = Math.Sin(angle * (Math.PI / 180));
            var x = cosine * (pt1.x - pt0.x) - sine * (pt1.y - pt0.y) + pt0.x;
            var y = sine * (pt1.x - pt0.x) + cosine * (pt1.y - pt0.y) + pt0.y;

            return new Vertex(x, y);
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
