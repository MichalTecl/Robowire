using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robowire
{
    public interface IGeneratedCodeListener
    {
        void OnContainerGenerated(string containerCode, bool hasErrors, Exception errors);
    }
}
