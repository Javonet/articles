using Sample.Core;

Console.WriteLine("Hello, World from .NET code!");

Javonet.Netcore.Sdk.Javonet.Activate("your-license-key");

var wrapper = new Wrapper();

Console.WriteLine(wrapper.HelloWorld());
Console.WriteLine($"Adding result from Python code: {wrapper.Add(5, 9)}");
