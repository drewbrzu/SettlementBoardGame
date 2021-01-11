using System;
using System.Drawing;
using System.Windows.Forms;

namespace SettlementBoardGame
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            panel1.BackColor = Color.White;
            panel1.Paint += new PaintEventHandler(panel1_Paint);
        }
        
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // https://stackoverflow.com/questions/5507605/c-sharp-drawing-on-panels
            // https://web.archive.org/web/20120330003635/http://bobpowell.net/picturebox.htm

            // https://swharden.com/CsharpDataVis/

            var p = sender as Panel;
            var g = e.Graphics;

            //g.FillRectangle(new SolidBrush(Color.FromArgb(0, Color.Black)), p.DisplayRectangle);

            Point[] points = new Point[4];

            points[0] = new Point(0, 0);
            points[1] = new Point(0, p.Height);
            points[2] = new Point(p.Width, p.Height);
            points[3] = new Point(p.Width, 0);

            Brush brush = new SolidBrush(Color.Aqua);

            g.FillPolygon(brush, points);
            // ~~~~~~~~~~~~~~~~~~~~~~~~~End of code from tutorial ~~~~~~~~~~~~~~~~~~~~~~

            double centerpointX = p.Width / 2;
            double centerpointY = p.Height / 2;
            double edgeLength = 100;
            Point[] hex = new Point[7];

            double x, y;
            double sine60 = Math.Sin((60.0 / 180.0) * Math.PI);
            double cosine60 = Math.Cos((60.0 / 180.0) * Math.PI);

            // Create top left point from center of tile
            x = centerpointX - edgeLength * sine60;
            y = centerpointY - edgeLength / 2;
            hex[0] = new Point((int)x, (int)y);

            // Next top center point
            x = centerpointX;
            y = centerpointY - (edgeLength * cosine60 + (edgeLength / 2));
            hex[1] = new Point((int)x, (int)y);

            // Next top right point
            x = centerpointX + edgeLength * sine60;
            y = centerpointY - edgeLength / 2;
            hex[2] = new Point((int)x, (int)y);

            // Next bottom right point
            x = centerpointX + edgeLength * sine60;
            y = centerpointY + edgeLength / 2;
            hex[3] = new Point((int)x, (int)y);

            // Next bottom center point
            x = centerpointX;
            y = centerpointY + (edgeLength * cosine60 + (edgeLength / 2));
            hex[4] = new Point((int)x, (int)y);

            // Next bottom left point
            x = centerpointX - (edgeLength * sine60);
            y = centerpointY + (edgeLength / 2);
            hex[5] = new Point((int)x, (int)y);

            hex[6] = new Point(hex[0].X, hex[0].Y);

            Brush b = new SolidBrush(Color.Green);
            g.FillPolygon(b, hex);
            
            
            Brush c = new SolidBrush(Color.White);
            double circleDiameter = 20;
            double circleRadius = circleDiameter / 2.0;
            g.FillEllipse(c, new Rectangle((int)(hex[0].X - circleRadius), (int)(hex[0].Y - circleDiameter / 2.0), (int)(circleDiameter), (int)(circleDiameter)));


        }

        private void draw_tile(Graphics g, Point[] points)
        {
            Brush c = new SolidBrush(Color.White);
            Brush b = new SolidBrush(Color.Aqua);
            g.FillPolygon(b, points);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel1.Refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }
    }
}
