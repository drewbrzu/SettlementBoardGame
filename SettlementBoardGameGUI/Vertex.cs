using System;
using System.Collections.Generic;
using System.Text;

namespace SettlementBoardGameGUI
{
    class Vertex
    {
        public Vertex(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public double x { get; private set; }
        public double y { get; private set; }
        public int ownedBy { get; set; }
        public ResourceType portType { get; set; }

    }
}
