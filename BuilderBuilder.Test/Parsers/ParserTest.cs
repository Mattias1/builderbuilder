using BuilderBuilder.Parsers;
using Xunit;

namespace BuilderBuilder.Test.Parsers;

public class ParserTest : Parser
{
    public override BuilderEntity Parse(string[] lines) {
        throw new NotImplementedException();
    }

    // Start and end
    [Fact]
    public void StartsWithTest() {
        const string line = "    testhahaha";
        const string pattern = "test";
        var result = StartsWith(line, pattern);
        Assert.True(result);
    }

    [Fact]
    public void EndsWithTest() {
        const string line = "hahahatest    ";
        const string pattern = "test";
        var result = EndsWith(line, pattern);
        Assert.True(result);
    }

    // Get pattern match
    [Fact]
    public void GetPatternMatchTest() {
        const string line = "    public class Thingy";
        var result = GetPatternMatch(line, @"public class (\w+)");
        Assert.Equal("Thingy", result);
    }

    [Fact]
    public void GetPatternMatchesTest() {
        const string line = "    public virtual string Thingy";
        var result = GetPatternMatches(line, @"public virtual (\w+) (\w+)");

        Assert.Equal(2, result.Length);
        Assert.Equal("string", result[0]);
        Assert.Equal("Thingy", result[1]);
    }
}