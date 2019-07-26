using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Kursova
{
    public class Val
    {
        protected Point center;
        protected int height;

        public Val(Point c, int heightVal)
        {
            center = c;
            height = heightVal;
        }

        virtual public void Draw(Pen pen, Graphics graph)
        {
            graph.DrawRectangle(pen, center.X - height / 2, center.Y-10, height, 20);
        }

        public void Show(Form1 form) { Draw(form.pen1, form.graph); } // Отображение, это рисование пером №1
        public void Hide(Form1 form) { Draw(form.pen2, form.graph); } // Стирание, это рисование пером №2
    }
}
