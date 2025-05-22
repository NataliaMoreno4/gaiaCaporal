namespace PlantillaBlazor.Web.Entities.Authentication
{
    /// <summary>
    /// Representa un atributo de página mediante el cual se puede indicar el módulo al cual pertenece cada página
    /// </summary>
    public class PageInfoAttribute : Attribute
    {
        /// <summary>
        /// Identificador del módulo asociado a la página
        /// </summary>
        public long IdModulo = 0;
        /// <summary>
        /// Determina si la página es suceptible a perfilamiento o no. Por defecto su valor es <see langword="true" />
        /// </summary>
        public bool Perfilable = true;

        public PageInfoAttribute(long idModulo = 0, bool perfilable = true)
        {
            this.IdModulo = idModulo;
            this.Perfilable = perfilable;
        }
    }
}
