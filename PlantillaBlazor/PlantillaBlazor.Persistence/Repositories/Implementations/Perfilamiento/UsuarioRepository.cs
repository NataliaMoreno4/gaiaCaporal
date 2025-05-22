using PlantillaBlazor.Domain.Entities.Perfilamiento;
using PlantillaBlazor.Persistence.Data;
using PlantillaBlazor.Persistence.Repositories.Common;
using PlantillaBlazor.Persistence.Repositories.Interfaces.Perfilamiento;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Persistence.Repositories.Implementations.Perfilamiento
{
    public class UsuarioRepository : EFCoreRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(IDbContextFactory<AppDbContext> dbContextFactory) : base(dbContextFactory)
        {
        }

        public async Task<long> GetCantidadIntentosFallidos(long idUsuario, int cantidadMinutos)
        {
            using var context = _dbContextFactory.CreateDbContext();

            return await context.AuditoriaLoginUsuario
                .Where(a => a.IdUsuario == idUsuario &&
                    EF.Functions.DateDiffMinute(a.FechaAdicion, DateTime.Now) <= 5 &&
                    a.Descripcion == "No exitoso, credenciales incorrectas")
                .CountAsync();
        }

        public async Task<bool> InactivarUsuariosNoActivos(int diasDesdeUltimoLoggeo)
        {
            using var context = _dbContextFactory.CreateDbContext();

            var usuarios = context.Usuarios
                .Where(u => EF.Functions.DateDiffDay(u.FechaUltimoIngreso, DateTime.Now) >= diasDesdeUltimoLoggeo
                && u.FechaUltimoIngreso != null && u.IsActive)
                .ToList();

            var usuarios2 = context.Usuarios
                .Where(u => EF.Functions.DateDiffDay(u.FechaAdicion, DateTime.Now) >= diasDesdeUltimoLoggeo
                && u.FechaUltimoIngreso == null && u.IsActive)
                .ToList();

            var usuariosFinal = new List<Usuario>();
            usuariosFinal.AddRange(usuarios);
            usuariosFinal.AddRange(usuarios2);

            usuariosFinal.ForEach(u => u.IsActive = false);

            context.Usuarios.UpdateRange(usuarios);

            int entities = await context.SaveChangesAsync();

            return entities > 0;
        }

        public async Task<bool> InsertarUsuario(Usuario usuario)
        {
            using var context = _dbContextFactory.CreateDbContext();

            Usuario temp = context.Usuarios
                .AsNoTracking()
                .FirstOrDefault(u => u.Id == usuario.Id);

            if (temp is null)
            {
                usuario.FechaAdicion = DateTime.Now;
                context.Usuarios.Add(usuario);
            }
            else
            {
                usuario.FechaUltimaActualizacion = DateTime.Now;
                context.Usuarios.Update(usuario);
            }

            int entities = await context.SaveChangesAsync();

            return entities > 0;
        }

        public async Task<bool> RegistrarAuditoriaCierreSesion(AuditoriaLoginUsuario auditoria)
        {
            using var context = _dbContextFactory.CreateDbContext();

            var auditoriaDb = context.AuditoriaLoginUsuario
                .AsNoTracking()
                .Where(a => a.Id == auditoria.Id)
                .FirstOrDefault();

            if (auditoriaDb is null) return false;

            auditoriaDb.FechaCierreSesion = DateTime.Now;
            auditoriaDb.MotivoCierreSesion = auditoria.MotivoCierreSesion;
            auditoriaDb.IpCierreSesion = auditoria.IpCierreSesion;

            context.AuditoriaLoginUsuario.Update(auditoriaDb);

            int entities = await context.SaveChangesAsync();

            return entities > 0;
        }

        public async Task<bool> RegistrarAuditoriaLogin(AuditoriaLoginUsuario auditoria)
        {
            using var context = _dbContextFactory.CreateDbContext();

            auditoria.FechaLogin = DateTime.Now;
            auditoria.FechaAdicion = DateTime.Now;

            context.AuditoriaLoginUsuario.Add(auditoria);

            int entities = await context.SaveChangesAsync();

            return entities > 0;
        }
        public async Task<AuditoriaLoginUsuario> GetAuditoriaLoginUsuario(long id)
        {
            using var context = _dbContextFactory.CreateDbContext();

            return await context.AuditoriaLoginUsuario.FindAsync(id);
        }
    }
}
