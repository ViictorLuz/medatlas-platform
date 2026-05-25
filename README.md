# 🩺 MedAtlas Platform - Backend

O **MedAtlas** é um sistema acadêmico voltado para estudantes de medicina, projetado para centralizar, organizar e otimizar o estudo de conteúdos médicos através de busca acadêmica, gerenciamento de biblioteca pessoal e leitura de documentos.

Este repositório armazena o ecossistema backend da plataforma, desenvolvido de forma incremental utilizando **ASP.NET Core 9**, **Clean Architecture**, **Domain-Driven Design (DDD)** tático e **Desenvolvimento Guiado por Testes (TDD)**.

---

## 🎯 Escopo Concluído

Até o momento, estabelecemos a fundação arquitetural do servidor, consolidamos o ecossistema de autenticação e estruturamos o pipeline completo de recepção, validação física, persistência e publicação assíncrona de documentos médicos.

### ✅ Entregas

- **Infraestrutura Local com Docker**  
  Ambiente de desenvolvimento totalmente orquestrado via Docker Compose, isolando os serviços necessários (**PostgreSQL**, **Redis**, **MinIO S3** e **RabbitMQ**) com credenciais seguras e políticas de reinicialização automática.

- **Solução Modernizada (`.slnx`)**  
  Configuração da Solution utilizando o novo formato XML simplificado do .NET, organizando os projetos em camadas limpas e desacopladas.

- **Domínio e Casos de Uso (Autenticação & Biblioteca)**  
  Implementação das regras de negócio do `Usuario` (com autovalidação de e-mail) e da entidade `Documento`, responsável pelo gerenciamento de metadados dos arquivos da biblioteca, utilizando **MediatR** para desacoplamento via **Mediator Pattern**.

- **Armazenamento de Objetos (MinIO S3)**  
  Integração com o SDK oficial da AWS para persistência física de streams de arquivos, adotando uma estrutura lógica isolada por usuário:

  ```text
  biblioteca/usuarios/{usuarioId}/{documentoId}.pdf
  ```

- **Mensageria Assíncrona (MassTransit + RabbitMQ)**  
  Configuração do barramento de mensagens para publicação do `DocumentoEnviadoEvent`, preparando a arquitetura para processamento assíncrono e extração de texto em background (**Event-Driven Architecture**).

- **Testes Unitários (TDD)**  
  Suíte de testes automatizados com **xUnit** e **FluentAssertions**, validando exaustivamente regras de domínio, handlers de aplicação e validadores de assinatura física de arquivos.

---

## 🏗️ Infraestrutura Local (Docker)

Toda a infraestrutura do projeto roda em containers isolados sob o namespace `medatlas`, evitando conflitos de portas com outras aplicações locais.

| Serviço | Versão | Porta(s) | Descrição |
|----------|---------|-----------|------------|
| 🐘 PostgreSQL | 16 | `5432` | Banco de dados relacional principal |
| 🛑 Redis | 7.2 | `6379` | Camada de cache de alta performance protegida por senha |
| 📦 MinIO S3 | Latest | `9000 / 9001` | Object Storage para armazenamento seguro dos PDFs da biblioteca |
| 🚀 RabbitMQ | 3.12 | `5672 / 15672` | Message broker para processamento assíncrono gerenciado via MassTransit |

---

## 📂 Organização do Projeto

A árvore de diretórios combina pastas físicas no padrão global de mercado (**Inglês**) com componentes lógicos na nossa língua nativa (**Português**) para representar a Linguagem Ubíqua do domínio.

```text
backend/
├── 📁 src/
│
│   ├── 📁 MedAtlas.API/
│   │   ├── 📁 Controllers/
│   │   │   └── 📄 BibliotecaController.cs
│   │   │       → Endpoint HTTP (POST) para recepção do upload
│   │   │
│   │   ├── 📄 appsettings.json
│   │   │       → Configurações globais da aplicação
│   │   │
│   │   └── 📄 Program.cs
│   │           → Pipeline HTTP, Kestrel, Swagger e amarrações do IoC
│   │
│   ├── 📁 MedAtlas.Application/
│   │   └── 📁 Features/
│   │       └── 📁 Library/
│   │           ├── 📁 Commands/
│   │           │   └── 📁 UploadDocument/
│   │           │       ├── 📄 EnviarDocumentoCommand.cs
│   │           │       │       → Payload do caso de uso de upload
│   │           │       │
│   │           │       └── 📄 EnviarDocumentoCommandHandler.cs
│   │           │               → Executor e orquestrador do fluxo de upload
│   │           │
│   │           ├── 📁 Common/
│   │           │   └── 📄 ValidadorPdf.cs
│   │           │           → Validação física da assinatura de bytes (%PDF)
│   │           │
│   │           └── 📁 Events/
│   │               └── 📄 DocumentoEnviadoEvent.cs
│   │                       → Contrato do evento publicado no barramento
│   │
│   ├── 📁 MedAtlas.Domain/
│   │   ├── 📁 Common/
│   │   │   └── 📄 Entidade.cs
│   │   │           → Entidade base com Id e comportamentos comuns
│   │   │
│   │   ├── 📁 Exceptions/
│   │   │   └── 📄 ExcecaoDeDominio.cs
│   │   │           → Tratamento unificado de regras de negócio
│   │   │
│   │   └── 📁 Modules/
│   │       ├── 📁 Authentication/
│   │       │   ├── 📁 Entities/
│   │       │   │   └── 📄 Usuario.cs
│   │       │   │
│   │       │   ├── 📁 Repositories/
│   │       │   │   └── 📄 IUsuarioRepository.cs
│   │       │   │
│   │       │   └── 📁 ValueObjects/
│   │       │       └── 📄 Email.cs
│   │       │
│   │       └── 📁 Library/
│   │           ├── 📁 Entities/
│   │           │   └── 📄 Documento.cs
│   │           │           → Entidade raiz do upload com autovalidação
│   │           │
│   │           └── 📁 Interfaces/
│   │               └── 📄 IStorageService.cs
│   │                       → Contrato da abstração de storage
│   │
│   └── 📁 MedAtlas.Infrastructure/
│       ├── 📁 DependencyInjection/
│       │   └── 📄 InjetorDeDependencias.cs
│       │           → Registro do IoC, SDK MinIO S3 e MassTransit
│       │
│       └── 📁 Storage/
│           └── 📄 MinIoStorageService.cs
│                   → Persistência concreta no bucket local
│
└── 📁 testes/
    └── 📁 MedAtlas.TestesUnitarios/
        ├── 📁 Application/
        │   └── 📁 Library/
        │       └── 📄 EnviarDocumentoCommandHandlerTestes.cs
        │               → Cobertura do fluxo do handler
        │
        └── 📁 Domain/
            ├── 📁 Authentication/
            │   └── 📄 UsuarioTestes.cs
            │           → Validação das regras do usuário
            │
            └── 📁 Library/
                └── 📄 DocumentoTestes.cs
                        → Validação das regras do documento
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

### Validação Física de Arquivos (Magic Numbers)

O sistema não confia exclusivamente na extensão `.pdf` enviada pelo cliente. O pipeline inspeciona os primeiros bytes do stream em memória para validar a assinatura física do arquivo (`%PDF-`), mitigando tentativas de personificação de arquivos maliciosos.

### Fail-Fast de Infraestrutura

A inversão de dependência no handler garante que as regras de domínio validem integridade de dados (como IDs obrigatórios) antes do upload físico para o storage S3, evitando consumo desnecessário de I/O e prevenindo arquivos órfãos na infraestrutura.

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
- **Mediator Pattern**
- **Event-Driven Architecture**

O objetivo é garantir um sistema **modular, escalável, testável e resiliente**, preparado para evolução contínua ao longo das próximas sprints.

---

## 📌 Status do Projeto

- **Sprint 1 — Concluída ✅**  
  Fundação arquitetural estabelecida, domínio de autenticação protegido por testes e infraestrutura local orquestrada.

- **Sprint 2 — Concluída ✅**  
  Pipeline de upload de documentos consolidado, integração com **MinIO S3**, publicação assíncrona de eventos com **MassTransit/RabbitMQ** e validação física de PDFs protegida por testes automatizados.
