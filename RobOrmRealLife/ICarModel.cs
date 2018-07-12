using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Robowire.RobOrm.Core;
using Robowire.RobOrm.SqlServer.Attributes;

namespace RobOrmRealLife
{
    [Entity]
    public interface ICarModel
    {
        int Id { get; }

        int ManufacturerId { get; set; }

        IManufacturer Manufacturer { get; }

        [NVarchar(255, false)]
        string Name { get; set; }
    }
}
