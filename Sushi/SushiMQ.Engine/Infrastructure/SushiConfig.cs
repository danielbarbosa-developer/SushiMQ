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