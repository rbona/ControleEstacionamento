using ControleEstacionamento.Entidades;
using Lavacao.DAL.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleEstacionamento.DAL.Repositorios
{
    public class MovimentacaoVeiculoRepositorio : Repositorio<MovimentacaoVeiculo>
    {
        public override IQueryable<MovimentacaoVeiculo> BuscarTodos()
        {
            return base.BuscarTodos().Include(mov => mov.tabelaPreco);
        }

        public override MovimentacaoVeiculo BuscarChave(params object[] chave)
        {
            MovimentacaoVeiculo moviVeic = null;

            try { moviVeic = Buscar(mov => mov.handle == (long)chave[0]).First(); } catch { }

            return moviVeic;
        }
    }
}