using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpExamples.Types
{
    /// <summary>
    /// Ссылочные типы.
    /// Значения создаются в куче. Ссылка на значение в куче распалагаются в стеке. 
    /// При передаче в функцию передается копия указателя. 
    /// Изменения объекта по ссылке отражаются для всех копий указателя - работа происходит с единственным объектом в куче.
    /// </summary>
    class ReferenceTypeExamples
    {
        public static void ShowExample()
        {
            object obj = null;  // Object - класс-родитель для всех классов в .NET
                                // любой объект в .NET может быть приведен к object
            int i1 = 123;
            obj = i1;
            // но в обратную сторону приведение может вызывыать ошибки
            //float fl1 = (float)obj;     // вызывает ошибку
            int i2 = (int)obj;

            // Объявление переменных ссылочного типа.
            UserInfo ui = null;
            ui = new UserInfo();
            ui.Name = "Alexander";
            ui.SecondName = "Pushkin";

            UserInfo ui2 = new UserInfo() { Name = "Sergey", SecondName = "Esenin" };

            #region Приведение типов
            object obj2 = ui2;
            UserInfo ui3 = obj2 as UserInfo;        // right
            ui3 = (UserInfo)obj2;                   // wrong

            RoomInfo ri1 = new RoomInfo() { Name = "Sikorsky", Floor = 2 };
            obj = ri1;
            //ui3 = (UserInfo)obj;          // здесь произойдет ошибка приведения (Exception)
            ui3 = obj as UserInfo;          // Ошибка не произойдет, но значение не может быть приведено, поэтому NULL
            if (ui3 is null)        // ui3 == null - может быть перегружен оператор ==, и тогда предсказать результат будет весьма трудно
                Console.WriteLine("Ошибка приведения объекта типа RoomInfo к типу UserInfo");

            if (obj is RoomInfo)
                Console.WriteLine("В переменной obj содержится значение типа RoomInfo");
            else if (obj is UserInfo)
                Console.WriteLine("В переменной obj содержится значение типа UserInfo");

            #endregion

            #region Хранение данных в куче
            Console.WriteLine();
            Console.WriteLine("Хранение значений ссылочных типов в куче");
            ui3 = ui;           // ui = Alexander Pushkin
            Console.WriteLine($"ui: {ui.Name} {ui.SecondName}");        // ui: Alexander Pushkin
            Console.WriteLine($"ui3: {ui.Name} {ui.SecondName}");       // ui3: Alexander Pushkin

            ui3.Name = "Aleksander";
            Console.WriteLine($"ui: {ui.Name} {ui.SecondName}");        // ui: Aleksander Pushkin
            Console.WriteLine($"ui3: {ui.Name} {ui.SecondName}");       // ui3: Aleksander Pushkin

            Console.WriteLine();
            Console.WriteLine("Изменение значения ссылочного типа в функции");
            Console.WriteLine($"ui: {ui.Name} {ui.SecondName}");        // ui: Aleksander Pushkin
            ChangeRT(ui);
            Console.WriteLine($"ui: {ui.Name} {ui.SecondName}");        // ui: Aleksander Pushkin

            #endregion
        }

        /// <summary>
        /// Небольшой вспомогательный класс
        /// </summary>
        class UserInfo
        {
            public string Name;
            public string SecondName;
        }

        class RoomInfo
        {
            public string Name;
            public int Floor;
        }

        static void ChangeRT(object inObj)
        {
            Console.WriteLine("Вызов функции ChangeRT");
            UserInfo ui = inObj as UserInfo;
            if (ui != null)
            {
                Console.WriteLine("UI.Name : " + ui.Name);
                ui.Name = "new_" + ui.Name;
                Console.WriteLine("new UI.Name : " + ui.Name);
            }
        }
    }
}