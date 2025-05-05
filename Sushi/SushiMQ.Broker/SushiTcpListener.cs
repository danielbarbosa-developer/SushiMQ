// Sushi MQ
// Copyright (C) 2025 Danzopen and Daniel Barbosa
//
// This file is part of Sushi MQ.
//
// Sushi MQ is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, version 3 of the License.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program. If not, see: <https://www.gnu.org/licenses/gpl-3.0.html>

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using SushiMQ.Broker.Interfaces;
using SushiMQ.Engine.Dtos;
using SushiMQ.Engine.Dtos.Enums;
using SushiMQ.Engine.Parsers;

namespace SushiMQ.Broker
{
    public class SushiTcpListener : ISushiListener
    {
        private readonly int _port;
        private TcpListener _listener;
        
        // TODO: Inject ILogger for better logging patterns

        public SushiTcpListener()
        {
            _port = 5050;
        }

        public async void Start()
        {
            _listener = new TcpListener(IPAddress.Any, _port);
            _listener.Start();
            Console.WriteLine($"🍣 Sushi Server started on port {_port}");

            while (true)
            {
                try
                {
                    var client = await _listener.AcceptTcpClientAsync();
                    _ = HandleClientAsync(client);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Accept error: {ex.Message}");
                }
            }
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            using var networkStream = client.GetStream();

            try
            {
                while (true)
                {
                    var totalLengthBuffer = new byte[4];
                    await ReadFull(networkStream, totalLengthBuffer);
                    int totalLength = BitConverter.ToInt32(totalLengthBuffer);

                    if (totalLength <= 0)
                        throw new InvalidDataException("Invalid total message length.");

                    var messageBuffer = new byte[totalLength];
                    await ReadFull(networkStream, messageBuffer);

                    byte messageType = messageBuffer[0];

                    Console.WriteLine($"📥 Received MessageType: {messageType}");

                    switch ((SushiResourceType)messageType)
                    {
                        case SushiResourceType.Health:
                            var pong = Encoding.UTF8.GetBytes("pong\n");
                            await networkStream.WriteAsync(pong);
                            break;

                        case SushiResourceType.Publish:
                            
                            byte ackModeByte = messageBuffer[1];
                            
                            var sushiProtocolMessage = await ReadAndParseMessageAsync(networkStream);
                            // Step 2 Validate Message
                            // Step 3 Call PublishHandler in Fire and Forget
                            // Step 4 Respond following protocol
                            break;

                        case SushiResourceType.Consume:
                            // TODO: Handle Consume
                            break;

                        case SushiResourceType.Register:
                            // TODO: Handle Register
                            break;

                        default:
                            var unknown = Encoding.UTF8.GetBytes("unknown message type\n");
                            await networkStream.WriteAsync(unknown);
                            break;
                    }
                }
                
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Client error: {ex.Message}");
            }
            finally
            {
                client.Close();
            }
        }

        private async Task<SushiProtocolMessage> ReadAndParseMessageAsync(NetworkStream networkStream)
        {
            var messageLengthBuffer = new byte[4];
            await ReadFull(networkStream, messageLengthBuffer);
            int messageLength = BitConverter.ToInt32(messageLengthBuffer);

            var messageBuffer = new byte[messageLength];
            await ReadFull(networkStream, messageBuffer);

            return SushiProtocolMessageParser.FromBytesSpan(messageBuffer);
        }

        private static async Task ReadFull(NetworkStream stream, byte[] buffer)
        {
            int offset = 0;
            while (offset < buffer.Length)
            {
                int read = await stream.ReadAsync(buffer, offset, buffer.Length - offset);
                if (read == 0)
                    throw new IOException("Client disconnected unexpectedly.");
                offset += read;
            }
        }
    }
}


