using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace Kursova
{
    public class ValMove: Val, Rot_obj
    {
        // Дополнительные параметры и свойства отдельного колеса:
        bool selected; // истина, когда колесо выбрано мышкой.
        protected Form1 form; // Форма для отображения 
        public ArrayList Connected_center = new ArrayList(0);
        // Список деталей, непосредственно присоединенных к центру
        private GearZ g1;
        Pen pen0;
        public ValMove(Point c, int height, Form1 form1):base(c, height)
        {
            selected = false;
            form = form1;

            form.MouseDown += new MouseEventHandler(MousDownHandler);
            form.MouseUp += new MouseEventHandler(MousUpHandler);
            form.MouseMove += new MouseEventHandler(MousMoveHandler);
            Gear1.OnGearMove += new Move_handler(On_Move);
           // pen0.Width = 2;
        }

        override public void Draw(Pen pen, Graphics graph)
        { // Переопределяем метод рисования с учетом, выбрано колесо или нет: 
          // 1. Задаем цвет центра в соответствии с тем, выбрано данное колесо мышкой, или нет:
            pen0 = new Pen(pen.Color);            
            if (selected && pen == form.pen1)
                pen0.Color = form.pen3.Color;

            graph.DrawRectangle(pen, center.X - height / 2, center.Y - 10, height, 20);

            graph.DrawLine(pen0, center.X - 10, center.Y, center.X + 10, center.Y);
            graph.DrawLine(pen0, center.X, center.Y - 8, center.X, center.Y + 8);

        }

        virtual protected void MousDownHandler(object sender, MouseEventArgs e)
        { // Если при получении сообщения о НАЖАТИИ любой кнопки мышки 
          //   курсор не далее чем в 10 пикселях от центра колеса,
          //   то колесо отмечается, как выбранное:
            if ((Math.Abs(center.X - e.X) < 10) && (Math.Abs(center.Y - e.Y) < 10))
                selected = true;
        }
        virtual protected void MousUpHandler  (object sender, MouseEventArgs e)
        { // При получении сообщения об ОТПУСКАНИИ любой кнопки мышки 
          // отметка выбора снимается:
            selected = false;
        }
        virtual protected void MousMoveHandler(object sender, MouseEventArgs e)
        { // При получении сообщения о ПЕРЕМЕЩЕНИИ курсора 
          // отмеченное вал перемещается вслед за ним:
            if (selected)
            {   // Стираем изображение отмеченного колеса и перемещаем его центр в позицию курсора
                Hide((Form1)sender);
                center.X = e.X;
                center.Y = e.Y;
                Gear1.OnGearMove += new Move_handler(On_Move);
                //RetransGear(this);
                // ******************  Добавлен вызов функции-ретранслятора *******************                
            }
            // Отображаем колесо независимо от того, было оно отмечено, или нет. 
            // Отображать следует все колеса, иначе двигающееся колесо может стереть все остальные.
            Show((Form1)sender);
        }

        public event Rot_handler Rotation; // Для сообщения о своем повороте

        void Send_rotation(string from, double ang_z, double ang_g)
        // Вызывается, чтобы сообщить всем о своем повороте
        {
            Rotation?.Invoke(from, ang_z, ang_g, Side.inside);
        }

       public void Rotation_from(string from, double ang_z, double ang_g, Side a_side)
        // Вызывается при получении сообщения о повороте ведущего колеса
        {
            Rotate(from, ang_g);
        }

        public bool Connect_to(Rot_obj A)
        {

                if (A is GearZ)
                {  
                    A.Rotation += Rotation_from;
                    return true;
                }
                return false;

        }

        public void Rotate(string from, double ang)
        {
            Send_rotation(from, 0, -6);
        }

        public void DisConnect_from(Rot_obj A)
        {
            A.Rotation -= Rotation_from;
        }

        private void On_Move(object mg)
        {

            if (mg is GearZ)
            {
                if ((((GearZ)mg).center.X+10 >= center.X - height / 2
                && (((GearZ)mg).center.X-10 <= center.X + height / 2))
                && (Math.Abs(((GearZ)mg).center.Y - center.Y) < 5))
                    {
                    if (!Connected_center.Contains(mg))
                    {
                        Connect_to((Rot_obj)mg);
                        Connected_center.Add(mg);
                        ((GearZ)mg).Connect_to(this);
                        ((GearZ)mg).Connected_els.Add(this);
                        g1 = ((GearZ)mg);
                    }                  
                }
                else if (Connected_center.Contains((GearZ)mg))
                {
                    DisConnect_from((GearZ)mg);
                    Connected_center.Remove((GearZ)mg);
                    ((GearZ)mg).DisConnect_from(this);
                    ((GearZ)mg).Connected_els.Remove(this);
                }
            }
        }
    }

}
