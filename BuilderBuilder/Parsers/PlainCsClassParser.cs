namespace BuilderBuilder.Parsers;

public class PlainCsClassParser : CsParser {
  private BuilderEntity _result = null!;

  public override BuilderEntity Parse(string[] lines) {
    _result = new BuilderEntity(persistable: false);

    foreach (var line in lines) {
      ParseName(line);
      ParseField(line);
    }

    return _result;
  }

  private void ParseName(string line) {
    var name = ParseClassOrStructName(line);
    if (name is not null) {
      _result.Name = name;
    }
  }

  private void ParseField(string line) {
    var field = ParsePublicField(line);
    if (field is not null) {
      _result.Fields.Add(new Field(field.Value.type, field.Value.name));
    }
  }
}
