

using System;
using System.Linq;
using System.Text;
using System.Text.Json;

using Dagger.SDK.SourceGenerator.Code;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Dagger.SDK.SourceGenerator.Types;
namespace Dagger.SDK.SourceGenerator;

[Generator]
public class SourceGenerator(CodeGenerator codeGenerator) : ISourceGenerator
{
    private static readonly Diagnostic FailedToReadSchemaFile = Diagnostic.Create(
        new DiagnosticDescriptor(
            id: "DAG002",
            title: "Failed to read introspection.json file",
            messageFormat: "Failed to read introspection.json file. The source generator will not generate any code.",
            category: "Dagger.SDK.SourceGenerator",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true),
        location: null);

    private static readonly Diagnostic NoSchemaFileFound = Diagnostic.Create(
        new DiagnosticDescriptor(
            id: "DAG001",
            title: "No introspection.json file found",
            messageFormat: "No introspection.json file was found in the additional files. The source generator will not generate any code.",
            category: "Dagger.SDK.SourceGenerator",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true),

        location: null);
    
    private static readonly Diagnostic FailedToGenerateCode = Diagnostic.Create(
        new DiagnosticDescriptor(
            id: "DAG003",
            title: "Failed to generate SDK code",
            messageFormat: "Failed to generate code. The source generator will not generate any code.",
            category: "Dagger.SDK.SourceGenerator",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true),
        location: null);
    
    public SourceGenerator() : this(new CodeGenerator(new CodeRenderer()))
    {
        
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        
    }

    public void Execute(GeneratorExecutionContext context)
    {
        var schemaFile = context.AdditionalFiles.FirstOrDefault(x => x.Path.EndsWith("introspection.json"));

        if (schemaFile is null)
        {
            context.ReportDiagnostic(NoSchemaFileFound);
            return;
        }
        
        var sourceText = schemaFile.GetText();
        
        if(sourceText is null)
        {
            context.ReportDiagnostic(FailedToReadSchemaFile);
            return;
        }

        try
        {
            Introspection introspection = JsonDocument.Parse(sourceText.ToString()).RootElement!.GetProperty("data").Deserialize<Introspection>()!;
            string code = codeGenerator.Generate(introspection);
            context.AddSource("Dagger.SDK.g.cs", SourceText.From(code, Encoding.UTF8));    
        }
        catch (Exception)
        {
            context.ReportDiagnostic(FailedToGenerateCode);
        }
    }
}
