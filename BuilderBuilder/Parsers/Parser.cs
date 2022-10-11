using System.Text.RegularExpressions;

namespace BuilderBuilder.Parsers;

public abstract class Parser
{
    public BuilderEntity Parse(string input) => Parse(SplitLines(input));

    public abstract BuilderEntity Parse(string[] lines);

    protected bool StartsWith(string line, string start) {
        return MatchesPattern(line, $@"^\s*{start}");
    }

    protected bool EndsWith(string line, string end) {
        return MatchesPattern(line, $@"{end}\s*$");
    }

    protected bool MatchesPattern(string line, string pattern) {
        return new Regex(pattern).IsMatch(line);
    }

    protected string GetPatternMatch(string line, string pattern, int groupNr = 1) {
        var matches = new Regex(pattern).Matches(line, 0);
        if (matches.Count == 0)
            return null;
        var groups = matches[0].Groups;

        if (groups.Count <= groupNr)
            return null;
        return groups[groupNr].Value;
    }

    protected string[] GetPatternMatches(string line, string pattern, params int[] groupNrs) {
        var matches = new Regex(pattern).Matches(line, 0);
        if (matches.Count == 0)
            return null;
        var groups = matches[0].Groups;

        var result = new string[groupNrs.Length > 0 ? groupNrs.Length : groups.Count - 1];
        var r = 0;
        for (var i = 1; i < groups.Count; i++) {
            if (groupNrs.Length == 0 || groupNrs.Contains(i)) {
                result[r++] = groups[i].Value;
            }
        }
        return result;
    }

    protected string[] SplitLines(string input) {
        return input
            .Replace("\r\n", "\n")
            .Split(new char[] { '\n' });
    }
}