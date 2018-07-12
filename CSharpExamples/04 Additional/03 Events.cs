using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpExamples.Additional
{
    /* События (Events) - Это элемент реактивного взаимодействия между компонентами программного кода.
     * События позволяют реализовать паттерн Observable/Observer средствами среды исполнения .NET.
     * При работе с Событиями, разработчик добавляет обработчик, который будет вызываться при наступлении события
     */
    class EventExamples
    {

        /// <summary>
        /// Описание сигнатуры функции - обработчика события
        /// В данном примере - функция должна принимать один строковый аргумент и не возвращать значения
        /// </summary>
        delegate void WriteToLogDelegate(string inmessage);

        class Listener
        {
            // Объект event (Событие) создается с типом Делегата (сигнатура функции), реализации которого могут быть добавлены
            // в качестве обработчиков.
            public event WriteToLogDelegate WasUpdated;

            public void RaiseEvent(string inMes)
            {
                //if (WasUpdated != null)                       // Такой вызов небезопасен, так как между этими двумя строчками
                //    WasUpdated(inMes);                        // управление может быть передано в другой поток, в котором произойдет
                                                                // открепление обработчика от события

                WriteToLogDelegate localEvent = WasUpdated;     // Такое поведение рекомендуется, так как во время выполнения кода, между
                if (localEvent != null)                         // проверкой и выполнение события от него могут отписаться все подписчики
                {                                               // и событие станет NULL, а вызов NULL приведет к Exception
                    localEvent(inMes);                          // В локальной переменной localEvent будет сохранен набор обработчиков,
                }                                               // подвязанных к Событию в момент присвоения (копирование списка)

                // Использование оператора ?. Позволяет избежать создание дополнительных переменных
                // WasUpdated?.Invoke(inMes);       // вызов события
            }
        }

        
        public static void ShowExample()
        {
            Console.WriteLine("Пример с событиями и делегатами\n\n");

            // Создаем объект, у котого есть событие
            var lister = new Listener();

            // Добавляем обработчиков к событию
            lister.WasUpdated += List_ToLog;         // Подвязываем реакцию на событие
            lister.WasUpdated += List_ToLog1;        // Подвязываем реакцию на событие
            lister.WasUpdated += Lister_ToLog;
            //lister.WasUpdated += Lister_ToLog;     // обработчик Lister_ToLog будет вызываться дважды по событтию

            // Вызываем событие
            lister.RaiseEvent("Вызов события 1");
            Console.WriteLine();

            // Отвзяываем один из обработчиков
            lister.WasUpdated -= Lister_ToLog;
            lister.WasUpdated -= Lister_ToLog;      // ошибки не будет, если пытаться отвязать обработчик, который не подвязан
            lister.WasUpdated -= Lister_ToLog;


            //lister.WasUpdated -= Lister_ToLog;    // Чтобы гарантировано иметь только один раз добавленный обработчик к событию 
            //lister.WasUpdated += Lister_ToLog;    // рекомендуется сначала его отвязать, а потом сразу подвязать

            // Вызываем событие
            lister.RaiseEvent("Вызов события 2");
            Console.WriteLine();

            #region Альтернативный способ подвязать событие
            // Мы не можем подвязать к событию метод, который не соответствует сигнатуре
            // lister.WasUpdated += GetFormatedOutput;
            // Но можно подвязать его через лямбда-функцию
            lister.WasUpdated += (x) => Console.WriteLine(GetFormatedOutput("TestTopic", x));
            // отвязать в этом случае событие будет "сложно" (через получение списка всех обработчиков и отвязывание 
            // по каким-то признакам соответствующего лямбда-выражению обработчика)

            lister.RaiseEvent("Вызов события 3");
            #endregion
        }

        #region Обработчики событий
        // Обработчиком может быть любой метод, который соответствует сигнатуре события
        private static void Lister_ToLog(string inmessage)
        {
            Console.WriteLine($"Метод 3 для вывода в лог: '{inmessage}'");
        }

        private static void List_ToLog(string inmessage)
        {
            Console.WriteLine($"Метод 1 для вывода в лог: '{inmessage}'");
        }

        private static void List_ToLog1(string inmessage)
        {
            Console.WriteLine($"Метод 2 для вывода в лог: '{inmessage}'");
        }
        #endregion

        // Метод, не подходящий под формат делегата
        static string GetFormatedOutput(string Topic, string Message)
        {
            return $"{Topic}: {Message}";
        }

        #region Reflection для событий
        static void GetInfoAboutEvent (object inObj)
        {
            if (inObj is null)
                return;
            if (inObj is WriteToLogDelegate)
            {
                WriteToLogDelegate evobj = inObj as WriteToLogDelegate;
                var evobjInv = evobj.GetInvocationList();
                if ((evobjInv == null) || (evobjInv.Length == 0))
                    Console.WriteLine("К событию не подвязано обработчиков");
                else
                    Console.WriteLine($"К событию подвязано {evobjInv.Length} обработчиков");
                // evobjInv[0].Target  - получение объекта, метод которого заявлен в качестве обработчика
                // evobjInv[0].Method - метод, заявленный в качестве обработчика
                // evobjInv[0].DynamicInvoke("") - вызов метода
            }
        }
        #endregion
    }
}
