using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Facebook.API.Data
{
    public class GenericRepository : IGenericRepository
    {
        public readonly DataContext _context ;
       public GenericRepository(DataContext context)
       {
            _context = context;
           
       }
       
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
           _context.Remove(entity);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync()> 0; //jeśli wiekszy od 0 to zwraca true, sprawdz w interfejsie
        }
    }
}