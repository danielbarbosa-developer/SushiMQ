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

using System.Text;
using Force.Crc32;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SushiMQ.Engine.Infrastructure;

public interface ISushiConfig
{
    SushiLineMetadataDto[] GetSushiLineMetadata();
    
}

public class SushiConfig : ISushiConfig
{
    private const string ConfigFilePath = "./sushi_mq.yml";

    public SushiLineMetadataDto[] GetSushiLineMetadata()
    {
        var sushiConfig = LoadSushiConfig();
        
        var sushiLineMetadataArray = new SushiLineMetadataDto[sushiConfig.SushiLines.Length];

        for (int i = 0; i < sushiConfig.SushiLines.Length; i++)
            sushiLineMetadataArray[i] = ConvertToSushiLineMetadataDto(sushiConfig.SushiLines[i]);
        
        sushiConfig.Dispose();
        
        return sushiLineMetadataArray;
    }

    private SushiLineMetadataDto ConvertToSushiLineMetadataDto(SushiLineConfig sushiLineConfig)
    {
        var sushiLineMetadata = new SushiLineMetadataDto()
        {
            SushiLineHash = Crc32Algorithm.Compute(Encoding.UTF8.GetBytes(sushiLineConfig.SushiLineName)),
            Storage = sushiLineConfig.SushiLineStorage ? (byte)1 : (byte)0,
            Consumption = (byte)sushiLineConfig.SushiLineConsumption,
        };
        return sushiLineMetadata;
    }

    private SushiConfigDto LoadSushiConfig()
    {
        var yaml = File.ReadAllText(ConfigFilePath);

        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(HyphenatedNamingConvention.Instance)
            .IgnoreUnmatchedProperties()
            .Build();

        var config = deserializer.Deserialize<SushiConfigDto>(yaml);
        return config;
    }
}