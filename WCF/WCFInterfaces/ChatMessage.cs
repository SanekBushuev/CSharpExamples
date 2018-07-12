using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace WCFInterfaces
{
    // подключить System.Runtime.Serialization

    /// <summary>
    /// Интерфейс сообщения в чате
    /// </summary>
    [DataContract]
    public class ChatMessage
    {
        [DataMember]
        public string UserName { get; protected set; }        // обязательно должен быть setter
        [DataMember]
        public string MessageText { get; protected set; }
        [DataMember]
        public DateTime Stamp { get; protected set; }

        public ChatMessage(string inUN, string inMes, DateTime inStamp)
        {
            UserName = inUN;
            MessageText = inMes;
            Stamp = inStamp;
        }
    }
}
