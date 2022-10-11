namespace BuilderBuilder.Parsers;

public class NhibernateParser : CsParser
{
    private BuilderEntity _result = null!;

    public override BuilderEntity Parse(string[] lines) {
        _result = new BuilderEntity(persistable: true);

        for (var i = 0; i < lines.Length; i++) {
            var line = lines[i];

            parseName(lines, i, line);
            parseField(lines, i, line);
        }

        return _result;
    }

    private void parseName(string[] lines, int i, string line) {
        const string classPattern = @"^\s*public\s+class\s+(\w+)";

        if (MatchesPattern(line, classPattern) && LineHasAttribute(lines, i, "Class")) {
            _result.Name = GetPatternMatch(line, classPattern);
        }
    }

    private void parseField(string[] lines, int i, string line) {
        var field = ParsePublicVirtualField(line);
        if (field != null && LineHasParsableAttribute(lines, i)) {
            var inverseType = Field.InverseHandlingType.None;

            if (LineHasOneToOneAttribute(lines, i)) {
                inverseType = Field.InverseHandlingType.OneToOne;
            }
            if (LineHasManyToOneAttribute(lines, i)) {
                inverseType = Field.InverseHandlingType.ManyToOne;
            }
            if (LineHasOneToManyAttribute(lines, i)) {
                inverseType = Field.InverseHandlingType.OneToMany;
            }
            if (LineHasManyToManyAttribute(lines, i)) {
                inverseType = Field.InverseHandlingType.ManyToMany;
            }

            _result.Fields.Add(new Field(field.Value.type, field.Value.name, inverseType));
        }
    }

    private bool LineHasParsableAttribute(string[] lines, int i) {
        return LineHasNormalAttribute(lines, i)
               || LineHasOneToOneAttribute(lines, i)
               || LineHasManyToOneAttribute(lines, i)
               || LineHasOneToManyAttribute(lines, i)
               || LineHasManyToManyAttribute(lines, i);
    }

    private bool LineHasNormalAttribute(string[] lines, int i) {
        return LineHasAttribute(lines, i, "Property") || LineHasAttribute(lines, i, "Id");
    }
    private bool LineHasOneToOneAttribute(string[] lines, int i) {
        return LineHasAttribute(lines, i, "OneToOne");
    }
    private bool LineHasManyToOneAttribute(string[] lines, int i) {
        return LineHasAttribute(lines, i, "ManyToOne");
    }
    private bool LineHasOneToManyAttribute(string[] lines, int i) {
        return LineHasAttribute(lines, i, "OneToMany");
    }
    private bool LineHasManyToManyAttribute(string[] lines, int i) {
        return LineHasAttribute(lines, i, "ManyToMany");
    }
}