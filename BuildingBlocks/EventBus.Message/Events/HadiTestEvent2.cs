using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Message.Events
{
    public class HadiTestEvent2 : IntegrationBaseEvent
    {
        public string Name { get; set; }
        public string Family { get; set; }
    }
}
