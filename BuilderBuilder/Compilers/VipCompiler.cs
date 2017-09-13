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

            addBuildMethod();
            addPersistMethods();

            closeClasses();
        }

        private void openTestHelperClass() {
            AddLine("using ...");
            AddEmptyLine();

            AddLine("namespace Declaratiegeneratie.WebApplication.VIPLive.Test. ...");
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
            AddLine($"private {EntityClass} {EntityPrivateVar};");
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
            AddLine("return this;");

            CloseBlock();
        }

        private void addBuildMethod() {
            AddEmptyLine();

            AddLine($"public {EntityClass} Build()");
            OpenBlock();

            AddLine($"return {EntityPrivateVar};");

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
