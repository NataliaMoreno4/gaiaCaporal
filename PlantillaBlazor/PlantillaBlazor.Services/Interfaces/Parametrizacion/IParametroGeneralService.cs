using PlantillaBlazor.Domain.Common.ResultModels;
using PlantillaBlazor.Domain.Entities.Parametrizacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Services.Interfaces.Parametrizacion
{
    public interface IParametroGeneralService
    {
        public Task<Result<long>> InsertarInfoParametroGeneral(ParametroGeneral parametroGeneral);
        public Task<IEnumerable<ParametroGeneral>> ConsultarParametrosGenerales();
        public Task<ParametroGeneral> ConsultarParametroGeneralById(long id);
        public Task<bool> EliminarParametroGeneral(long id);
        public Task<bool> EliminarParametroDetallado(long id);
    }
}
