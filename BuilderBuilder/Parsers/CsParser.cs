namespace BuilderBuilder.Parsers;

public abstract class CsParser : Parser {
  private const string AttributeInside = @"\s*[a-zA-Z_].*";
  private const string Type = @"[a-zA-Z0-9_<>?]";

  protected bool IsAttributeLine(string line) {
    return MatchesPattern(line, $@"^\s*\[{AttributeInside}\]");
  }

  protected bool IsAttribute(string line, string attribute) {
    return MatchesPattern(line,
        $@"^\s*\[({AttributeInside},\s*)*\s*{attribute}\s*(\(.*\))?\s*(\s*,{AttributeInside})*\]");
  }

  protected bool LineHasAttribute(string[] lines, int index, string attribute) {
    for (var i = index - 1; i >= 0; i--) {
      var line = lines[i];
      if (!IsAttributeLine(line)) {
        return false;
      }

      if (IsAttribute(line, attribute)) {
        return true;
      }
    }
    return false;
  }

  protected string? ParseClassOrStructName(string line) {
    const string classOrStructPattern = @"^\s*(?:public\s+)?(?:sealed\s+)?(?:readonly\s+)?(?:class|struct)\s+(\w+)";

    if (MatchesPattern(line, classOrStructPattern)) {
      return GetPatternMatch(line, classOrStructPattern);
    }
    return null;
  }

  protected (string type, string name)? ParsePublicField(string line) {
    return ParsePublicField(line, false);
  }

  protected (string type, string name)? ParsePublicVirtualField(string line) {
    return ParsePublicField(line, true);
  }

  private (string type, string name)? ParsePublicField(string line, bool forceVirtual) {
    var fieldPattern = @"public\s+" +
        (forceVirtual ? @"virtual\s+" : @"(?:virtual\s+|override\s+)?") +
        $@"({Type}+)\s+(\w+)\s*" +
        @"(\()?" +
        @"(?:\{\s*get;\s*set;\s*\})?";

    if (!MatchesPattern(line, fieldPattern)) {
      return null;
    }

    var values = GetPatternMatches(line, fieldPattern);
    if (values[0] == "class" || (values.Length >= 3 && values[2] == "(")) {
      return null;
    }
    return (values[0], values[1]);
  }

  protected string? ParseConstructor(string[] lines, int index, string name) {
    var startPattern = $@"public\s+{name}\s*\(";
    var firstParamsPattern = $@"public\s+{name}\s*\(([^)]*)";
    const string paramsPattern = @"([^)]*)";
    const string endPattern = @"\)";

    if (!MatchesPattern(lines[index], startPattern)) {
      return null;
    }

    var result = MatchesPattern(lines[index], firstParamsPattern)
        ? GetPatternMatch(lines[index], firstParamsPattern)
        : "";
    if (MatchesPattern(lines[index], endPattern)) {
      return result;
    }

    for (var i = index + 1; i < lines.Length; i++) {
      var line = lines[i];
      result += MatchesPattern(line, paramsPattern) ? GetPatternMatch(line, paramsPattern) : "";
      if (MatchesPattern(line, endPattern)) {
        return result;
      }
    }
    return null;
  }
}
