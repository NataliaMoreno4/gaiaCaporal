using PlantillaBlazor.Domain.Entities.Auditoria;
using PlantillaBlazor.Persistence.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Persistence.Repositories.Interfaces.Otp
{
    public interface IOtpRepository : IGenericRepository<AuditoriaOtp>
    {
        public Task<bool> InsertarOtp(AuditoriaOtp auditoriaOtp);
        public Task<bool> ActualizarOtp(AuditoriaOtp auditoriaOtp);

        public Task<AuditoriaOtp> GetUltimoOtpByProcess(string identificacionProceso, string tipoProceso);
    }
}
