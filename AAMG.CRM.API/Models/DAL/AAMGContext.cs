using AAMG.CRMAPI.Models.EN;
using Microsoft.EntityFrameworkCore;

namespace AAMG.CRMAPI.Models.DAL
{
    public class AAMGContext: DbContext
    {

            public AAMGContext(DbContextOptions<AAMGContext> options) : base(options) { }
            public DbSet<Customer> Customers { get; set; }
        
    }
}
