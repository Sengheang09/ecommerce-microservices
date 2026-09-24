# E-Commerce Distributed Polyglot Microservices Platform

[![Spring Boot](https://img.shields.io/badge/Spring%20Boot-3.4.3-brightgreen.svg?logo=springboot)](https://spring.io/projects/spring-boot)

[![Spring Cloud](https://img.shields.io/badge/Spring%20Cloud-2024.0.0-green.svg)](https://spring.io/projects/spring-cloud)

[![.NET](https://img.shields.io/badge/.NET-10.0-purple.svg?logo=dotnet)](https://dotnet.microsoft.com/)

[![Java](https://img.shields.io/badge/Java-17-orange.svg?logo=openjdk)](https://www.oracle.com/java/)

[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-17-blue.svg?logo=postgresql)](https://www.postgresql.org/)

[![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-red.svg?logo=microsoftsqlserver)](https://www.microsoft.com/sql-server)

[![Docker](https://img.shields.io/badge/Docker-Enabled-2496ED.svg?logo=docker)](https://www.docker.com/)
[![License](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

An enterprise-grade, distributed polyglot microservices platform designed for high scalability, fault tolerance, and independent domain deployments. Built using **Spring Boot 3 (Java 17)** for authentication, **ASP.NET Core (.NET 10)** for high-throughput product and order processing, and **Spring Cloud Gateway** for centralized traffic management.

---
##  Table of Contents
- [Architectural Overview](#-architectural-overview)
- [System Landscape & Port Mapping](#-system-landscape--port-mapping)
- [Microservices Breakdown](#-microservices-breakdown)
  - [1. API Gateway](#1-api-gateway-service)
  - [2. Authentication Service](#2-authentication-service)
  - [3. Core Domain & Order Service](#3-core-domain--order-service)
- [Database Architecture (Database-per-Service)](#-database-architecture)
- [API Catalog & Endpoint Reference](#-api-catalog--endpoint-reference)
- [Environment Configuration & Security](#-environment-configuration--security)
- [Getting Started & Local Setup](#-getting-started--local-setup)
- [Future Roadmap](#-future-roadmap)

---

## Architectural Overview

The system adheres to standard microservices principles including **Database-per-Service**, **API Gateway pattern**, **Stateless JWT Authentication**, and **Decoupled Polyglot Architecture**.

```mermaid
flowchart TD
    Client[" Client Applications (React / Vue / Mobile)"] -->|Single Entry Point :8080| Gateway[" Spring Cloud Gateway (:8080)"]

    subgraph Edge Layer
        Gateway -->|"/api/auth/**"| AuthSvc[" Auth Service (:8081)\nJava 17 / Spring Boot 3\nFlyway Migrations"]
        
        Gateway -->|"/api/** (Customers, Products, Cart, Orders)"| CoreSvc[" Core & Order Service (:5059)\nC# / ASP.NET Core (.NET 10)\nEntity Framework Core"]
    end

    subgraph Data Persistence Layer
        AuthSvc -->|JDBC / Port 5432| Postgres[("🐘 PostgreSQL 17\n(ecommerce_auth_db)")]
        CoreSvc -->|T-SQL / Port 1433| SqlServer[("🗄️ SQL Server 2022\n(ecommerce_user_db)")]
    end