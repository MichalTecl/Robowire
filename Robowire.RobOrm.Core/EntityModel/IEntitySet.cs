using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robowire.RobOrm.Core.EntityModel
{
    public interface IEntitySet : IEnumerable<IEntity>
    {
        IEntity Find(object primaryKeyValue);

        void Add(IEntity entity);
    }
}
