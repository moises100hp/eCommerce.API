using Bogus;
using eCommerce.API.Controllers;
using eCommerce.API.Data;
using eCommerce.API.Models;
using eCommerce.API.Repositories;
using eCommerce.Tests.Fixtures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace eCommerce.Tests.Data.Repositories
{
    public class UsuarioRepositoryTests
    {
        private readonly Faker<Usuario> _faker;
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public UsuarioRepositoryTests()
        {
            _faker = UsuarioTestsFixture.ObterUsuarioFaker();

            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public void Insert_DeveGravarUsuarioNoBanco_QuandoDadosValidos()
        {
            // Arrange
            using var context = new ApplicationDbContext(_options);
            var repository = new UsuarioRepository(context); 
            var usuarioFake = _faker.Generate();

            // Act
            repository.Insert(usuarioFake);
            context.SaveChanges();

            // Assert
            var usuarioNoBanco = context.Usuarios.FirstOrDefault(u => u.Email == usuarioFake.Email);
            Assert.NotNull(usuarioNoBanco);
            Assert.Equal(usuarioFake.Nome, usuarioNoBanco.Nome);
        }
    }
}
