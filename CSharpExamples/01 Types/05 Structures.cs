using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpExamples.Types
{
    class StructuresExamples
    {
        // Описание структур: https://docs.microsoft.com/ru-ru/dotnet/csharp/programming-guide/classes-and-structs/using-structs
        /* В отличие от классов, структуры не поддерживают наследование - Структура не может наследовать от другой структуры 
         * или класса и не может быть базовой для класса. 
         * Однако структуры наследуются от базового класса Object. Любой экземпляр структуры может быть приведен к Object.
         * Структуры могут реализовывать интерфейсы так же, как это делают классы.
         */
    }

    /// <summary>
    /// Структура для описания прямоугольника
    /// </summary>
    struct Rectangle
    {
        public int Left;
        public int Top;
        public int Width { get; set; }
        public int Height { get; set; }
        
        //Constructor
        public Rectangle(int left, int top, int width, int height)
        {
            Left = left;
            Top = top;
            Width = width;
            Height = height;
        }
    }

    /// <summary>
    /// Интерфейс для всех фигур
    /// </summary>
    interface IShape
    {
        int Left { get; set; }
        int Top { get; set; }
    }
    struct Circle : IShape
    {
        // Interface IShape
        public int Left { get; set; }
        public int Top {  get; set; }

        public int Radius { get; set; }

        public Circle(int Left, int Top, int Radius)
        {
            this.Left = Left;
            this.Top = Top;
            this.Radius = Radius;
        }
    }
}
