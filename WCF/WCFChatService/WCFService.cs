using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WCFInterfaces;

namespace WCFChatService
{
    /// <summary>
    /// Реализация сервиса обработки сообщений
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]         // Специальный атрибут, чтобы был синглтон
    class WCFService : IChatServer
    {
        /// <summary>
        /// Список известных пользователей
        /// </summary>
        List<string> _knownUsers = new List<string>();
        /// <summary>
        /// Список дат, когда последний раз получались сообщения
        /// </summary>
        Dictionary<string, DateTime> _lastUpdate = new Dictionary<string, DateTime>();
        /// <summary>
        /// Список полученных сообщений
        /// </summary>
        List<ChatMessage> _messageList = new List<ChatMessage>();

        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="inUserName"></param>
        public void RegisterUser(string inUserName)
        {
            if (!_knownUsers.Contains(inUserName))
            {
                _knownUsers.Add(inUserName);
            }
            _lastUpdate[inUserName] = DateTime.Now;
        }

        /// <summary>
        /// Получение всех сообщений для пользователя
        /// </summary>
        /// <param name="inMessage"></param>
        public void SendMessage(ChatMessage inMessage)
        {
            if (inMessage == null)
                return;
            if (!_knownUsers.Contains(inMessage.UserName))
                return;
            _messageList.Add(inMessage);
            Console.WriteLine($"{inMessage.UserName} ({inMessage.Stamp.ToString("HH:mm")}) >> {inMessage.MessageText}");
        }

        /// <summary>
        /// Отправить сообщение
        /// </summary>
        /// <param name="inUserName"></param>
        /// <returns></returns>
        public List<ChatMessage> ReceiveMessages(string inUserName)
        {
            List<ChatMessage> resList = new List<ChatMessage>();
            if (!_knownUsers.Contains(inUserName))
                return resList;
            DateTime lastUpdated = DateTime.MinValue;
            if (_lastUpdate.ContainsKey(inUserName))
                lastUpdated = _lastUpdate[inUserName];

            resList = _messageList.Where(x => (x.UserName != inUserName) && (x.Stamp >= lastUpdated)).ToList();

            _lastUpdate[inUserName] = DateTime.Now;     // Обновляем время обновления данных клиентом

            return resList;

        }
    }
}
