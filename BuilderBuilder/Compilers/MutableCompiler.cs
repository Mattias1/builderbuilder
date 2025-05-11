namespace BuilderBuilder.Compilers;

public class MutableCompiler : Compiler {
  private string EntityClass => BuilderEntity.Name;
  private string BuilderClass => EntityClass + "Builder";

  protected override void Compile() {
    OpenTestHelperClass();
    AddExampleBuilder();

    OpenBuilderClass();
    InitBuilderClass();

    foreach (var field in BuilderEntity.Fields) {
      AddField(field);
    }

    if (BuilderEntity.Persistable) {
      AddAutoBuildMethod();
    }

    CloseClasses();
  }

  private void OpenTestHelperClass() {
    if (Settings.IncludeUsingDots) {
      AddLine("using ...");
      AddEmptyLine();
    }

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

    AddLine(BuilderEntity.Persistable
        ? $"public class {BuilderClass} : AbstractEntityBuilder<{EntityClass}>"
        : $"public class {BuilderClass} : AbstractBuilder<{EntityClass}>");
    OpenBlock();
  }

  private void InitBuilderClass() {
    AddLine($"public {BuilderClass}() : base() {{ }}");
    AddEmptyLine();

    AddLine($"public {BuilderClass}({EntityClass} {LocalVar(EntityClass)}) : base({LocalVar(EntityClass)}) {{ }}");
  }

  private void AddField(Field field) {
    var type = field.Type;
    var name = field.Name;
    var localVarName = LocalVar(field.Name);
    var item = AbstractVar("Item");

    AddEmptyLine();

    AddLine($"public {BuilderClass} With{name}({type} {localVarName})");
    WithBlock(() => {
      AddLine($"{item}.{name} = {localVarName};");
      switch (field.InverseHandling) {
        case Field.InverseHandlingType.OneToOne:
          AddLine($"{localVarName}.{EntityClass} = {item};");
          break;
        case Field.InverseHandlingType.ManyToOne:
        case Field.InverseHandlingType.ManyToMany:
          AddLine($"{localVarName}.{EntityClass}s.Add({item});");
          break;
        case Field.InverseHandlingType.OneToMany:
          AddLine($"foreach (var obj in {localVarName})");
          WithBlock(() => {
            AddLine($"obj.{EntityClass} = {item};");
          });
          break;
      }

      AddLine("return this;");
    });
  }

  private void AddAutoBuildMethod() {
    var item = AbstractVar("Item");

    AddEmptyLine();

    AddLine($"public override {EntityClass} AutoBuild()");
    WithBlock(() => {
      AddLine($"if ({item}.Id is null)");
      WithBlock(() => {
        AddLine("WithId(IdGenerator.Next());");
      });
      AddLine("return Build();");
    });
  }

  private void CloseClasses() => CloseBlocks(2);
}
