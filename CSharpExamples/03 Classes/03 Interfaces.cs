using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpExamples.Classes
{
    /// <summary>
    /// Интерфейсы определяют набор методов у реализующих классов.
    /// Не содержат реализации и используются для того, чтобы разорвать связанность частей программной системы:
    ///     интефрейс может быть описан в базовой библиотеке
    ///     конкретные реализации могут быть описаны в специализированных библиотеках (реализующих специфику)
    /// В C# класс может быть отнаследован только от одного родительского класса, но при этом может реализовывать несколько интерфейсов.
    /// С помощью интерфейсов в C# реализуют "множественное наследование", доступное в ряде языков (С++)
    /// </summary>
    public class InterfaceExamples
    {
        public static void ShowExample()
        {
            #region Пример неоднозначности интерфейсов
            GetValueClass gvc = new GetValueClass();
            // Переменная типа интерфейса может быть использована в качестве ссылки на объект, реализующий этот интерфейс
            IGetValue1 igv1 = gvc;
            IGetValue2 igv2 = gvc;
            Console.WriteLine($"Обращение к методу через объект класса: {gvc.GetValue()}");
            Console.WriteLine($"Обращение к методу через интерфейс IGetValue1: {igv1.GetValue()}");
            Console.WriteLine($"Обращение к методу через интерфейс IGetValue2: {igv2.GetValue()}");
            #endregion

            // В новые версии C# планируется добавить возможность добавлять базовую реализацию для объявляемых в
            // интерфейсах методах. По мнению разработчиков это позволит в будущем вносить изменения в интерфейс
            // без изменений в уже реализованных на его основе классах.
        }

        #region Неоднозначность интерфейсов
        public interface IGetValue1
        {
            string GetValue();
        }

        public interface IGetValue2
        {
            string GetValue();
        }
        public interface IGetValue3 : IGetValue1, IGetValue2
        { }

        public class GetValueClass : IGetValue1, IGetValue2
        {
            public string GetValue()        // соответствует обоим интерфейсам при отсутствии явной реализации какого интерфейса
            {
                return "test value";
            }

            // Явная реализация метода определенного интерфейса.
            // Здесь нет возможности указывать модификатор доступа, так как метод реализует интерфейс, а значит должен быть public
            /*public*/
            string IGetValue1.GetValue()        // можно закомментировать и проверить вывод еще раз
            {
                return "Implementation of IGetValue1 interface";
            }
        }

        #endregion

        #region Наследование интерфейсов
        // Интерфейсы можно наследовать, в результирующем будет множество всех методов родительских интерфейсов
        interface IBaseShape
        {
            /// <summary>
            /// Получение центра фигуры
            /// </summary>
            /// <returns></returns>
            System.Drawing.Point GetCenter();
        }

        interface IRectangleShape: IBaseShape
        {
            /// <summary>
            /// Получение левой верхней позиции
            /// </summary>
            /// <returns></returns>
            System.Drawing.Point GetTopLeftCorner();
            int GetWidth();
            int GetHeight();
        }

        // Класс, реализующий только один интерфейс IRectangleShape
        class Square : IRectangleShape
        {
            Point IBaseShape.GetCenter()
            {
                throw new NotImplementedException();
            }

            int IRectangleShape.GetHeight()
            {
                throw new NotImplementedException();
            }

            Point IRectangleShape.GetTopLeftCorner()
            {
                throw new NotImplementedException();
            }

            int IRectangleShape.GetWidth()
            {
                throw new NotImplementedException();
            }
        }
        #endregion
    }

    /* P.S. В новой версии C# (восьмой) ожидается появление возможности реализации методов по умолчанию.
     * Т.е. в интерфейсе появится возможность описать какую-то реализацию, котора автоматически будет добавлено во все
     * классы, которые реализуют указанный интерфейс.
     * Считается, что это позволит обогощать интерфейсы (добавлять новые методы) без необходимости дописывания всех наследников.
     */
}
