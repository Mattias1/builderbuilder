namespace BuilderBuilder.Compilers;

// This compiler assumes that C#8 nullability is enabled, and that it's fields are not structs
// (if so, one needs to add .Value to the build method parameter).
public class ImmutableCompiler : Compiler
{
    private string EntityClass => BuilderEntity.Name;
    private string BuilderClass => EntityClass + "Builder";

    protected override void Compile() {
        openTestHelperClass();
        addExampleBuilder();

        openBuilderClass();

        foreach (var field in BuilderEntity.Fields) {
            addFieldVariable(field);
        }
        foreach (var field in BuilderEntity.Fields) {
            addFieldSetter(field);
        }

        addBuildMethod();

        closeClasses();
    }

    private void openTestHelperClass() {
        AddLine("using ...");
        AddEmptyLine();

        AddLine("namespace ...");
        OpenBlock();

        AddLine($"public class {EntityClass}TestHelper");
        OpenBlock();
    }

    private void addExampleBuilder() {
        AddLine($"public static {BuilderClass} Builder()");
        WithBlock(() => {
            AddLine($"return new {BuilderClass}();");
        });
    }

    private void openBuilderClass() {
        AddEmptyLines(2);

        AddLine($"public class {BuilderClass}");
        OpenBlock();
    }

    private void addFieldVariable(Field field) {
        var type = Nullable(field.Type);
        var _name = PrivateVar(field.Name);

        AddLine($"private {type} {_name};");
    }

    private void addFieldSetter(Field field) {
        var type = field.Type;
        var Name = field.Name;
        var name = LocalVar(field.Name);
        var _name = PrivateVar(field.Name);

        AddEmptyLine();

        AddLine($"public {BuilderClass} With{Name}({type} {name})");
        WithBlock(() => {
            AddLine($"{_name} = {name};");
            AddLine("return this;");
        });
    }

    private void addBuildMethod() {
        var parameters = string.Join(", ", BuilderEntity.Fields.Select(f => PrivateVar(f.Name)));

        AddEmptyLine();

        AddLine($"public {EntityClass} Build()");
        WithBlock(() => {
            foreach (var field in BuilderEntity.Fields.Where(f => !f.Type.EndsWith("?"))) {
                var _name = PrivateVar(field.Name);
                var Name = field.Name;

                AddLine($"if ({_name} is null)");
                WithBlock(() => {
                    AddLine($"throw new InvalidOperationException(\"{Name} is not nullable\");");
                });
            }

            AddEmptyLine();

            AddLine($"return new {EntityClass}({parameters});");
        });
    }

    private void closeClasses() => CloseBlocks(3);
}