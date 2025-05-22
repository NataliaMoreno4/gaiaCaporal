using PlantillaBlazor.Domain.Common.ResultModels;
using PlantillaBlazor.Domain.DTO.OTP;
using PlantillaBlazor.Domain.DTO.Perfilamiento;
using PlantillaBlazor.Domain.Entities.Perfilamiento;

namespace PlantillaBlazor.Services.Interfaces.Perfilamiento
{
    public interface IUsuarioService
    {
        public Task<IEnumerable<Usuario>> GetUsuarios();
        public Task<ResultLogin<long>> ProcesarLoginUsuario(UserDTO usuarioDto, string ipAddress);
        public Task<bool> RegistrarAuditoriaCierreSesion(long idAuditoria, string ipAddress, string motivo);
        public Task<Result<long>> InsertarUsuario(Usuario usuario, string ipAddress);
        public Task<Usuario> GetUsuarioById(long idUsuario);
        public Task<Usuario> GetUsuarioByUser(string usuario);
        public Task<Result<bool>> GestionarSolicitudRecuperacionContraseña(string nombreUsuario, string ipAddress, string motivo);
        public Task<SolicitudRecuperacionClave> GetSolicitudContraseña(long idSolicitud);
        public Task<SolicitudRecuperacionClave> GenerarSolicitudContraseña(long idUsuario, string ipAddress, string motivo);
        public Task<Result<bool>> CompletarSolicitudContraseña(long idSolicitud, string nuevaPass, string ipAddress);
        public Task<Result<long>> EnviarCodigoOTPLogin(long idUsuario, string ipAccion);
        public Task<Result<bool>> ValidarCodigoOTP(OtpDto otpCode, long idUsuario, string ipAccion);
        public Task<Result<long>> EnviarCodigoOTPPass(long idUsuario, string ipAccion);
        public Task<Result<bool>> ValidarCodigoOTPPass(OtpDto otpCode, long idUsuario, string ipAccion);
        public Task ProcesarIngreso(Usuario usuario, string ipAddress, string descripcion);
        public Task<IEnumerable<Modulo>> GetModulosUsuario(long idUsuario);
        public Task<Result<bool>> ValidarInformacionUsuario(Usuario usuario);
        public Task<bool> InactivarUsuariosNoActivos(int diasDesdeUltimoLoggeo);
        public Task<bool> InhabilitarSolicitudesCambioPassCaducadas(int dias);
        public Task<Result<SessionDTO>> ProcesarIngresoUsuarioOAuth(OAuthUserDTO user);
        public Task<Result<bool>> ActualizarInfoUsuarioOAuth(Usuario usuario);
    }
}
