using FixIt_Model;
using Microsoft.EntityFrameworkCore;

namespace FixIt_Data.Context
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> option) : base(option)
        {

        }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Issue> Issues { get; set; }
    }
}
