using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpExamples.Types
{
    /// <summary>
    /// Массивы.
    /// Массивы в C# - это наследники класса System.Array. Любой экземпляр массива может быть приведен к System.Array.
    /// Все массивы хранятся в куче.
    /// </summary>
    class ArrayExamples
    {
        public static void ShowExample()
        {
            Console.WriteLine("Пример работы с массивами");

            // Массив - это коллекциия однотипных объектов заранее определенной длины
            
            // Объявление массива
            int[] intAr1 = new int[10];      // объявление массива длиной в 10 элементов. Каждый элемент равен default(T), где T - тип элемента
            int[] intAr2 = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };  // объявление массива длины, определяемой количеством элементов в {}
            int[] intAr3 = new int[] { };       // пустой массив
            Console.Write("intAr1: "); PrintArray(intAr1);

            Console.WriteLine($"Длина массива: {intAr1.Length}");
            Console.WriteLine($"Первый элемент массива по индексу 0 - {intAr2[0]}");
            Console.WriteLine($"Последний элемент массива по индексу Length-1 - {intAr2[intAr2.Length-1]}");    // intAr2[intAr.Length] - вызовет ошибку. Выйти за пределы массива не получится!
            
            #region Изменение длины массива
            // изменить размер массива после создания нельзя.
            Console.WriteLine("Расширяем массив");
            int[] intAr4 = intAr2;
            if (object.ReferenceEquals(intAr2, intAr4))
                Console.WriteLine("Переменные intAr2 и intAr4 указывают на один массив");

            Array.Resize(ref intAr2, intAr2.Length + 5);
            // на самом деле создает копию массива указанной длины и копирует допустимое количество элементов

            Console.WriteLine($"Массив intAr2 удлинен до длины {intAr2.Length}");
            if (object.ReferenceEquals(intAr2, intAr4))
                Console.WriteLine("Переменные intAr2 и intAr4 указывают на один массив");
            else
                Console.WriteLine("Переменные intAr2 и intAr4 указывают на разные массивы");
            Console.Write("intAr2: "); PrintArray(intAr2);
            Console.Write("intAr4: "); PrintArray(intAr4);
            #endregion

            #region  Многомерные массивы
            Console.WriteLine("\nМногомерные массивы");
            //При создании многомерных массивов необходимо задавать количество элементов по каждому измерению
            int[,] dim2Arr = new int[10, 15];
            int[,] dim2arr2 = new int[,] {  { 1, 2, 3 }, 
                                            { 4, 5, 6 } };
            Console.WriteLine($"Количество измерений dim2Arr - {dim2Arr.Rank}");
            for (int rank =0; rank < dim2Arr.Rank; rank++)
            {
                Console.WriteLine($"\tизмерение {rank} содержит элементов - {dim2Arr.GetLength(rank)}");
            }

            // заполним массив
            for (int i = 0; i < dim2Arr.GetLength(0); i++)
                for (int j = 0; j < dim2Arr.GetLength(1); j++)
                    dim2Arr[i, j] = i*dim2Arr.GetLength(1)+ j;
            Console.Write($"dim2Arr[{dim2Arr.GetLength(0)}, {dim2Arr.GetLength(1)}]: ");
            PrintArray(dim2Arr);
            #endregion

            #region Коллекции
            Console.WriteLine("\nКоллекции");

            // С массивами удобно работать, когда известно количество элементов.
            // Когда количество изначально неизвестно удобнее работать с коллекциями.
            // В C# есть заготовки для всех базовых коллекций - Stack, Deck, Queue
            // Все коллекции находятся в пространстве имен System.Collections

            System.Collections.ArrayList arlist = new System.Collections.ArrayList();
            // ArrayList - одномерная коллекция, которая может хранить объекты любых типов
            arlist.Add(3);
            arlist.Add("string");
            arlist.Add(true);
            arlist.Add(new System.Random());
            // получает ссылку на object, которую надо корректно преобразовать к нужному типу
            Console.Write("arlist: ");
            PrintIEnumerable(arlist);

            System.Collections.Hashtable hash = new System.Collections.Hashtable();
            hash.Add("key1", "value");      // добавление значения с указанием ключа
            //hash.Add("key1", 233);        // при попытке добавить значение с уже имеющимся ключем - Exception
            hash["key1"] = 1233;            // Добавление или изменение значения по ключу
            hash[123] = "value2";           // ключи и значения могут быть различных типов

            Console.WriteLine("Hashtable elements:");
            foreach (object key in hash.Keys)
            {
                Console.WriteLine($"\t{key}({key.GetType().Name}) => {hash[key]}({hash[key].GetType().Name})");
            }
            hash.ContainsKey(321);
            object obj = hash[321];
            if (obj == null)
                Console.WriteLine($"Получение значения по неизвестному ключу возвращает null");

            // В пространстве имен System.Collections можно найти и другие варианты коллекций
            //System.Collections.Stack
            //System.Collections.Queue
            //System.Collections.SortedList 
            #endregion

            #region Обобщенные коллекции (Generic)
            // Оригинальные коллекции из пространства имен System.Collections неудобны тем, что в них можно поместить
            // объект любого типа, и это надо учитывать при получении значения из коллекции.

            System.Collections.Generic.List<string> intLIst = new List<string>();
            intLIst.Add("sdkjfd");
            intLIst.Contains("");
            string str = intLIst[0];

            //System.Collections.Generic.Dictionary<int, string> 
            #endregion
        }

        /// <summary>
        /// Вывод массива на экран.
        /// В качестве аргумента ожидается объект типа System.Array, а значит в этот метод может быть передан любой массив
        /// </summary>
        /// <param name="inArr"></param>
        private static void PrintArray(System.Array inArr)
        {
            if (inArr == null)
                Console.WriteLine("Массив не задан");
            else if (inArr.Length == 0)
                Console.WriteLine("Задан массив пустой длины");
            else
            {
                // С помощью цикла foreach можно обойти все элементы массива. 
                // А переменная obj типа object может принять в себя элемент массива любого типа
                foreach (object obj in inArr)
                    Console.Write($"{obj} ");
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Вывод на экран коллекции, реализующей интерфейс IEnumerable (ArrayList, System.Array, List<T>, Dictionary и т.п.)
        /// По элементам IEnumerable можно пройти с помощью с помощью цикла foreach
        /// </summary>
        /// <param name="arlist"></param>
        private static void PrintIEnumerable(System.Collections.IEnumerable arlist)
        {
            foreach (object obj in arlist)
            {
                switch (obj)
                {
                    case int i:
                        Console.Write($"(int){i}, ");
                        break;
                    case string s:
                        Console.Write($"(string)'{s}', ");
                        break;
                    case bool b:
                        Console.Write($"(bool){b}, ");
                        break;
                    default:
                        Console.Write($"({obj.GetType().Name})'{obj}', ");
                        break;
                }
            }
        }
    }
}
