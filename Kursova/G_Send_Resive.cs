using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursova
{
    public enum Side
    {
        inside, // Зубчатое колесо с внутренним зацеплением
        outside, // Зубчатое колесо с внешним зацеплением
        shaft, // Вал
        motor  // Мотор
    };

    public delegate void Move_handler // Обработчик перемещения детали
    (object Moved // Переместившаяся деталь
    );

    public delegate void Rot_handler // Обработчик поворота ведущей детали
    (
      string from,  // Список имен ведущих деталей (для самопроверки).
      double ang_z, // Когда ведущая деталь - колесо, 
                    //       то на сколько зубьев оно повернулось.  
      double ang_g, // Каков текущий угол поворота ведущей детали.
      Side a_side   // Каков тип ведущей детали. 
    );

    //=====================================================================================
    public interface Rot_obj
    { // Все то, что должен уметь делать    
        void Rotate(string from, double ang); // Повернуться на угол ang, 
        // с учетом списка имен движителей from. 
        // Список from нужен для проверки имени источников вращения, 
        // без него может возникнуть "эхо" сообщений о повороте.  
        event Rot_handler Rotation; // Событие: сообщить о своем повороте.
        bool Connect_to(Rot_obj A);      // Подключиться к движителю A.
        void DisConnect_from(Rot_obj A); // Отключиться от движителя A.
    }
}
