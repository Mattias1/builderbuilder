using System.Text;

namespace BuilderBuilder.Compilers;

public abstract class Compiler
{
    private static int TabSize => 4;

    protected bool UseBrackets => true;

    protected BuilderEntity BuilderEntity { get; private set; } = null!;

    protected StringBuilder StringBuilder { get; set; } = null!;

    private int _indent;

    public string Compile(BuilderEntity entity) {
        BuilderEntity = entity;
        StringBuilder = new StringBuilder();

        Compile();

        return StringBuilder.ToString();
    }

    protected abstract void Compile();

    protected void AddEmptyLine() => AddEmptyLines(1);
    protected void AddEmptyLines(int numberOfLines) {
        for (var i = 0; i < numberOfLines; i++) {
            StringBuilder.AppendLine();
        }
    }

    protected void AddLine(string line) {
        StringBuilder.Append(new string(' ', _indent * TabSize));
        StringBuilder.AppendLine(line);
    }

    protected void OpenBlock() {
        if (UseBrackets) {
            AddLine("{");
        }
        _indent++;
    }

    protected void CloseBlock() => CloseBlocks(1);
    protected void CloseBlocks(int numberOfBlocks) {
        for (var i = 0; i < numberOfBlocks; i++) {
            _indent--;

            if (UseBrackets) {
                AddLine("}");
            }
        }
    }

    protected void WithBlock(Action addBlockContent) {
        OpenBlock();
        addBlockContent();
        CloseBlock();
    }

    protected static string LocalVar(string name) {
        if (string.IsNullOrEmpty(name)) {
            return "";
        }
        return char.ToLowerInvariant(name[0]) + name.Substring(1);
    }

    protected static string PrivateVar(string name) => '_' + LocalVar(name);

    protected static string Nullable(string type) => type.EndsWith("?") ? type : $"{type}?";
}