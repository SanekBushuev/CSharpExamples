using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpExamples.Types
{
    /// <summary>
    /// Перечисления.
    /// Имеют прямое отражение на множество целых чисел - каждому элементу перечисления сопоставлено одно значение из целых чисел.
    /// Значение Enum можно привести к int, и обратно.
    /// </summary>
    class EnumExamples
    {
        public static void ShowExample()
        {
            Console.WriteLine("Пример работы с перечислениями");

            // Объявление переменной типа enum
            Colors colEn = default(Colors);     // default(Enum) вернет 0.
            Console.WriteLine($"Переменная colEn содержит значение {colEn}");
            // Если в Enum определен элемент с 0, то будет выведено его строковое представление (Red), либо число

            // преобразование Enum в число
            colEn = Colors.Green;
            int colEnInt = (int)colEn;          // преобразуем переменную типа Enum в число
            Colors colEn2 = (Colors)colEnInt;   // преобразуем переменную числового типа в Enum
            Console.WriteLine($"Числовое значение для {colEn} = {colEnInt}, при преобразовании его в enum Colors = {colEn2}");

            // Попытка преобразование в число меньшей размерности
            short sh1 = (short)colEn;       // аналогично числовым преобразованиям пройдет без проблем, если константа под значением
                                            // enum в допустимых пределах целевого типа. Иначе будет получено непредсказуемое значение

            #region Работа с enum с атрибутом Flag
            Console.WriteLine("\n[Flag] enum LogStatus");
            var obj = LogStatus.Info | LogStatus.Critical;
            Console.WriteLine(obj);                                 //Info, Critical  <- автоматический вывод
            Console.WriteLine(obj & LogStatus.None);                //0
            Console.WriteLine(obj & LogStatus.Info);                //Info
            Console.WriteLine(obj & LogStatus.Warning);             //0
            Console.WriteLine(obj & LogStatus.Error);               //0
            Console.WriteLine(obj & LogStatus.Critical);            //Critical
            Console.WriteLine();

            // Использование в условии if
            if ((obj & LogStatus.Critical) == LogStatus.Critical)       // & - бинарная операция И (побитовая И)
                Console.WriteLine($"{obj} содержит {LogStatus.Critical}");
            #endregion

            #region Преобразование в строку и из строки
            string colEnStr = colEn.ToString();
            Console.WriteLine($"Строковое представление для элемента {colEn} = '{colEnStr}'");
            Colors colEn3;
            if (Enum.TryParse(colEnStr, out colEn3))
            {
                Console.WriteLine($"Строковое значение '{colEnStr}' было разобрано как элемент перечисления Colors: {colEn3}");
            }
            if (!Enum.TryParse("Красный", out colEn3))
                Console.WriteLine($"Строковое значение 'Красный' не было разобрано как элемент перечисления Colors");
            #endregion

            #region Перебор всех элементов перечисления
            // Как перебрать все возможные значения перечисления, если вы знаете тип перечисления
            string[] enumNames = Enum.GetNames(typeof(NumberType));
            foreach (string enName in enumNames)
            {
                NumberType enEl = GetValue<NumberType>(enName);
                Console.WriteLine($"Элемент перечисления: {enName} -> распознан как {enEl} ({(int)enEl})");
            }
            #endregion
        }

        /// <summary>
        /// Функция, которая получает на вход строку и пытается преобразовать к перечислению, определенному с помощью T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strValue"></param>
        /// <returns></returns>
        private static T GetValue<T>(string strValue)
            where T : struct
        {
            T result = default(T);
            if (Enum.TryParse(strValue, out result))
            {
                return result;
            }
            return result;
        }
    }

    enum Colors
    {
        Red = 1,        // Можно задать значение для первого элемента, отличное от 0 (по умолчанию), например 1
        Green = 5,          // остальные будут пронумерованы последовательно  Green = 2, Blue = 3
        Blue = 11
    }

    enum Shapes : long        // Можно определить Перечисление на другом типе, если нужно хранить какие-то константы, не помещающиеся в int
    {
        Square = 15134523643734,
        Rectangle = 5,
        Circle = 7,
        Triangle = 156
    }

    // Специальный атрибут [Flags] используется, чтобы показать, что значение может содержать несколько элементов из Enum в виде битовой маски
    [Flags]
    enum NumberType
    {
        /// <summary>
        /// Простое число
        /// </summary>
        Prime = 0b001,      //1
        /// <summary>
        /// Число фибоначи
        /// </summary>
        Fibonachi = 0b010,     //1      // запись числа в бинарном виде == 2
        /// <summary>
        /// Число является квадратом числа
        /// </summary>
        Square = 0b100,         //4
        /// <summary>
        /// Число, которое и простое и квадрат
        /// </summary>
        PrimeAndSquare = 0b101
    }

    [Flags]
    enum LogStatus
    {
        None = 1,
        Info = 2,
        Warning = 4,
        Error = 8,
        Critical = 16
    }
    

}
