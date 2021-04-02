using System;
using System.Collections.Generic;
using System.Linq;
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
        public List<Edge> edges { get; private set; }
        public List<Vertex> vertices { get; private set; }
        public List<Player> players { get; private set; }
        

        private void InitializeBoard(double screenCenterX, double screenCenterY)
        {
            // DOCUMENTATION ON GRAPH DATA STRUCTURES:
            // https://cis300.cs.ksu.edu/graphs/impl/
            // https://docs.microsoft.com/en-us/previous-versions/ms379574(v=vs.80)?redirectedfrom=MSDN#datastructures20_5_topic3
            //

            double x, y, xOffset, yOffset, edgeLength;
            double sine60 = Math.Sin(60.0 / 180.0 * Math.PI);
            double cosine60 = Math.Cos(60.0 / 180.0 * Math.PI);

            vertices = new List<Vertex>();
            edges = new List<Edge>();
            tiles = new List<Tile>();

            x = 0;
            y = 0;
            xOffset = 0;
            yOffset = 0;
            edgeLength = 60;

            // First create top center point
            x = screenCenterX;
            y = screenCenterY - (edgeLength * cosine60 + (edgeLength / 2));
            var pt0_0 = new Vertex(x, y);
            vertices.Add(pt0_0);

            // Next top right point from center of tile
            x = screenCenterX + edgeLength * sine60;
            y = screenCenterY - edgeLength / 2;
            var pt0_1 = new Vertex(x, y);
            vertices.Add(pt0_1);

            // Create remaining points by rotating the first edge around by 120° to form a hexagon.
            var pt0_2 = rotateByAngle(pt0_1, pt0_0, -120);
            var pt0_3 = rotateByAngle(pt0_2, pt0_1, -120);
            var pt0_4 = rotateByAngle(pt0_3, pt0_2, -120);
            var pt0_5 = rotateByAngle(pt0_4, pt0_3, -120);
            vertices.Add(pt0_2);
            vertices.Add(pt0_3);
            vertices.Add(pt0_4);
            vertices.Add(pt0_5);

            var edge0_0 = new Edge(ref pt0_0, ref pt0_1);
            var edge0_1 = new Edge(ref pt0_1, ref pt0_2);
            var edge0_2 = new Edge(ref pt0_2, ref pt0_3);
            var edge0_3 = new Edge(ref pt0_3, ref pt0_4);
            var edge0_4 = new Edge(ref pt0_4, ref pt0_5);
            var edge0_5 = new Edge(ref pt0_5, ref pt0_0);
            edges.Add(edge0_0);
            edges.Add(edge0_1);
            edges.Add(edge0_2);
            edges.Add(edge0_3);
            edges.Add(edge0_4);
            edges.Add(edge0_5);


            // Create the first tile (the center one).
            var tile0 = new Tile(ref edge0_0, ref edge0_1, ref edge0_2, ref edge0_3, ref edge0_4, ref edge0_5, ref pt0_0, ref pt0_1, ref pt0_2, ref pt0_3, ref pt0_4, ref pt0_5, ResourceType.Desert, 0);
            tiles.Add(tile0);

            // Now fill in tiles surrounding this center tile
            List<Edge> outsideEdges = new List<Edge>();
            outsideEdges = edges.Where(e => e.outsideEdge == true).ToList();
            addHexagonsAroundBorder(outsideEdges, true);

            // Now add another circle of hexes surrounding the rest of the map.
            List <Edge> outsideEdges2 = new List<Edge>();
            outsideEdges2 = edges.Where(e => e.outsideEdge == true).ToList();
            addHexagonsAroundBorder(outsideEdges2, false);

        }

        private void addHexagonsAroundBorder(List<Edge> outsideEdges, bool clockwise)
        {
            int rotationDirection = 1;
            if (clockwise)
            {
                rotationDirection = 1;
            }
            else
            {
                rotationDirection = -1;
            }
            foreach (var edge in outsideEdges)
            {
                var edge0 = edge;
                edge0.outsideEdge = false;
                var pt0 = edge0.point0;
                var pt1 = edge0.point1;
                var pt2 = rotateByAngle(pt1, pt0, 120 * rotationDirection);
                var pt3 = rotateByAngle(pt2, pt1, 120 * rotationDirection);
                var pt4 = rotateByAngle(pt3, pt2, 120 * rotationDirection);
                var pt5 = rotateByAngle(pt4, pt3, 120 * rotationDirection);

                var pt2_match = vertices.Where(list => list.x == pt2.x && list.y == pt2.y).FirstOrDefault();
                var pt3_match = vertices.Where(list => list.x == pt3.x && list.y == pt3.y).FirstOrDefault();
                var pt4_match = vertices.Where(list => list.x == pt4.x && list.y == pt4.y).FirstOrDefault();
                var pt5_match = vertices.Where(list => list.x == pt5.x && list.y == pt5.y).FirstOrDefault();

                // If all points match existing points then this hexagon already exists so skip it.
                if (pt2_match != null && pt3_match != null && pt4_match != null & pt5_match != null)
                {
                    continue;
                }

                Edge edge1, edge2, edge3, edge4, edge5;
                // Edge 1
                if (pt2_match != null)
                {
                    pt2 = pt2_match;
                    edge1 = pt2.getConnectedEdge(pt1);
                    edge1.outsideEdge = false;
                }
                else
                {
                    vertices.Add(pt2);
                    edge1 = new Edge(ref pt1, ref pt2);
                    edges.Add(edge1);
                }
                // Edge 2
                if (pt3_match != null)
                {
                    pt3 = pt3_match;
                    edge2 = pt3.getConnectedEdge(pt2);
                    edge2.outsideEdge = false;
                }
                else
                {
                    vertices.Add(pt3);
                    edge2 = new Edge(ref pt2, ref pt3);
                    edges.Add(edge2);
                }
                // Edge 3
                if (pt4_match != null)
                {
                    pt4 = pt4_match;
                    edge3 = pt4.getConnectedEdge(pt3);
                    edge3.outsideEdge = false;
                }
                else
                {
                    vertices.Add(pt4);
                    edge3 = new Edge(ref pt3, ref pt4);
                    edges.Add(edge3);
                }
                // Edge 4 & 5
                if (pt5_match != null)
                {
                    pt5 = pt5_match;
                    edge5 = pt5.getConnectedEdge(pt0);
                    edge5.outsideEdge = false;

                    edge4 = new Edge(ref pt4, ref pt5);
                    edges.Add(edge4);
                }
                else
                {
                    vertices.Add(pt5);
                    edge4 = new Edge(ref pt4, ref pt5);
                    edge5 = new Edge(ref pt5, ref pt0);
                    edges.Add(edge4);
                    edges.Add(edge5);
                }

                //Create a new tile object to store all this information.
                var tile = new Tile(ref edge0, ref edge1, ref edge2, ref edge3, ref edge4, ref edge5, ref pt0, ref pt1, ref pt2, ref pt3, ref pt4, ref pt5, ResourceType.Brick, 0);
                tiles.Add(tile);
            }
        }

        private Vertex[] findVerticesMatchingRoll(int diceRoll)
        {
            var vertexList = new List<Vertex>();
            foreach(var tile in tiles)
            {
                if(tile.rollNumber == diceRoll)
                {
                    // TODO: This might be a good opportunity to cache the results of which vertices match a given roll.
                    // Loop through all vertices that are owned by a player.
                    foreach (var vertex in tile.vertices.Where(pt => pt.ownedBy != 0))
                    {
                        // Give resource to player who owns the vertex.
                        // If a city is built on the vertex, then give 2 resource cards.
                        if(vertex.settlementSize == 2)
                        {
                            players[vertex.ownedBy + 1].resourceCards.Add(tile.resource);
                        }
                        players[vertex.ownedBy + 1].resourceCards.Add(tile.resource);

                    }
                }
            }
            
            return null;
        }
        private Vertex findMatchingVertex(List<Vertex> vertexList, Vertex newVertex)
        {
            var matchVertex = vertexList.Where(list => list.x == newVertex.x && list.y == newVertex.y).FirstOrDefault();
            if(matchVertex == null)
            {
                matchVertex = newVertex;
                vertexList.Add(matchVertex);
            }
            return matchVertex;
        }

        private Edge findMatchingEdge(List<Edge> edgeList, Edge newEdge)
        {
            var matchEdge = edgeList.Where(list => list.matches(newEdge)).FirstOrDefault();
            if (matchEdge == null)
            {
                edgeList.Add(newEdge);
                return newEdge;
            }
            else
            {
                matchEdge.outsideEdge = false;
                return matchEdge;
            }
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

    }
}
