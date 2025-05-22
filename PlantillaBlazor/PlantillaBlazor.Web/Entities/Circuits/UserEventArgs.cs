using PlantillaBlazor.Web.Entities.Authorization;

/// <summary>
/// Describe el argumento para el manejo del evento <see cref="UserEventHandler"/>
/// </summary>
public class UserEventArgs : EventArgs
{
    /// <summary>
    /// Objeto <see cref="UserSession"/> que describe la sesión de usuario implicada en el evento
    /// </summary>
    public UserSession Usuario { get; set; }
}
