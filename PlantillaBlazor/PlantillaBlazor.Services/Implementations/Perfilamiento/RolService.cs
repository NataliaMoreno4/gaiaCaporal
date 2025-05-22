using PlantillaBlazor.Services.Interfaces.Auditoria;
using PlantillaBlazor.Services.Interfaces.Perfilamiento;
using PlantillaBlazor.Domain.Entities.Auditoria;
using PlantillaBlazor.Domain.Entities.Perfilamiento;
using PlantillaBlazor.Persistence.Repositories.Interfaces.Perfilamiento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using PlantillaBlazor.Domain.Common.ResultModels;

namespace PlantillaBlazor.Services.Implementations.Perfilamiento
{
    public class RolService : IRolService
    {
        private readonly IRolRepository _rolRepository;
        private readonly IModuloService _moduloService;
        private readonly IAuditoriaService _auditoriaService;
        public RolService
        (
            IRolRepository rolRepository,
            IModuloService moduloService,
            IAuditoriaService auditoriaService
        )
        {
            _rolRepository = rolRepository;
            _moduloService = moduloService;
            _auditoriaService = auditoriaService;
        }

        public async Task<IEnumerable<Modulo>> GetModulosRol(long idRol)
        {
            var filtros = new List<Expression<Func<Rol, bool>>>();
            var includes = new List<Expression<Func<Rol, object>>>();

            filtros.Add(r => r.Id == idRol);
            includes.Add(r => r.ListaRolModulo);

            var roles = await _rolRepository.Get(filtros: filtros, includes: includes);

            var rol = roles.FirstOrDefault();

            if (rol is null) return new List<Modulo>();

            IEnumerable<Modulo> modulos = await _moduloService.GetModulos();

            return rol.ListaRolModulo.Select(rm => modulos.FirstOrDefault(m => m.Id == rm.IdModulo));
        }

        public IEnumerable<Modulo> GetModulosRolSync(long idRol)
        {
            var filtros = new List<Expression<Func<Rol, bool>>>();
            var includes = new List<Expression<Func<Rol, object>>>();

            filtros.Add(r => r.Id == idRol);
            includes.Add(r => r.ListaRolModulo);

            var roles = _rolRepository.GetSync(filtros: filtros, includes: includes);

            var rol = roles.FirstOrDefault();

            if (rol is null) return new List<Modulo>();

            IEnumerable<Modulo> modulos = _moduloService.GetModulosSync();

            return rol.ListaRolModulo.Select(rm => modulos.FirstOrDefault(m => m.Id == rm.IdModulo));
        }

        public async Task<Rol> GetRol(long idRol)
        {
            return await _rolRepository.GetRolById(idRol);
        }

        public async Task<IEnumerable<Rol>> GetRoles()
        {
            var includes = new List<Expression<Func<Rol, object>>>();

            includes.Add(r => r.ListaRolModulo);

            return await _rolRepository.Get(includes: includes);
        }

        public IEnumerable<Rol> GetRolesSync()
        {
            return _rolRepository.GetSync();
        }

        public async Task<Result<long>> InsertarRol(Rol rol, string ipAddress)
        {
            Result<bool> response = ValidarInformacionRol(rol);

            if (!response.IsSuccess) return Result<long>.Failure(response.Error);

            Rol rolDb = await _rolRepository.GetById(rol.Id);

            if (!await _rolRepository.InsertarRol(rol))
            {
                return Result<long>.Failure("No fue posible guardar la información");
            }

            if (rolDb is null)
            {
                AuditoriaEvento auditoriaEvento = new AuditoriaEvento()
                {
                    Accion = "Registro de información de rol nuevo",
                    Concepto = nameof(Rol),
                    Descripcion = "Se ha registrado un nuevo rol",
                    IdentificadorProceso = rol.Id.ToString(),
                    IpAddress = ipAddress,
                    IdUsuarioAccion = rol.IdUsuarioAdiciono.GetValueOrDefault(),
                };

                await _auditoriaService.RegistrarAuditoriaEvento(auditoriaEvento);
            }
            else
            {
                AuditoriaEvento auditoriaEvento = new AuditoriaEvento()
                {
                    Accion = "Actualizacion de información de rol",
                    Concepto = nameof(Rol),
                    Descripcion = "Se ha actualizado la información del rol",
                    IdentificadorProceso = rol.Id.ToString(),
                    IpAddress = ipAddress,
                    IdUsuarioAccion = rol.IdUsuarioUltimaActualizacion.GetValueOrDefault(),
                };

                await _auditoriaService.RegistrarAuditoriaEvento(auditoriaEvento);
            }

            return Result<long>.Success(rol.Id);
        }

        public Result<bool> ValidarInformacionRol(Rol rol)
        {
            #region Validación

            if (string.IsNullOrEmpty(rol.Nombre))
            {
                return Result<bool>.Failure("Debe indicar el nombre del rol");
            }

            return Result<bool>.Success(true);

            #endregion
        }
    }
}
