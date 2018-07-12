using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpExamples.Types
{
    /// <summary>
    /// Преобразование типов
    /// </summary>
    class TypeConvertExmaples
    {
        public static void ShowExample()
        {
            Console.WriteLine("Пример преобразования типов");

            //Преобразование строки в целочисленное число
            string strval = "7,258";
            int intval;
            if (int.TryParse(strval, out intval))               // производит преобразование типа и возвращает true, 
                Console.WriteLine($"intval  = {intval}");       // либо возвращает false, если не может преобразовать тип
            else
                Console.WriteLine($"невозможно получить целое из '{strval}'");


            // преобразование строки в число с плавающей точкой
            double douval;
            if (double.TryParse(strval, 
                                System.Globalization.NumberStyles.None,             // можно управлять разбором исходной строки
                                System.Globalization.CultureInfo.InvariantCulture,  // определяет "культуру" числа в строковом значении (с точкой или запятой и т.п.)
                                out douval))
                Console.WriteLine($"douval = {douval}");
            else
                Console.WriteLine($"невозможно получить double из '{strval}'");


            // Преобразование к типу Decimal
            decimal decval;
            if (decimal.TryParse(strval, out decval))
                Console.WriteLine($"decval = {decval}");
            else
                Console.WriteLine($"невозможно получить decimal из '{strval}'");


            // Чтение даты из строки
            DateTime datval = DateTime.Now;
            if (!DateTime.TryParseExact("22/02/2017", "dd/MM/yyyy",     // ddMMyyyy HH:mm:ss.fff
                    System.Globalization.CultureInfo.CurrentCulture,
                    System.Globalization.DateTimeStyles.None,
                    out datval))
                Console.WriteLine("Не получилось разобрать дату");
            else
                Console.WriteLine($"Дата разобрана в {datval.ToString("dd MMMM yyyy")}");



            #region  Использование Converter
            /* Специальный класс System.Convert содержит методы для преобразование типов
                System.Convert.To_T - методы ToInt, ToString и т.п. преобразуют входящий аргумент к целевому типу T
             */
            var bool2dec = System.Convert.ToDecimal(true);          // Может вызвать Exception
            Console.WriteLine($"Bool to decimal is '{bool2dec}'");

            decimal dec1 = (decimal) System.Convert.ChangeType(strval, typeof(decimal));
            #endregion

            //var obj = ((object)b2d) as Type;
            //if (obj != null)

            #region Небезопасное преобразование типов
            var intva22l = int.Parse("4.77");           // преобразование может пройти как успешно, так и вызвать Exception
            #endregion

        }
    }
}
