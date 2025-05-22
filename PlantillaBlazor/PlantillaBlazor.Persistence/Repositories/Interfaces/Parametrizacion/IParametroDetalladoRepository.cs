using PlantillaBlazor.Domain.Entities.Parametrizacion;
using PlantillaBlazor.Persistence.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Persistence.Repositories.Interfaces.Parametrizacion
{
    public interface IParametroDetalladoRepository : IGenericRepository<ParametroDetallado>
    {
        public Task<bool> EliminarParametroDetallado(long id);
    }
}
