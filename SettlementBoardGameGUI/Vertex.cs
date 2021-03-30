using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SettlementBoardGameGUI
{
    class Vertex
    {
        public Vertex(double x, double y)
        {
            this.x = Math.Round(x, 3);
            this.y = Math.Round(y, 3);
            this.connectedEdges = new List<Edge>();
        }

        public double x { get; private set; }
        public double y { get; private set; }

        public List<Edge> connectedEdges { get; protected set; }
        public int ownedBy { get; set; }
        public ResourceType portType { get; set; }
        public int settlementSize { get; set; }

        public bool matches(Vertex vert)
        {
            if(vert.x == this.x && vert.y == this.y)
            {
                return true;
            }
            return false;
        }
        public Edge getConnectedEdge(Vertex point)
        {
            return this.connectedEdges.Where(edge => (edge.point0.x == point.x && edge.point0.y == point.y) || (edge.point1.x == point.x && edge.point1.y == point.y)).FirstOrDefault();
        }

    }
}
