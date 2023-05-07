// See https://aka.ms/new-console-template for more information
// Refer https://blog.csdn.net/csdn2990/article/details/129664309

using BenchmarkDotNet.Running;
using dxStudyYieldReturn;

var sumery = BenchmarkRunner.Run<BenchmarkTest>();
Console.WriteLine("Hello, World!");
