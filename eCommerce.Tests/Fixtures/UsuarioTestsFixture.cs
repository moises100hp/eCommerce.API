using Bogus;
using eCommerce.API.Models;
using Bogus.Extensions.Brazil;

namespace eCommerce.Tests.Fixtures
{
    public static class UsuarioTestsFixture
    {
        public static Faker<Usuario> ObterUsuarioFaker()
        {
            return new Faker<Usuario>("pt_BR")
                .RuleFor(u => u.Nome, f => f.Name.FullName())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.Sexo, f => f.PickRandom("M", "F"))
                .RuleFor(u => u.RG, f => f.Random.ReplaceNumbers("#########"))
                .RuleFor(u => u.CPF, f => f.Person.Cpf())
                .RuleFor(u => u.NomeMae, f => f.Name.FullName())
                .RuleFor(u => u.SituacaoCadastro, f => f.PickRandom("Ativo", "Inativo"))
                .RuleFor(u => u.DataCadastro, f => f.Date.Past(1));
        }
    }
}
