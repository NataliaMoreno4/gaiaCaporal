using PlantillaBlazor.Domain.Entities.Parametrizacion;
using PlantillaBlazor.Persistence.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Persistence.Repositories.Interfaces.Parametrizacion
{
    public interface IParametroGeneralRepository : IGenericRepository<ParametroGeneral>
    {
        public Task<bool> InsertarParametroGeneral(ParametroGeneral parametroGeneral);
        public Task<bool> ActualizarParametroGeneral(ParametroGeneral parametroGeneral);
        public Task<ParametroGeneral> ConsultarParametroGeneralById(long id);
        public Task<bool> EliminarParametroGeneral(long id);
    }
}
