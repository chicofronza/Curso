using System;
using System.Collections.Generic;
using System.Linq;
using Curso.Data;
using Curso.Domain;
using Curso.ValueObjects;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CursoEFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new AplicationContext();

            //Executa a migração
            // db.Database.Migrate();

            //verifica se existe migrações pendentes
            var existeMigrationsPendentes = db.Database.GetPendingMigrations().Any();
            
            //InserirDados();
            //InserirDadosEmMassa();
            //ConsultaDados();
            //CadastrarPedido();
            //ConsultarPedidoCarregamentoAdiantado();
            //AtualizarDados();
        }

        private static void RemoverRegistro()
        {
            using var db =  new AplicationContext();
            var cliente = db.Clientes.Find(2);

            //db.Clientes.Remove(cliente);
            //db.Remove(cliente);
            db.Entry(cliente).State = EntityState.Deleted;
            
            db.SaveChanges();
        }

        private static void AtualizarDados()
        {
            using var db =  new AplicationContext();
            var cliente = db.Clientes.Find(1);
            //cliente.Nome = "Cliente Alterado Passo1";
            var clienteDesconectado = new Cliente 
            {
                Nome = "Cliente Desconectado",
                Telefone = "7966669999"
            };

            db.Entry(cliente).CurrentValues.SetValues(clienteDesconectado);
            //db.Clientes.Update(cliente);
            db.SaveChanges();
        }

        private static void ConsultarPedidoCarregamentoAdiantado()
        {
            using var db = new AplicationContext();
            var pedidos = db.Pedidos
                            .Include(p => p.Items)
                            .ThenInclude(p => p.Produto)
                            .ToList();

            Console.WriteLine(pedidos.Count);
        }

        private static void CadastrarPedido()
        {
            using var db = new AplicationContext();
            var cliente = db.Clientes.FirstOrDefault();
            var produto = db.Produtos.FirstOrDefault();

            var pedido = new Pedido
            {
                ClienteId = cliente.Id,
                IniciadoEm = DateTime.Now,
                FinalizadoEm = DateTime.Now,
                Observacao = "Pedido Teste",
                Status = StatusPedido.Analise,
                TipoFrete = TipoFrete.SemFrete,
                Items = new List<PedidoItem>
                {
                    new PedidoItem 
                    {
                        ProdutoId = produto.Id,
                        Desconto = 0,
                        Quantidade = 1,
                        Valor = 10
                    }
                }
            };

            db.Pedidos.Add(pedido);
            db.SaveChanges();
        }

        private static void ConsultaDados()
        {
            using var db = new AplicationContext();
            var consultaPorSintaxe  = (from c in db.Clientes where c.Id > 0 select c).ToList();
            var consultaPorSintaxe2  = db.Clientes.Where(p => p.Id > 0).ToList();

            string id = "1";
            var consultaPorSintaxe3 = db.Clientes.FromSqlRaw("select * from [dbo].[Clientes] where id = @id", new SqlParameter("id", id)).ToList();            
        }

        private static void InserirDadosEmMassa()
        {
            var produto = new Produto
            {
                Descricao = "Produto Teste",
                CodigoBarras = "1234567891231",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaParaVenda,
                Ativo = true
            };

            var Cliente = new Cliente
            {
                Nome = "Rafael Almeida",
                CEP = "99999000",
                Cidade = "Itabaiana",
                Estado = "SE",
                Telefone = "99000001111"
            };

            using var db = new AplicationContext();
            db.AddRange(produto, Cliente);

            var registros = db.SaveChanges();
            Console.WriteLine($"Total de registros(s): {registros}");
        }

        private static void InserirDados()
        {
            var produto = new Produto
            {
                Descricao = "Produto Teste",
                CodigoBarras = "1234567891231",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaParaVenda,
                Ativo = true
            };

            using var db = new AplicationContext();
            //db.Produtos.Add(produto);
            //db.Set<Produto>().Add(produto);
            //db.Entry(produto).State = EntityState.Added;
            db.Add(produto);
            var registros = db.SaveChanges();
            Console.WriteLine($"Total de registros(s): {registros}");
        }
    }
}
