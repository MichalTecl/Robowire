using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Robowire.RobOrm.Core;

namespace RobOrmRealLife
{
    [Entity]
    public interface IServiceCheck : IEntityBase
    {
        IServiceCenter Center { get; }
        int CenterId { get; set; }

        ICar Subject { get; }

        int? SubjectId { get; set; }
    }

}
