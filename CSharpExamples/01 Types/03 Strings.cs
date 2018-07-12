using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpExamples.Types
{
    /// <summary>
    /// Строки.
    /// Для строк имеется специальная структура в CLR. 
    /// Все строковые константы попадают в таблицу в единственном экземпляре
    /// В результате выполнения действия со строкой создается новый объект строки
    /// 
    /// Интересное про строки: https://habr.com/post/172627/
    /// </summary>
    class StringExamples
    {
        public static void ShowExample()
        {
            Console.WriteLine("Пример работы со стороками");

            string str1 = "Hello world";
            System.String str2 = "Hello" + " world";

            Console.WriteLine($"str1 == str2 -> {str1 == str2}");
            if (Object.ReferenceEquals(str1, str2))
                Console.WriteLine("str1 и str2 ссылаются на один объект в куче");
            else
                Console.WriteLine("str1 и str2 ссылаются на разные объекты в куче");



            string str3 = "Hello";
            string str4 = " world";
            string str5 = str3 + str4;

            Console.WriteLine($"\nstr1 == str5 -> {str1 == str5}");
            if (Object.ReferenceEquals(str1, str5))
                Console.WriteLine("str1 и str5 ссылаются на один объект в куче");
            else
                Console.WriteLine("str1 и str5 ссылаются на разные объекты в куче");

            #region Интернирование строк
            // CLR хранит строки в специальной буферной таблице.
            // С помощью String.Intern можно поместить строку во время выполнения в эту таблицу
            // По умолчанию в хэш-таблицу попадают только строковые литералы во время выполнения
            Console.WriteLine("\nИнтернирование строки");
            str5 = string.Intern(str5); // помещение и получение ссылки на сохраненный литерал в хэш-таблице
            Console.WriteLine($"str1 == str5 -> {str1 == str5}");
            if (Object.ReferenceEquals(str1, str5))
                Console.WriteLine("str1 и str5 ссылаются на один объект в куче");
            else
                Console.WriteLine("str1 и str5 ссылаются на разные объекты в куче");

            string str5_1 = string.IsInterned(str5 + " ");
            if (str5_1 is null)
                Console.WriteLine($"Строка '{str5} ' не найдена в хэш-таблице строковых литералов.");

            str5_1 = string.Intern(str5 + " ");
            if (str5_1 is null)
                Console.WriteLine($"Строка '{str5} ' не найдена в хэш-таблице строковых литералов.");
            else
                Console.WriteLine($"Строка '{str5} ' помещена в хэш-таблицу строковых литералов.");

            // поиск по таблице достаточно долгий процесс, поэтому только литералы, прописанные в коде, попадают в таблицу
            #endregion

            #region Функции работы со строками
            string strEx = "Съешь еще этих французских булок";
            Console.WriteLine($"\nРазбираем строку \"{strEx}\"");
            // Получение произвольного символа из строки
            char firstChar = strEx[0];
            char lastChar = strEx[strEx.Length - 1];
            Console.WriteLine($"Первый символ строки - '{firstChar}', послдений символ - '{lastChar}'");

            // поиск подстроки в строке
            int fInd = strEx.IndexOf("еще");        // -> 6
            fInd = strEx.IndexOf("е", 5);           // -> 6
            fInd = strEx.IndexOf("е", 9);           // -> -1    - значение не найдено в строке

            // Разбиение строки на составляющие
            string[] parts = strEx.Split(new char[] { ' ', ',', '.', '?', '!' });
            Console.WriteLine($"В стркое {parts.Length} слов");
            // Разбиение строки на составляющие, но не более N элементов
            parts = strEx.Split(new char[] { ' ' }, 4);
            Console.WriteLine($"В стркое {parts.Length} слов, последнее слово - '{parts[3]}'");

            // Объединение массива строк
            string[] parts7 = new string[7] { "hello", "world", "once", "again", "one", "more", "world" };
            string str7 = string.Join(" ", parts7); //hellop world ...
            Console.WriteLine($"Объединенная строка через пробел str7: {str7}");
            string str7_2 = string.Join(",", parts7);
            Console.WriteLine($"Объединенная строка через запятую str7_2: {str7_2}");

            // Проверка наличия подстроки
            if (strEx.Contains("еще"))
                Console.WriteLine("Слово \"еще\" найдено в строке '"+strEx+"'");
            // strEx.IndexOf("еще");

            // Проверка строки на пустоту
            if ((strEx == null) || (strEx.Length == 0) || (strEx == string.Empty))
                Console.WriteLine("Строка пуста");
            if (string.IsNullOrEmpty(strEx))            // Специальная функция в классе String
                Console.WriteLine("Строка пуста");
            #endregion

            #region Строки со спецсимволами
            //Перевод строки -> \n
            // TAB -> \t
            // " -> \"
            string strF = "Hello"+ Environment.NewLine + "\tWorld";     
            Console.WriteLine($"Пример с переводом строки:'{strF}'");

            strF = "\\n - для первода строки\n\\t - для символа \"TAB\"";
            Console.WriteLine($"Пример с выводом спец символа '\\':'{strF}'");

            // если в строке надо вывести \ его надо задвоить
            string path = "C:\\Temp\\Subdirectory\\Filename.ext";
            // либо отключить выведение спецсимволов
            string path2 = @"C:\Temp\Subdirectory\Filename.ext
kljlklj- ""файл"""; // "" - выведут одну кавычку в этом случае
            #endregion

            #region Форматирование строк для вывода
            // String.Format позволяет сформировать строку из шаблона и конкретных значений, которые указываются в аргументах метода
            string str8 = string.Format("Здесь будет число: {3}", 7.023, 23, 22, true);             // = "Здесь будет число: 7,023"     <- только один аргумент использовался
            string str8_1 = $"Здесь будет дата: {DateTime.Now}";                                    // = "Здесь будет дата: 01.07.2018 22:58:47"
            string str8_2 = string.Format("Здесь будет число: {1} {0:000.000} ", 7.023, true);      // = "Здесь будет число: True 007,023 "
            string str8_3 = string.Format("Здесь будет число: {0} {0} {0} {0} ", 1007.2.ToString("###,##0.00####")); //= "Здесь будет число: 1 007,20 1 007,20 1 007,20 1 007,20 "
            double dval = 7.248d;
            string str8_4 = $"Здесь будет число: {dval:###,##0.00####}  {dval:###,##0.00####}";     // =  "Здесь будет число: 7,248  7,248"
            string str9 = $"Здесь тоже будет число: {dval}. Вот так вот";                           // = "Здесь тоже будет число: 7.248. Вот так вот"
            string str0 = "Здесь тоже будет число:" + dval + ". Вот так вот";
            // Дополнительные форматы
            double posValue = 1234;
            double negValue = -1234;
            double zeroValue = 0;

            Console.WriteLine(zeroValue.ToString("##;(##);**Zero**"));
            Console.WriteLine(String.Format("{0:##;(##);**Zero**}", zeroValue));      // = **Zero**
            Console.WriteLine(String.Format("{0:##;(##);**Zero**}", posValue));       // = 1234
            Console.WriteLine(String.Format("{0:##;(##);**Zero**}", negValue));       // = (1234)
            // Подробнее: https://msdn.microsoft.com/ru-ru/library/0c899ak8(v=vs.110).aspx
            #endregion


        }
    }
}
