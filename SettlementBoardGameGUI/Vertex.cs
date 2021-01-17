using System;
using System.Collections.Generic;
using System.Text;

namespace SettlementBoardGameGUI
{
    class Vertex
    {
        public Vertex(double x, double y)
        {
            this.x = Math.Round(x, 3);
            this.y = Math.Round(y, 3);
        }

        public double x { get; private set; }
        public double y { get; private set; }
        public int ownedBy { get; set; }
        public ResourceType portType { get; set; }

        public bool matches(Vertex vert)
        {
            if(vert.x == this.x && vert.y == this.y)
            {
                return true;
            }
            return false;
        }

    }
}
