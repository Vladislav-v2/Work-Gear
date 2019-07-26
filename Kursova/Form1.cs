using System;
using System.Drawing;
using System.Windows.Forms;

namespace Kursova
{
    public partial class Form1 : Form
    {
        public Graphics graph; // поверхность диалоговой формы, на которой рисуются детали
        public Pen pen1, pen2, pen3; // перья разных цветов для рисования, 
        Gear0 g1;
        Val vall;
        int q = 70;

        private void button1_Click(object sender, EventArgs e)
        {
            Point p1 = new Point(q, 100);
            q += 15;
            g1 = new GearZ(q + 1, Side.outside, p1, q/2, 1.5, this);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Point p1 = new Point(q, 100);
            g1 = new GearZ_Rot(p1, q, 1.5, this, 1);
            q += 70;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Point p1 = new Point(200, q + 35);
            q += 30;
            vall = new ValMove(p1, 150, this);
        }

        // в т.ч. pen2 для стирания линий, имеет цвет фона.
        public int scale = 5; // масштаб рисования: к-во пикселей на 1 мм реального размера детали  

        public Form1()
        {   
            InitializeComponent();

            graph = CreateGraphics(); //  новая поверхность на форме для рисования деталей
            pen1 = new Pen(Color.Black);    // перо для рисования контура детали
            pen2 = new Pen(DefaultBackColor);  // перо цвета фона для стирания линий 
                                                          //      детали перед ее перемещением
            pen3 = new Pen(Color.Green);  // перо для выделения отдельных частей детали
        }
    }
}
