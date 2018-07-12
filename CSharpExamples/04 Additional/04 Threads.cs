using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpExamples.Additional
{
    class ThreadExamples
    {
        public static void ShowExample()
        {
            /* Три варианта запуска вывода двух массивов на консоль в параллельных  потоках.
             * Каждый из примеров надо запускать по отдельности и смотреть. что будет выведено на экран.
             */
            Console.WriteLine("Пример с потоками\n\n");
            //ThreadByThreadExample();
            //Log();
            //ThreadByTimerExample();
            Log();
            ThreadByAsyncAwaitExample();
        }

        static void Log(string inMes = "")
        {
            Console.WriteLine(inMes);
        }

        static Random rnd = new Random(67681388);

        /// <summary>
        /// Запуск функции в отдельном потоке явно, с помощью объекта System.Threading.Thread
        /// </summary>
        [Obsolete("Будет удалена в апреле 2017")]   // Атрибут, который позволяет отметить метод как устаревший. Во время компиляции будет видно.
        private static void ThreadByThreadExample()
        {
            List<int> EvenNumber = new List<int>(); // четные числа
            List<int> OddNumber = new List<int>();  // нечетные числа


            for (int i = 0; i < 50; i++)
            {
                int value = rnd.Next(0, 1001);
                if (value % 2 == 1)
                    OddNumber.Add(value);
                else
                    EvenNumber.Add(value);
            }

            Log($"Массив нечетных чисел содержит {OddNumber.Count}");
            Log($"Массив четных чисел содержит {EvenNumber.Count}");

            Log();
            Log("Выводим два массива конкурентно");
            System.Threading.Thread oddThread = new System.Threading.Thread(() => ShowIntList(OddNumber));
            System.Threading.Thread evenThread = new System.Threading.Thread(() => ShowIntList(EvenNumber));
            oddThread.Start();
            evenThread.Start();
            //    oddThread.
        }

        /// <summary>
        /// Запуск функции в потоке через таймер.
        /// Таймер можно выполнить однократно или выполнять периодически. Если два вызова наложатся (второй тик таймера произойдет
        /// до завершения метода, запущенного по первому тику), второй запустится в отдельном потоке.
        /// </summary>
        private static void ThreadByTimerExample()
        {
            List<int> EvenNumber = new List<int>(); // четные числа
            List<int> OddNumber = new List<int>();  // нечетные числа


            for (int i = 0; i < 50; i++)
            {
                int value = rnd.Next(0, 1001);
                if (value % 2 == 1)
                    OddNumber.Add(value);
                else
                    EvenNumber.Add(value);
            }

            Log($"Массив нечетных чисел содержит {OddNumber.Count}");
            Log($"Массив четных чисел содержит {EvenNumber.Count}");

            System.Timers.Timer tm = new System.Timers.Timer(1000);
            tm.AutoReset = false;
            tm.Elapsed += (x, y) => ShowIntList(OddNumber);

            System.Timers.Timer tm2 = new System.Timers.Timer(3000);
            tm2.AutoReset = false;
            tm2.Elapsed += (x, y) => ShowIntList(EvenNumber);

            tm.Start();
            tm2.Start();
        }
        
        #region Utils
        private static void ShowIntList(IEnumerable<int> inList)
        {
            foreach (int i in inList)
            {
                LogThread($"({System.Threading.Thread.CurrentThread.ManagedThreadId}) -> {i}");
                System.Threading.Thread.Sleep(rnd.Next(1000, 3001));
            }
        }

        static object d_lock = new object();
        private static void LogThread(string inmessage = "")
        {
            lock (d_lock)
            {
                System.Console.WriteLine(inmessage);
            }
        }
        #endregion

        #region Потоки через TASK/async/await
        //http://ru.stackoverflow.com/questions/316990/%D0%9A%D0%B0%D0%BA-%D1%80%D0%B0%D0%B1%D0%BE%D1%82%D0%B0%D1%8E%D1%82-await-async
        //https://msdn.microsoft.com/ru-ru/library/hh156528.aspx
        private async static void ThreadByAsyncAwaitExample()
        {
            List<int> EvenNumber = new List<int>(); // четные числа
            List<int> OddNumber = new List<int>();  // нечетные числа


            for (int i = 0; i < 10; i++)
            {
                int value = rnd.Next(0, 1001);
                if (value % 2 == 1)
                    OddNumber.Add(value);
                else
                    EvenNumber.Add(value);
            }

            Log($"Стартуем потоки");

            var task = StartAsync(EvenNumber);
            var task2 = StartAsync(OddNumber);

            //var res2 = await task;

            Task.WaitAll(task, task2);
            Log($"Потоки завершены");

        }

        private static async Task<bool> StartAsync(IEnumerable<int> inList)
        {
            //await ThreadAsync(inList);
            await Task.Run(() => { ShowIntList(inList); Log("Task complete"); });
            Log("StartAsync completed");
            return true;
        }

        #endregion
    }
}
