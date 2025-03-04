﻿IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [Clientes] (
    [Id] int NOT NULL IDENTITY,
    [Nome] varchar(80) NOT NULL,
    [Telefone] char(11) NULL,
    [CEP] char(8) NOT NULL,
    [Estado] char(2) NOT NULL,
    [Cidade] nvarchar(60) NOT NULL,
    CONSTRAINT [PK_Clientes] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Produtos] (
    [Id] int NOT NULL IDENTITY,
    [CodigoBarras] varchar(14) NOT NULL,
    [Descricao] varchar(60) NULL,
    [Valor] decimal(18,2) NOT NULL,
    [TipoProduto] nvarchar(max) NOT NULL,
    [Ativo] bit NOT NULL,
    CONSTRAINT [PK_Produtos] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Pedidos] (
    [Id] int NOT NULL IDENTITY,
    [ClienteId] int NOT NULL,
    [IniciadoEm] datetime2 NOT NULL DEFAULT (getDate()),
    [FinalizadoEm] datetime2 NOT NULL,
    [TipoFrete] int NOT NULL,
    [Status] nvarchar(max) NOT NULL,
    [Observacao] varchar(512) NULL,
    CONSTRAINT [PK_Pedidos] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Pedidos_Clientes_ClienteId] FOREIGN KEY ([ClienteId]) REFERENCES [Clientes] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [PedidoItems] (
    [Id] int NOT NULL IDENTITY,
    [PedidoId] int NOT NULL,
    [ProdutoId] int NOT NULL,
    [Quantidade] int NOT NULL DEFAULT 1,
    [Valor] decimal(18,2) NOT NULL,
    [Desconto] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_PedidoItems] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PedidoItems_Pedidos_PedidoId] FOREIGN KEY ([PedidoId]) REFERENCES [Pedidos] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_PedidoItems_Produtos_ProdutoId] FOREIGN KEY ([ProdutoId]) REFERENCES [Produtos] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [idx_cliente_telefone] ON [Clientes] ([Telefone]);

GO

CREATE INDEX [IX_PedidoItems_PedidoId] ON [PedidoItems] ([PedidoId]);

GO

CREATE INDEX [IX_PedidoItems_ProdutoId] ON [PedidoItems] ([ProdutoId]);

GO

CREATE INDEX [IX_Pedidos_ClienteId] ON [Pedidos] ([ClienteId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210407221630_PrimeiraMigracao', N'3.1.13');

GO

