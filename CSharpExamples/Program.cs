using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpExamples
{
    class Program
    {
        /* Точка входа для приложения.
         * Среда CLR при старте приложения .NET, если не указано явно, ищет первый попавшийся класс, в котором реализован статический метод Main:
         *      static void Main(string[] args) или static int Main(string[] args)
         * У разработчика всегда есть возможность указать точку входа явно (выбрав класс, в котором реализован метод Main) в настройках проекта
         */
        static void Main(string[] args)
        {
            /* Для запуска примера, раскомментируйте интересующий вас пример и запустите программу на выполнение (кнопка Start в панеле сверху)
             * или через меню Debug -> Start Debug
             */

            Console.WriteLine("\t\tПримеры C#\n");
            Console.WriteLine();

            //=======//
            // ТИПЫ  //
            //=======//

            // 01. Типы значений
            //Types.ValuedTypesExamples.ShowExample();

            // 02. Ссылочные типы
            //Types.ReferenceTypeExamples.ShowExample();

            // 03. Строки
            //Types.StringExamples.ShowExample();

            // 04. Перечисления
            //Types.EnumExamples.ShowExample();

            // 06. Преобразование типов (конвертация типов)
            //Types.TypeConvertExmaples.ShowExample();

            // 07. Массивы
            //Types.ArrayExamples.ShowExample();


            //==========================//
            // УПРАВЛЯЮЩИЕ КОНСТРУКЦИИ  //
            //==========================//

            // 01. Циклы
            //ControlStructures.LoopExamples.ShowExample();

            // 02. Условные операторы
            //ControlStructures.ConditionExamples.ShowExample();

            // 03. Подпрограммы
            //ControlStructures.MethodExamples.ShowExample();

            //========//
            // КЛАССЫ //
            //========//

            // 01. Классы
            //CSharpExamples.Classes.ClasseExamples.ShowExample();

            // 02. Наследование
            //Classes.InheritanceExamples.ShowExample();

            // 03. Интерфейсы
            //CSharpExamples.Classes.InterfaceExamples.ShowExample();

            //========================//
            // ДОПОЛНИТЕЛЬНЫЕ МОМЕНТЫ //
            //========================//

            // 01. Relfection - работа с метаинформацией о типах
             CSharpExamples.Additional.ReflectionExamples.ShowExample();

            // 02. Linq - обработка коллекций
            //CSharpExamples.Additional.LinqExamples.ShowExample();

            // 03. Events - события и делегаты
            //CSharpExamples.Additional.EventExamples.ShowExample();

            // 04. Threads - работа с потоками 
            //CSharpExamples.Additional.ThreadExamples.ShowExample();

            // 05. RDBMS - работа с базой данных
            //CSharpExamples.Additional.RDBMSExamples.ShowExample();

            Console.WriteLine();
            Console.WriteLine("Работа завершена. Нажмите любую клавишу");
            Console.ReadKey();
        }
    }
}
