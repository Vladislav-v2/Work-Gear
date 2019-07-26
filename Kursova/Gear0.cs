using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursova
{
     class Gear0
    {
        public Point center;

        protected double angle = 0;
        protected int width;
        public double Angle { get { return angle; } } // Свойство колеса, позволяет узнать (но не изменить) его поворот.
       
        // КОНСТРУКТОРЫ:
        public Gear0(Point c, // Точка центра колеса (координаты в пикселях).
                      int width)
        {// Так как создается новое колесо, увеличим значение счетчика колес на 1:      
            
            // Заполним поля объекта в соответствии с заданным модулем и числом зубьев,   
            center = c;
        }

        virtual public void Draw(Pen pen, Graphics graph)
        { 
            // Рисуем окружность с указателем угла поворота колеса:
            //graph.FillRectangle(pen.Brush, center.X - 10, center.Y - 50, 20, 100);
        }

        public void Show(Form1 form) { Draw(form.pen1, form.graph); } // Отображение, это рисование пером №1
        public void Hide(Form1 form) { Draw(form.pen2, form.graph); } // Стирание, это рисование пером №2
    }
}
