using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using WCFInterfaces;

namespace WCFChatService
{
    class Program
    {
        // Приложение хостящее WCF-сервис
        static void Main(string[] args)
        {
            Console.WriteLine("Хостинг WCFService");

            // Разворачиваем хостинг
            // Объект, который будет выполнять функции сервера
            WCFService service = new WCFService();

            // Технические настройки для запуска хоста
            string address = "net.tcp://localhost:9090/ChatService";
            // Хост-процесс для сервиса
            ServiceHost service_host = new ServiceHost(service, new Uri(address));
            // настраиваем, как будет обрабатываться сервис (как единный объект или по новому для каждого запроса)
            ServiceMetadataBehavior behavior = new ServiceMetadataBehavior();
            service_host.Description.Behaviors.Add(behavior);
            // Настройки сетевого подключения
            NetTcpBinding tcpbinding = new NetTcpBinding();
            tcpbinding.Security.Mode = SecurityMode.Transport;
            tcpbinding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            tcpbinding.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
            tcpbinding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
            // Добавляем точку обработки (указываем интерфейс обработчика, сетевые настройки и прослушиваемый адрес)
            service_host.AddServiceEndpoint(typeof(IChatServer), tcpbinding, address);

            // Специальная "точка" для возможности подключения к проектам из студии
            service_host.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexTcpBinding(), "mex");
            // Запускаем хост
            service_host.Open();

            Console.WriteLine("WCFService готов к работе.\nПо завершению нажмите любую клавишу");
            Console.ReadKey();
        }
    }//
}
