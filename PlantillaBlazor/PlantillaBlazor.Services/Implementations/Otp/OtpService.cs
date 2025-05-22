using PlantillaBlazor.Services.Interfaces.Otp;
using PlantillaBlazor.Domain.Entities.Auditoria;
using PlantillaBlazor.Persistence.Repositories.Interfaces.Otp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Services.Implementations.Otp
{
    public class OtpService : IOtpService
    {
        private readonly IOtpRepository _otpRepository;

        public OtpService(IOtpRepository otpRepository)
        {
            _otpRepository = otpRepository;
        }

        public async Task<bool> CompletarOtp(AuditoriaOtp auditoriaOtp)
        {
            auditoriaOtp.FechaValidacion = DateTime.Now;
            auditoriaOtp.Estado = "VERIFICADO";

            return await _otpRepository.ActualizarOtp(auditoriaOtp);
        }

        public async Task<AuditoriaOtp> GenerarCodigoOtp(string identificacionProceso, string tipoProceso, string descripcion)
        {
            AuditoriaOtp auditoriaOtp = new AuditoriaOtp()
            {
                IdentificacionProceso = identificacionProceso,
                TipoProceso = tipoProceso,
                Estado = "ENVIADO",
                FechaAdicion = DateTime.Now
            };

            Random R = new Random();
            int otpCode = R.Next(100000, 999999);

            auditoriaOtp.Codigo = otpCode.ToString();

            return auditoriaOtp;
        }

        public async Task<AuditoriaOtp> GetOtpById(long id)
        {
            return await _otpRepository.GetById(id);
        }

        public async Task<AuditoriaOtp> GetUltimoOtpByProcess(string identificacionProceso, string tipoProceso)
        {
            return await _otpRepository.GetUltimoOtpByProcess(identificacionProceso, tipoProceso);
        }

        public async Task<bool> GuardarCodigoOtp(AuditoriaOtp auditoriaOtp)
        {
            return await _otpRepository.InsertarOtp(auditoriaOtp);
        }
    }
}
