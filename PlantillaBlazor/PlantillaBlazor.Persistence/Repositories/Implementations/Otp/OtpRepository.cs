using PlantillaBlazor.Domain.Entities.Auditoria;
using PlantillaBlazor.Persistence.Data;
using PlantillaBlazor.Persistence.Repositories.Common;
using PlantillaBlazor.Persistence.Repositories.Interfaces.Otp;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Persistence.Repositories.Implementations.Otp
{
    public class OtpRepository : EFCoreRepository<AuditoriaOtp>, IOtpRepository
    {
        public OtpRepository(IDbContextFactory<AppDbContext> dbContextFactory) : base(dbContextFactory)
        {

        }

        public async Task<bool> ActualizarOtp(AuditoriaOtp auditoriaOtp)
        {
            using var context = _dbContextFactory.CreateDbContext();

            context.AuditoriaOtp.Update(auditoriaOtp);

            int entities = await context.SaveChangesAsync();

            return entities > 0;
        }

        public async Task<AuditoriaOtp> GetUltimoOtpByProcess(string identificacionProceso, string tipoProceso)
        {
            using var context = _dbContextFactory.CreateDbContext();

            return context.AuditoriaOtp
                .Where(otp => otp.IdentificacionProceso.Equals(identificacionProceso) && otp.TipoProceso.Equals(tipoProceso))
                .OrderByDescending(otp => otp.FechaAdicion)
                .FirstOrDefault();
        }

        public async Task<bool> InsertarOtp(AuditoriaOtp auditoriaOtp)
        {
            using var context = _dbContextFactory.CreateDbContext();

            context.AuditoriaOtp.Add(auditoriaOtp);

            int entities = await context.SaveChangesAsync();

            return entities > 0;
        }
    }
}
