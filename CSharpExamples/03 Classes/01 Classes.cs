using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpExamples.Classes
{
    /// <summary>
    /// Классы.
    /// Публичные, Защищенные, Приватные, Видимые внутри проекта.
    /// Абстрактные классы.
    /// Абстрактные методы.
    /// Интерфейсы.
    /// Виртуальные функции.
    /// Перегрузка функций.
    /// Наследование.
    /// Поля и Свойства.
    /// </summary>
    class ClasseExamples
    {
        public static void ShowExample()
        {
            Console.WriteLine("Пример различных классов\n\n");

            #region Пример с SimpleClass
            SimpleClass sc = null;
            if (sc is null)                                     // Проверка переменной ссылочного типа на null может быть проведена 
                Console.WriteLine("sc is null");                // двумя выражениями (obj is null) и (obj == null)
            if (sc == null)                                     // Но надо учитывать, что в классе переменной obj может быть переопределен
                Console.WriteLine("sc == null");                // оператор == так, что результат может быть совсем непредсказуемым
            else                                                // Выражение (obj is null) работает с ссылкой и однозначно определяет,
                Console.WriteLine("sc != null");                // что ссылка ведет в "пустоту"
            #endregion

            MyDate[] mdarr = new MyDate[10];
            Array.Sort(mdarr);
            
            //Создание объекта класса без дефолтного конструктора
            WithStaticConstructor wsc = new WithStaticConstructor("Empty");

            Console.WriteLine("Создан объект типа WithStaticConstructor");

            wsc = new WithStaticConstructor("Empty");

            Console.WriteLine("Создан объект типа WithStaticConstructor");
        }

        #region Simple class
        public class SimpleClass
        {
            #region Fields
            internal string _name;
            string _secName;
            int _age;
            DateTime _lastUpdate;
            readonly long ReadOnlyLong;
            #endregion

            #region Properties
            public string Name { get; set; }    // эта переменная никак не будет связана с локальным полем _name
            public string SecondName { get; private set; }  // установка значения доступна только из этого класса, аналогично не связана с переменной _secName
            /* Когда используется автосоздание тел для GET и SET, создаются "теневые" переменные, которые хранят значение, но к которым нет прямого доступа
             */
            public int GetOnlyInt { get; }
            public int Age => _age;            // Это свойство. которое доступно только для чтения (возвращает результат из поля _age)
            /* Эквивалентно:
             * public int Age
             * { get { _age += 1; return _age; }}
             */
            public DateTime LastUpdate          // obj.LastUpdate = DateTime.Now;       // -> value
            {
                set         // В сеттере есть неявнообъявленная переменная value, в которой содержится устанавливаемое значение
                {
                    //if (value .Equals.Equals.)

                    _lastUpdate = value;
                }
            }
            #endregion

            #region Methods
            public void DoSomethingForAll()
            {
                //GetOnlyInt = 3;          // Присвоение значений READONLY полям и GET-only свойствам в методах класса невозможно
                //ReadOnlyLong = 4l;

                // Публичные методы доступны для всех и формируют API класса
            }

            protected void DoSomethingForNested()
            {
                // protected методы доступны для всех наследников класса
            }

            internal void DoSomethingForEveryoneInAssembly()
            {
                // internal методы доступны для всех в рамках текущей сборки
                // protected internal - для всех в этой сборке и для всех наследниках (даже в других)
            }

            private void DoSomethingOnlyForMe()
            {
                // private методы доступны только в рамках этого класса
            }

            // Полиморфизм
            protected virtual void DoSomethingSpecific()
            {
                // virtual метод может быть переопределен в классах иерархии
            }


            //private virtual void DoSomethingSpecific2()
            //{
            //    // virtual и abstract методы не могут быть private
            //}
            #endregion

            #region Constructors

            protected SimpleClass(string SecondName)        // этот конструктор может быть вызван ниже по иерархии. но не снаружи
            {
                this.SecondName = SecondName;

                // Присвоение значений READONLY полям и GET-only свойствам возможно только в конструкторах
                GetOnlyInt = 5;
                ReadOnlyLong = 4l;
            }

            public SimpleClass(string Name, string SecName, int Age)
            {
                this.Name = Name;
                this._secName = SecName;
                //this.Age = Age;         // поле Age объявлено только для чтения
                this._age = Age;

                _name = "все, что угодно";  // нигде не используется
            }
            #endregion

            #region static методы
            // Статические методы, свойства и поля класса доступны без создания экземпляра.
            // Обращение к ним приосходит через имя класса: SimpleClass.ConvertToSimple
            // Используются для реализации какой-то логики, связанной с классом, но не требующей данных объекта
            // Например все методы T.TryParse или T.Parse являются static методами
            public static SimpleClass ConvertToSimple(object inObj)
            {
                if (inObj is SimpleClass)
                    return inObj as SimpleClass;
                return null;
            }
            #endregion

            #region Перегрузка операторов
            public static bool operator ==(SimpleClass obj1, SimpleClass obj2)
            {
                if (obj2 is null)               // инвертируем поведение конструкции ==
                    return !(obj1 is null);
                return object.ReferenceEquals(obj1, obj2);
            }

            // Обязательно требуется перегрузить в паре == и !=
            public static bool operator !=(SimpleClass obj1, SimpleClass obj2)
            {
                return !(obj1 == obj2);
            }
            #endregion
        }

        public class NotSimpleClass : SimpleClass
        {
            public NotSimpleClass() : base ("by default")
            {

            }

            protected override void DoSomethingSpecific()
            {
                base.DoSomethingSpecific();
            }

        }
        #endregion

        #region MyDate class

        internal class MyDate : IComparable
        {
            protected int ProtectedField;
            public static void SetZeroDate(DateTime inDate)
            {
                ZeroDate = inDate;
            }
            private static DateTime ZeroDate = new DateTime(1900, 01, 01);
            private DateTime dtObject;

            public MyDate()
            {
                dtObject = DateTime.Now.Date;
            }

            public MyDate(int year, int month, int day)
            {
                dtObject = new DateTime(year, month, day);
            }

            #region Интерфейс
            /// <summary>
            /// Дней с 01.01.1900
            /// </summary>
            public int TotalDays
            {
                get
                {
                    var vv = (dtObject - ZeroDate);
                    return (int)vv.TotalDays;
                }
            }

            public int Year { get { return dtObject.Year; } }

            public int Month { get { return dtObject.Month; } }

            public int Day { get { return dtObject.Day; } }
            #endregion

            #region ToString
            public override string ToString()
            {
                return ToString("yyyy-MM-dd");
                //return $"TotalDays.ToString()";
            }
            public string ToString(string format)
            {
                return dtObject.ToString(format);
            }
            #endregion

            #region Для поддержки использования в качестве ключей коллекций

            public override bool Equals(object obj)
            {
                MyDate objB = obj as MyDate;
                if (objB == null)
                    return false;
                return //base.Equals(obj);
                    this.TotalDays.Equals(objB.TotalDays);
            }

            //public static implicit operator == (Date d1, Date d2)
            //{
            //    return d1.Equals(d2);
            //}

            public override int GetHashCode()
            {
                return 0;
            }
            #endregion

            #region Неявное преобразование
            public DateTime asDateTime()
            {
                return dtObject;
            }

            // MyDate md = DateTime.Now;

            public static implicit operator DateTime(MyDate d)
            {
                return d.dtObject.AddDays(-3);
            }

            public static implicit operator MyDate(DateTime dt)
            {
                return new MyDate(dt.Year, dt.Month, dt.Day);
            }

            // int days = md;
            public static implicit operator int (MyDate md)
            {
                return md.TotalDays;
            }
            #endregion

            #region Сравнение
            /// <summary>
            /// Функция сравнения. Поддержка интерфейса IComparable
            /// < 0 => obj меньше this
            /// = 0 => obj совпадает с this
            /// > 0 => obj больше this
            /// </summary>z
            /// <param name="obj"></param>
            /// <returns></returns>
            public int CompareTo(object obj)
            {
                MyDate B = obj as MyDate;
                if (B == null)
                    throw new InvalidCastException("Не удалось привести к типу Date");
                return this.TotalDays.CompareTo(B.TotalDays);
            }

            #endregion
        }

        
        #endregion

        #region Calendar class
        /// <summary>
        /// Общее представление календаря
        /// </summary>
        internal class Calendar
        {
            public Calendar()
            {
                // Фиксируем стандартные выходные дни
                standardHolidays.Add(DayOfWeek.Sunday);
                standardHolidays.Add(DayOfWeek.Saturday);
            }

            public Calendar(List<DayOfWeek> inHolidayDays) : this()
            {
                foreach (var wd in inHolidayDays)
                {
                    standardHolidays.Add(wd);
                }
            }

            private string calendarName = "Календарь по умолчанию";
            /// <summary>
            /// Получить имя календаря
            /// </summary>
            public virtual string Name { get { return calendarName; } }

            /// <summary>
            /// Список добавленных дней выходных дней
            /// </summary>
            protected List<DateTime> addedHolidays = new List<DateTime>();
            /// <summary>
            /// Список удаленных выходных дней
            /// </summary>
            protected List<DateTime> removedHolidays = new List<DateTime>();
            /// <summary>
            /// Список дней недели, которые являются выходными по умолчанию (субботу + воскресенье)
            /// </summary>
            public List<DayOfWeek> standardHolidays = new List<DayOfWeek>();


            /// <summary>
            ///  Добавить выходной день (рабочий, который не является рабочим)
            /// </summary>
            /// <param name="inHoliday"></param>
            public void AddHoliday(DateTime inHoliday)
            {
                removedHolidays.Remove(inHoliday.Date);     // если не будет найдено значение, ничего не удалится, ошибки не будет
                this.addedHolidays.Add(inHoliday.Date);
            }
            /// <summary>
            /// Добавить рабочий выходной (выходной день, который является рабочим)
            /// </summary>
            /// <param name="inHoliday"></param>
            public void RemoveHoliday(DateTime inHoliday)
            {
                addedHolidays.Remove(inHoliday.Date);
                this.removedHolidays.Add(inHoliday.Date);
            }

            /// <summary>
            /// Проверка, является ли день выходным
            /// </summary>
            /// <param name="inHoliday"></param>
            /// <returns></returns>
            public bool IsHoliday(DateTime inHoliday)
            {
                /*  День является выходным, если это стандартный выходной день и не входит в список рабочих выходных дней       */
                DateTime date = inHoliday.Date;
                if ((standardHolidays.Contains(date.DayOfWeek))
                    && (!removedHolidays.Contains(date)))
                    return true;
                /* Либо если это рабочий день и входит в список выходных рабочих дней       */
                if ((!standardHolidays.Contains(date.DayOfWeek))
                    && (addedHolidays.Contains(date)))
                    return true;
                /* В противном случае - это рабочий день */
                return false;
            }

            /// <summary>
            /// Проверка, что дата - последний день месяца
            /// </summary>
            /// <param name="inDay"></param>
            /// <returns></returns>
            public bool isEndOfMonth(DateTime inDay)
            {
                return inDay.Date.Month != inDay.AddDays(1).Date.Month;
            }

            /// <summary>
            /// Отобразить все выходные дни календаря
            /// </summary>
            public void ShowAllHolidays(DateTime fromDate, DateTime toDate)
            {
                Console.WriteLine($"Выходные дни календаря {this.Name}");
                DateTime dt = fromDate;
                while (dt < toDate)
                {
                    if (IsHoliday(dt))
                        Console.WriteLine(dt.ToString("dd.MM.yyyy"));
                    dt = dt.AddDays(1);
                }
            }

            #region Переопределение операторов
            /// <summary>
            /// проверка равенства двух календарей
            /// </summary>
            /// <param name="cal1"></param>
            /// <param name="cal2"></param>
            /// <returns></returns>
            public static bool operator ==(Calendar cal1, Calendar cal2)
            {
                if (object.ReferenceEquals(cal1, cal2))
                    return true;
                if ((cal1 == null) || (cal2 == null))
                    return false;
                return (cal1.Name == cal2.Name);
            }
            /// <summary>
            /// проверка неравенства двух календарей
            /// </summary>
            /// <param name="cal1"></param>
            /// <param name="cal2"></param>
            /// <returns></returns>
            public static bool operator !=(Calendar cal1, Calendar cal2)        // Необходимо обязательно в паре переопределять == и !=
            {
                return !(cal1 == cal2);
            }

            /// <summary>
            /// Объединение календарей
            /// </summary>
            /// <param name="cal1"></param>
            /// <param name="cal2"></param>
            /// <returns></returns>
            public static Calendar operator +(Calendar cal1, Calendar cal2)
            {
                Calendar rescal = new Calendar();
                rescal.addedHolidays.AddRange(cal1.addedHolidays);
                rescal.removedHolidays.AddRange(cal1.removedHolidays);
                foreach (DateTime dt in cal2.addedHolidays)
                    rescal.AddHoliday(dt);
                foreach (DateTime dt in cal2.removedHolidays)
                    rescal.RemoveHoliday(dt);
                return rescal;
            }
            #endregion


        }
        #endregion

        #region Abstract class
        /// <summary>
        /// Абстрактные классы служат для реализации общей логики поведения иерархии классов
        /// Нельзя создать инстанс абстрактного класса, но можно использовать переменную типа абстрактного класса
        /// чтобы единообразно работать с объектами из иерархии. 
        /// Абстрактный класс - это интерфейс с реализацией
        /// </summary>
        abstract class BaseModule
        {
            protected string _name;
            public BaseModule(string inName)
            {
                _name = inName;
            }

            public List<string> GetParameters()
            {
                return null;    // Реализация общего функционала для всех наследников.
            }

            /// <summary>
            /// Абстрактный метод абстрактного класса определяет API, и должен быть ОБЯЗАТЕЛЬНО определен в наследниках
            /// </summary>
            public abstract void DoSomethingImplemented();

        }
        #endregion

        #region Запечатанные классы
        /// <summary>
        /// Класс, отмеченный модификатором sealed не может выступать в качестве родителя в наследовании.
        /// Т.е. такой класс закрывает возможность дальнейшего наследования
        /// </summary>
        sealed class UnnestedClass
        {

        }
        #endregion

        #region Static class
        /// <summary>
        /// Статический класс - это объединение каких-то методов, которые могут существовать вне ООП структуры
        /// Нельзя создавать объектов статического класса
        /// Все функции статического класса должны быть статическим
        /// Не может участвовать в наследовании (быть наследником или родителем)
        /// Чаще всего используются для выделения каких-то утилит
        /// </summary>
        static class ProjectUtils
        {
            static string name="const string";
            public static string GetName()
            {
                return ProjectUtils.name;
            }
        }
        #endregion

        #region Static constructor & Private constructor
        class WithStaticConstructor
        {
            static string UserName;
            static WithStaticConstructor()      // конструктор будет вызван при первом обращении к классу
            {
                // здесь можно единоразово инициализировать какие-то общие (статические) коллекции, которые будут применяться во всех
                // экземплярах класса
                UserName = System.Environment.UserDomainName;

                Console.WriteLine($"Static constructor");
            }

            private WithStaticConstructor()
            {
                // Для всех классов всегда есть конструктор без параметров
                // Если по логике всегда требуется передача параметров в конструктор, дефолтный конструктор можно скрыть, 
                // объявив его приватным и объявив новый с необходимыми параметрами
            }

            private string _serviceName;
            public WithStaticConstructor(string serviceName)
            {
                _serviceName = serviceName;
            }
        }
        #endregion

    }
}
