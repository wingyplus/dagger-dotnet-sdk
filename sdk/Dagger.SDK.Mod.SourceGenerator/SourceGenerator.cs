using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Dagger.SDK.Mod.SourceGenerator;

/// <summary>
/// Generate source code for running Dagger Function.
/// </summary>
[Generator(LanguageNames.CSharp)]
public class SourceGenerator : IIncrementalGenerator
{
    private const string EntrypointAttribute = "Dagger.SDK.Mod.EntrypointAttribute";
    private const string ObjectAttribute = "Dagger.SDK.Mod.ObjectAttribute";
    private const string FunctionAttribute = "Dagger.SDK.Mod.FunctionAttribute";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(postInitializationContext =>
        {
            postInitializationContext.AddSource("Dagger.SDK.Mod_Attributes.g.cs",
                GenerateSources.ModuleAttributesSource());
            postInitializationContext.AddSource("Dagger.SDK.Mod_Interfaces.g.cs",
                GenerateSources.ModuleInterfacesSource());
        });

        var entrypoint = context.SyntaxProvider.ForAttributeWithMetadataName(
            fullyQualifiedMetadataName: EntrypointAttribute,
            predicate: IsPartialClass,
            transform: ToObjectContext
        );

        context.RegisterSourceOutput(entrypoint, GenerateEntrypoint);

        var objects = context.SyntaxProvider.ForAttributeWithMetadataName(
            fullyQualifiedMetadataName: ObjectAttribute,
            predicate: IsPartialClass,
            transform: ToObjectContext
        );

        context.RegisterSourceOutput(objects, GenerateIDagSetter);

        var functions = context.SyntaxProvider.ForAttributeWithMetadataName(
            fullyQualifiedMetadataName: FunctionAttribute,
            predicate: IsPublicMethod,
            transform: ToFunctionContext
        );

        var objectWithFunctions = objects.Combine(functions.Collect()).Select(GroupIntoObjectContext);

        context.RegisterSourceOutput(objectWithFunctions, GenerateObjectTypeDef);
    }

    private T ToEntrypointContext<T>(GeneratorAttributeSyntaxContext arg1, CancellationToken arg2)
    {
        throw new NotImplementedException();
    }


    private ObjectContext GroupIntoObjectContext((ObjectContext Left, ImmutableArray<FunctionContext> Right) tuple,
        CancellationToken token)
    {
        var (objectContext, functionContexts) = tuple;

        objectContext.Functions = functionContexts.Where(context =>
        {
            var classDecl = (ClassDeclarationSyntax)context.Syntax.Parent!;
            return objectContext.Syntax.Equals(classDecl);
        });

        return objectContext;
    }

    private static FunctionContext ToFunctionContext(GeneratorAttributeSyntaxContext context, CancellationToken token)
    {
        return new FunctionContext
        {
            Syntax = (MethodDeclarationSyntax)context.TargetNode, Symbol = (IMethodSymbol)context.TargetSymbol
        };
    }

    private static bool IsPublicMethod(SyntaxNode node, CancellationToken token)
    {
        return node is MethodDeclarationSyntax methodDecl &&
               methodDecl.Modifiers.Any(SyntaxKind.PublicKeyword);
    }

    private static ObjectContext ToObjectContext(
        GeneratorAttributeSyntaxContext context,
        CancellationToken token)
    {
        var classDecl = (ClassDeclarationSyntax)context.TargetNode;
        var symbol = (INamedTypeSymbol)context.TargetSymbol;

        return new ObjectContext { Syntax = classDecl, Symbol = symbol };
    }

    private static bool IsPartialClass(SyntaxNode node, CancellationToken token)
    {
        return node is ClassDeclarationSyntax classDecl &&
               classDecl.Modifiers.Any(SyntaxKind.PublicKeyword) &&
               classDecl.Modifiers.Any(SyntaxKind.PartialKeyword);
    }

    private static void GenerateObjectTypeDef(SourceProductionContext context,
        ObjectContext objectContext)
    {
        var withFunctionDefs = objectContext
            .Functions
            .Select(static m => m.Name)
            .Select(static methodName => $"""
                                          .WithFunction(
                                              dag.Function("{methodName}", dag.TypeDef().WithKind(Dagger.SDK.TypeDefKind.STRING_KIND))
                                          )
                                          """);

        var source = $$"""
                       namespace {{objectContext.Namespace}};

                       public partial class {{objectContext.Name}}
                       {
                           public Dagger.SDK.TypeDef ToObjectTypeDef(Dagger.SDK.Query dag)
                           {
                               var objTypeDef = dag.TypeDef().WithObject("{{objectContext.Name}}");
                               return objTypeDef
                               {{string.Join("\n", withFunctionDefs)}};
                           }
                       }
                       """;
        context.AddSource($"{objectContext.Name}_ObjectTypeDef.g.cs",
            SourceText.From(CSharpSource.Format(source), Encoding.UTF8));
    }

    private static void GenerateIDagSetter(SourceProductionContext context, ObjectContext objectContext)
    {
        var source = $$"""
                       namespace {{objectContext.Namespace}};

                       public partial class {{objectContext.Name}} : Dagger.SDK.Mod.IDagSetter
                       {
                           private Dagger.SDK.Query _dag;

                           public void SetDag(Dagger.SDK.Query dag)
                           {
                               _dag = dag;
                           }
                       }
                       """;

        context.AddSource($"{objectContext.Name}_IDagSetter.g.cs", SourceText.From(CSharpSource.Format(source), Encoding.UTF8));
    }

    private static void GenerateEntrypoint(SourceProductionContext context, ObjectContext objectContext)
    {
        var entrypoint = $$"""
                           namespace {{objectContext.Namespace}};

                           public partial class {{objectContext.Name}} : Dagger.SDK.Mod.IEntrypoint
                           {
                               public Dagger.SDK.Module Register(Dagger.SDK.Query dag, Dagger.SDK.Module module)
                               {
                                   return module.WithObject(ToObjectTypeDef(dag));
                               }
                           }
                           """;
        context.AddSource($"{objectContext.Name}_IEntrypoint.g.cs", SourceText.From(CSharpSource.Format(entrypoint), Encoding.UTF8));

        var main = $$"""
                     namespace {{objectContext.Namespace}};

                     public static class Entrypoint
                     {
                         public static async Task Invoke(string[] args)
                         {
                             var dag = Dagger.SDK.Dagger.Connect();
                             await Invoke<{{objectContext.Name}}>(dag);
                         }

                         private static async Task Invoke<T>(Dagger.SDK.Query dag) where T : class, Dagger.SDK.Mod.IDagSetter, Dagger.SDK.Mod.IEntrypoint, new()
                         {
                             T root = new();
                             var fnCall = dag.CurrentFunctionCall();
                             var parentName = await fnCall.ParentName();
                             // TODO: Get module name to check root type name match with it.

                             var result = parentName switch
                             {
                                 // TODO: Dagger.SDK should automatic serialize into id.
                                 "" => await root.Register(dag, dag.Module()).Id(),
                                 _ => throw new Exception($"{parentName} is not supported at the moment.")
                             };

                             await fnCall.ReturnValue(IntoJson(result));
                         }

                         private static Dagger.SDK.Json IntoJson(object result)
                         {
                             return new Dagger.SDK.Json { Value = System.Text.Json.JsonSerializer.Serialize(result) };
                         }
                     }
                     """;

        context.AddSource("Entrypoint.g.cs", SourceText.From(CSharpSource.Format(main), Encoding.UTF8));
    }
}
