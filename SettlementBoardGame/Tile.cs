using System;
using System.Collections.Generic;
using System.Text;

namespace SettlementBoardGame
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
        public Tile(ref Edge edge0, ref Edge edge1, ref Edge edge2, ref Edge edge3, ref Edge edge4, ref Edge edge5, ResourceType resource, int rollNumber)
        {
            edges[0] = edge0;
            edges[1] = edge1;
            edges[2] = edge2;
            edges[3] = edge3;
            edges[4] = edge4;
            edges[5] = edge5;

            vertices[0] = edge0.point0;
            vertices[1] = edge1.point0;
            vertices[2] = edge2.point0;
            vertices[3] = edge3.point0;
            vertices[4] = edge4.point0;
            vertices[5] = edge5.point0;

            this.resource = resource;
            this.rollNumber = rollNumber;

        }

        public Edge[] edges { get; private set; }
        public Vertex[] vertices { get; private set; }
        public ResourceType resource { get; private set; }
        public int rollNumber { get; private set; }


    }
}
