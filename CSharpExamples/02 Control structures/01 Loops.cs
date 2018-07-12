using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Управляющие структуры
/// </summary>
namespace CSharpExamples.ControlStructures
{
    /// <summary>
    /// Циклы
    /// Циклы позволяют выполнить некоторую последовательность команд несколько раз
    /// </summary>
    class LoopExamples
    {
        public static void ShowExample()
        {
            Console.WriteLine("Пример работы с циклами\n");

            #region Цикл for
            Console.WriteLine("Цикл for");
            // Если известно, сколько точно шагов необходимо сделать, при этом надо знать порядковый номер прохода
            int[] intArr = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            for (int i=0; i < intArr.Length; i++)       // i принимает значения от 0 до intArr.Length-1 включительно
            {
                Console.Write($"[i]={intArr[i]}, ");
            }
            Console.WriteLine();

            // структура конструкции 
            // for (<начальная инициализация>; <условие выполнения>; <завершающие действия>)
            // { <тело цикла> }
            // <начальная инициализация> - действия, выполняемые только один раз перед началом выполнения тела цикла
            // <условие выполнение> - логическая конструкция, при значении TRUE которой происходит выполнение тела цикла
            // <завершающие действия> - действия, выполняемые в конце каждой итерации
            Console.WriteLine($"\nВыполняем for (int i =0, j = 10; j >= i; i++, j=j-2)");
            for (int i =0, j = 10;
                j >= i; 
                i++, j=j-2)
            {
                Console.WriteLine($"\ti = {i}, j = {j}");
            }

            // Бесконечный цикл. <начальная инициализация> и <завершающие действия> могут быть опущены
            Console.WriteLine($"\nБесконечный цикл for (; true; )");
            int iter = 0;
            for (; true; )
            {
                iter++;
                if (iter >= 10)
                    break;              // прекратить выполнение текущего цикла (выход за пределы цикла)
                Console.Write($"{iter}, ");
            }
            Console.WriteLine();

            // Пропуск некоторых итераций цикла
            Console.WriteLine($"\nБесконечный цикл for (; true; ) с пропусками");
            iter = 0;
            for (; true; )
            {
                iter++;
                if (iter >= 10)
                    break;
                if ((iter % 2) == 1)        // (int)(iter / 2)      - приведение к целому
                    continue;           // прекратить выполнение текущей итерации цикла и перейти к следующей (<завершающие действия> выполняются)
                Console.Write($"{iter}, ");
            }
            #endregion

            #region Цикл while
            // Цикл while используется для повторения команд заранее неизвестное количество раз, пока выполняется условие
            // Структура:
            // while (<условие выполнение>)
            // { <тело цикла> }
            // <условие выполнение> - логическая конструкция, при значении TRUE которой происходит выполнение тела цикла
            // тело цикла может быть не выполнено ни одного раза
            Console.WriteLine("\nЦикл while");
            iter = 0;
            while (iter <= 10)
            {
                Console.Write($"{iter}, ");
                iter++;
            }       // будут выведены числа от 0 до 10 включительно
            Console.WriteLine();

            // Бесконечный цикл
            bool flag = true;
            iter = 0;
            while (flag)
            {
                iter += 10;
                if (iter >= 100)
                    flag = false;
            }
            Console.WriteLine($"Бесконечный цикл while завершился с iter = {iter}");
            #endregion

            #region Цикл Do...while
            /* Цикл do..while используется для повторения команда заранее неизвестное количество раз, пока выполняется условие
             * Но как минимум один раз тело цикла будет выполнено
             * Структура:
             * do
             * { <тело цикла> }
             * while (<условие повторения>)
             * <условие повторения> - логическое условие, при значении TRUE которого происходит повторение тела цикла
             */
            Console.WriteLine("\nЦикл do..while");
            do
            {
                Console.WriteLine("Тело цикла do..while c FALSE в условии");    // будет выведено один раз
            } while (false);

            // Бесконечный цикл
            do
            {
                Console.WriteLine("Тело бесконечного цикла do..while");
                break;  // вызывает прерывание текущего тела цикла и переход к следующей за ним команде
            } while (true);

            #endregion

            #region Цикл foreach
            /* Цикл foreach обеспечивает простой и понятный способ итерации по элементам массива или любой перечислимой коллекции.
             * Если надо перебрать все элементы массива или коллекции, и при этом не надо знать порядковый номер элемента в массиве.
             * Структура:
             * foreach (<T> <varname> in <array>)
             * { <тело цикла> }
             * <T> - тип элементов <array>
             * <varname> - имя локальной переменной, которой можно пользоваться внутри тела цикла
             * <array> - массив или коллекция с однородными объектами (которые могут быть приведены к типу T)
             */
            Console.WriteLine("\nЦикл foreach");
            Console.Write("Вывод массива чисел: ");
            intArr = new int[] { 1, 3, 5, 7, 11 };
            foreach (int intEl in intArr)
            {
                Console.Write($"{intEl}, ");
            }
            Console.WriteLine();

            // Коллекция дат
            List<DateTime> dateList = new List<DateTime>();
            foreach (int intEl in intArr)
            {
                dateList.Add(new DateTime(2018, 07, intEl));
            }
            Console.Write("Вывод массива чисел: ");
            try
            {
                foreach (DateTime dateEl in dateList)
                {
                    Console.Write($"{dateEl:dd/MM/yyyy}, ");
                //    dateList.Add(dateEl.AddMonths(1));        // Нельзя изменять коллекцию во время выполнения
                }
            }
            catch (Exception ex)
            {
                string strex = ex.ToString();
            }
            Console.WriteLine();

            // Бесконечный цикл с помощью foreach невозможен

            #endregion
        }
    }//
}
