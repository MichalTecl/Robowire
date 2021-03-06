﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Robowire.RobOrm.Core;
using Robowire.RobOrm.SqlServer.Attributes;

namespace RobOrmRealLife
{
    [Entity]
    public interface IManufacturer : IEntityBase
    {
        [NVarchar(255, false)]
        string Name { get; set; }

        IEnumerable<ICarModel> Models { get; }
    }
}
