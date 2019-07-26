using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kursova
{
     class Gear1 : Gear0
    {
        // Дополнительные параметры и свойства отдельного колеса:
        protected bool selected; // истина, когда колесо выбрано мышкой.
        protected Form1 form; // Форма для отображения 
        protected float ang;
        protected Pen defPen;
         
        public Gear1(Point c, int width, Form1 form1) : base(c, width)
        {
            selected = false;
            form = form1;
            ang = (float)(angle * width / 180.0);//текущий угол поворота детали
            defPen = new Pen(form.BackColor);
            form.MouseDown += new MouseEventHandler(MousDownHandler);
            form.MouseUp += new MouseEventHandler(MousUpHandler);
            form.MouseMove += new MouseEventHandler(MousMoveHandler);
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

            if (angle >= 180) angle = 1;
            if (angle <= 0) angle = 179;
            ang = (float)(angle * width / 180.0);

            graph.DrawLine(pen, center.X - 10, center.Y - width / 2 + ang, center.X + 10, center.Y - width / 2 + ang);

        }

        virtual protected void MousDownHandler(object sender, MouseEventArgs e)
        { // Если при получении сообщения о НАЖАТИИ любой кнопки мышки 
          //   курсор не далее чем в 10 пикселях от центра колеса,
          //   то колесо отмечается, как выбранное:
            if ((Math.Abs(center.X - e.X) < 10) && (Math.Abs(center.Y - e.Y) < 10))
                selected = true;
        }

        virtual protected void MousUpHandler(object sender, MouseEventArgs e)
        { // При получении сообщения об ОТПУСКАНИИ любой кнопки мышки 
          // отметка выбора снимается:
            selected = false;
        }

        virtual protected void MousMoveHandler(object sender, MouseEventArgs e)
        { // При получении сообщения о ПЕРЕМЕЩЕНИИ курсора 
          // отмеченное колесо перемещается вслед за ним:
            if (selected)
            {   // Стираем изображение отмеченного колеса и перемещаем его центр в позицию курсора
                Hide((Form1)sender);
                center.X = e.X;
                center.Y = e.Y;
                
                // ******************  Добавлен вызов функции-ретранслятора *******************
                RetransGear(this); // для рассылки сообщения о перемещении
                                   // ****************************************************************************
            }
            // Отображаем колесо независимо от того, было оно отмечено, или нет. 
            // Отображать следует все колеса, иначе двигающееся колесо может стереть все остальные.
            Show((Form1)sender);
        }

        static public event Move_handler OnGearMove; // Для сообщения о перемещении   

        // Каждая деталь при своем перемещении будет вызывать эту функцию,
        // чтобы сообщить всем о своем перемещении: 
        static public void RetransGear(
                         object MovedGear // Деталь, которая переместилась 
                               )
        {
            OnGearMove?.Invoke(MovedGear);
        }
    }
}