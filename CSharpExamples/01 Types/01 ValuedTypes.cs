
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpExamples.Types
{
    /// <summary>
    /// Типы значений.
    /// Хранятся на стеке. 
    /// При передаче в качестве параметров в функции, создаются копии значений. 
    /// При изменении локальных копий внутри функции, изменений оригинального значения не происходит.
    /// </summary>
    class ValuedTypesExamples
    {
        public static void ShowExample()
        {
            // целочисленные типы
            sbyte sb1 = -123;
            short  s1 = -123;
            int    i1 = -123;
            long   l1 = -123;
            // беззнаковые варианты
            byte   b1 = 255;
            ushort us1 = 123;
            uint   ui1 = 123;
            ulong  ul1 = 123;

            char c1 = 'a';

            // var varname = DateTime.Now.AddDays(2);

            #region Приведение целочисленных типов
            // Приведение к типу с меньшей размерностью вызывает ошибку
            // sb1 =  s1 - ошибка приведения типов
            // но при явном указании целевого типа, операция приведения может быть осуществленна
            sb1 = (sbyte)s1;        
            // но надо проверять значения
            s1 = sbyte.MaxValue + 10;   // 127 + 10
            sb1 = (sbyte)s1;            // -119
            // если целевое значение помещается в переменной, преобразование проходит без проблем
            s1 = 10;
            sb1 = (sbyte)s1;

            // Приведение типа с меньшей размерностью к типу с большей - осуществляется без проблем
            l1 = i1;
            #endregion

            // типы с плавающей точкой
            float f1 = 1.23f;               // Используются буквы f и m на конце, чтобы дать компилятору информацию о типе значения
            double d1 = 1.23;

            decimal dec1 = 1.23m;       //128 битное значение (16 байт), с повышенной точностью, подходит для финансовых расчетов
                                        // decimal хранится в особом виде, операции с decimal не поддерживаются процессором в отличии
                                        // от типов с плавающей точкой, как следствие операции с decimal выполняются в десятки раз медленее

            #region Приведение типов с плавающей точкой
            // при приведении к типу с меньшей размерностью необходимо указывать целевой тип
            f1 = (float)d1;
            // приведение к типу с большей размерностью не требует указания целевого типа
            d1 = f1;

            dec1 = (decimal)d1;  // к типу decimal нет неявного преобразования. Надо явно указывать тип
            dec1 = (decimal)f1;

            #endregion


            // Логический тип. Может принимать только два значения
            bool bool1 = true;
                 bool1 = false;

            // Перечисления
            Colors col1 = Colors.Green;

            NumberType nt1 = NumberType.Prime;
            int nt1i = (int)nt1;
            nt1 = (NumberType)nt1i;

            nt1 = NumberType.Prime | NumberType.Fibonachi;
            //if (nt1 == NumberType.PrimeAndSquare)
            {
                Console.WriteLine($"nt1 = {nt1}");
            }


            // Структуры
            DateTime dt1 = DateTime.Now;

            Rectangle rect1 = new Rectangle(0,0,10, 20);
            Rectangle rect2;
            rect2.Left = 0;
            rect2.Top = 10;
            //  rect2.Width = 30;         // не может быть сделано, так как Widht и Height объявлены как Property
            //  rect2.Height = 50;

            #region Хранение значений в стеке
            // Типы значений хранятся на стеке и при передаче в качестве параметров метода создается их копия.
            // Изменение значения в вызываемом методе никак не сказывается на оригинальном значении
            Console.WriteLine("Изменяем целочисленное значение в процедуре");
            Console.WriteLine($"Оригинальное значение переменной i1 = '{i1}'.");
            ChangeIntValue(i1);
            Console.WriteLine($"Значение переменной i1 после вызова функции = '{i1}'.");

            Console.WriteLine();
            Console.WriteLine("Изменяем переменную типа struct в процедуре");
            Console.WriteLine($"Оригинальное значение структуры: {RectangleToString(rect1)}");
            ChangeStructValue(rect1);
            Console.WriteLine($"Структура после вызова процедуры: {RectangleToString(rect1)}");

            Console.WriteLine();
            int i5 = 234;
            int i6 = i5;        // создается копия значения переменной i5
            i6 += 10;
            Console.WriteLine($"Переменная i5={i5}, i6={i6}");      // значения в двух переменных отличаются
            #endregion


            #region Настоящие имена типов
            // short, int, long, byte - это специальные константы, которые были введены для удобства разработки. 
            // каждый из этих типов подменяется на реальный системный тип во время компиляции.:
            //      System.Int16 = short;
            //      System.Int32 = int;
            //      System.Int64 = long;
            System.Int32 i2 = 234;
            i2 = i1;            // 
            i1 = i2;
            #endregion
            
            #region Константы типов
            int i3 = int.MinValue;
            int i4 = int.MaxValue;
            //...
            char c2 = char.MinValue;

            float f2 = float.MinValue;
            float f3 = float.MaxValue;
            float f4 = float.NaN;
            float f5 = float.NegativeInfinity;
            float f6 = float.PositiveInfinity;
            double eps = double.Epsilon;      // минимальное позитивное вещественное число
            double pi = Math.PI;            // предопределенная константа
            double e = Math.E;
            #endregion

            #region Значения по умолчанию
            Console.WriteLine();
            int i7 = default(int);
            
            Console.WriteLine($"Значение по умолчанию для int = {i7}");
            #endregion

            #region Nullable-типы
            /* В .NET есть nullable-варианты типов значений. Значение nullable-варианта типа может принимать то же самое множество значений,
             * что и оригинальный тип, а также значение NULL             
             * Чтобы описать переменную nullable-типа, надо добавить к типу символ "?" в конец
             */
            int? nulint = null;
            if (nulint.HasValue)
                Console.WriteLine("В Nullable-int содержится значение");
            else
                Console.WriteLine("В Nullable-int не содержится значение");
            // В переменную nullable-типа можно присвоить значение оригинального типа
            nulint = 10;

            // Выборка значения из nullable-переменной выполняется с помощью обращения к свойству Value
            int notnulint = 0;
            if (nulint.HasValue)
                notnulint = nulint.Value;
            #endregion
        }

        /// <summary>
        /// Функция, которая получает на входе значение типа int и преобразует его внутри
        /// </summary>
        /// <param name="inArg"></param>
        private static void ChangeIntValue(int inArg)
        {
            Console.WriteLine("Вызов функции ChangeIntValue");

            Console.Write($"Меняем значение с '{inArg}'");
            inArg = inArg + 1000;
            Console.WriteLine($" на '{inArg}'");
        }

        /// <summary>
        /// Функция, которая получает struct Rectange на вход и изменяет ее свойства
        /// </summary>
        /// <param name="inRect"></param>
        private static void ChangeStructValue(Rectangle inRect)
        {
            inRect.Left += 100;
            inRect.Top += (new Random()).Next(200);
            Console.WriteLine($"Двигаем прямоугольник: {RectangleToString(inRect)}");

        }
        /// <summary>
        /// ПОдготовка строки с информацией об объекте struct Rectangle
        /// </summary>
        /// <param name="inRect"></param>
        /// <returns></returns>
        private static string RectangleToString(Rectangle inRect)
        {
            return $"Rectangle {inRect.Width}x{inRect.Height} в позиции ({inRect.Left}, {inRect.Top})";
        }
    }
}
