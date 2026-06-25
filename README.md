# AI Chat Platform

A cloud-agnostic, enterprise-grade AI chat platform built with:

- Aspire
- Blazor (.NET 10 / C# 14)
- Radzen Blazor UI
- WebSockets
- LlamaSharp
- Qwen 3.6 35B GGUF
- Local GGUF inference
- On-premises and air-gapped deployment support

---

# Overview

The platform provides a real-time conversational AI experience through a Blazor web application. Communication between the browser and server occurs over WebSockets for low-latency streaming responses.

The backend hosts LlamaSharp and loads a local Qwen 3.6 35B GGUF model for inference.

The solution is designed to be:

- Cloud agnostic
- Kubernetes compatible
- VM compatible
- Bare-metal compatible
- Air-gap deployable
- Vendor neutral

---

# High-Level Architecture

```mermaid
flowchart LR

    User[User Browser]

    subgraph Frontend
        Blazor[Blazor UI]
        Radzen[Radzen Components]
    end

    subgraph Application
        WS[WebSocket Gateway]
        ChatSvc[Chat Service]
        SessionSvc[Session Service]
    end

    subgraph AI Runtime
        LlamaSharp[LlamaSharp]
        Qwen[Qwen 3.6 35B GGUF]
    end

    subgraph Persistence
        DB[(SQL Database)]
    end

    User --> Blazor
    Blazor --> Radzen

    Blazor <--> WS

    WS --> ChatSvc
    ChatSvc --> SessionSvc

    ChatSvc --> LlamaSharp
    LlamaSharp --> Qwen

    SessionSvc --> DB
    ChatSvc --> DB
```

---

# Component Architecture

```mermaid
flowchart TB

    subgraph Client
        Browser[Browser]
        RadzenUI[Radzen Blazor UI]
    end

    subgraph Web Layer
        AspireHost[Aspire App Host]
        BlazorServer[Blazor Server]
        WebSockets[WebSocket Endpoint]
    end

    subgraph Services
        ChatService[Chat Service]
        ConversationService[Conversation Service]
        PromptService[Prompt Service]
    end

    subgraph AI
        LlamaSharpRuntime[LlamaSharp Runtime]
        GGUF[Qwen 3.6 35B GGUF]
    end

    subgraph Data
        SQL[(SQL Database)]
    end

    Browser --> RadzenUI
    RadzenUI --> BlazorServer

    BlazorServer --> WebSockets

    WebSockets --> ChatService

    ChatService --> ConversationService
    ChatService --> PromptService

    ChatService --> LlamaSharpRuntime
    LlamaSharpRuntime --> GGUF

    ConversationService --> SQL
```

---

# Deployment Architecture

## Cloud Agnostic / Air-Gapped Deployment

```mermaid
flowchart TB

    subgraph UserNetwork
        User[Users]
    end

    subgraph Deployment Environment

        LB[Load Balancer / Reverse Proxy]

        subgraph Application Tier
            App1[Aspire + Blazor Node]
            App2[Aspire + Blazor Node]
        end

        subgraph AI Tier
            AI1[LlamaSharp Inference]
            Model[Qwen 3.6 35B GGUF]
        end

        subgraph Data Tier
            SQL[(SQL Database)]
        end
    end

    User --> LB

    LB --> App1
    LB --> App2

    App1 --> AI1
    App2 --> AI1

    AI1 --> Model

    App1 --> SQL
    App2 --> SQL
```

### Supported Hosting Models

- Azure
- AWS
- Google Cloud
- OpenShift
- Kubernetes
- Docker Compose
- Virtual Machines
- Bare Metal
- Air-Gapped Secure Networks

---

# Sequence Diagram

## User Chat Interaction

```mermaid
sequenceDiagram

    actor User

    participant UI as Blazor UI
    participant WS as WebSocket Gateway
    participant Chat as Chat Service
    participant Llama as LlamaSharp
    participant Model as Qwen 3.6 35B GGUF
    participant DB as Database

    User->>UI: Enter Prompt

    UI->>WS: WebSocket Message

    WS->>Chat: Forward Prompt

    Chat->>DB: Save User Message

    Chat->>Llama: Generate Response

    Llama->>Model: Run Inference

    Model-->>Llama: Token Stream

    loop Streaming Tokens
        Llama-->>Chat: Token
        Chat-->>WS: Token
        WS-->>UI: Token
    end

    Chat->>DB: Save Assistant Response

    UI-->>User: Render Streaming Reply
```

---

# Entity Relationship Diagram (ERD)

```mermaid
erDiagram

    USER ||--o{ CHAT_SESSION : owns
    CHAT_SESSION ||--o{ MESSAGE : contains
    MODEL ||--o{ CHAT_SESSION : uses

    USER {
        guid UserId
        string Username
        datetime CreatedAt
    }

    CHAT_SESSION {
        guid SessionId
        guid UserId
        guid ModelId
        datetime StartedAt
        datetime LastActivity
    }

    MESSAGE {
        guid MessageId
        guid SessionId
        string Role
        text Content
        int TokenCount
        datetime Timestamp
    }

    MODEL {
        guid ModelId
        string Name
        string Version
        string Format
        string Path
    }
```

---

# Runtime Flow

```mermaid
flowchart LR

    Prompt[User Prompt]
        --> Validation[Input Validation]

    Validation
        --> Context[Conversation Context Builder]

    Context
        --> Inference[LlamaSharp Inference]

    Inference
        --> QwenModel[Qwen 3.6 35B GGUF]

    QwenModel
        --> Tokens[Streaming Tokens]

    Tokens
        --> WebSocket[WebSocket Stream]

    WebSocket
        --> UI[Blazor Chat UI]
```

---

# Technology Stack

| Layer | Technology |
|---------|------------|
| Frontend | Blazor |
| Components | Radzen Blazor |
| Realtime | WebSockets |
| Backend | ASP.NET Core |
| Orchestration | Aspire |
| AI Runtime | LlamaSharp |
| LLM | Qwen 3.6 35B GGUF |
| Database | SQL Server / PostgreSQL |
| Deployment | Docker / Kubernetes / VM / Bare Metal |
| Networking | Reverse Proxy + WebSockets |

---

# Security Considerations

- TLS everywhere
- Optional SSO/OIDC integration
- Role-based access control
- Audit logging
- Air-gapped deployment support
- No dependency on external AI services
- Local model execution
- Local data residency

---

# Non-Functional Requirements

- Horizontal scaling of application nodes
- Streaming token responses
- High concurrency WebSocket connections
- Cloud-neutral deployment
- Air-gapped support
- GPU acceleration
- Local model hosting
- Enterprise observability via Aspire