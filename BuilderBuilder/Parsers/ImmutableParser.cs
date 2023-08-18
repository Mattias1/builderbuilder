namespace BuilderBuilder.Parsers;

public class ImmutableParser : CsParser {
  private BuilderEntity _result = null!;
  private List<string[]> _parametersOfConstructors = new();

  public override BuilderEntity Parse(string[] lines) {
    _result = new BuilderEntity(persistable: false);
    _parametersOfConstructors = new List<string[]>();

    for (var i = 0; i < lines.Length; i++) {
      var line = lines[i];

      ParseName(line);
      ParseConstructor(lines, i);
    }

    var parameters = GetLastConstructorWithMostParameters();
    AddFields(parameters);

    return _result;
  }

  private void ParseName(string line) {
    const string classOrStructPattern = @"^\s*(?:public\s+)?(?:readonly\s+)?(?:class|struct)\s+(\w+)";

    if (MatchesPattern(line, classOrStructPattern)) {
      _result.Name = GetPatternMatch(line, classOrStructPattern);
    }
  }

  private void ParseConstructor(string[] lines, int i) {
    var constructorParameters = ParseConstructor(lines, i, _result.Name);
    if (constructorParameters != null) {
      var parameters = constructorParameters.Split(',').Select(p => p.Trim()).ToArray();
      _parametersOfConstructors.Add(parameters);
    }
  }

  private IEnumerable<string> GetLastConstructorWithMostParameters() {
    var mostParameters = _parametersOfConstructors.Max(p => p.Length);
    return _parametersOfConstructors.Last(p => p.Length == mostParameters);
  }

  private void AddFields(IEnumerable<string> constructorParameters) {
    foreach (var bothParameters in constructorParameters) {
      var split = bothParameters.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
      if (split.Length < 2) {
        continue;
      }

      _result.Fields.Add(new Field(split[0], UpperFirst(split[1])));
    }
  }

  private static string UpperFirst(string s) => char.ToUpper(s[0]) + s.Substring(1);
}
