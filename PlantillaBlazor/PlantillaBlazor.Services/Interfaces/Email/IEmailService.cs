namespace PlantillaBlazor.Services.Interfaces.Email
{
    /// <summary>
    /// Establece las operaciones para realizar envíos de correo
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Realiza el envío de un correo electrónico
        /// </summary>
        /// <param name="emailInfo">Objeto <see cref="EmailInfoDTO"/> el cual contiene toda la información necesaria para realizar el envío de un correo electrónico</param>
        /// <returns><see langword="true" /> si el envío se realizó correctamente, <see langword="false" /> en caso contrario</returns>

    }
}
