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
    public class SolicitudCambioContraseñaRepository : EFCoreRepository<SolicitudRecuperacionClave>, ISolicitudCambioContraseñaRepository
    {
        public SolicitudCambioContraseñaRepository(IDbContextFactory<AppDbContext> dbContextFactory) : base(dbContextFactory)
        {

        }

        public async Task<bool> InhabilitarSolicitudesCaducadas(int dias)
        {
            using var context = _dbContextFactory.CreateDbContext();

            int entities = await context.SolicitudesRecuperacionClave
                .AsNoTracking()
                .Where(s => s.Estado != "Finalizada")
                .Where(s => EF.Functions.DateDiffDay(s.FechaAdicion, DateTime.Now) >= dias)
                .ExecuteUpdateAsync(
                    s =>
                        s.SetProperty(sc => sc.Estado, sc => "Cancelada")
                );

            return entities > 0;
        }
        public async Task<bool> InsertarSolicitudRecuperacionContraseña(SolicitudRecuperacionClave solicitud)
        {
            using var context = _dbContextFactory.CreateDbContext();

            SolicitudRecuperacionClave temp = context.SolicitudesRecuperacionClave
                .AsNoTracking()
                .FirstOrDefault(s => s.Id == solicitud.Id);

            if (temp is null)
            {
                solicitud.FechaAdicion = DateTime.Now;
                context.SolicitudesRecuperacionClave.Add(solicitud);
            }
            else
            {
                solicitud.FechaUltimaActualizacion = DateTime.Now;
                context.SolicitudesRecuperacionClave.Update(solicitud);
            }

            int entities = await context.SaveChangesAsync();

            return entities > 0;
        }
    }
}
