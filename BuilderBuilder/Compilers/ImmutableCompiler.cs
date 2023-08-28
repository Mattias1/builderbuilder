namespace BuilderBuilder.Compilers;

// This compiler assumes that C#8 nullability is enabled, and that it's fields are not structs
// (if so, one needs to add .Value to the build method parameter).
public class ImmutableCompiler : Compiler {
  private string EntityClass => BuilderEntity.Name;
  private string BuilderClass => EntityClass + "Builder";

  protected override void Compile() {
    OpenTestHelperClass();
    AddExampleBuilder();

    OpenBuilderClass();

    foreach (var field in BuilderEntity.Fields) {
      AddFieldVariable(field);
    }

    foreach (var field in BuilderEntity.Fields) {
      AddFieldSetter(field);
    }

    AddBuildMethod();

    CloseClasses();
  }

  private void OpenTestHelperClass() {
    AddLine("using ...");
    AddEmptyLine();

    AddLine($"namespace {Settings.Namespace};");
    AddEmptyLine();

    AddLine($"public class {EntityClass}TestHelper");
    OpenBlock();
  }

  private void AddExampleBuilder() {
    AddLine($"public static {BuilderClass} Builder()");
    WithBlock(() => {
      AddLine($"return new {BuilderClass}();");
    });
  }

  private void OpenBuilderClass() {
    AddEmptyLines(2);

    AddLine($"public class {BuilderClass}");
    OpenBlock();
  }

  private void AddFieldVariable(Field field) {
    var type = Nullable(field.Type);
    var name = PrivateVar(field.Name);

    AddLine($"private {type} {name};");
  }

  private void AddFieldSetter(Field field) {
    var type = field.Type;
    var publicPropertyName = field.Name;
    var localVarName = LocalVar(field.Name);
    var privatePropertyName = PrivateVar(field.Name);

    AddEmptyLine();

    AddLine($"public {BuilderClass} With{publicPropertyName}({type} {localVarName})");
    WithBlock(() => {
      AddLine($"{privatePropertyName} = {localVarName};");
      AddLine("return this;");
    });
  }

  private void AddBuildMethod() {
    var parameters = string.Join(", ", BuilderEntity.Fields.Select(f => PrivateVar(f.Name)));

    AddEmptyLine();

    AddLine($"public {EntityClass} Build()");
    WithBlock(() => {
      foreach (var field in BuilderEntity.Fields.Where(f => !f.Type.EndsWith("?"))) {
        var privatePropertyName = PrivateVar(field.Name);

        AddLine($"if ({privatePropertyName} is null)");
        WithBlock(() => {
          AddLine($"throw new InvalidOperationException(\"{field.Name} is not nullable\");");
        });
      }

      AddEmptyLine();

      AddLine($"return new {EntityClass}({parameters});");
    });
  }

  private void CloseClasses() => CloseBlocks(2);
}
