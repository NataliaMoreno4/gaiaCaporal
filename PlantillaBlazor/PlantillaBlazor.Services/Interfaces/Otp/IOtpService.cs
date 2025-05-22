using PlantillaBlazor.Domain.Entities.Auditoria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Services.Interfaces.Otp
{
    public interface IOtpService
    {
        public Task<AuditoriaOtp> GenerarCodigoOtp(string identificacionProceso, string tipoProceso, string descripcion);

        public Task<bool> GuardarCodigoOtp(AuditoriaOtp auditoriaOtp);

        public Task<AuditoriaOtp> GetUltimoOtpByProcess(string identificacionProceso, string tipoProceso);

        public Task<bool> CompletarOtp(AuditoriaOtp auditoriaOtp);

        public Task<AuditoriaOtp> GetOtpById(long id);
    }
}
