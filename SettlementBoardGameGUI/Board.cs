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
            for(int i = 0; i < 6; i++)
            {
                // First calculate the location of the points and edges of this hexagon.
                var startingEdge = tile0.edges[i];
                var pt0 = startingEdge.point0;
                var pt1 = startingEdge.point1;
                var pt2 = rotateByAngle(pt1, pt0, 120);
                var pt3 = rotateByAngle(pt2, pt1, 120);
                var pt4 = rotateByAngle(pt3, pt2, 120);
                var pt5 = rotateByAngle(pt4, pt3, 120);
                
                var edge0 = startingEdge;
                edge0.outsideEdge = false;
                var edge1 = new Edge(ref pt1, ref pt2);
                var edge2 = new Edge(ref pt2, ref pt3);
                var edge3 = new Edge(ref pt3, ref pt4);
                var edge4 = new Edge(ref pt4, ref pt5);
                var edge5 = new Edge(ref pt5, ref pt0);

                // Next check to see if any of the points or edges already exist from drawing other hexagons.

                // First hex surrounding the center hex does not share any additional edges with existing hexes.
                // However, 2nd through 5th surrounding hex shares 2 edges with existing hexes (1 edge with center hex and 1 edge with previous hex).
                if (i > 0 && i < 5)
                {
                    edge5 = tiles[tiles.Count - 1].edges[1];
                    edge5.outsideEdge = false;
                    pt5 = edge5.point1;
                    // edge1 and pt2 are new to the list, but must be added here because for the 5th hex they are not new.
                    edges.Add(edge1);
                    vertices.Add(pt2);
                }
                // Last hex shares 3 edges.
                else if (i == 5)
                {
                    edge5 = tiles[tiles.Count - 1].edges[1];
                    edge5.outsideEdge = false;
                    pt5 = edge5.point1;
                    edge1 = tiles[tiles.Count - 5].edges[5];
                    edge1.outsideEdge = false;
                    pt2 = edge2.point0;
                }
                else 
                {
                    // First hex surrounging the center hex does not share edge1 or edge 5 so make sure to add them to the master list here
                    edges.Add(edge1);
                    edges.Add(edge5);
                    vertices.Add(pt2);
                    vertices.Add(pt5);
                }

                // These edges and points are always new for all of the first 7 hexagons.
                edges.Add(edge2);
                edges.Add(edge3);
                edges.Add(edge4);

                vertices.Add(pt3);
                vertices.Add(pt4);

                // Lastly create a new tile object to store all this information.
                var tile = new Tile(ref edge0, ref edge1, ref edge2, ref edge3, ref edge4, ref edge5, ref pt0, ref pt1, ref pt2, ref pt3, ref pt4, ref pt5, ResourceType.Brick, 0);
                tiles.Add(tile);
            }

            // Now add another circle of hexes surrounding the rest of the map.
            List<Edge> outsideEdges = new List<Edge>();
            outsideEdges = edges.Where(e => e.outsideEdge == true).ToList();

            //foreach(var edge in outsideEdges)
            //{
            //    var edge0 = edge;
            //    edge0.outsideEdge = false;
            //    var pt0 = edge0.point0;
            //    var pt1 = edge0.point1;
            //    var pt2 = rotateByAngle(pt1, pt0, -120);
            //    var pt3 = rotateByAngle(pt2, pt1, -120);
            //    var pt4 = rotateByAngle(pt3, pt2, -120);
            //    var pt5 = rotateByAngle(pt4, pt3, -120);

            //    pt2 = findMatchingVertex(vertices, pt2);
            //    pt3 = findMatchingVertex(vertices, pt3);
            //    pt4 = findMatchingVertex(vertices, pt4);
            //    pt5 = findMatchingVertex(vertices, pt5);

            //    var edge1 = new Edge(ref pt1, ref pt2);
            //    var edge2 = new Edge(ref pt2, ref pt3);
            //    var edge3 = new Edge(ref pt3, ref pt4);
            //    var edge4 = new Edge(ref pt4, ref pt5);
            //    var edge5 = new Edge(ref pt5, ref pt0);

            //    var currentEdgeCount = edges.Count;
            //    edge1 = findMatchingEdge(edges, edge1);
            //    edge2 = findMatchingEdge(edges, edge2);
            //    edge3 = findMatchingEdge(edges, edge3);
            //    edge4 = findMatchingEdge(edges, edge4);
            //    edge5 = findMatchingEdge(edges, edge5);

            //     If all of the edges have matches and no new edges were added to the list then it means this tile has already been created so it should not be added again.
            //    if (currentEdgeCount != edges.Count)
            //    {
            //         Create a new tile object to store all this information.
            //        var tile = new Tile(ref edge0, ref edge1, ref edge2, ref edge3, ref edge4, ref edge5, ref pt0, ref pt1, ref pt2, ref pt3, ref pt4, ref pt5, ResourceType.Brick, 0);
            //        tiles.Add(tile);
            //    }
            //}

            foreach (var edge in outsideEdges)//for (int e = 0; e < 6; e++)
            {
                //var edge0 = outsideEdges[e];
                var edge0 = edge;
                edge0.outsideEdge = false;
                var pt0 = edge0.point0;
                var pt1 = edge0.point1;
                var pt2 = rotateByAngle(pt1, pt0, -120);
                var pt3 = rotateByAngle(pt2, pt1, -120);
                var pt4 = rotateByAngle(pt3, pt2, -120);
                var pt5 = rotateByAngle(pt4, pt3, -120);

                var currentVertexCount = vertices.Count;
                pt2 = findMatchingVertex(vertices, pt2);
                pt3 = findMatchingVertex(vertices, pt3);
                pt4 = findMatchingVertex(vertices, pt4);
                pt5 = findMatchingVertex(vertices, pt5);

                // If all of the vertices have matches and no new vertices were added to the list then it means this tile has already been created so it should not be added again.
                if (vertices.Count > currentVertexCount)
                {
                    var edge1 = new Edge(ref pt1, ref pt2);
                    var edge2 = new Edge(ref pt2, ref pt3);
                    var edge3 = new Edge(ref pt3, ref pt4);
                    var edge4 = new Edge(ref pt4, ref pt5);
                    var edge5 = new Edge(ref pt5, ref pt0);

                    var currentEdgeCount = edges.Count;
                    edge1 = findMatchingEdge(edges, edge1);
                    edge2 = findMatchingEdge(edges, edge2);
                    edge3 = findMatchingEdge(edges, edge3);
                    edge4 = findMatchingEdge(edges, edge4);
                    edge5 = findMatchingEdge(edges, edge5);

                    //Create a new tile object to store all this information.
                    var tile = new Tile(ref edge0, ref edge1, ref edge2, ref edge3, ref edge4, ref edge5, ref pt0, ref pt1, ref pt2, ref pt3, ref pt4, ref pt5, ResourceType.Brick, 0);
                    tiles.Add(tile);
                }
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
