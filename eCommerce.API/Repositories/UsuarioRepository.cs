using Dapper;
using eCommerce.API.Models;
using System.Data;
using Microsoft.Data.SqlClient;

namespace eCommerce.API.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private IDbConnection _connection;
        public UsuarioRepository()
        {
            _connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=eCommerce;Integrated Security=True;Connect Timeout=30;Encrypt=True;Application Intent=ReadWrite;Multi Subnet Failover=False;");
        }

        public IEnumerable<Usuario> Get()
        {
            List<Usuario> usuarios = new List<Usuario>();

            string sql = @"SELECT * FROM Usuarios as U 
	                         LEFT JOIN Contatos C ON C.UsuarioId = U.Id 
                             LEFT JOIN EnderecosEntrega EE ON EE.UsuarioId = U.Id
                             LEFT JOIN UsuariosDepartamentos UD ON UD.UsuarioId = U.Id
                             LEFT JOIN Departamentos D ON UD.DepartamentoId = D.Id";

            _connection.Query<Usuario, Contato, EnderecoEntrega, Departamento, Usuario>(sql,
                (usuario, contato, enderecoEntrega, departamento) =>
                {
                    var usuarioExistente = usuarios.SingleOrDefault(i => i.Id == usuario.Id);

                    if (usuarioExistente == null)
                    {
                        usuario.EnderecosEntrega = new List<EnderecoEntrega>();
                        usuario.Departamentos = new List<Departamento>();
                        usuario.Contato = contato;
                        usuarios.Add(usuario);
                    }
                    else
                        usuario = usuarioExistente;

                    if (usuario.EnderecosEntrega.SingleOrDefault(i => i.Id == enderecoEntrega.Id) is null)
                        usuario.EnderecosEntrega.Add(enderecoEntrega);

                    if (usuario.Departamentos.SingleOrDefault(i => i.Id == departamento.Id) is null)
                        usuario.Departamentos.Add(departamento);

                    return usuario;
                });

            return usuarios;
        }

        public Usuario? Get(int id)
        {
            List<Usuario> usuarios = new List<Usuario>();

            string sql = @"SELECT * FROM Usuarios as U 
	                         LEFT JOIN Contatos C ON C.UsuarioId = U.Id 
                             LEFT JOIN EnderecosEntrega EE ON EE.UsuarioId = U.Id
                             LEFT JOIN UsuariosDepartamentos UD ON UD.UsuarioId = U.Id
                             LEFT JOIN Departamentos D ON UD.DepartamentoId = D.Id
                           WHERE U.Id = @Id";

            _connection.Query<Usuario, Contato, EnderecoEntrega, Departamento, Usuario>(sql,
                (usuario, contato, enderecoEntrega, departamento) =>
                {
                    var usuarioExistente = usuarios.SingleOrDefault(i => i.Id == usuario.Id);

                    if (usuarioExistente == null)
                    {
                        usuario.EnderecosEntrega = new List<EnderecoEntrega>();
                        usuario.Departamentos = new List<Departamento>();
                        usuario.Contato = contato;
                        usuarios.Add(usuario);
                    }
                    else
                        usuario = usuarioExistente;

                    if (usuario.EnderecosEntrega.SingleOrDefault(i => i.Id == enderecoEntrega.Id) is null)
                        usuario.EnderecosEntrega.Add(enderecoEntrega);

                    if (usuario.Departamentos.SingleOrDefault(i => i.Id == departamento.Id) is null)
                        usuario.Departamentos.Add(departamento);

                    return usuario;
                }, new { Id = id });

            return usuarios.FirstOrDefault();
        }

        public void Insert(Usuario usuario)
        {
            _connection.Open();
            var transaction = _connection.BeginTransaction();

            try
            {
                string sql = "INSERT INTO Usuarios (Nome, Email, Sexo, RG, CPF, NomeMae, SituacaoCadastro, DataCadastro) VALUES (@Nome, @Email, @Sexo, @RG, @CPF, @NomeMae, @SituacaoCadastro, @DataCadastro); SELECT CAST( SCOPE_IDENTITY() AS INT);";
                usuario.Id = _connection.Query<int>(sql, usuario, transaction).Single();

                if (usuario.Contato == null)
                    return;

                usuario.Contato.UsuarioId = usuario.Id;
                string sqlContato = "INSERT INTO Contatos (UsuarioId, Telefone, Celular) VALUES (@UsuarioId, @Telefone, @Celular); SELECT CAST( SCOPE_IDENTITY() AS INT);";
                usuario.Contato.Id = _connection.Query<int>(sqlContato, usuario.Contato, transaction).Single();

                CadastrarEnderecoEntrega(usuario, transaction);

                CadastrarDepartamentos(usuario, transaction);

                transaction.Commit();
            }
            catch
            {
                try
                {
                    transaction.Rollback();
                }
                catch (Exception) { }
            }
            finally
            {
                _connection.Close();
            }
        }

        private void CadastrarEnderecoEntrega(Usuario usuario, IDbTransaction transaction)
        {
            if (usuario.EnderecosEntrega != null && usuario.EnderecosEntrega.Count > 0)
            {
                foreach (var enderecoEntrega in usuario.EnderecosEntrega)
                {
                    enderecoEntrega.UsuarioId = usuario.Id;
                    string sqlEndereco = @"INSERT INTO EnderecosEntrega (UsuarioId, NomeEndereco, CEP, Cidade, Estado, Bairro, Endereco, Numero, Complemento) VALUES (@UsuarioId, @NomeEndereco, @CEP, @Cidade, @Estado, @Bairro, @Endereco, @Numero, @Complemento);  SELECT CAST( SCOPE_IDENTITY() AS INT);";
                    enderecoEntrega.Id = _connection.Query<int>(sqlEndereco, enderecoEntrega, transaction).SingleOrDefault();
                }
            }
        }

        private void CadastrarDepartamentos(Usuario usuario, IDbTransaction transaction)
        {
            if (usuario.Departamentos != null && usuario.Departamentos.Count > 0)
            {
                foreach (var departamento in usuario.Departamentos)
                {
                    string sqlDepartamento = @"INSERT INTO UsuariosDepartamentos (UsuarioId, DepartamentoId) VALUES (@UsuarioId, @DepartamentoId);  SELECT CAST( SCOPE_IDENTITY() AS INT);";
                    _connection.Execute(sqlDepartamento, new { UsuarioId = usuario.Id, DepartamentoId = departamento.Id }, transaction);
                }
            }
        }

        public void Update(Usuario usuario)
        {
            _connection.Open();
            var transaction = _connection.BeginTransaction();

            try
            {
                string sql = "UPDATE Usuarios SET Nome = @Nome, Email = @Email, Sexo = @Sexo, RG = @RG, CPF = @CPF, NomeMae = @NomeMae, SituacaoCadastro = @SituacaoCadastro, DataCadastro = @DataCadastro WHERE Id = @Id";
                _connection.Execute(sql, usuario, transaction);

                if (usuario.Contato != null)
                {
                    string sqlContato = "UPDATE Contatos SET UsuarioId = @UsuarioId, Telefone = @Telefone, Celular = @Celular WHERE Id = @Id;";
                    _connection.Execute(sqlContato, usuario.Contato, transaction);
                }

                string sqlDeletarEnderecosEntrega = "DELETE FROM EnderecosEntrega WHERE UsuarioId = @Id";
                _connection.Execute(sqlDeletarEnderecosEntrega, usuario, transaction);

                CadastrarEnderecoEntrega(usuario, transaction);

                string sqlDeletarUsuarioDepartamentos = "DELETE FROM UsuariosDepartamentos WHERE UsuarioId = @Id";
                _connection.Execute(sqlDeletarUsuarioDepartamentos, usuario, transaction);
                
                CadastrarDepartamentos(usuario, transaction);

                transaction.Commit();
            }
            catch (Exception)
            {
                try
                {
                    transaction.Rollback();
                }
                catch (Exception) { }
            }
            finally
            {
                _connection.Close();
            }
        }

        public void Delete(int id)
        {
            var usuarioDB = _connection.QueryFirstOrDefault<Usuario>("SELECT * FROM Usuarios WHERE Id = @Id", new { Id = id });

            if (usuarioDB == null)
                return;

            _connection.Execute("DELETE FROM Usuarios WHERE Id = @Id", new { Id = id });
        }
    }
}
