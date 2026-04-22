-- ======================================================
-- 1. CRIAÇÃO DAS TABELAS
-- ======================================================

CREATE TABLE [dbo].[Usuarios] (
	[Id] INT IDENTITY(1,1) NOT NULL,
	[Nome] NVARCHAR(70) NOT NULL,
	[Email] NVARCHAR(100) NOT NULL,
	[Sexo] CHAR(1) NULL,
	[RG] VARCHAR(15) NULL,
	[CPF] CHAR(14) NULL,
	[NomeMae] VARCHAR(70) NULL,
	[SituacaoCadastro] CHAR(1) NOT NULL,
	[DataCadastro] DATETIME NOT NULL,
	CONSTRAINT [PK_Usuarios] PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Contatos] (
	[Id] INT IDENTITY(1,1) NOT NULL,
	[UsuarioId] INT NOT NULL,
	[Telefone] VARCHAR(15) NULL,
	[Celular] VARCHAR(15) NULL,
	CONSTRAINT [PK_Contatos] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Contatos_Usuarios] FOREIGN KEY ([UsuarioId]) REFERENCES [dbo].[Usuarios]([Id]) ON DELETE CASCADE
);

CREATE TABLE [dbo].[EnderecosEntrega] (
	[Id] INT IDENTITY(1,1) NOT NULL,
	[UsuarioId] INT NOT NULL,
	[NomeEndereco] VARCHAR(100) NOT NULL,
	[CEP] CHAR(10) NOT NULL,
	[Estado] CHAR(2) NOT NULL,
	[Cidade] VARCHAR(120) NOT NULL,
	[Bairro] VARCHAR(200) NOT NULL,
	[Endereco] VARCHAR(200) NOT NULL,
	[Numero] VARCHAR(20) NULL,
	[Complemento] VARCHAR(20) NULL,
	CONSTRAINT [PK_EnderecosEntrega] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_EnderecosEntrega_Usuarios] FOREIGN KEY ([UsuarioId]) REFERENCES [dbo].[Usuarios]([Id]) ON DELETE CASCADE
);

CREATE TABLE [dbo].[Departamentos] (
	[Id] INT IDENTITY(1,1) NOT NULL,
	[Nome] VARCHAR(100) NOT NULL,
	CONSTRAINT [PK_Departamentos] PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[UsuariosDepartamentos] (
	[UsuarioId] INT NOT NULL,
	[DepartamentoId] INT NOT NULL,
	CONSTRAINT [PK_UsuariosDepartamentos] PRIMARY KEY CLUSTERED ([UsuarioId], [DepartamentoId]),
	CONSTRAINT [FK_UsuariosDepartamentos_Usuarios] FOREIGN KEY ([UsuarioId]) REFERENCES [dbo].[Usuarios]([Id]) ON DELETE CASCADE,
	CONSTRAINT [FK_UsuariosDepartamentos_Departamentos] FOREIGN KEY ([DepartamentoId]) REFERENCES [dbo].[Departamentos]([Id]) ON DELETE CASCADE
);
GO

-- ======================================================
-- 2. STORED PROCEDURES
-- ======================================================

CREATE PROCEDURE dbo.SelecionarUsuarios AS SELECT * FROM [dbo].[Usuarios] GO

CREATE PROCEDURE dbo.SelecionarUsuario(@id INT) AS SELECT * FROM [dbo].[Usuarios] WHERE Id = @id GO

CREATE PROCEDURE dbo.CadastrarUsuario(
	@Nome NVARCHAR(70), @Email NVARCHAR(100), @Sexo CHAR(1), @RG VARCHAR(15), 
	@CPF CHAR(14), @NomeMae VARCHAR(70), @SituacaoCadastro CHAR(1), @dataCadastro DATETIME) AS
	INSERT INTO [dbo].[Usuarios] (Nome, Email, Sexo, RG, CPF, NomeMae, SituacaoCadastro, DataCadastro)
	VALUES (@Nome, @Email, @Sexo, @RG, @CPF, @NomeMae, @SituacaoCadastro, @dataCadastro) 
GO

CREATE PROCEDURE dbo.AtualizarUsuario(
	@Id INT, @Nome NVARCHAR(70), @Email NVARCHAR(100), @Sexo CHAR(1), @RG VARCHAR(15), 
	@CPF CHAR(14), @NomeMae VARCHAR(70), @SituacaoCadastro CHAR(1), @dataCadastro DATETIME) AS
	UPDATE [dbo].[Usuarios] SET Nome = @Nome, Email = @Email, Sexo = @Sexo, RG = @RG, 
	CPF = @CPF, NomeMae = @NomeMae, SituacaoCadastro = @SituacaoCadastro, DataCadastro = @dataCadastro
	WHERE Id = @Id 
GO

CREATE PROCEDURE dbo.DeletarUsuario(@Id INT) AS DELETE FROM [dbo].[Usuarios] WHERE Id = @Id GO
GO

-- ======================================================
-- 3. POPULANDO OS DADOS (SEED)
-- ======================================================

-- Inserindo 10 Departamentos
INSERT INTO [dbo].[Departamentos] (Nome) VALUES 
('Engenharia de Software'), ('Data Science'), ('UX/UI Design'), ('DevOps'), 
('Recursos Humanos'), ('Financeiro'), ('Jurídico'), ('Marketing Digital'), 
('Suporte Técnico'), ('Diretoria');

-- Inserindo 10 Usuários
INSERT INTO [dbo].[Usuarios] (Nome, Email, Sexo, RG, CPF, NomeMae, SituacaoCadastro, DataCadastro) VALUES 
('Ricardo Souza', 'ricardo.souza@dev.com', 'M', '11.111.111-1', '111.111.111-11', 'Ana Souza', 'A', GETDATE()),
('Fernanda Lima', 'fernanda.lima@tech.com', 'F', '22.222.222-2', '222.222.222-22', 'Maria Lima', 'A', GETDATE()),
('Thiago Silva', 'thiago.silva@cloud.io', 'M', '33.333.333-3', '333.333.333-33', 'Carla Silva', 'A', GETDATE()),
('Juliana Costa', 'juliana.c@web.net', 'F', '44.444.444-4', '444.444.444-44', 'Sonia Costa', 'I', GETDATE()),
('Marcos Rocha', 'marcos.r@ai.com', 'M', '55.555.555-5', '555.555.555-55', 'Elena Rocha', 'A', GETDATE()),
('Beatriz Nucci', 'beatriz.n@ux.com', 'F', '66.666.666-6', '666.666.666-66', 'Rosa Nucci', 'A', GETDATE()),
('Gabriel Santos', 'gabriel.s@devops.br', 'M', '77.777.777-7', '777.777.777-77', 'Vera Santos', 'A', GETDATE()),
('Aline Mendes', 'aline.m@marketing.com', 'F', '88.888.888-8', '888.888.888-88', 'Lia Mendes', 'A', GETDATE()),
('Paulo Junior', 'paulo.j@finance.com', 'M', '99.999.999-9', '999.999.999-99', 'Julia Junior', 'I', GETDATE()),
('Larissa Vale', 'larissa.v@legal.com', 'F', '00.000.000-0', '000.000.000-00', 'Zilda Vale', 'A', GETDATE());

-- Inserindo 10 Contatos
INSERT INTO [dbo].[Contatos] (UsuarioId, Telefone, Celular) VALUES 
(1, '(11) 3333-1111', '(11) 91111-1111'),
(2, '(21) 3333-2222', '(21) 92222-2222'),
(3, '(31) 3333-3333', '(31) 93333-3333'),
(4, NULL, '(41) 94444-4444'),
(5, '(51) 3333-5555', '(51) 95555-5555'),
(6, NULL, '(61) 96666-6666'),
(7, '(71) 3333-7777', '(71) 97777-7777'),
(8, '(81) 3333-8888', '(81) 98888-8888'),
(9, '(91) 3333-9999', '(91) 99999-9999'),
(10, '(11) 3333-0000', '(11) 90000-0000');

-- Inserindo 10 Endereços
INSERT INTO [dbo].[EnderecosEntrega] (UsuarioId, NomeEndereco, CEP, Estado, Cidade, Bairro, Endereco, Numero, Complemento) VALUES 
(1, 'Casa', '01310-100', 'SP', 'São Paulo', 'Bela Vista', 'Av. Paulista', '1000', 'Apto 10'),
(2, 'Trabalho', '20040-002', 'RJ', 'Rio de Janeiro', 'Centro', 'Av. Rio Branco', '50', 'Sala 202'),
(3, 'Principal', '30140-001', 'MG', 'Belo Horizonte', 'Savassi', 'Rua Sergipe', '120', NULL),
(4, 'Entrega', '80010-000', 'PR', 'Curitiba', 'Centro', 'Rua XV de Novembro', '300', 'Bloco A'),
(5, 'Casa', '90010-001', 'RS', 'Porto Alegre', 'Centro', 'Rua dos Andradas', '45', NULL),
(6, 'Residencial', '70040-010', 'DF', 'Brasília', 'Asa Norte', 'SCLN 202', '15', 'Casa 2'),
(7, 'Escritório', '40010-000', 'BA', 'Salvador', 'Comércio', 'Av. da França', '10', NULL),
(8, 'Principal', '50010-000', 'PE', 'Recife', 'Bairro do Recife', 'Av. Alfredo Lisboa', '500', 'Loja 1'),
(9, 'Casa Pais', '66010-000', 'PA', 'Belém', 'Campina', 'Rua 15 de Novembro', '88', NULL),
(1, 'Apartamento Praia', '11000-000', 'SP', 'Santos', 'Gonzaga', 'Av. da Praia', '500', 'Cobertura');

-- Inserindo 10 Associações Usuários/Departamentos
INSERT INTO [dbo].[UsuariosDepartamentos] (UsuarioId, DepartamentoId) VALUES 
(1, 1), (2, 1), (3, 4), (4, 5), (5, 2), (6, 3), (7, 4), (8, 8), (9, 6), (10, 7);
GO