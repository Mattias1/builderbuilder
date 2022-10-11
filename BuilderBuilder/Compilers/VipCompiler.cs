namespace BuilderBuilder.Compilers;

public class VipCompiler : Compiler
{
    private string EntityClass => BuilderEntity.Name;
    private string BuilderClass => EntityClass + "Builder";

    protected override void Compile()
    {
        OpenTestHelperClass();
        AddExampleBuilder();

        OpenBuilderClass();
        InitBuilderClass();

        foreach (var field in BuilderEntity.Fields)
        {
            AddField(field);
        }

        if (BuilderEntity.Persistable)
        {
            AddAutoBuildMethod();
        }

        CloseClasses();
    }

    private void OpenTestHelperClass()
    {
        AddLine("using ...");
        AddEmptyLine();

        AddLine("namespace VipLive.WebApplication.VIPLive.Test. ...");
        OpenBlock();

        AddLine($"public class {EntityClass}TestHelper");
        OpenBlock();
    }

    private void AddExampleBuilder()
    {
        AddLine($"public static {BuilderClass} Builder()");
        WithBlock(() => { AddLine($"return new {BuilderClass}();"); });
    }

    private void OpenBuilderClass()
    {
        AddEmptyLines(2);

        AddLine(BuilderEntity.Persistable
            ? $"public class {BuilderClass} : AbstractEntityBuilder<{EntityClass}>"
            : $"public class {BuilderClass} : AbstractBuilder<{EntityClass}>");
        OpenBlock();
    }

    private void InitBuilderClass()
    {
        AddLine($"public {BuilderClass}() : base() {{ }}");
        AddEmptyLine();

        AddLine($"public {BuilderClass}({EntityClass} {LocalVar(EntityClass)}) : base({LocalVar(EntityClass)}) {{ }}");
    }

    private void AddField(Field field)
    {
        var type = field.Type;
        var name = field.Name;
        var localVarName = LocalVar(field.Name);

        AddEmptyLine();

        AddLine($"public {BuilderClass} With{name}({type} {localVarName})");
        WithBlock(() =>
        {
            AddLine($"Item.{name} = {localVarName};");
            if (field.InverseHandling == Field.InverseHandlingType.OneToOne)
            {
                AddLine($"{localVarName}.{EntityClass} = Item;");
            }

            if (field.InverseHandling == Field.InverseHandlingType.ManyToOne ||
                field.InverseHandling == Field.InverseHandlingType.ManyToMany)
            {
                AddLine($"{localVarName}.{EntityClass}s.Add(Item);");
            }

            if (field.InverseHandling == Field.InverseHandlingType.OneToMany)
            {
                AddLine($"foreach (var obj in {localVarName})");
                WithBlock(() => { AddLine($"obj.{EntityClass} = Item;"); });
            }

            AddLine("return this;");
        });
    }

    private void AddAutoBuildMethod()
    {
        AddEmptyLine();

        AddLine($"public override {EntityClass} AutoBuild()");
        WithBlock(() =>
        {
            AddLine("if (Item.Id is null)");
            WithBlock(() => { AddLine("WithId(IdGenerator.Next());"); });
            AddLine("return Build();");
        });
    }

    private void CloseClasses()
    {
        CloseBlocks(3);
    }
}