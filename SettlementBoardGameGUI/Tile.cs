using System;
using System.Collections.Generic;
using System.Text;

namespace SettlementBoardGameGUI
{
    enum ResourceType
    {
        Desert,
        Lumber,
        Wool,
        Grain,
        Brick,
        Ore
    }

    class Tile
    {
        public Tile(ref Edge edge0, ref Edge edge1, ref Edge edge2, ref Edge edge3, ref Edge edge4, ref Edge edge5, ref Vertex pt0, ref Vertex pt1, ref Vertex pt2, ref Vertex pt3, ref Vertex pt4, ref Vertex pt5, ResourceType resource, int rollNumber)
        {
            edges = new Edge[6];
            vertices = new Vertex[6];

            edges[0] = edge0;
            edges[1] = edge1;
            edges[2] = edge2;
            edges[3] = edge3;
            edges[4] = edge4;
            edges[5] = edge5;

            vertices[0] = pt0;
            vertices[1] = pt1;
            vertices[2] = pt2;
            vertices[3] = pt3;
            vertices[4] = pt4;
            vertices[5] = pt5;

            this.resource = resource;
            this.rollNumber = rollNumber;

        }

        public Edge[] edges { get; private set; }
        public Vertex[] vertices { get; private set; }
        public ResourceType resource { get; private set; }
        public int rollNumber { get; private set; }
    }
}
