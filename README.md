# gRPC-C-Master-Class-Build-Modern-API-Microservices

# Course Notes: gRPC and Protocol Buffers

## Overview

- **Protocol Buffers**:
  - Binary format, very close to how data is stored in memory.
  - Requires less CPU than JSON, which is human-readable but larger and CPU-intensive.
  - Efficient serialization and deserialization.

## Supported Languages

- Java
- Go
- C#
- C → C++
- Python

Note: There are four different implementations, and there might be differences in features among them. For example, C# supports HTTP/3.

## HTTP/2 vs. HTTP/1.1

- **HTTP/2**:
  - gRPC Based on HTTP/2, which is much faster than HTTP/1.1.
  - Test it using this [visualization](https://imagekit.io/demo/http2-vs-http1?utm_source=blog&utm_medium=blog&utm_campaign=Blog).

### Why HTTP/1.1 is Slower

- Uses a separate TCP connection for each request.
- Doesn’t compress headers.
- Handles each request/response on a single TCP connection, which may stay open for a long time, consuming more resources and increasing latency.

### Why HTTP/2 is Faster

- Uses one TCP connection shared by multiple requests (Less chatter).
- Supports server push: the server pushes messages to the client without the client needing to request them.
- Supports multiplexing: both the server and client can send multiple messages over the same TCP connection.
- Compresses headers and data into binary format (less bandwidth).
- Secure by default.

## Performance Metrics

- **Latency**: Indicates delay.
- **Throughput**: Measures the actual data transfer rate.
- **Bandwidth**: Specifies maximum capacity.

Optimizing latency reduces delays, improving throughput increases transfer speed, and adding bandwidth enhances capacity.

## Types of gRPC

1. **Unary**: Same as REST.
2. **Server Streaming**: The client sends one request, but the server can send many responses.
3. **Client Streaming**: The client sends multiple requests, and the server returns one response.
4. **Bi-directional Streaming**: The client can send multiple requests, and the server returns multiple responses.

## gRPC Error Handling

- Default error codes are provided. Refer to the gRPC [documentation](https://grpc.io/docs/guides/status-codes/) for more details.

## gRPC Deadline Exceed

- Each RPC connection should have a deadline. If it exceeds the deadline, the RPC will terminate. Refer to the gRPC [documentation](https://grpc.io/docs/guides/deadlines/) for more details.

## SSL Connection

- gRPC supports SSL/TLS for secure communication.

## gRPC reflection

- Provide a way to dynamically discover the services and models inside gRPC service so it helps consumers to create their gRPC client to consume the gRPC service.
- Evans is a CLI tool that allows you to interact with gRPC services without needing to write a client application. It leverages gRPC reflection to discover services and methods. 
