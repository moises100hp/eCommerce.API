using eCommerce.API.Data;
using eCommerce.API.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace eCommerce.API.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {

        private readonly ApplicationDbContext _context;

        public UsuarioRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public IEnumerable<Usuario> Get()
        {
            return _context.Usuarios;
        }

        public Usuario Get(int id)
        {
            return _context.Usuarios.FirstOrDefault(u => u.Id == id);
        }

        public void Insert(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
        }

        public void Update(Usuario usuario)
        {
            var usuarioDB = _context.Usuarios.FirstOrDefault(u => u.Id == usuario.Id);

            if (usuarioDB == null)
                return;

            _context.Entry(usuarioDB).CurrentValues.SetValues(usuario);

            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var usuarioDB = _context.Usuarios.FirstOrDefault(u => u.Id == id);

            if (usuarioDB == null)
                return;

            _context.Usuarios.Remove(usuarioDB);
            _context.SaveChanges();
        }
    }
}
