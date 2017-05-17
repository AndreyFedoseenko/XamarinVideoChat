using DeviceSpecificAppServerService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceSpecificAppServerService.Interfaces
{
    public interface IOpenToxProvider
    {
        SessionInfo GetSession();
    }
}
