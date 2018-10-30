namespace BuilderBuilder
{
    public class VipCompiler : Compiler
    {
        private string EntityClass => BuilderEntity.Name;
        private string BuilderClass => EntityClass + "Builder";
        private string EntityPrivateVar => PrivateVar(EntityClass);

        protected override void Compile() {
            openTestHelperClass();
            addExampleBuilder();

            openBuilderClass();
            initBuilderClass();

            foreach (Field field in BuilderEntity.Fields) {
                addField(field);
            }

            addBuildMethods();
            addPersistMethods();

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
            OpenBlock();

            AddLine($"return new {BuilderClass}();");

            CloseBlock();
        }

        private void openBuilderClass() {
            AddEmptyLines(2);

            AddLine($"public class {BuilderClass}");
            OpenBlock();
        }

        private void initBuilderClass() {
            AddLine($"private readonly {EntityClass} {EntityPrivateVar};");
            AddEmptyLine();

            AddLine($"public {BuilderClass}() : this(new {EntityClass}()) {{ }}");
            AddEmptyLine();

            AddLine($"public {BuilderClass}({EntityClass} {LocalVar(EntityClass)})");
            OpenBlock();

            AddLine($"{EntityPrivateVar} = {LocalVar(EntityClass)};");

            CloseBlock();
        }

        private void addField(Field field) {
            string type = field.Type;
            string Name = field.Name;
            string name = LocalVar(field.Name);

            AddEmptyLine();

            AddLine($"public {BuilderClass} With{Name}({type} {name})");
            OpenBlock();

            AddLine($"{EntityPrivateVar}.{Name} = {name};");
            if (field.InverseHandling == Field.InverseHandlingType.OneToOne) {
                AddLine($"{name}.{EntityClass} = {EntityPrivateVar};");
            }
            if (field.InverseHandling == Field.InverseHandlingType.ManyToOne || field.InverseHandling == Field.InverseHandlingType.ManyToMany) {
                AddLine($"{name}.{EntityClass}s.Add({EntityPrivateVar});");
            }
            if (field.InverseHandling == Field.InverseHandlingType.OneToMany) {
                AddLine($"foreach (var obj in {name})");
                OpenBlock();
                AddLine($"obj.{EntityClass} = {EntityPrivateVar};");
                CloseBlock();
            }
            AddLine("return this;");

            CloseBlock();
        }

        private void addBuildMethods() {
            AddEmptyLine();

            AddLine($"public {EntityClass} Build()");
            OpenBlock();

            AddLine($"return {EntityPrivateVar};");

            CloseBlock();

            AddEmptyLine();

            AddLine($"public {EntityClass} AutoBuild()");
            OpenBlock();

            AddLine($"if ({EntityPrivateVar}.Id is null)");
            OpenBlock();

            AddLine("WithId(IdGenerator.Next());");

            CloseBlock();
            AddLine("return Build();");

            CloseBlock();
        }

        private void addPersistMethods() {
            AddEmptyLine();

            AddLine($"public {EntityClass} Persist(DeclaratiegeneratieZorggroepenDbTest context)");
            OpenBlock();

            AddLine("SaveToDatabase(context);");
            AddLine("return Build();");

            CloseBlock();

            AddEmptyLine();

            AddLine("private void SaveToDatabase(DeclaratiegeneratieZorggroepenDbTest context)");
            OpenBlock();

            AddLine($"context.SaveToDatabase({EntityPrivateVar});");

            CloseBlock();
        }

        private void closeClasses() {
            CloseBlocks(3);
        }
    }
}
