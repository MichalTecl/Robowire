using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobOrmRealLife
{
    public interface IModelRelated
    {

        int ModelId { get; set; }

        ICarModel Model { get; }
    }
}
