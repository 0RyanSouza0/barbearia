# Barbearia API

API REST para gerenciamento de uma barbearia, com foco em cadastro de clientes, produtos, pedidos, agendamentos e regras de negócio aplicadas na camada de aplicação/domínio.

---

## 🚀 Tecnologias

- C#
- ASP.NET Core
- Entity Framework Core
- SQL Server
- FluentValidation
- xUnit
- Moq
- FluentAssertions
- GitHub Actions
- TypeScript / CSS (Frontend)

---



## 📁 Estrutura do projeto

```txt
barbearia/
├── .github/workflows/      # Pipeline de CI (GitHub Actions)
├── frontend/               # Aplicação frontend
├── src/                    # Código fonte da API
├── test/Barbearia.Tests/   # Testes automatizados
├── Barbearia.slnx
└── .gitignore

⚙️ Funcionalidades
Cadastro e gerenciamento de clientes
Cadastro e gerenciamento de produtos
Controle de pedidos
Gerenciamento de itens de pedido
Agendamentos
Validações com FluentValidation
Padronização de respostas com Result Pattern
Testes unitários da camada de aplicação
Integração contínua com GitHub Actions
🔁 CI/CD

O projeto utiliza GitHub Actions para validar automaticamente o código a cada Pull Request ou push na branch principal.

Pipeline atual:

restore → build → test

Status esperado no GitHub:

CI - Barbearia API / build-test
▶️ Como executar o projeto

Clone o repositório:

git clone https://github.com/0RyanSouza0/barbearia.git
cd barbearia

Restaure os pacotes:

dotnet restore

Execute o build:

dotnet build

Execute os testes:

dotnet test

Execute a API:

dotnet run --project src/Barbearia.Api
🔐 Variáveis de ambiente

Este projeto utiliza variáveis de ambiente para dados sensíveis, como connection strings.

Crie um arquivo local, por exemplo:

env.barbearia.sh

Exemplo:

export ConnectionStrings__DefaultConnection="Server=localhost;Database=BarbeariaDb;User Id=SEU_USUARIO;Password=SUA_SENHA;"

Depois carregue:

source env.barbearia.sh

⚠️ Arquivos de ambiente não devem ser versionados no Git.

🧪 Testes

Para rodar todos os testes:

dotnet test

Os testes cobrem:

Serviços da aplicação
Regras de negócio
Validações com FluentValidation
Uso de mocks com Moq
🎯 Objetivo do projeto

Este projeto foi desenvolvido com objetivo de praticar:

Arquitetura em camadas (Clean Architecture)
Boas práticas com ASP.NET Core
Validação de dados
Testes unitários
Versionamento com Git
Integração contínua (CI)
👨‍💻 Autor

Desenvolvido por Ryan Dias

