using Bogus;
using eCommerce.API.Controllers;
using eCommerce.API.Models;
using eCommerce.API.Repositories;
using eCommerce.Tests.Fixtures;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace eCommerce.Tests.Controllers
{
    public class UsuariosControllerTests
    {
        private readonly Faker<Usuario> _faker;

        public UsuariosControllerTests()
        {
            _faker = UsuarioTestsFixture.ObterUsuarioFaker();
        }


        [Fact]
        public void Insert_DeveRetornarCriado_QuandoUsuarioForValido()
        {
            //Arrange

            var repositoryMock = new Mock<IUsuarioRepository>();
            var controller = new UsuariosController(repositoryMock.Object);

            repositoryMock.Setup(r => r.Insert(It.IsAny<Usuario>()));

            var usuarioFake = _faker.Generate();

            //Act

            var resultado = controller.Insert(usuarioFake);

            //Assert

            Assert.IsType<CreatedResult>(resultado);

            repositoryMock.Verify(r => r.Insert(It.IsAny<Usuario>()), Times.Once);
        }
    }
}
