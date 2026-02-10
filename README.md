# Auth API - .NET 8 + JWT

API REST desenvolvida em **ASP.NET Core .NET 8** com autenticação JWT.

## Funcionalidades
- Registro de usuário
- Login com BCrypt
- Geração de JWT
- Endpoints protegidos com `[Authorize]`
- Endpoint `/users/me` usando claims do token
- SQLite com Entity Framework Core

## Tecnologias
- .NET 8
- ASP.NET Core
- Entity Framework Core
- SQLite
- JWT Bearer
- BCrypt

## Como rodar o projeto

```bash
dotnet restore
dotnet run
