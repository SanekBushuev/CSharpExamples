using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpExamples.Additional
{
    /* Generic или обобщения
     * Это возможность в языке C# объявлять типы и методы без указания конкретного типа.
     * Создавая generic класс - вы создаете целое множество "одинаковых" классов, отличающихся только типом.
     * Аналогично для generic методов - метод один, но он применим ко многим типам объектов.
     * Простейшие примеры - коллекции из System.Collections.Generic
     */
    class GenericExamples
    {
        public static void ShowExample()
        {
            Log("Generics\n");

            Print<Classes.ClasseExamples.MyDate>(DateTime.Now);

        }

        private static void Print<T>(T inParam)
            where T: class                          // ограничиваем множество типов, которые могут быть подставлены вместо T классами
        {
            if (inParam is null)
                Log("Param is null");
            Type typinfo = typeof(T);
            Log($"Object type is {typinfo.ToString()}");
            Log($"Object value is {inParam}");
        }

        #region Utils
        private static void Log(string inMes = "")
        {
            Console.WriteLine(inMes);
        }
        #endregion
    }
}
