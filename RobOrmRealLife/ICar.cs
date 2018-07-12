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
    public interface ICar
    {
        int Id { get; }

        [NVarchar(255, false)]
        string Note { get; set; }

        int OwnerId { get; set; }

        ICustomer Owner { get; }

        int ModelId { get; set; }

        ICarModel Model { get; }

        [ForeignKey(nameof(IServiceCenter.Id))]
        IServiceCenter SoldIn { get; }

        int SoldInId { get; set; }
    }
}
