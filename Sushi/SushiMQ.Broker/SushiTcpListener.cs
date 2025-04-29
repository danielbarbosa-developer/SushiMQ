// Sushi MQ
// Copyright (C) 2025 Danzopen and Daniel Barbosa
//
// This file is part of Sushi MQ.
//
// Sushi MQ is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, **version 3** of the License.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program. If not, see: <https://www.gnu.org/licenses/gpl-3.0.html>
//
// This license ensures that you can use, study, share, and improve this software
// freely, as long as you preserve this license and credit the original authors.
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using SushiMQ.Broker.Interfaces;

namespace SushiMQ.Broker
{
    public class SushiTcpListener : ISushiListener
    {
        private readonly int _port;
        private TcpListener _listener;

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
                    var routeSizeBuffer = new byte[2];
                    int read = await networkStream.ReadAsync(routeSizeBuffer, 0, 2);
                    if (read == 0) break;
                    ushort routeSize = BitConverter.ToUInt16(routeSizeBuffer);

                    var routeBuffer = new byte[routeSize];
                    await ReadFull(networkStream, routeBuffer);
                    string route = Encoding.UTF8.GetString(routeBuffer);

                    var bsonSizeBuffer = new byte[4];
                    await ReadFull(networkStream, bsonSizeBuffer);
                    int bsonSize = BitConverter.ToInt32(bsonSizeBuffer);

                    var bsonBuffer = new byte[bsonSize];
                    await ReadFull(networkStream, bsonBuffer);
                    
                    Console.WriteLine($"📥 Route: {route}");

                    if (route == "health")
                    {
                        var pong = Encoding.UTF8.GetBytes("pong\n");
                        await networkStream.WriteAsync(pong);
                    }
                    else if (route == "echo")
                    {
                        var test = "{test: 1}";
                        var response = test.ToBson();
                        await networkStream.WriteAsync(response);
                    }
                    else
                    {
                        var unknown = Encoding.UTF8.GetBytes("unknown route\n");
                        await networkStream.WriteAsync(unknown);
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