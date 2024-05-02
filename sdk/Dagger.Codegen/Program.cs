using System.CommandLine;
using System.Text.Json;

using Dagger.Codegen;

var introspectionJsonFileOption = new Option<FileInfo>("--introspection-json", description: "Introspection Json file path.");

var rootCommand = new RootCommand();
rootCommand.Add(introspectionJsonFileOption);
rootCommand.SetHandler((file) => {
    JsonSerializer.Deserialize<Introspection>(File.ReadAllText(file.FullName));
}, introspectionJsonFileOption);
rootCommand.Invoke(args);
