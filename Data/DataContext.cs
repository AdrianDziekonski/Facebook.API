using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Facebook.API.Data
{
    public class DataContext :DbContext
    {
        public DataContext(DbContextOptions<DataContext> options):base(options) {}  

        public  DbSet<Value> Values { get; set; }  
        ////To jest tabela do bazy danych
        
        public DbSet<User> Users { get; set; }
        
    }
}