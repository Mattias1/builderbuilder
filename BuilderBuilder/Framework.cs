using BuilderBuilder.Compilers;
using BuilderBuilder.Parsers;

namespace BuilderBuilder;

internal static class Frameworks
{
    private static Framework[]? _frameworks;

    public static Framework[] All => _frameworks ??= new Framework[]
    {
        new("C# NHibernate", "CsNhibernate", new NhibernateParser(), new VipCompiler()),
        new("Plain C# class", "PlainCsClass", new PlainCsClassParser(), new VipCompiler()),
        new("C# immutable", "Immutable", new ImmutableParser(), new ImmutableCompiler())
    };

    public static Framework FromSlug(string slug)
    {
        return All.FirstOrDefault(f => f.Slug == slug) ?? All.First();
    }

    public static int IndexOf(Framework framework)
    {
        for (var i = 0; i < All.Length; i++)
        {
            if (All[i] == framework)
            {
                return i;
            }
        }

        return -1;
    }
}

internal class Framework
{
    public string Name { get; }
    public string Slug { get; }

    public Parser Parser { get; }
    public Compiler Compiler { get; }

    public Framework(string name, string slug, Parser parser, Compiler compiler)
    {
        Name = name;
        Slug = slug;
        Parser = parser;
        Compiler = compiler;
    }
}