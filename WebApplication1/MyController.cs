using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Robowire.RoboApi;

namespace WebApplication1
{
    [Controller("ahoj")]
    public class MyController
    {
        public DateTime GetDate()
        {
            return DateTime.Now;
        }

        public string Reverse(string id)
        {
            return new string(id.Reverse().ToArray());
        }
    }
}