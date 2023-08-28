using System.Text;

namespace BuilderBuilder.Compilers;

public abstract class Compiler {
  protected Settings Settings { get; private set; } = null!;

  private bool UseSpaces => Settings.NrOfSpaces > 0;
  private int TabSize => UseSpaces ? Settings.NrOfSpaces : 1;
  private bool UseBrackets => true;
  private bool UseEgyptianBrackets => Settings.EgyptianBracesIndentStyle;

  protected BuilderEntity BuilderEntity { get; private set; } = null!;
  protected StringBuilder StringBuilder { get; set; } = null!;

  private int _indent;

  public string Compile(BuilderEntity entity, Settings settings) {
    Settings = settings;
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
    StringBuilder.Append(new string(UseSpaces ? ' ' : '\t', _indent * TabSize));
    StringBuilder.AppendLine(line);
  }

  protected void OpenBlock() {
    if (UseBrackets) {
      if (UseEgyptianBrackets && StringBuilder[^1] == '\n') {
        StringBuilder.Length--;
        if (StringBuilder[^1] == '\r') {
          StringBuilder.Length--;
        }
      }

      if (UseEgyptianBrackets) {
        StringBuilder.AppendLine(" {");
      } else {
        AddLine("{");
      }
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
