using System;
using System.Linq;

namespace Lavacao.DAL.Repositorios.Base
{
    public interface IRepositorio<TEntidade> where TEntidade : class
    {
        IQueryable<TEntidade> BuscarTodos();
        IQueryable<TEntidade> Buscar(Func<TEntidade,bool> filtro);
        TEntidade BuscarChave(params object[] chave);
        void Atualizar(TEntidade objeto);
        void Salvar();
        void Adicionar(TEntidade objeto);
        void Excluir(Func<TEntidade, bool> filtro);
    }
}
