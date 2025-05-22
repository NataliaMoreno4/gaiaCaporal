using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Persistence.Repositories.Common
{
    public interface IGenericRepository<T> where T : class
    {
        public Task<IEnumerable<T>> Get
        (
            List<Expression<Func<T, bool>>> filtros = null,
            List<Expression<Func<T, object>>> includes = null,
            List<Expression<Func<T, object>>> ordenamientos = null,
            int? pagina = null,
            int? tamañoPagina = null
        );

        public IEnumerable<T> GetSync
        (
            List<Expression<Func<T, bool>>> filtros = null,
            List<Expression<Func<T, object>>> includes = null,
            List<Expression<Func<T, object>>> ordenamientos = null,
            int? pagina = null,
            int? tamañoPagina = null
        );

        public Task<T> GetById(long id);
    }
}
