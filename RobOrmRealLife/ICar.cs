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
    public interface ICar : IEntityBase, IModelRelated
    {

        [NVarchar(255, false)]
        string Note { get; set; }

        int OwnerId { get; set; }

        ICustomer Owner { get; }

        [ForeignKey(nameof(IServiceCenter.Id))]
        IServiceCenter SoldIn { get; }

        int SoldInId { get; set; }
    }
}
