using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Message.Events
{
    public class HadiTestEvent : IntegrationBaseEvent
    {
        public string Name { get; set; }
    }
}
