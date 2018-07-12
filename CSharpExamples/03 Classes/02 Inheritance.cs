using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CSharpExamples.Classes.ClasseExamples;         // using static позволяет получить к статичным методам и типам, объявленным в классе

namespace CSharpExamples.Classes
{
    /// <summary>
    /// Наследование.
    /// 
    /// </summary>
    class InheritanceExamples
    {
        public static void ShowExample()
        {
            Calendar cal = new Calendar();

            #region Проверка календаря по умолчанию
            ShowDayPeriod(cal, new DateTime(2017, 02, 13), 14);

            Log(); Console.ReadKey();
            #region Добавляем выходной день
            Log("Добавляем выходной день");
            cal.AddHoliday(new DateTime(2017, 02, 23));
            ShowDayPeriod(cal, new DateTime(2017, 02, 13), 14);
            #endregion

            Log(); Console.ReadKey();
            #region Добавляем рабочий день
            Log("Добавляем рабочий день");
            cal.RemoveHoliday(new DateTime(2017, 02, 26));
            ShowDayPeriod(cal, new DateTime(2017, 02, 13), 14);
            #endregion

            Log(); Console.ReadKey();
            #region Добавляем ошибки
            Log("Добавляем ошибки");
            cal.RemoveHoliday(new DateTime(2017, 02, 26));
            ShowDayPeriod(cal, new DateTime(2017, 02, 13), 14);
            #endregion
            #endregion

            #region Полиморфизм методов
            Calendar cal1 = new Calendar();
            Calendar cal2 = new USCalendar();

            Log(cal1.Name);
            Log(cal2.Name);
            Log(((USCalendar)cal2).Name);

            #endregion

            #region Скрытие метода
            ExA exref = new ExA();
            Log($"ExA.ShowMessage: {exref.ShowMessage()}");     // -> ExA
            exref = new ExB();
            Log($"ExB.ShowMessage: {exref.ShowMessage()}");     // -> ExB
            exref = new ExC();
            Log($"ExC.ShowMessage: {exref.ShowMessage()}");     // -> ExA
            exref = new ExD();
            Log($"ExD.ShowMessage: {exref.ShowMessage()}");     // -> ExA

            ExC exref2 = (ExC)exref;
            Log($"ExD.ShowMessage: {exref2.ShowMessage()}");     // -> ExD
            #endregion

            //функция вывода в консоль
            void Log(string inMes = "")
            {
                Console.WriteLine(inMes);
            }
        }

        #region Класс-наследник SimpleClass
        /* Наследование указывается после ":"
         *     class <Имя класс> : <Родительский класс> {, <Интерфейс>}
         * Класс может быть унаследован только от одного класса. 
         * Если явно не указан класс-родитель, то наследуется от Object. 
         * В .NET все классы являются наследниками Object (напрямую или по иерархии).
         * Класс может реализовывать несколько интерфейсов.
         * К объекту класса можно ссылаться с помощью переменной любого класса выше по иерархии 
         * или с помощью переменной любого интерфейса, который этот класс реализует
         */
        class ComplexClass : SimpleClass
        {
            protected ComplexClass(string SecondName) 
                : base(SecondName)      // вызов конструктора базового класса
            {
            }

            protected override void DoSomethingSpecific()
            {
                base.DoSomethingSpecific();     // вызов реализации базового класса
                // собственная реализация
            }
        }
        #endregion

        #region Вывод периода дат с их статусом
        /// <summary>
        /// Вывести набор дат, начиная с указанной в inDt на период Period
        /// </summary>
        /// <param name="cal">Используемый календарь</param>
        /// <param name="inDt">Дата начала</param>
        /// <param name="Period">Длительность периода</param>
        private static void ShowDayPeriod(Calendar cal, DateTime inDt, int Period)
        {
            DateTime dtend = inDt.AddDays(Period);
            ShowDayPeriod(cal, inDt, dtend);
        }

        /// <summary>
        /// Вывод набора дат на указанном периоде
        /// </summary>
        /// <param name="cal">Календарь</param>
        /// <param name="inDt">Дата начала</param>
        /// <param name="inDtEnd">Дата окончания</param>
        private static void ShowDayPeriod(Calendar cal, DateTime inDt, DateTime inDtEnd)
        {
            DateTime dt = inDt;
            DateTime dtend = inDtEnd;
            while (dt < dtend)
            {
                string daytype = cal.IsHoliday(dt) ? "выходной день" : "будень";
                Console.WriteLine($"{dt.ToString("dd.MM.yyyy")} ({dt.DayOfWeek}) - {daytype}");
                dt = dt.AddDays(1);
            }
        }
        #endregion

        /// <summary>
        /// Реализация производного календаря на базе уже существующего
        /// </summary>
        class USCalendar : Calendar
        {
            public USCalendar() : base(new List<DayOfWeek>() { DayOfWeek.Friday })
            {

            }

            private string calendarName = "Календарь США";
            public override string Name { get { return calendarName; } }

            /*  Меняем стандартные выходные на пятницу-воскресенье
                Добавляем иные выходные в конструкторе
                Изменяем имя календаря

            */
            //public USCalendar():base()
            //{
            //    // Добавляем стандартные выходные для этого календаря
            //    // 4 июля
            //}

            /* переопределить стандартный метод isHoliday (сделать его virtual) */

            public void ShowAllHolidays(DateTime fromDate, DateTime toDate)
            {
                Console.WriteLine($"Календарь {this.Name}");
                foreach (DateTime dt in this.addedHolidays)
                {
                    Console.WriteLine($"{dt.ToString("dd.MM.yyyy")} ({dt.DayOfWeek}) - выходной");
                }
            }
        }

        #region Закрытие метода
        /* Небольшая иерархия: 
         * А -> B & C
         * C -> D         
         */
        class ExA
        {
            public virtual string ShowMessage()
            {
                return "ExA";
            }
        }

        class ExB: ExA
        {
            public override string ShowMessage()
            {
                return "ExB";
            }
        }

        class ExC: ExA
        {
            public new virtual string ShowMessage()     // new в данном месте разрывает цепочку перегрузки оригинального метода ExA.ShowMessage
            {                                           // и начинает новую, с первой функцией в классе ExC
                return "ExC";
            }
        }

        class ExD: ExC
        {
            public override string ShowMessage()
            {
                return "ExD";
            }
        }
        #endregion

    }
}
