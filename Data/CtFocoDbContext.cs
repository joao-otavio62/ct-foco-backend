using Microsoft.EntityFrameworkCore;
using ct_foco_backend.Models;




namespace ct_foco_backend.Data
{
    public class CtFocoDbContext : DbContext
    {
        public CtFocoDbContext(DbContextOptions<CtFocoDbContext> options) : base(options)
        {
        }
        public DbSet<Members> Members { get; set; }
        public DbSet<StaffMembers> TeamMembers { get; set; }

    }
}
