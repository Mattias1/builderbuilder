namespace BuilderBuilder.Parsers;

public class PlainCsClassParser : CsParser
{
    private BuilderEntity _result = null!;

    public override BuilderEntity Parse(string[] lines)
    {
        _result = new BuilderEntity(persistable: false);

        foreach (var line in lines)
        {
            ParseName(line);
            ParseField(line);
        }

        return _result;
    }

    private void ParseName(string line)
    {
        const string classPattern = @"^\s*(?:public\s+)?class\s+(\w+)";

        if (MatchesPattern(line, classPattern))
        {
            _result.Name = GetPatternMatch(line, classPattern);
        }
    }

    private void ParseField(string line)
    {
        var field = ParsePublicField(line);
        if (field != null)
        {
            _result.Fields.Add(new Field(field.Value.type, field.Value.name));
        }
    }
}