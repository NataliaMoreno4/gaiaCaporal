using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Services.Interfaces.Messages
{
    public interface IWhatsappMessageSender
    {
        public Task<bool> SendWhatsappMessage(string phoneNumber, string message, string concepto, string identificacionProceso, string tipoProceso);
        public Task<bool> SendWhatsappMessageOTP(string phoneNumber, string message, string concepto, string identificacionProceso, string tipoProceso);
    }
}
