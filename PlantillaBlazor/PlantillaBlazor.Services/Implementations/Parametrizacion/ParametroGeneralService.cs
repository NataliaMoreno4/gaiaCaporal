using PlantillaBlazor.Services.Interfaces.Parametrizacion;
using PlantillaBlazor.Domain.Entities.Parametrizacion;
using PlantillaBlazor.Persistence.Repositories.Interfaces.Parametrizacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using PlantillaBlazor.Domain.Common.ResultModels;

namespace PlantillaBlazor.Services.Implementations.Parametrizacion
{
    public class ParametroGeneralService : IParametroGeneralService
    {
        private readonly IParametroGeneralRepository _parametroGeneralRepository;
        private readonly IParametroDetalladoRepository _parametroDetalladoRepository;

        public ParametroGeneralService(
            IParametroGeneralRepository parametroGeneralRepository,
            IParametroDetalladoRepository parametroDetalladoRepository
            )
        {
            _parametroDetalladoRepository = parametroDetalladoRepository;
            _parametroGeneralRepository = parametroGeneralRepository;
        }

        public async Task<ParametroGeneral> ConsultarParametroGeneralById(long id)
        {
            var filtros = new List<Expression<Func<ParametroGeneral, bool>>>();

            filtros.Add(p => p.Id == id);

            var includes = new List<Expression<Func<ParametroGeneral, object>>>();

            includes.Add(p => p.ListaParametrosDetallados);

            IEnumerable<ParametroGeneral> temp = await _parametroGeneralRepository.Get(filtros: filtros, includes: includes);

            return temp.FirstOrDefault();
        }

        public async Task<IEnumerable<ParametroGeneral>> ConsultarParametrosGenerales()
        {
            var includes = new List<Expression<Func<ParametroGeneral, object>>>();

            includes.Add(p => p.ListaParametrosDetallados);

            return await _parametroGeneralRepository.Get(includes: includes);

        }

        public async Task<bool> EliminarParametroDetallado(long id)
        {
            return await _parametroDetalladoRepository.EliminarParametroDetallado(id);
        }

        public async Task<bool> EliminarParametroGeneral(long id)
        {
            return await _parametroGeneralRepository.EliminarParametroGeneral(id);
        }

        public async Task<Result<long>> InsertarInfoParametroGeneral(ParametroGeneral parametroGeneral)
        {
            #region Validacion

            if (string.IsNullOrEmpty(parametroGeneral.Nombre))
            {
                return Result<long>.Failure("Debe indicar el nombre del parámetro general");
            }

            if (parametroGeneral.ListaParametrosDetallados.Count <= 0)
            {
                return Result<long>.Failure("Debe indicar al menos un parámetro detallado");
            }

            foreach (var pDetallado in parametroGeneral.ListaParametrosDetallados)
            {
                if (string.IsNullOrEmpty(pDetallado.Nombre))
                {
                    return Result<long>.Failure($"Debe indicar el nombre para el parámetro detallado #{parametroGeneral.ListaParametrosDetallados.IndexOf(pDetallado) + 1}");
                }
            }

            #endregion

            ParametroGeneral temp = await _parametroGeneralRepository.ConsultarParametroGeneralById(parametroGeneral.Id);

            if (temp is null)
            {
                if (await _parametroGeneralRepository.InsertarParametroGeneral(parametroGeneral))
                {
                    return Result<long>.Success(parametroGeneral.Id);
                }
            }
            else
            {
                if (await _parametroGeneralRepository.ActualizarParametroGeneral(parametroGeneral))
                {
                    return Result<long>.Success(parametroGeneral.Id);
                }
            }

            return Result<long>.Failure("No fue posible guardar la información");
        }
    }
}
