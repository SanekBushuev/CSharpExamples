using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CSharpExamples.Classes.ClasseExamples;

namespace CSharpExamples.Additional
{
    /* Linq - Language Integrated Query - способ обработки коллекций с помощью запросов, аналогичных по подходу языку SQL.
     * Также Linq дает возможность обрабатывать коллекции традиционным способом с помощью ".методов".
     * SQL-подобные запросы и запросы ".методами" почти равноценны по функционалу, но есть некоторые моменты. которые удобнее
     * реализовать только одним из вариантов.
     * 
     * Linq-запрос формирует набор действий, который надо предпринять над коллекцией, чтобы получить желаемый результат.
     * Выполнение набора действий происходит только в момент обращения к данным результата:
     *      в цикле foreach 
     *      или в момент приведения результата к ToArray или ToList
     *      в ряде других вызовов.
     *  
     * 
     * Хороший набор примеров того, что можно сделать с помощью Linq: https://code.msdn.microsoft.com/101-LINQ-Samples-3fb9811b
     */

    class LinqExamples
    {
        public static void ShowExample()
        {
            Console.WriteLine("Пример Linq\n\n");
            System.Random rand = new Random();
            #region Создание массива объектов MyDate
            List<MyDate> mdList = new List<MyDate>();
            DateTime yearStart = new DateTime(2018, 1, 1);
            for (int i=0; i < 10; i++)
            {
                DateTime dt = yearStart.AddDays(rand.Next(365));
                mdList.Add(dt);
            }
            Console.WriteLine("Массив дат: ");
            PrintArray(mdList);

            //var uniqDays = mdList.Select(x => x.Day).Distinct();      - получение списка уникальных дней
            #endregion

            #region Группировка по кварталам
            Console.WriteLine($"Количество дат по кварталам");
            foreach (var quarstat in mdList.GroupBy(x => ((x.Month-1) / 3) + 1).OrderBy(x => x.Key))
            {
                Console.WriteLine($"{quarstat.Key}: {quarstat.Count()}");
                PrintArray(quarstat.ToList());
            }
            #endregion

            if (mdList.Any(x => x.Day == 15))
                Console.WriteLine("Есть дата 15 в коллекции");

            var resObjList = mdList.Where(x => x.Day < 15);

            #region Использование сортировки
            List<int> intList = new List<int>();
            for (int i = 0; i < 15; i++)
                intList.Add(rand.Next(100));
            Console.WriteLine("Original array");
            PrintArray(intList);

            var orderedIntList = intList.OrderByDescending(x => x).ToList();
            PrintArray(orderedIntList);
            #endregion

            #region Анонимные классы
            /* В C# можно использовать анонимные классы - классы, без имени, оперирование которыми происходит в рамках метода */
            var anobj = new { name = "Alexander", secName = "Bushuev" };
            Console.WriteLine($"Anonym class: {anobj.GetType().ToString()}: Name = {anobj.name}, Second name = {anobj.secName}");

            // Анонимные классы используются в Linq запросах
            var strangeDates = mdList.Select(x => new { quarter = (x.Month - 1) / 3 + 1,
                                                        month = x.Month,
                                                        day = x.Day}).ToList();
            Console.WriteLine("Anonym classes: ");
            foreach (var anobj2 in strangeDates)
            {
                Console.WriteLine($"Квартал: {anobj2.quarter}, месяц: {anobj2.month}, день: {anobj2.day}");
            }
            // Ограничение - невозможность передать объекты анонимных классов в какой-нибудь метод. Только в рамках текущего.
            #endregion

            #region SQL-подобный синтаксис
            // Синтаксис очень походит на SQL, но есть отличие - конструкция SELECT в самом конце.
            // В SELECT указывается, что мы хотим вернуть из результатов выборки выше
            var strangeDates2 = from x in mdList
                                orderby x.asDateTime().DayOfYear
                                select new
                                {
                                    quarter = (x.Month - 1) / 3 + 1,
                                    month = x.Month,
                                    day = x.Day
                                };
            // Пример с группировкой
            var quartDays = from dt in mdList
                            group dt by (dt.Month - 1) / 3 into dtgroup     // далее работаем с группами через dtgroup
                            orderby dtgroup.Key
                            select new { quarter = dtgroup.Key + 1, days = dtgroup.Count() };

            Console.WriteLine("Дней по кварталам:");
            foreach (var qs in quartDays)
            {
                Console.WriteLine($"Квартал: {qs.quarter}, дней: {qs.days}");
            }
            
            #endregion
        }

        static void PrintArray<T>(IEnumerable<T> inArr)
        {
            if (inArr == null)
                return;
            foreach (T obj in inArr)
            {
                Console.Write($"{obj}, ");
            }
            Console.WriteLine();
        }

        public static void ExampleLambdaFunction()          //TODO
        {
            void Log(string inmes)
            {
                Console.WriteLine(inmes);
            }
            Action<string> Log2 = (x) => Console.WriteLine(x);

            List<MyDate> dlist = new List<MyDate>();
            for (int day = 1; day < 32; day++)
            {
                dlist.Add(new MyDate(2017, 01, day));
            }
            Console.WriteLine($"dlist.Count = {dlist.Count}");

            var bar = new MyDate(2017, 01, 15);
            var dl2 = dlist.Where(x => x.CompareTo(bar) > 0);
            var dl2_1 = dlist.Where(CheckDate);

            Console.WriteLine($"dl2.Count = {dl2.Count()}");
            foreach (var vd in dl2)
            {
                Console.WriteLine($"dl2.element = {vd.ToString()}");
            }
            dlist.ForEach(x => Console.WriteLine(x.ToString()));

            dlist.Exists(x => x.Year == 2017);
            List<double> doubleList = new List<double>();
            var resList = doubleList.Select(x => new { cel = (int)x, man = (double)(x % 1) });
            foreach (var el in resList)
            {
                Console.WriteLine($"cel = {el.cel}, man = {el.man}");
            }

            var dl3 = dlist.Select(x => x.Year);
            Console.WriteLine($"dl3 element type is {dl3.FirstOrDefault().GetType().ToString()}");

            //---------------------------
            Func<int> cfu = () => 2;
            Func<int, int> fu = (x) => x + 1;
            var fures = fu(3);
            Console.WriteLine($"fu type is '{fu.GetType().ToString()}'");
            Console.WriteLine($"fu(3) = {fu(3)}");

            Func<int, double, int> gu = (x, z) =>
            {
                var y = fu(x);
                Console.WriteLine($"fu({x}) = {y}");
                return x;
            };
            gu(5, 10.0); gu(6, 0.0);

            Func<int, double, (int, int)> gu2 = (x, z) =>
            {
                var y = fu(x);
                Console.WriteLine($"fu({x}) = {y}");
                return (x, x+1);
            };
            int func_const = 5;
            Func<int, int, int> vu = (x, y) => x + y;       // vu(x, y) {return x+y;}
            Func<int, int> du = (x) => vu(x, func_const);

            // Собираем замыкание. на вход функции sf подается функция от двух целых и какое-то число
            // на выходе функция, которая имеет разрядность на один элемент меньше, при этом вызывает функцию, 
            // переданную в качестве первого аргумента sf и с фиксированным первым аргументом
            Func<Func<int, int, int>, int, Func<int, int>> sf =
                (f, x) => (a) => f(a, x);
            var sf2 = sf(vu, 5);
            sf2(3);

        }

        static bool CheckDate(MyDate inObj)
        {
            return inObj.Day > 15;
        }

    }
}
