# Fiap Cloud Games Users API

**Descrição**

API responsável pelo fluxo de usuários do ecossistema Fiap Cloud Games, incluindo autenticação via Firebase, persistência em banco relacional e publicaçãoo de eventos para integração com outros serviços.

**Funcionalidades**
- Cadastro de usuário.
- Autenticação/login de usuário.
- Promoção de usuário para perfil administrador.
- Integração com Firebase Authentication.
- Publicação de evento de usuário criado em fila RabbitMQ.
- Health checks em `/health`, `/health/ready` e `/health/live`.
- Documentação OpenAPI com Scalar.
- Observabilidade com New Relic e logs estruturados via Serilog.

**Tecnologias**
- .NET 10 e ASP.NET Core.
- MediatR.
- Entity Framework Core + Npgsql (PostgreSQL).
- Firebase Admin SDK.
- MassTransit com RabbitMQ.
- FluentValidation.
- Serilog.
- New Relic.
- OpenAPI + Scalar.
- Docker (Dockerfile para build/test/publish).

**Estrutura do Projeto**
- `FiapCloudGames.Users.Api`: API, controllers, middlewares e configurações.
- `FiapCloudGames.Users.Application`: casos de uso, handlers, validações e pipelines.
- `FiapCloudGames.Users.Domain`: entidades, regras de negócio e contratos do domínio.
- `FiapCloudGames.Users.Infrastructure`: EF Core, migrations e acesso a dados.
- `FiapCloudGames.Users.Queue`: mensageria e publishers RabbitMQ.
- `FiapCloudGames.Users.AntiCorruption`: integração com Firebase e mapeamento de exceções.
- `FiapCloudGames.Users.Observability`: integração com New Relic e filtros.
- `FiapCloudGames.Users.Shared`: utilitários e contratos compartilhados.
- `FiapCloudGames.Users.Contract`: contratos e DTOs.
- `FiapCloudGames.Users.Tests`: testes unitários.
- `fiap-cloudgames-users-api\docker`: Dockerfile do serviço.

**Configuração e credenciais**

O projeto não versiona variáveis de ambiente nem segredos. Os valores sensíveis no `appsettings.json` ficam vazios e devem ser preenchidos localmente. Use o `appsettings.Example.json` como referência da estrutura e das chaves esperadas.

As configurações podem ser sobrescritas por variáveis de ambiente usando o padrão do ASP.NET (`__` como separador de níveis — ex.: `ConnectionStrings__DefaultConnection`, `Firebase__ApiKey`, `SqsSettings__AccessKey`).

**Como usar o Dockerfile**
Comandos de exemplo (no diretório raiz do projeto):

```bash
docker build -f docker/Dockerfile -t projetofiap/users-api:1 .
```

```bash
docker run -d -p 8082:8082 --name users-api projetofiap/users-api:1
```

Observações:
- A imagem expõe a porta `8082` (`EXPOSE 8082`), então o mapeamento recomendado é `-p 8082:8082`.
- O Dockerfile executa `dotnet FiapCloudGames.Users.Api.dll`.
