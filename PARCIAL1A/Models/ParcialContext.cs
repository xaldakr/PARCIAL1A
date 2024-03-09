using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
namespace PARCIAL1A.Models
{
    public class ParcialContext : DbContext
    {
        public ParcialContext(DbContextOptions<ParcialContext> options) : base(options)
        {

        }
        
    }
}

