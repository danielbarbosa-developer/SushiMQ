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
// using BenchmarkDotNet.Attributes;
// using BenchmarkDotNet.Jobs;
// using SushiMQ.Engine.Infrastructure;
// using TestUtils.BSON;
// using TestUtils.Seeds;
//
// namespace SushiMQ.Engine.Benchmark;
//
// [MemoryDiagnoser]
// [SimpleJob(RuntimeMoniker.Net80, baseline: true)]
// public class ReadOnceQueueAddingBenchmark
// {
//     private byte[] bson;
//     
//     private uint sushiLineHash;
//     
//     private ISushiEngine sushiEngine;
//
//     [GlobalSetup]
//     public void Setup()
//     {
//         bson = BsonConvertionHelper.ConvertJsonToBytes(JsonData.GenerateJson());
//         sushiLineHash = 2750574782;
//         var config = new SushiConfig();
//         sushiEngine = new ReadOnceEngine(config);
//         sushiEngine.Start();
//     }
//
//     [Benchmark]
//     public void ReadOnceQueue_SushiLine_AddMessage_With_Success()
//     {
//         sushiEngine.AddMessage(sushiLineHash, bson);
//     }
//     
//     [Benchmark]
//     public void ReadOnceQueue_SushiLine_AddMessage_Concurrency_With_Success()
//     {
//         const int parallelTasks = 1000;
//         Parallel.For(0, parallelTasks, _ =>
//         {
//             sushiEngine.AddMessage(sushiLineHash, bson);
//         });
//     }
// }