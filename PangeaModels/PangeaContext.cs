using Microsoft.EntityFrameworkCore;

namespace Pangea.Models;

public class PangeaContext : DbContext
{
    public PangeaContext() 
        : base()
    {
    }

    public PangeaContext(DbContextOptions<PangeaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<PartnerRate> PartnerRates { get; set; } = null!;
}