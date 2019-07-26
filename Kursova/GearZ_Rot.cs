using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kursova
{
    class GearZ_Rot : GearZ
    {
        private Timer timer1;

        public GearZ_Rot(Point c, int width, double m1, Form1 form1, double speed   // Скорость вращения (об/сек) 
            ) : base(1, Side.outside, c, width, m1, form1)
        {
            timer1 = new Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = (int)(30 / speed);
                //Convert.ToInt32(28.33 / speed);
            timer1.Enabled = true;
        }

        override public void Draw(Pen pen, Graphics graph)
        { // Переопределяем метод рисования с учетом, выбрано колесо или нет: 
          // 1. Задаем цвет центра в соответствии с тем, выбрано данное колесо мышкой, или нет:
            //graph.FillRectangle(defPen.Brush, center.X - 10, center.Y - width / 2, 20, width);

            Pen pen0 = new Pen(new SolidBrush(pen.Color)); pen0.Width = 2;
            if (selected && pen == form.pen1)
                pen0.Color = form.pen3.Color;
            pen.Width = 2;
            // 2. Рисуем окружность с указателем угла поворота колеса:
            graph.DrawRectangle(pen, center.X - 10, center.Y - width / 2, 20, width);
            // 3. Рисуем перекрестие в центре колеса:   
            graph.DrawLine(pen0, center.X - 10, center.Y, center.X + 10, center.Y);
            graph.DrawLine(pen0, center.X, center.Y - 15, center.X, center.Y + 15);

            graph.DrawRectangle(pen, center.X + 10, center.Y - 10, 20, 20);
            graph.DrawRectangle(pen, center.X + 30, center.Y - 25, 40, 50);

            if (angle >= 180) angle = 1;
            if (angle <=0) angle = 179;
            ang = (float)(angle * width / 180.0);

            graph.DrawLine(pen, center.X - 10, center.Y - width / 2 + ang, center.X + 10, center.Y - width / 2 + ang);

        }
        private void timer1_Tick(object sender, EventArgs e)
        {// (очередной шаг) 
            Rotate("", 6); // Повернуть колесо на 6 градусов
        }
    }
}
