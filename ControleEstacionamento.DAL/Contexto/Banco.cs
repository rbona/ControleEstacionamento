﻿using ControleEstacionamento.Entidades;
using System.Data.Entity;

namespace ControleEstacionamento.DAL.Contexto
{
    public class Banco : DbContext
    {
        public Banco() : base("ConnDB") { }

        public DbSet<TabelaPreco> TabelasPrecos { get; set; }
    }
}