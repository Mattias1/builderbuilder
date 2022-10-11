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
        var line = "    testhahaha";
        var pattern = "test";
        var result = StartsWith(line, pattern);
        Assert.True(result);
    }

    [Fact]
    public void EndsWithTest() {
        var line = "hahahatest    ";
        var pattern = "test";
        var result = EndsWith(line, pattern);
        Assert.True(result);
    }

    // Get pattern match
    [Fact]
    public void GetPatternMatchTest() {
        var line = "    public class Thingy";
        var result = GetPatternMatch(line, @"public class (\w+)");
        Assert.Equal("Thingy", result);
    }

    [Fact]
    public void GetPatternMatchesTest() {
        var line = "    public virtual string Thingy";
        var result = GetPatternMatches(line, @"public virtual (\w+) (\w+)");

        Assert.Equal(2, result.Length);
        Assert.Equal("string", result[0]);
        Assert.Equal("Thingy", result[1]);
    }
}