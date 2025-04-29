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

using Microsoft.Extensions.Hosting;
using SushiMQ.Broker.Interfaces;
using SushiMQ.Engine;

namespace SushiMQ.Broker;

public class SushiHostedServer : IHostedService
{
    private ISushiListener _listener;
    private ISushiEngine _engine;
    
    public SushiHostedServer(ISushiListener listener, ISushiEngine engine)
    {
        _listener = listener;
        _engine = engine;
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _engine.Start();
        _listener.Start();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}