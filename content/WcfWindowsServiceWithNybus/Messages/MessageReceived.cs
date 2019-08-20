using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nybus;

namespace EMG.WcfWindowsServiceWithNybus.Messages
{
    public class MessageReceived : IEvent
    {
        public string Message { get; set; }
    }
}
