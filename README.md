# 🩺 MedAtlas Platform - Backend

O **MedAtlas** é um sistema acadêmico voltado para estudantes de medicina, projetado para centralizar, organizar e otimizar o estudo de conteúdos médicos através de busca acadêmica, gerenciamento de biblioteca pessoal e leitura de documentos.

Este repositório armazena o ecossistema backend da plataforma, desenvolvido de forma incremental utilizando **ASP.NET Core 9**, **Clean Architecture**, **Domain-Driven Design (DDD)** tático e **Desenvolvimento Guiado por Testes (TDD)**.

---

## 🎯 Escopo Concluído

Neste primeiro ciclo, estabelecemos a fundação arquitetural do servidor e blindamos as regras de negócio iniciais do módulo de autenticação contra estados inválidos.

### ✅ Entregas

- **Infraestrutura Local com Docker**  
  Ambiente de desenvolvimento totalmente orquestrado via Docker Compose, isolando os serviços necessários com credenciais seguras e políticas de reinicialização automática.

- **Solução Modernizada (`.slnx`)**  
  Configuração da Solution utilizando o novo formato XML simplificado do .NET, organizando os projetos em camadas limpas e desacopladas.

- **Domínio de Autenticação**  
  Implementação do coração do sistema (`MedAtlas.Domain`) contendo a entidade `Usuario` e o Value Object `Email`, com regras de autovalidação em português.

- **Testes Unitários (TDD)**  
  Criação da suíte de testes automatizados com xUnit e FluentAssertions, garantindo que as regras de negócio do usuário permaneçam protegidas contra falhas.

---

## 🏗️ Infraestrutura Local (Docker)

Toda a infraestrutura do projeto roda em containers isolados sob o namespace `medatlas`, evitando conflitos de portas com outras aplicações locais.

| Serviço | Versão | Porta(s) | Descrição |
|----------|---------|-----------|------------|
| 🐘 PostgreSQL | 16 | `5432` | Banco de dados relacional principal |
| 🛑 Redis | 7.2 | `6379` | Camada de cache de alta performance protegida por senha |
| 📦 MinIO S3 | Latest | `9000 / 9001` | Object Storage para armazenamento seguro de PDFs |
| 🚀 RabbitMQ | 3.12 | `5672 / 15672` | Message broker para processamento assíncrono via MassTransit |

---

## 📂 Organização do Projeto

A árvore de diretórios combina pastas físicas no padrão global de mercado (**Inglês**) com componentes lógicos na nossa língua nativa (**Português**) para representar a Linguagem Ubíqua do domínio.

```text
backend/
├── 📁 src/
│   ├── 📁 MedAtlas.Domain/
│   │   └── Regras puras de negócio, Entidades e Objetos de Valor
│   │
│   ├── 📁 MedAtlas.Application/
│   │   └── Casos de uso (Commands/Queries) e Validadores
│   │
│   ├── 📁 MedAtlas.Infrastructure/
│   │   └── Persistência (EF/Dapper) e integrações externas
│   │
│   └── 📁 MedAtlas.API/
│       └── Controllers, Endpoints HTTP e configurações
│
└── 📁 testes/
    ├── 📁 MedAtlas.TestesUnitarios/
    │   └── Testes focados nas regras de domínio
    │
    └── 📁 MedAtlas.TestesIntegracao/
        └── Testes de fluxo e persistência
```

---

## 🔒 Práticas de Segurança Aplicadas

### Segurança por Design
As entidades do domínio não aceitam dados inconsistentes ou nulos, impedindo que falhas alcancem as camadas de persistência.

### Proteção de Credenciais
Senhas e chaves secretas locais são gerenciadas exclusivamente através de:

- `.env`
- `appsettings.Development.json`

Ambos permanecem protegidos no `.gitignore`, evitando vazamento acidental para o GitHub.

### Separação de Responsabilidades
Exceções internas do domínio mantêm mensagens técnicas para desenvolvedores, enquanto mensagens amigáveis de validação permanecem isoladas na camada de aplicação.

---

## 🚀 Como Executar e Validar o Projeto

### Pré-requisitos

Antes de iniciar, certifique-se de possuir instalado:

- **.NET 9 SDK**
- **Docker Desktop**

---

### 1️⃣ Configurar as Credenciais

Duplique o arquivo `.env.example`, renomeie a cópia para `.env` e preencha suas credenciais locais.

Depois, crie o arquivo `appsettings.Development.json` utilizando o modelo fornecido pelo projeto.

---

### 2️⃣ Subir a Infraestrutura

Execute o comando abaixo na raiz do projeto:

```bash
docker compose up -d
```

---

### 3️⃣ Rodar os Testes Automatizados

Na raiz da pasta `backend/`, execute:

```bash
dotnet test
```

---

## 🧪 Qualidade e Arquitetura

O backend segue uma abordagem arquitetural baseada em:

- **Clean Architecture**
- **Domain-Driven Design (DDD)**
- **Test-Driven Development (TDD)**
- **SOLID Principles**
- **Separation of Concerns**

O objetivo é garantir um sistema **modular, escalável, testável e resiliente**, preparado para evolução contínua ao longo das próximas sprints.

---

## 📌 Status do Projeto

**Sprint 1 — Concluída ✅**

Fundação arquitetural estabelecida, domínio de autenticação protegido por testes e infraestrutura local pronta para evolução dos próximos módulos.
