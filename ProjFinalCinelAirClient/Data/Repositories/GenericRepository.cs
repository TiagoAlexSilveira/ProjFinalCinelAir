using Microsoft.EntityFrameworkCore;
using ProjFinalCinelAir.CommonCore.Data;
using ProjFinalCinelAir.CommonCore.Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace ProjFinalCinelAirClient.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity
    {
        //tem de levar context 
        private readonly DataContext _context;

        public GenericRepository(DataContext context)
        {
            _context = context;
        }


        public async Task CreateAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await SaveAllAsync();
        }


        private async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }


        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await SaveAllAsync();
        }


        public async Task<bool> ExistAsync(int id)
        {
            return await _context.Set<T>().AnyAsync(e => e.Id == id);
        }


        public IQueryable<T> GetAll()
        {                                            //não quero ficar com os registos em memória
            return _context.Set<T>().AsNoTracking(); //se não meter AsNoTracking() ele guarda-me todos os registos em memória
        }


        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().AsNoTracking<T>().FirstOrDefaultAsync(e => e.Id == id); //reconhece o id pela outra interface que implementámos
        }


        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await SaveAllAsync();
        }
    }
}
