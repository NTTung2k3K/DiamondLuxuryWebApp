using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.BackgroundServiceHost.Service.MaterialService
{
    public interface IMaterialService
    {
        Task CreateDefaultMaterial(CancellationToken cancellationToken);

    }
}
