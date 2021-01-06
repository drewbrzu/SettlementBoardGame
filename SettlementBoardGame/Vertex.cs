using System;
using System.Collections.Generic;
using System.Text;

namespace SettlementBoardGame
{
    class Vertex
    {
        public Vertex(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int x { get; private set; }
        public int y { get; private set; }
        public int ownedBy { get; set; }


    }
}
