using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Persistence.Common;
using CleanArchitectureTemplate.Persistence.Configurations;
using CleanArchitectureTemplate.Persistence.Settings.Options;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitectureTemplate.Persistence.EntityFramework
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IMediator _mediator;
        private readonly IOptions<DatabaseOptions> _databaseOptions;

        public ApplicationDbContext(DbContextOptions options,
            IOptions<DatabaseOptions> databaseOptions,
            IMediator mediator) : base(options)
        {
            _mediator = mediator;
            _databaseOptions = databaseOptions;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite(_databaseOptions.Value.Connection);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Write Fluent API configurations here
            //Property Configurations
            modelBuilder.HasDefaultSchema("Weather");

            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new WeatherForecastConfiguration());
        }

        public virtual DbSet<WeatherForecast> WeatherForecast { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _mediator.DispatchDomainEvents(this);

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}