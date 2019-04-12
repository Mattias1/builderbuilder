namespace BuilderBuilder
{
    public class VipCompiler : Compiler
    {
        private string EntityClass => BuilderEntity.Name;
        private string BuilderClass => EntityClass + "Builder";

        protected override void Compile() {
            openTestHelperClass();
            addExampleBuilder();

            openBuilderClass();
            initBuilderClass();

            foreach (Field field in BuilderEntity.Fields) {
                addField(field);
            }

            if (BuilderEntity.Persistable) {
                addAutoBuildMethod();
            }

            closeClasses();
        }

        private void openTestHelperClass() {
            AddLine("using ...");
            AddEmptyLine();

            AddLine("namespace VipLive.WebApplication.VIPLive.Test. ...");
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

            if (BuilderEntity.Persistable) {
                AddLine($"public class {BuilderClass} : AbstractEntityBuilder<{EntityClass}>");
            }
            else {
                AddLine($"public class {BuilderClass} : AbstractBuilder<{EntityClass}>");
            }
            OpenBlock();
        }

        private void initBuilderClass() {
            AddLine($"public {BuilderClass}() : base() {{ }}");
            AddEmptyLine();

            AddLine($"public {BuilderClass}({EntityClass} {LocalVar(EntityClass)}) : base({LocalVar(EntityClass)}) {{ }}");
        }

        private void addField(Field field) {
            string type = field.Type;
            string Name = field.Name;
            string name = LocalVar(field.Name);

            AddEmptyLine();

            AddLine($"public {BuilderClass} With{Name}({type} {name})");
            WithBlock(() => {
                AddLine($"Item.{Name} = {name};");
                if (field.InverseHandling == Field.InverseHandlingType.OneToOne) {
                    AddLine($"{name}.{EntityClass} = Item;");
                }
                if (field.InverseHandling == Field.InverseHandlingType.ManyToOne || field.InverseHandling == Field.InverseHandlingType.ManyToMany) {
                    AddLine($"{name}.{EntityClass}s.Add(Item);");
                }
                if (field.InverseHandling == Field.InverseHandlingType.OneToMany) {
                    AddLine($"foreach (var obj in {name})");
                    WithBlock(() => {
                        AddLine($"obj.{EntityClass} = Item;");
                    });
                }
                AddLine("return this;");
            });
        }

        private void addAutoBuildMethod() {
            AddEmptyLine();

            AddLine($"public override {EntityClass} AutoBuild()");
            WithBlock(() => {
                AddLine($"if (Item.Id is null)");
                WithBlock(() => {
                    AddLine("WithId(IdGenerator.Next());");
                });
                AddLine("return Build();");
            });
        }

        private void closeClasses() {
            CloseBlocks(3);
        }
    }
}
