using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WCFInterfaces;

namespace WCFChatClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Приложение-клиент WCFService");

                // Создаем объект класса-клиента WCFService
                string endPoint = "net.tcp://localhost:9090/ChatService";                           // Сетевой адрес сервера
                var netTcpBinding = new NetTcpBinding();        // Объект-описатель используемых настроек
                _channelFactory = new ChannelFactory<IChatServer>(netTcpBinding, endPoint);
                _client = _channelFactory.CreateChannel();


                Console.Write("Введите свое имя: ");
                UserName = Console.ReadLine();

                // Регистрируем пользователя
                _client.RegisterUser(UserName);

                // Запускаем таймер получения сообщений с сервера 
                mainLoopTimer = new System.Timers.Timer(10000);     // срабатывание каждые 10 секунд
                mainLoopTimer.Elapsed += MainLoopTimer_Elapsed;
                mainLoopTimer.Start();

                Console.WriteLine("Подключены к серверу. Для прекращения введите '...'");
                do
                {
                    Console.Write($"{UserName} >> ");
                    string message = Console.ReadLine();
                    if (string.IsNullOrEmpty(message))
                        continue;
                    if (message == "...")
                        break;
                    _client.SendMessage(new ChatMessage(UserName, message, DateTime.Now));
                }
                while (true);

                mainLoopTimer.Stop();

                Console.WriteLine("Работа клиента завершена.");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                ShowException(ex);
            }
        }

        /// <summary>
        /// Таймер, который будет регулярно запрашивать список новых сообщений у сервиса-сервера
        /// </summary>
        static System.Timers.Timer mainLoopTimer;
        /// <summary>
        /// Специальная фабрика создающая подключение к удаленным сервисам
        /// </summary>
        static ChannelFactory<IChatServer> _channelFactory = null;
        /// <summary>
        /// Общий интерфейс для всех чат-сервисов
        /// </summary>
        static IChatServer _client = null;
        static string UserName = "";

        private static void MainLoopTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                mainLoopTimer.Stop();
                // Обновляем список сообщений и выводим их на экран
                if (_client == null)
                    return;

                List<ChatMessage> newMessages = _client.ReceiveMessages(UserName);
                if ((newMessages != null) && (newMessages.Count > 0))
                    Console.WriteLine();
                foreach (var mes in newMessages)
                {
                    Console.WriteLine($"{mes.UserName} ({mes.Stamp.ToString("HH:mm")}) >> {mes.MessageText}");
                }
                if ((newMessages != null) && (newMessages.Count > 0))
                    Console.Write($"{UserName} >> ");
            }
            catch (Exception ex)
            {
                ShowException(ex);
            }
            finally
            {
                mainLoopTimer.Start();
            }
        }

        static private void ShowException(Exception ex)
        {
            var origColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"EXCEPTION: {ex.Message}");
            Console.ForegroundColor = origColor;
        }
    }
}
