using ControleEstacionamento.Entidades;
using System.Data.Entity;

namespace ControleEstacionamento.DAL.Contexto
{
    public class Banco : DbContext
    {
        public Banco() : base("ConnDB") { }

        public DbSet<MovimentacaoVeiculo> MovimentacoesVeiculos { get; set; }
        public DbSet<TabelaPreco> TabelasPrecos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
