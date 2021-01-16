using System;
using System.Collections.Generic;
using System.Text;

namespace SettlementBoardGameGUI
{
    class Edge
    {
        public Edge(ref Vertex pt0, ref Vertex pt1)
        {
            this.point0 = pt0;
            this.point1 = pt1;
            this.outsideEdge = true;
        }

        public Vertex point0 { get; private set; }
        public Vertex point1 { get; private set; }
        public int ownedBy { get; set; }
        public bool outsideEdge { get; set; }
    }
}
