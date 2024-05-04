using System.CommandLine;
using System.Text.Json;

using Dagger.Codegen;

var introspectionJsonFileOption = new Option<FileInfo>("--introspection-json", description: "Introspection Json file path.");

var rootCommand = new RootCommand();
rootCommand.Add(introspectionJsonFileOption);
rootCommand.SetHandler((file) =>
{
    var introspection = JsonSerializer.Deserialize<Introspection>(File.ReadAllText(file.FullName))!;
    var generator = new CodeGenerator(new Dagger.Codegen.CSharp.CodeRenderer());
    Console.WriteLine(generator.Generate(introspection));
}, introspectionJsonFileOption);
rootCommand.Invoke(args);
