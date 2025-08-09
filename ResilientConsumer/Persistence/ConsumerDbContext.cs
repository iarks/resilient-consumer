using Microsoft.EntityFrameworkCore;

namespace ResilientConsumer.Persistence;

public class ConsumerDbContext: DbContext
{
    public ConsumerDbContext(DbContextOptions<ConsumerDbContext> options) : base(options)
    {
        
    }

    public ConsumerDbContext()
    {
        
    }
}