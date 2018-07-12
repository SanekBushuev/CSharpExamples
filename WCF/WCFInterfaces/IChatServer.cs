using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;


namespace WCFInterfaces
{
    // https://docs.microsoft.com/en-us/dotnet/framework/wcf/how-to-host-a-wcf-service-in-a-managed-application
    // подключить System.ServiceModel

    [ServiceContract]
    public interface IChatServer
    {
        /// <summary>
        /// Зарегистрировать пользователя
        /// </summary>
        /// <param name="inUserName"></param>
        [OperationContract]
        void RegisterUser(string inUserName);
        /// <summary>
        /// Отправить сообщение
        /// </summary>
        /// <param name="inMessage"></param>
        [OperationContract]
        void SendMessage(ChatMessage inMessage);
        /// <summary>
        /// Получить сообщения
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<ChatMessage> ReceiveMessages(string inUserName);
    }
}
