# Sushi Message Queue

![Cute Sushi](assets/CuteSushi.png)

![CI Status](https://github.com/danzopen/sushi/actions/workflows/build.yml/badge.svg)


**Sushi MQ** is an open-source, fast and easy to use messaging system built with modern C#. Designed for performance applications.

Inspired by event streaming platforms like Kafka and RabbitMQ, Sushi MQ is designed to provide a fast, flexible, and lightweight messaging core — built for modern distributed systems.

> Just like a sushi conveyor belt, Sushi MQ keeps your data flowing — clean, efficient, and always moving.

---

## ✨ Highlights

- **Built with TcpListener** — Uses `TcpListener` with asynchronous processing for clean, scalable message handling.

- **Binary Protocol** — Efficient, low-overhead message routing with compact binary headers.

- **BSON Message Payloads** — Schema-flexible, high-speed serialization format.

- **In-Memory Queues** — Prioritizes speed with optional persistence.

- **Publish/Subscribe Semantics** — For real-time and decoupled communication patterns.

- **Priority-Based Delivery** — Dynamic prioritization of messages for low/high urgency.

---


## 📌 Project Status

| Feature                                  | Status        | Description                                              |
|------------------------------------------|---------------|----------------------------------------------------------|
| TCP server with `TcpListener` + async I/O | 🛠️ In Progress   | High-performance TCP listener handling client connections |
| Custom binary protocol with route headers | 🛠️ In Progress   | Lightweight protocol with binary routing headers         |
| BSON-encoded messages                    | ✅ Completed   | Fast and flexible serialization with BSON                |
| In-memory message queue engine           | 🛠️ In Progress  | Core queuing logic running in memory                     |
| Logical message channels (Sushi Lines)   | 🛠️ In Progress  | Simplified topic model for clean routing                 |
| Basic publish/subscribe support          | 🛠️ In Progress  | Clients can publish and subscribe to message lines       |
| Priority-Based Delivery    | 💡 Planned     | Dynamic prioritization of messages for low/high urgency  |
| Persistent storage layer                 | 💡 Planned | Optional durability and replay capability                |
| Command-line interface for management    | 💡 Planned  | Manage queues, monitor traffic, and debug flows          |
| Web-based admin dashboard                | 💡 Planned     | Visual dashboard for monitoring and control              |
| gRPC and HTTP protocol support           | 💡 Planned     | Expose Sushi MQ via multiple transport protocols         |


---

## 💡 Who Is It For?

Sushi MQ is ideal for:

- Developers building high-performance microservices
- Teams needing a fast internal message bus
- Projects requiring custom messaging protocols
- Anyone who wants messaging speed without the weight of Kafka or RabbitMQ

---

## 🤝 Contribute

We welcome contributions from the community! Whether you're into systems programming, .NET internals, protocols, or just curious about messaging queues — there's a place for you here.

Ways to contribute:
- Suggest ideas or features
- Report bugs or performance issues
- Submit pull requests
- Improve documentation

---

## 📜 License

This project is licensed under the **GNU General Public License (GPL)** Version 3, 29 June 2007.

Sushi MQ  - Copyright (C) [2025] **Danzopen** and **Daniel Barbosa**.

You may not use this project except in compliance with the License. You may obtain a copy of the License at:

[https://www.gnu.org/licenses/gpl-3.0.html](https://www.gnu.org/licenses/gpl-3.0.html)


---

