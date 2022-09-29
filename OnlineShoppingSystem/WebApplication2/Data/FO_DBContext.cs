using Microsoft.EntityFrameworkCore;
using OnlineShoppingSystem.Models;

namespace OnlineShoppingSystem.Data
{
    public class FO_DBContext :DbContext
    {
        public FO_DBContext(DbContextOptions<FO_DBContext> options): base(options)
        {

        }


        public DbSet<Employee> Employees { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=FoodOrderingDB; Integrated Security=True");
            base.OnConfiguring(optionsBuilder);
        }

    }
}
