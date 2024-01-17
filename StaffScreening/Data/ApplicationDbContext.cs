using Microsoft.EntityFrameworkCore;
using StaffScreening.Models;

namespace StaffScreening.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        #nullable disable
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<ScreeningQuestionnaire> Screenings { get; set; }
        #nullable restore
    }
}
