using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;        // В этом пространстве имен описаны классы для работы с MS SQL Server
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace CSharpExamples.Additional
{
    /// <summary>
    /// Набор примеров работы с базой данных - MS SQL Server
    /// </summary>
    class RDBMSExamples
    {
        /* RDBMS - система управлениями реляционными базами данных
         * В .NET реализовали специальную абстракцию для работы с базами даных - ADO.NET
         * ADO.NET позволяет единообразно, без знания особенностей конкретной СУБД, работать с множеством различных СУБД
         * Использование абстракций ADO.NET позволяет реализовать СУБД-агностик решение, которое может работать с различными СУБД
         * без изменения кода
         */
        // http://ConnectionStrings.net - сайт содержит варианты формирования строки подключения для ADO.NET для большинства СУБД

        public static void ShowExample()
        {
            Console.WriteLine("Пример c СУБД\n\n");

            //Log("Simple request example");
            //SimpleRequest();

            //Log("Parametric request example");
            //ParametricRequest();
            //// Вывод через SimpleRequest
            //SimpleRequest();

            //Log("Data adapter example");
            //DataAdapterRequest();

            //Log("Dapper example");
            DapperExample();
        }

        static void Log(string inMes = "")
        {
            Console.WriteLine(inMes);
        }

        /// <summary>
        /// Строка подключения к локальному серверу БД
        /// Скрипт создания тестовой БД на MS SQL Server Express в конце файла
        /// </summary>
        private static string GLOBAL_CONNECTION_STRING = @"Data Source=DESKTOP-RSMVHC8\SQLEXPRESS; Initial Catalog=testDB; User Id=testuser;Password=testuser; MultipleActiveResultSets=True";

        #region RDBMS
        private static void SimpleRequest()
        {
            // 1. Создаем объект DbConnection - подключение к БД (указываем в конструкторе строку подключения к БД)
            DbConnection conn = new System.Data.SqlClient.SqlConnection(GLOBAL_CONNECTION_STRING);
            try
            {
                // 2. Обязательно открываем подключение к БД
                conn.Open();
                
                string sqlQuery = @"select id, Name, SecName, Position, BirthDay, IQ from UserNames";

                // 3. Создаем команду
                var cmd = conn.CreateCommand();
                // 4. Указываем запрос, который необходимо выполнить
                cmd.CommandText = sqlQuery;
                // 5. Выполняем запрос и возвращаем результат в DataReader
                //cmd.exec
                DbDataReader dr = cmd.ExecuteReader();
                try
                {
                    // 6. Получаем информацию о столбцах
                    for (int fc = 0; fc < dr.FieldCount; fc++)
                        Log($"Field: {dr.GetName(fc)}, type = {dr.GetFieldType(fc)}");

                    // 7. Выводим информацию из результата запроса
                    int rowCnt = 1;
                    while (dr.Read())           // Вычитывает строчку из БД и предоставляет доступ к прочитанным данным через DataReader
                    {       // dr.get<T> - интерфейс доступа к данным строки БД. Индексация с 0.
                            // dr[i] - получение результата в виде object
                        string line = "";
                        //dr.GetString(1);
                        //dr.IsDBNull(1);
                        //DBNull.Value
                        for (int fc = 0; fc < dr.FieldCount; fc++)
                            line += dr[fc].ToString() + "\t";
                        Log($"Row #{rowCnt}: {line}");
                        rowCnt++;
                    }
                }
                finally
                {
                    if (dr != null)
                        dr.Close();
                }
            }
            catch (Exception ex)
            {
                Log($"EXCEPTION: {ex.Message}");
                throw new Exception("Ошибка в методе", ex);
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }

        /// <summary>
        /// Параметрические запросы используются для того, чтобы 
        ///     избежать необходимости преобразовывать значения в тот формат, который будет понят БД
        ///     многократно выполнить одинаковый запрос с разными значениями
        ///     избежать SQL-инъекций (механизм подстановки параметров в запросы защищает от изменения запроса и вставки вредоносного кода)
        ///         Если у нас запрос "SELECT * from table where id = "
        ///             в который мы в конец пытаемся самостоятельно добавить то, что ввел пользователь. 
        ///             Если пользователь введет в поле ввода: 
        ///                 1; drop database;
        ///             он может удалить БД
        /// 
        /// </summary>
        private static void ParametricRequest()
        {
            using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(GLOBAL_CONNECTION_STRING))
            {
                conn.Open();
                
                //insert into UserNames (Name, SecName, Position, BirthDay, iq)
                //values('Vlad', 'Mamaev', 'Head of unit', '1988-02-14', 169);
                
                string sqlQuery = @"insert into UserNames (Name, SecName, Position, BirthDay, iq) 
                                    values (@Name, @SecName, @Position, @BirthDay, @iq)";
                // В запросе, места, в которые должны подставляться значения задаются с помощью @<имя>
                var cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;

                // Описание параметров происходит без указания символа @
                cmd.Parameters.Add(new SqlParameter("Name", System.Data.SqlDbType.VarChar, 40));
                cmd.Parameters.Add(new SqlParameter("SecName", System.Data.SqlDbType.VarChar, 50));
                cmd.Parameters.Add(new SqlParameter("Position", System.Data.SqlDbType.VarChar, 100));
                cmd.Parameters.Add(new SqlParameter("BirthDay", System.Data.SqlDbType.DateTime));
                cmd.Parameters.Add(new SqlParameter("IQ", System.Data.SqlDbType.Int));
                // Параметрический запрос можно "подготовить" - выполнить часть функционала, связанного с предобработкой запроса
                // В результате многократные вызовы подобного запроса могут происходить быстрее
                cmd.Prepare();      // подготовка запроса

                // Значения задаются с помощью 
                cmd.Parameters[0].Value = "Сергей";
                cmd.Parameters[1].Value = "Обухов";
                cmd.Parameters[2].Value = "Ведущий специалист";
                cmd.Parameters[3].Value = (new DateTime(1994, 7, 10)).AddDays(2500 + (new Random()).Next(365));
                cmd.Parameters[4].Value = 70 + (new Random()).Next(60);
                
                cmd.ExecuteNonQuery();
                Log("Строка добавлена");
            }
        }

        /// <summary>
        /// Небольшой пример SQL-инъекции
        /// </summary>
        /// <param name="inNum"></param>
        /// <param name="inStr"></param>
        private static void SQLInjection(int inNum, string inStr)
        {
            string sqlQuery = @"INSERT INTO testTable (intVal, strVal) values (" + inNum.ToString() + ", '" + inStr + "')";
            //INSERT INTO testTable (intVal, strVal) values ( 123 , 'asdfasd')
            // в строковый параметр -> '); drop database; select max('asdfasd
            // Результирующий запрос -> INSERT INTO testTable (intVal, strVal) values ( 123 , ''); drop database; select max('asdfasd')
        }

        /// <summary>
        /// Механизм дата адаптеров позволяет настроить способ отображения данных из таблицы БД в таблицу в объекте DataSet
        /// Отображение может быть настроено в обе стороны: из БД в объект и из объекта в БД
        /// DataSet - это абстракция базы данных в памяти во время выполнения программы
        /// В DataSet могут быть таблицы и связи между ними
        /// </summary>
        private static void DataAdapterRequest()
        {
            using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(GLOBAL_CONNECTION_STRING))
            {
                conn.Open();
                string sqlQuery = "select * from UserNames";    // Строка выборки данных для Дата Адаптера
                //Формируем дата адаптер для выборки данных из таблицы
                System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter(sqlQuery, conn);
                //Создаем объект DataSet
                System.Data.DataSet ds = new System.Data.DataSet();
                // С помощью дата адаптера заливаем данные в таблицу DataSet'а
                sda.Fill(ds);   // Дата адаптер подтягивает не только данные, но и структуру таблицы в БД

                // Объявляем метод с помощью лямбда-выражения. Метод выводит строки из первой таблицы в DataSet
                Action printDS = () =>
                {
                    Log("Print DS table[0] rows");
                    int rowcnd = 1;
                    foreach (System.Data.DataRow row in ds.Tables[0].Rows)
                    {
                        Log($"#{rowcnd}: {row[0]}\t{row[1]}");
                        rowcnd++;
                    }
                };
                printDS();      // вызов метода

                // Добавляем данные в таблицу в DataSet
                System.Data.DataRow drow = ds.Tables[0].NewRow();
                drow[1] = "Екатерина";
                drow[2] = "Кузнецова";
                drow[3] = "Руководитель проекта";
                drow[4] = new DateTime(1990, 09, 13);
                drow[5] = 130;
                ds.Tables[0].Rows.Add( drow);
                printDS();      // вызов метода

                // Обновляем данные в базе данных
                //Требуется определить команду, с помощью которой будет выполняться вставка в таблицу значений
                //sda.InsertCommand = @"insert into UserNames (Name, SecName, Position, BirthDay, iq) 
                //                    values (@Name, @SecName, @Position, @BirthDay, @iq)";
                //sda.Update(ds);
                
            }
        }
        #endregion

        #region Dapper
        // Dapper - это ORM, используемая на StackOverflow. Позволяет минимизировать операции вычитки данных из БД в объекты. 
        // И из объектов в БД
        // Подключаем пространство имен Dapper
        // Dapper расширяет стандартный Connection и добавляет в него несколько методов

        // Класс-хранилище информации о пользователе
        class UserInfo
        {
            public string Name { get; set; }
            public string SecName { get; set; }
            public string Position { get; set; }
            public DateTime BirthDay { get; set; }
            public int IQ { get; set; }
        }

        private static void DapperExample()
        {
            // select Name, SecName, Position, BirthDay, IQ from UserNames;
            using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(GLOBAL_CONNECTION_STRING))
            {
                conn.Open();
            
                // Чтение данных с помощью Dapper
                var userColl = conn.Query<UserInfo>("select Name, SecName, Position, BirthDay, IQ from UserNames");
                foreach (var usin in userColl)
                {
                    Log($"\t{usin.Name} {usin.SecName}, {usin.Position}, BD: {usin.BirthDay:yyyy-MM-dd}, IQ: {usin.IQ}");
                }

                // Вставка данных
                // Для вставки данных используется команда Execute
                // Параметры @par заменяются соответствующими полями объекта, переданного в качестве параметра
                conn.Execute(@"insert into UserNames (Name, SecName, Position, BirthDay, iq)
                            values (@par1, @par2, @par3, @par4, @par5)",
                            new object[]
                            {
                                new { par1 = "Вячеслав", par2 = "Кудрявцев", par3 = "Специалист", par4 = new DateTime(1999, 5, 25), par5 = 120 },
                                new { par1 = "Вячеслав2", par2 = "Кудрявцев2", par3 = "Специалист2", par4 = new DateTime(1999, 5, 25), par5 = 120 }
                            });

                //UserInfo UserInfoObj = null;
                //conn.Execute(@"insert into UserNames (Name, SecName, Position, BirthDay, iq)
                //            values (Name, SecName, Position, BirthDay, Iq)",
                //            UserInfoObj);

                // Если мы имеем объект типа UserInfo, то мы можем назвать параметры в соответствии с именами полей/свойств
                // и передавать в качестве параметра конкретный объект с данными
            }
        }
        #endregion
    }
}

/* Скрипт создания тестовой БД с именем testDB:
 * 
 -- Запрос списка таблиц
select * from information_schema.tables;

-- Создание таблицы для хранения данных

create table UserNames 
(
	id int identity,
	Name varchar(40),
	SecName varchar(50),
	Position varchar(100),
	BirthDay datetime,
	iq int
)

-- Первичное заполнение данными

insert into UserNames(Name, SecName, Position, BirthDay, iq)
values('Alex', 'Bushuev', 'Chief expert', '1994-12-12', 169);

insert into UserNames(Name, SecName, Position, BirthDay, iq)
values('Vlad', 'Mamaev', 'Head of unit', '1988-02-14', 169);

-- Выборка данных из целевой таблицы
select* from UserNames;
*/