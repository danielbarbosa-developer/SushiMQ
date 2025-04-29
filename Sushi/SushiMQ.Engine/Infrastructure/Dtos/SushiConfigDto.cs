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

namespace SushiMQ.Engine.Infrastructure;

public record SushiConfigDto : IDisposable
{
    public SushiServerConfig Server { get; set; }
    
    public SushiLineConfig[] SushiLines { get; set; }
    
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}

public record SushiServerConfig
{
    public string Host { get; set; }
    public int Port { get; set; }
}

public record SushiLineConfig
{
    public string SushiLineName { get; set; }
    public bool SushiLineStorage { get; set; }
    public SushiLineConsumption SushiLineConsumption { get; set; }
}


public enum SushiLineConsumption : byte
{
    ReadOnce,
    Fanout,
    ReadMany
}