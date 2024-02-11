using Microsoft.EntityFrameworkCore;

namespace Pangea.Models;

public class PangeaContext : DbContext
{
    public PangeaContext(DbContextOptions<PangeaContext> options)
        : base(options)
    {
    }

    public DbSet<PartnerRate> PartnerRates { get; set; } = null!;
}