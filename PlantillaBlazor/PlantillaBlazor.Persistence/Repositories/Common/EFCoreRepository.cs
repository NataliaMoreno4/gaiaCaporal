using Microsoft.EntityFrameworkCore;
using PlantillaBlazor.Persistence.Data;
using System.Linq.Expressions;

namespace PlantillaBlazor.Persistence.Repositories.Common
{
    public abstract class EFCoreRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly IDbContextFactory<AppDbContext> _dbContextFactory;

        protected EFCoreRepository(IDbContextFactory<AppDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public IEnumerable<T> GetSync
        (
            List<Expression<Func<T, bool>>> filtros = null,
            List<Expression<Func<T, object>>> includes = null,
            List<Expression<Func<T, object>>> ordenamientos = null,
            int? pagina = null,
            int? tamañoPagina = null
        )
        {
            using var context = _dbContextFactory.CreateDbContext();

            IQueryable<T> consulta = context.Set<T>();


            if (filtros != null)
            {
                foreach (var filtro in filtros)
                {
                    consulta = consulta.Where(filtro);
                }
            }

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    consulta = consulta.Include(include);
                }
            }

            if (ordenamientos != null)
            {
                foreach (var ordenamiento in ordenamientos)
                {
                    consulta = consulta.OrderBy(ordenamiento);
                }
            }

            if (pagina != null && tamañoPagina != null)
            {
                consulta = consulta.Skip((pagina.Value - 1) * tamañoPagina.Value).Take(tamañoPagina.Value);
            }

            consulta = consulta.AsSplitQuery();

            return consulta.ToList();
        }

        public async Task<IEnumerable<T>> Get
        (
            List<Expression<Func<T, bool>>> filtros = null,
            List<Expression<Func<T, object>>> includes = null,
            List<Expression<Func<T, object>>> ordenamientos = null,
            int? pagina = null,
            int? tamañoPagina = null
        )
        {
            using var context = _dbContextFactory.CreateDbContext();

            IQueryable<T> consulta = context.Set<T>();


            if (filtros != null)
            {
                foreach (var filtro in filtros)
                {
                    consulta = consulta.Where(filtro);
                }
            }

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    consulta = consulta.Include(include);
                }
            }

            if (ordenamientos != null)
            {
                foreach (var ordenamiento in ordenamientos)
                {
                    consulta = consulta.OrderBy(ordenamiento);
                }
            }

            if (pagina != null && tamañoPagina != null)
            {
                consulta = consulta.Skip((pagina.Value - 1) * tamañoPagina.Value).Take(tamañoPagina.Value);
            }

            consulta = consulta.AsSplitQuery();

            return await consulta.ToListAsync();
        }

        public async Task<T> GetById(long id)
        {
            using var context = _dbContextFactory.CreateDbContext();

            return await context.Set<T>().FindAsync(id);
        }
    }
}
