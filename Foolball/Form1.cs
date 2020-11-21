using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Foolball
{
    public partial class Form1 : Form
    {
        private Timer timer1 = new Timer();

        int pX, pY, radius = 50, angle;
        int deltaX = 1, deltaY = 1;

        public Form1()
        {
            InitializeComponent();

            timer1.Interval = 30;
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Enabled = true;
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Paint += new PaintEventHandler(Form1_Paint);
        }


        void timer1_Tick(object sender, EventArgs e)
        {
            if (pX - radius <= 0)
                deltaX = 1;
            if (pX + radius >= ClientRectangle.Width)
                deltaX = -1;

            pX += deltaX;


            if (pY - radius <= 0)
                deltaY = 1;
            if (pY + radius >= ClientRectangle.Height)
                deltaY = -1;

            
            pY += deltaY;


            Invalidate();
        }


        /// <summary>
        /// 
        /// </summary>
        private void drawCircle(Graphics g)
        {
            
            Pen myP = new Pen(Color.Green, 3);
            SolidBrush mySB = new SolidBrush(Color.DarkSeaGreen);
            g.DrawEllipse(myP, pX, pY, radius, radius);
            g.FillEllipse(mySB, pX, pY, radius, radius);    
        }

        
        void Form1_Paint(object sender, PaintEventArgs e)
        {
            drawCircle(e.Graphics);

        }
    }
}