using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kursova
{
      class GearZ : Gear1, Rot_obj
    {
        public ArrayList Connected_els = new ArrayList(0);
        // Список деталей, непосредственно присоединенных к данной детали

        public ArrayList Connected_center = new ArrayList(0);
        // Список деталей, непосредственно присоединенных к центру

        protected int ord_num;  // Порядковый номер детали в механизме
        protected int num_z;    // Число зубьев
        protected double m;     // Модуль зуба
        protected Side side_z;  // Признак зацепления (Внутреннее/Наружное) 
        public GearZ(int ord, // Порядковый номер колеса в механизме
                              Side s, // Вид зацепления (Внутреннее/Наружное)
                             Point c, // Точка центра колеса
                             int width, // диаметр колеса
                            double m1, // Модуль зуба для расчета количества зубов колеса
                          Form1 form1 // Форма для отображения 
                            )
                    : base(c, width,
                            form1) // Вызывается родительский конструктор)
        {
            ord_num = ord; num_z = (int)(width * 2 / m1); side_z = s; m = m1;
            OnGearMove += new Move_handler(On_Move);
            this.width = width;
            // Сразу подключаемся к функции-ретранслятору сообщений (в форме)
        }

        public string Name
        {
            get { return "<g" + ord_num.ToString() + '>'; }
        }

        public event Rot_handler Rotation; // Для сообщения о своем повороте

        void Send_rotation(string from, double ang_z, double ang_g)
        // Вызывается, чтобы сообщить всем о своем повороте
        {
            Rotation?.Invoke(from, ang_z, ang_g, side_z);
        }

        public bool Connect_to(Rot_obj A)
        // Вызывется колесом А для сцепления с данным колесом, 
        // когда они при перемещении соприкоснутся
        {
            if (A is GearZ)
            {  // При подключении колеса к колесу проверить направление зубьев
                if ((((GearZ)A).m != m)
                     ||
                    ((((GearZ)A).side_z == Side.inside) && (this.side_z == Side.inside)
                    || (((GearZ)A).side_z == Side.inside) && (this.num_z > ((GearZ)A).num_z)
                    || (this.side_z == Side.inside) && (((GearZ)A).num_z > this.num_z)
                    )
                   )
                {  //Console.WriteLine("Connection {0} to {1} is unpossible! ",ord_num,A.ord_num );
                    return false;
                }
                A.Rotation += Rotation_from;
                return true;
            }
            if (A is ValMove) 
            {
                A.Rotation += Rotation_from_center;
                return true;
            }
            return false;
        }

        public void DisConnect_from(Rot_obj A)
        // Вызовется колесом А для расцепления с данным колесом, 
        // когда оно при перемещении отошло от него
        {
            if (A is GearZ)
            {
                A.Rotation -= Rotation_from;
            }else
                A.Rotation -= Rotation_from_center;         
        }

        void Rotation_from_center(string from, double ang_z, double ang_g, Side a_side)
        // Вызывается при получении сообщения о повороте ведущего колеса
        {

            if (from.IndexOf(Name) == -1)
            {              
                Rotate(from, ang_g);
            }
        }

        void Rotation_from(string from, double ang_z, double ang_g, Side a_side)
        // Вызывается при получении сообщения о повороте ведущего колеса
        {

            if ((a_side == Side.inside || a_side == Side.outside)
                  && (from.IndexOf(Name) == -1))
            {
                if (a_side == side_z) ang_z = -ang_z;
                Rotate(from, ang_z * 360.0 / num_z);
            }

        }

        public void Rotate(string from, double ang)
        {
            Hide(form);
            angle += ang;
            Show(form);
            Send_rotation(from + Name, num_z * ang / 360.0, angle);
        }


        private void On_Move(object mg)
        {
            if (mg is GearZ)
            {
                bool a2 = (((GearZ)mg).center.X+10 >= center.X - 10) 
                       && (((GearZ)mg).center.X-10 <= center.X + 10);

                bool b2 = Math.Abs(((center.Y + width / 2) - (((GearZ)mg).center.Y - ((GearZ)mg).width / 2))) < 5
                       || Math.Abs(((center.Y - width / 2) - (((GearZ)mg).center.Y + ((GearZ)mg).width / 2))) < 5; 

                {
                    if ( a2 && b2)
                    {
                        if (!Connected_els.Contains(mg) && this.Connect_to((Rot_obj)mg))
                        {
                            Connected_els.Add(mg);
                            ((GearZ)mg).Connect_to(this);
                            ((GearZ)mg).Connected_els.Add(this);
                        }

                    }
                    else if (Connected_els.Contains((GearZ)mg))
                    {
                        DisConnect_from((Rot_obj)mg);
                        Connected_els.Remove(mg);
                        ((Rot_obj)mg).DisConnect_from(this);
                        ((GearZ)mg).Connected_els.Remove(this);
                    }
                }
            } 
        }
    }
}
