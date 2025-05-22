using PlantillaBlazor.Web.Entities.Authorization;
using PlantillaBlazor.Web.Entities.Circuits;
using System.Collections.Concurrent;

namespace PlantillaBlazor.Web.Services.Circuits
{
    /// <summary>
    /// Interfaz mediante la cual se exponen las operaciones para gestionar los eventos de desconexión y conexión de usuarios dentro del sistio web
    /// </summary>
    public interface ICircuitUserService
    {
        /// <summary>
        /// Diccionario que detalla los usuarios conectados al sistema, mediante pares (circuito,sesión de usuario)
        /// </summary>
        ConcurrentDictionary<string, CircuitUser> Circuits { get; }
        /// <summary>
        /// Evento que se disparará al momento en que se conecta un usuario al sistema
        /// </summary>
        event UserEventHandler UserConnected;
        /// <summary>
        /// Evento que se disparará el momento en que se desconecta un usuario del sistema
        /// </summary>
        event UserEventHandler UserDisconnected;
        /// <summary>
        /// Procesa la conexión de un usuario en concreto
        /// </summary>
        /// <param name="CircuitId">Identificador del circuito del usuario</param>
        /// <param name="user">Sesión de usuario del usuario conectado</param>
        void Connect(string CircuitId, UserSession user);
        /// <summary>
        /// Procesa la desconexión de un usuario en concreto
        /// </summary>
        /// <param name="CircuitId">Identificador del circuito desconectado</param>
        void Disconnect(string CircuitId);
    }
}
