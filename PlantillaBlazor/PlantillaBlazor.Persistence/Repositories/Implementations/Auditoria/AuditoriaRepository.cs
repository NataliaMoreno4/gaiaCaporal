using PlantillaBlazor.Domain.Entities.Auditoria;
using PlantillaBlazor.Persistence.Data;
using PlantillaBlazor.Persistence.Repositories.Common;
using PlantillaBlazor.Persistence.Repositories.Interfaces.Auditoria;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Persistence.Repositories.Implementations.Auditoria
{
    public class AuditoriaRepository : IAuditoriaRepository
    {
        private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

        public AuditoriaRepository(IDbContextFactory<AppDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<bool> RegistrarAuditoriaDescargaArchivo(AuditoriaDescargaArchivo auditoriaDescargaArchivo)
        {
            using var context = _dbContextFactory.CreateDbContext();

            auditoriaDescargaArchivo.FechaAdicion = DateTime.Now;

            context.AuditoriaDescargaArchivos.Add(auditoriaDescargaArchivo);

            int entities = await context.SaveChangesAsync();

            return entities > 0;
        }

        public async Task<bool> RegistrarAuditoriaEnvioEmail(AuditoriaEnvioEmail auditoriaEmail)
        {
            using var context = _dbContextFactory.CreateDbContext();

            auditoriaEmail.FechaAdicion = DateTime.Now;

            context.AuditoriaEnvioEmail.Add(auditoriaEmail);

            int entities = await context.SaveChangesAsync();

            return entities > 0;
        }

        public async Task<bool> RegistrarAuditoriaEnvioSMS(AuditoriaEnvioSMS auditoriaEnvioSms)
        {
            using var context = _dbContextFactory.CreateDbContext();

            auditoriaEnvioSms.FechaAdicion = DateTime.Now;

            context.AuditoriaEnvioSMS.Add(auditoriaEnvioSms);

            int entities = await context.SaveChangesAsync();

            return entities > 0;
        }

        public async Task<bool> RegistrarAuditoriaEnvioWpp(AuditoriaEnvioWpp auditoriaEnvioWpp)
        {
            using var context = _dbContextFactory.CreateDbContext();

            auditoriaEnvioWpp.FechaAdicion = DateTime.Now;

            context.AuditoriaEnvioWpp.Add(auditoriaEnvioWpp);

            int entities = await context.SaveChangesAsync();

            return entities > 0;
        }

        public async Task<bool> RegistrarAuditoriaEvento(AuditoriaEvento auditoriaEvento)
        {
            using var context = _dbContextFactory.CreateDbContext();

            auditoriaEvento.FechaAdicion = DateTime.Now;

            context.AuditoriaEventos.Add(auditoriaEvento);

            int entities = await context.SaveChangesAsync();

            return entities > 0;
        }

        public async Task<bool> RegistrarAuditoriaNavegacion(AuditoriaNavegacion auditoriaNavegacion)
        {
            using var context = _dbContextFactory.CreateDbContext();

            auditoriaNavegacion.FechaAdicion = DateTime.Now;

            context.AuditoriaNavegacion.Add(auditoriaNavegacion);
            int entities = await context.SaveChangesAsync();

            return entities > 0;
        }

        public async Task<bool> RegistrarAuditoriaConsumoRegistraduria(AuditoriaConsumoRegistraduria auditoriaRegistraduria)
        {
            using var context = _dbContextFactory.CreateDbContext();

            auditoriaRegistraduria.FechaAdicion = DateTime.Now;

            context.AuditoriaConsumoRegistraduria.Add(auditoriaRegistraduria);
            int entities = await context.SaveChangesAsync();

            return entities > 0;
        }
    }
}
