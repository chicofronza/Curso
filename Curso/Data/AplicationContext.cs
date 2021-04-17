using System;
using System.Linq;
using Curso.Data.Configurations;
using Curso.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Curso.Data
{
    public class AplicationContext : DbContext
    {
        private static readonly ILoggerFactory _logger = LoggerFactory.Create(p => p.AddConsole());
        
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder)
        {
            //Data Source= .;Initial Catalog=EFCoreDB;User ID=sa;Password=Chico@1234
            //@"Server=localhost,1401;Database=prodcat;User ID=SA;Password=Numsey@Password!"
            optionsBuilder
                .UseLoggerFactory(_logger)
                .EnableSensitiveDataLogging() //Habilita a visualização das informações
                .UseSqlServer("Data Source= .;Initial Catalog=EFCoreDB;User ID=sa;Password=Chico@1234");
                // .UseSqlServer("Data Source= .;Initial Catalog=EFCoreDB;User ID=sa;Password=Chico@1234", 
                //                 p => p.EnableRetryOnFailure(maxRetryCount:2, 
                //                                             maxRetryDelay: TimeSpan.FromSeconds(5), 
                //                                             errorNumbersToAdd: null,).MigrationsHistoryTable("curso_ef_core"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AplicationContext).Assembly);
        }

        private void MapearPropriedadeEsquecidas(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entity.GetProperties().Where(p => p.ClrType == typeof(string));

                foreach (var property in properties)
                {
                    if(string.IsNullOrEmpty(property.GetColumnType()) && !property.GetMaxLength().HasValue)
                    {
                        property.SetColumnType("varchar(100)");
                    }
                }
            }
        }        
    }
}