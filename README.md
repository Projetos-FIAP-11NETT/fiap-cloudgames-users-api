# Fiap Cloud Games Users API 

**Descrição**

API responsável pelo fluxo de usuários do ecossistema Fiap Cloud Games, incluindo autenticação via Firebase, persistência em banco relacional e publicaçãoo de eventos para integração com outros serviços.

**Funcionalidades**
- Cadastro de usuário.
- Autenticação/login de usuário.
- Promoção de usuário para perfil administrador.
- Integração com Firebase Authentication.
- Cache de sessão distribuído em Redis.
- Publicação de evento de usuário criado em fila (RabbitMQ / Amazon SQS).
- Health checks em `/health`, `/health/ready` e `/health/live`.
- Documentação OpenAPI com Scalar.
- Observabilidade com New Relic e logs estruturados via Serilog.

**Tecnologias**
- .NET 10 e ASP.NET Core.
- MediatR.
- Entity Framework Core + Npgsql (PostgreSQL).
- Redis (cache de sessão distribuído — `RedisSessionCacheService`).
- Firebase Admin SDK.
- MassTransit com RabbitMQ / Amazon SQS.
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

---

## Deploy (CI/CD — GitHub Actions)

O deploy é automatizado por 3 workflows em `.github/workflows/`, que juntos implementam um fluxo **GitHub Flow com `develop`**: toda mudança nasce numa branch de feature, vira PR pra `develop`, e o deploy real é disparado por uma **tag de versão**.

| Workflow | Dispara em | O que faz |
|---|---|---|
| `01-ci-push.yml` | `push` em `feature/**`, `bugfix/**` ou `hotfix/**` | Build + testes; se não existir PR aberto para `develop` a partir dessa branch, cria um automaticamente (via `gh pr create`) |
| `02-ci-pull-request.yml` | PR aberto/atualizado contra `develop` ou `main` | *Quality gate*: build, testes com cobertura, checagem de pacotes NuGet vulneráveis (**bloqueia** o merge) e desatualizados (apenas aviso) |
| `03-cd-release.yml` | `push` de tag `v*` | Build + testes, build da imagem Docker, publica no ECR e faz deploy no EKS |

### Passo a passo — do código à produção

**1. Criar a branch de feature a partir de `develop`:**

```bash
git checkout develop
git pull origin develop
git checkout -b feature/minha-mudanca
```

**2. Commitar e dar push:**

```bash
git add .
git commit -m "feat: minha mudança"
git push -u origin feature/minha-mudanca
```

Isso dispara o `01-ci-push.yml`: builda, roda os testes e — se ainda não existir — abre automaticamente um Pull Request de `feature/minha-mudanca` para `develop`.

**3. Revisão e merge:**

A cada push no PR, o `02-ci-pull-request.yml` roda o quality gate (build, testes + cobertura, vulnerabilidades). Só faz merge do PR em `develop` depois dele passar.

**4. Gerar e enviar a tag de release** (isso é o que efetivamente dispara o deploy):

```bash
git checkout develop
git pull origin develop
git tag v1.4.0
git push origin v1.4.0
```

> A tag **precisa apontar para um commit que já esteja em `develop`** — o `03-cd-release.yml` valida isso (`git merge-base --is-ancestor`) e falha se a tag não pertencer à branch.

O `03-cd-release.yml` então:
1. Builda e testa a aplicação.
2. Builda a imagem Docker, marcando com a tag da versão (`$IMAGE_TAG`) **e** com `latest`.
3. Autentica no ECR com as credenciais AWS (Academy) configuradas como secrets do repositório e faz o push das duas tags.
4. Roda o scanner **Trivy** na imagem (severidade HIGH/CRITICAL — hoje não bloqueia o pipeline).
5. Faz checkout do repositório `fiap-cloudgames-infrastructure` (branch `feature/fase-4`), conecta o `kubectl` ao cluster EKS e aplica os manifests em `k8s/users/api`.
6. Executa `kubectl rollout restart deployment/users-deployment` e aguarda o rollout — como o manifest referencia a imagem sem tag explícita (equivale a `:latest`), o restart força o pod a repuxar a imagem recém-publicada.

> **AWS Academy:** como as credenciais de sessão expiram, os secrets `AWS_ACCESS_KEY_ID` / `AWS_SECRET_ACCESS_KEY` / `AWS_SESSION_TOKEN` do repositório (Settings → Secrets and variables → Actions) precisam ser atualizados a cada sessão de lab antes de enviar uma nova tag — senão o job `publish-image`/`deploy-eks` falha na autenticação.

Todos os workflows também podem ser disparados manualmente (`workflow_dispatch`) na aba **Actions** do GitHub.
