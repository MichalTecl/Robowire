using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robowire.Core
{
    public static class TypeNameHelper
    {
        private const string c_validChars = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ_";

        public static string GetTypeMark(Type t)
        {
            var tn = t.Name;

            for (var i = 0; i < tn.Length; i++)
            {
                if (!c_validChars.Contains(tn[i]))
                {
                    tn = tn.Replace(tn[i], '_');
                }
            }

            return tn;
        }
    }
}
