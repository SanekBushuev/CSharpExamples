using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpExamples.ControlStructures
{
    /// <summary>
    /// Функции и процедуры.
    /// </summary>
    class MethodExamples
    {

        public static void ShowExample()
        {
            Console.WriteLine("Пример описания методов и функций" + Environment.NewLine);
            /* ПРоцедура отличается от функции тем, что не возвращает какого-либо значения
             * В C# есть только функции, для описания процедур в качестве возвращаемого типа указывается тип void.
             */

            // Вызов процедур
            ShowUserName();
            SayHello("Alexander");

            // Вызов функций
            string resI2S = IntToString(42);
            string resStr = PrepareMessage("Alexander");

            #region Передача параметров в Функции по указателю (с ref)
            int intValue = 10;
            if (AddValue(ref intValue, 32))         // указанеие ref при передаче параметра - обязательно
            {                                       // Передаваемая переменная обязательно должна быть инициализирована (участвовать в выражении =)
                Console.WriteLine($"К оригиналоьному значению 10 в функции было добавлено 32 => {intValue}");
            }
            // в качестве ref-аргумента нельзя передать константу
            // AddValue(ref 5, 32);
            #endregion

            #region Передача параметров в функцию с вовзращаемыми значениямиы
            string strValue;
            double douValue;
            if (MultiResult(intValue, out douValue, out strValue))      // Указание out обязательно. Нельзя указывать константы
            {                                                           // передаваемые в out-аргументы переменные могут быть не инициализированы
                Console.WriteLine($"Из MultiResult были получены double:{douValue} и string:{strValue}");
            }
            #endregion

            #region Функции с переменным количеством аргументов
            // Переменные аргументы должны быть одного типа
            string str1 = MultiConcat("a");
            string str2 = MultiConcat("a", "b", "c");
            string str3 = MultiConcat(new string[] { "a", "b", "c", "d" });         // допускается передавать подготовленный массив значений в качестве параметра
            #endregion

            #region Функция со значениями по умолчанию
            str1 = FuncWithDefaults("Alexander");
            str2 = FuncWithDefaults("Alexander", 33);
            str3 = FuncWithDefaults("Maria", 32, false);
            str3 = FuncWithDefaults("Maria", isMen: false, Age: 44);
            #endregion

            var tup = GetTupleResultFunction();
            tup.Item1 = 5;

            (int int2, string str33) = GetTupleResultFunction();            
        }

        #region Процедуры
        static void ShowUserName()
        {
            Console.WriteLine("Current user: " + Environment.UserName);
        }

        // Сжатое описание метода из одного оператора
        static void SayHello(string inName) => Console.WriteLine($"Hello {inName}");
        #endregion

        #region Функции
        static string IntToString(int inValue)
        {
            return inValue.ToString();              // return используется для остановки работы функции и возврата результата
            // функции требуют обязательного наличия return в коде. При этом все ветки программы должны иметь возвращаемое значение
        }

        // Сжатое описание функции из одного оператора
        static string PrepareMessage(string inName) => $"Hello {inName}";
        #endregion

        #region Перегрузка функций 
        // Перегрузка функций - создание функций с тем же именем, но иным набором аргументов
        // Сигнатура функции - это последовательность типов аргументов + типа результата
        // У всех перегруженных функций должны быть различные сигнатуры
        // PrepareMessage: (string -> string)
        // PrepareMessage: (int -> string)
        static string PrepareMessage(int inValue)
        {
            return "Int argument is " + inValue.ToString();
        }

        // Нельзя перегрузить функцию с тем же набором аргументов
        //static string PrepareMessage(int newInValue) => $"{newInValue}";
        #endregion

        #region ref аргументы
        // ref аргументы
        // При передаче в функцию параметра типа значений (Valued Type), создается копия значения.
        // Изменение значения внутри функции не скажется на оригинальной переменной
        // передавая параметр с ref (должно быть в параметрах функции указано), можно изменить это поведение
        private static bool AddValue(ref int value, int additional)
        {
            value = value + additional;
            return true;
        }
        #endregion

        #region out аргументы
        // out аргументы
        // Если из функции надо вернуть несколько значений, то
        //  1. Можно создать структуру или класс с нужными элементами
        //  2. Можно объявить несколько параметров с ключевым словом out
        private static bool MultiResult(int inParam, out double outDouble, out string outString)
        {
            //outString = outDouble.ToString();     // входящие out аргументы нельзя использовать до первого присвоения
            outDouble = (double)inParam;
            outString = inParam.ToString();
            inParam = inParam + 100;
            return true;
        }
        #endregion

        #region params аргументы
        // Неизвестное количество аргументов. 
        // Последний аргумент функции может быть массивом, объявленным с params, в этом случае, функция может быть вызвана 
        // с произвольным количеством аргументов:
        // Примеры:
        //  MultiConcat()
        //  MultiConcat("hello");
        //  MultiConcat("hello", "world", "I", "am", "here");
        //  string[] elements = ...
        //  MultiConcat(elements)
        private static string MultiConcat (params string[] args)
        {
            string resStr = "";
            foreach (string str in args)
            {
                resStr += str + ", ";
            }
            return resStr;
        }
        #endregion

        #region Значения по умолчанию
        private static string FuncWithDefaults(string Name, int Age = 32, bool isMen = true)
        {
            return $"{Name} (возраст: {Age}, пол: {((isMen)?"М":"Ж")} ";
        }
        #endregion


        #region Объявление функций в функциях
        // Область видимости внутренней функции - только в рамках обрамляющей функции
        // Можно описывать в любом месте функции. Рекомендуется описывать в конце функции.
        private static void InFuncFunc()
        {
            Log("Использование объявленной в функции функции");

            return;

            void Log(string inmes)
            {
                Console.WriteLine(inmes);
            }
        }
        #endregion

        #region Кортеж в качестве значения 
        // Функция, которая возвращает результат типа ValueTuple (Требует внешний пакет)
        // Для работы требуется установить внешний пакет из Nuget - ValueTuple от Microsoft
        static private (int intval, string strval) GetTupleResultFunction()
        {
            return ( 1, "result");
        }
        #endregion
    }
}
