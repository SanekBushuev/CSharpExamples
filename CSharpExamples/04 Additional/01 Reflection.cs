using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CSharpExamples.Additional
{
    class ReflectionExamples
    {
        /* Reflection позволяет получить информацию об объектах и типах .NET прямо во время выполнения вашего кода.
         * В пространстве имен System.Reflection представлены классы для работы с метаинформацией.
         */
        public static void ShowExample()
        {
            Console.WriteLine("Пример работы с Reflection\n\n");

            
            // Получение информации о типе
            DateTime dt = DateTime.Now;
            ShowTypeInfo(dt);

            System.Random rand = new Random();
            ShowTypeInfo(rand);

            //Вывод минимального значения. если оно объявлено в классе
            GetMinValueForObject(dt);
            GetMinValueForObject(rand);
            GetMinValueForObject(5);

            #region Неявное создание объектов стандартных классов
            // Когда в коде мы указываем какую-то константу (число или строку), мы автоматически получаем объект, у которого можно вызывать методы
            string[] parts = "1,2,4,6,6,8".Split(',');
            string[] parts2 = "1,2;3!4.5?6".Split(",.?!$".ToCharArray());

            #endregion

            #region Создание объекта по имени класса
            CreateObjectByTypeName();
            #endregion

            #region Загрузка типов из сборки
            GetAllTypesOfAssembly();
            #endregion
        }
        /// <summary>
        /// Вывод информации на консоль
        /// </summary>
        /// <param name="inmes"></param>
        static void Log(string inmes = "")
        {
            Console.WriteLine(inmes);
        }
        /// <summary>
        /// Вывод информации о типе объекта
        /// </summary>
        /// <param name="obj">Значение любого типа</param>
        private static void ShowTypeInfo(object obj)
        {
            if (obj == null)
                return;
            Type dttype = obj.GetType();
            Log($"Type name: {dttype.Name}");
            Log($"Full type name; {dttype.FullName}");
            Log($"Это класс: {dttype.IsClass}");
            Log($"Это тип значения: { dttype.IsValueType}");
            Log();
        }
        /// <summary>
        /// Получение минимального значения для типа
        /// </summary>
        /// <param name="obj"></param>
        static void GetMinValueForObject(object obj)
        {
            if (obj == null)
                return;
            #region Получение Минимального допустимого значения
            var ut = obj.GetType();
            MemberInfo[] mis = ut.GetMember("MinValue");
            if ((mis != null) && (mis.Length > 0))
            {
                MemberInfo mi = mis[0];
                object val = "";
                switch (mi.MemberType)
                {
                    case MemberTypes.Field:
                        val = ((FieldInfo)mi).GetValue(obj);
                        break;
                    case MemberTypes.Property:
                        val = ((PropertyInfo)mi).GetValue(obj);
                        break;
                    case MemberTypes.Method:
                        val = ((MethodInfo)mi).Invoke(obj, null);
                        break ;
                }
                Console.WriteLine($"{ut.Name} -> MinValue  = {val}");
            }
            else
            {
                Console.WriteLine($"{ut.Name} -> Not found MinValue");
            }
            #endregion
        }

        /// <summary>
        /// Создание экземпляра класса по его имени
        /// </summary>
        static void CreateObjectByTypeName()
        {
            Log("\nПример создания объекта по типу");
            try
            {
                // полное имя класса <namespace.classname>, <assembly name>
                string typename = "CSharpExamples.Classes.InterfaceExamples+GetValueClass, CSharpExamples";       
                // GetValueClass является вложенным в другой класс и должен указываться через "+"
                // Можно опускать имя сборки.
                //string typename = "CSharpExamples.Classes.InterfaceExamples+GetValueClass";       
                Log($"Создаем объект класса '{typename}'");
                Type type = Type.GetType(typename);
                if (type == null)
                {
                    Log($"Не найден тип '{typename}'");
                    return;
                }
                var obj = Activator.CreateInstance(type);
                if (obj == null)
                {
                    Log($"Не удалось создать объект типа {typename}");
                    return;

                }
                // далее приводим объект к интересующему типу или интерфейсу и работаем с ним
                CSharpExamples.Classes.InterfaceExamples.IGetValue1 igv1 = obj as Classes.InterfaceExamples.IGetValue1;
                Classes.InterfaceExamples.IGetValue2 igv2 = obj as Classes.InterfaceExamples.IGetValue2;
                if (igv1 == null)
                    Log($"Не удалось привести объект к интерфейсу IGetValue1");
                else
                    Log($"IGetValue1.GetValue ->  {igv1.GetValue()}");

                if (igv2 == null)
                    Log($"Не удалось привести объект к интерфейсу IGetValue2");
                else
                    Log($"IGetValue2.GetValue ->  {igv2.GetValue()}");
            }
            catch (Exception ex)
            {
                Log($"Ошибка при попытке создать экземпляр по имени типа: {ex.ToString()}");

            }
        }

        /// <summary>
        /// Получить все типы, описанные в сборке
        /// </summary>
        static void GetAllTypesOfAssembly()
        {
            Log("\nПолучаем все типы сборки по имени");
            //string assemblyname = @"C:\Users\Александр\Documents\Projects\CSharpExamples\bin\Debug\CSharpExamples.exe";
            string assemblyname = Assembly.GetExecutingAssembly().Location;
            if (!System.IO.File.Exists(assemblyname))
            {
                Log($"Сборка '{assemblyname}' не найдена");
                return;
            }
            var assembly = Assembly.LoadFile(assemblyname);
            if (assembly == null)
            {
                Log("Не удалось загрузить сборку");
                return;
            }

            foreach (Type type in assembly.GetTypes())
            {
                Log($"Type: {type.FullName}");
            }

            // Получение всех типов, которые реализуют интерфейс IGetValue1
            Log("\n\nВсе типы, реализующие интерфейс IGetValue1:");

            var getvaluetypes = assembly.GetTypes().Where(x => x.IsAssignableFrom(typeof(Classes.InterfaceExamples.IGetValue1)));
            foreach (Type type in getvaluetypes)
                Log($"Type: {type.FullName}");
        }
    }//
}
