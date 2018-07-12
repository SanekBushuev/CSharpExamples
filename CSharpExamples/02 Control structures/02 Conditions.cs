using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpExamples.ControlStructures
{
    /// <summary>
    /// Условные операторы.
    /// </summary>
    class ConditionExamples
    {
        public static void ShowExample()
        {
            Console.WriteLine("Пример использования условных операторов" + Environment.NewLine);

            #region Условный оператор if
            /* Оператор if используется для простого ветвения внутри программы по условию
             * Структура:
             * if (<условие>)
             * { <ветка команд при <условие> == true> }
             * else
             * { <ветка команд при <условие> != true> }
             * 
             * <условие> всегда задается в скобках
             * <условие> должно быть выражением с результатом типа bool
             * <ветка команд> может быть одним выражением или {последовательностью}
             */

            DateTime today = DateTime.Now;
            if ((today.DayOfWeek == DayOfWeek.Sunday) || (today.DayOfWeek == DayOfWeek.Saturday))
                Console.WriteLine("Выходные!!! УРА!!!");
            else
                Console.WriteLine("Рабочая неделя");

            /*
             Логические операции 
                 || - логическое ИЛИ
                 && - логическое  И
             Приоритеры классические: сначала И, потом ИЛИ.
             Но рекомендуется выделять логические выражения в скобки

             */

            // многоступенчатое ветвление
            if (today.DayOfWeek == DayOfWeek.Friday)
                Console.WriteLine("До выходных осталось всего ничего");
            else if ((today.DayOfWeek == DayOfWeek.Sunday) || (today.DayOfWeek == DayOfWeek.Saturday))
                Console.WriteLine("Выходные");
            else
                Console.WriteLine("Рабочая неделя");
            // В результате прохода через этот блок if.. else if .. else будет выполнена только одна из веток команд

            // Операции сравнения
            object obj = null;
            int intEl = 15;

            // проверка на NULL
            if (obj is null)                    // obj == null, работать не будет
                Console.WriteLine("obj is null");
            if (!(obj is null))
                Console.WriteLine("obj not is null");
            if (intEl < 20)
                Console.WriteLine($"{intEl} < 20");
            if (intEl == 15)
                Console.WriteLine($"{intEl} == 15");
            if (intEl >= 14)
                Console.WriteLine($"{intEl} >= 14");
            //if (intEl)                              // требуется укзаать выражение типа bool, неопределено неявное преобразование int в bool
            //    Console.WriteLine
            #endregion

            #region Сжатая форма для if
            // Часто надо проверить определенное условие и в зависимости от результата присвоить одной переменной одно значение
            int resSal = 0;
            int kpi = 85;
            if (kpi > 80)
                resSal = 100;
            else
                resSal = 70;

            // конструкция выше может быть переписана в
            resSal = (kpi > 80) ? 100 : 70;
            // Структура:
            // (<условие>) ? <значение по true> : <значение по false>;
            // Все части выражения являются обязательными. В классическом if часть else можно опустить
            #endregion

            #region Сжатая форма для if ( is null)
            // иногда надо проверить ссылку на NULL, и если она NULL присвоить какое-то значение по умолчанию
            object dbObj = null;
            if (dbObj is null)
                dbObj = DBNull.Value;           // специальная константа ADO.NET для вставки в БД значения NULL
            // Конструкция может быть переписана
            object dbObj2 = dbObj ?? DBNull.Value;
            // Структура
            // <проверяемое на NULL выражение> ?? <значение, в случае если выражение слева равно NULL>

            System.Random rand = null;
            int probval = (rand != null)? rand.Next(100) : 50;
            probval = rand?.Next(100) ?? 50;
            #endregion

            #region Оператор ветвления switch
            /*Применяется тогда, когда тестируемое значение может принимать значение из некоторого ограниченного множества
             * Структура:
             * switch (<тестируемое значение>)
             * {
             *      case <константа1> : <набор команд>; 
             *                      break;
             *      case <константа2> :
             *      case <константа3> : <набор команд, который будет выполнен при равенстве значения конст1 или конст2>; 
             *                      break;
             *      default:    <набор команд, который будет выполнен при неравенстве значения любой из констант>
             *      // default должен быть последним!!! default может быть опущен
             * }
             */
            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Monday:
                case DayOfWeek.Tuesday:
                case DayOfWeek.Wednesday:
                        Console.WriteLine("Первая половина недели");
                        Console.WriteLine("");
                        break;
                case DayOfWeek.Thursday:
                case DayOfWeek.Friday:      Console.WriteLine("Вторая половина недели");
                    break;
                default:
                    Console.WriteLine("Выходные");
                    break;
            }

            int day = DateTime.Now.Day;
            switch (DateTime.Now.Day)
            {
                //case <= 15: break;            // Задать диапазон в case нельзя - только константа на равенство
                //case day: break;              // Обязательно требуется указывать константу! Нельзя использовать переменную в качестве ключа
            }
            #endregion

            #region Сопоставление с образцом
            /* Сопоставление с образцом - это нововведение последней версии C#
             * Позволяет проверять <тестируемое значение> по типу значения
             * В части case меняется на :
             *  case <тип значения> <имя переменной или '_'>: ...
             *  если тип тестируемого значения совпадается с <тип значения>, в указанную переменную заносится приведенное к типу значения, 
             *  которое можно использовать внутри ветки операторов до break
             *  если указать "<тип> _", то ни в какую переменную <теструемое значение>, приведенное к типу, записано не будет (не надо)
             */
            List<object> valList = new List<object>();
            valList.Add((int)15);
            valList.Add(DateTime.Now);
            valList.Add("string value");
            valList.Add(true);

            foreach (object obj1 in valList)
            {
                switch (obj1)
                {
                    //case object obj2: Console.WriteLine("Object");
                    //    break;
                    case null: break;
                    case int i: Console.WriteLine($"Числовое значение {i}");
                        break;
                    case DateTime dt: Console.WriteLine($"DateTime значение: {dt}");
                        break;
                    //case string str: Console.WriteLine($"Строковое значение '{str}'");
                    case string _:      // не важно значение, важен тип
                        Console.WriteLine($"Строковое значение ");
                        break;
                    case bool bl: Console.WriteLine($"Логическое значение {bl}");
                        break;
                    default: Console.WriteLine($"Значение иного типа");
                        break;
                }
            }
            #endregion
        }

    }
}
