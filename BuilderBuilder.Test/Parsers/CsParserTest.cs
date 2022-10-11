using BuilderBuilder.Parsers;
using Xunit;

namespace BuilderBuilder.Test.Parsers;

public class CsParserTest : CsParser
{
    public override BuilderEntity Parse(string[] lines) {
        throw new NotImplementedException();
    }

    // Is attribute line
    [Fact]
    public void IsAttributeLine_Simple() {
        var line = "[attribute]";
        var result = IsAttributeLine(line);
        Assert.True(result);
    }

    [Fact]
    public void IsAttributeLine_Advanced() {
        var line = "  [ question(answer = 42), test ]  ";
        var result = IsAttributeLine(line);
        Assert.True(result);
    }

    [Fact]
    public void IsAttributeLine_MissingStart_False() {
        var line = "  test]  ";
        var result = IsAttributeLine(line);
        Assert.False(result);
    }

    [Fact]
    public void IsAttributeLine_MissingEnd_False() {
        var line = "  [test  ";
        var result = IsAttributeLine(line);
        Assert.False(result);
    }

    [Fact]
    public void IsAttributeLine_Comment() {
        var line = "  [ question(answer = 42), test ] // Comment";
        var result = IsAttributeLine(line);
        Assert.True(result);
    }

    // Is attribute
    [Fact]
    public void IsAttribute_Simple() {
        var line = "  [attribute]  ";
        var pattern = "attribute";
        var result = IsAttribute(line, pattern);
        Assert.True(result);
    }

    [Fact]
    public void IsAttribute_First() {
        var line = "  [ question(answer = 42) , test ]  ";
        var pattern = "question";
        var result = IsAttribute(line, pattern);
        Assert.True(result);
    }

    [Fact]
    public void IsAttribute_Second() {
        var line = "  [ question(answer = 42) , test ]  ";
        var pattern = "test";
        var result = IsAttribute(line, pattern);
        Assert.True(result);
    }

    [Fact]
    public void IsAttribute_None() {
        var line = "  [ question(answer = 42), test ]  ";
        var pattern = "none";
        var result = IsAttribute(line, pattern);
        Assert.False(result);
    }

    [Fact]
    public void IsAttribute_Comment() {
        var line = "  [ question(answer = 42), test ] // Comment";
        var pattern = "question";
        var result = IsAttribute(line, pattern);
        Assert.True(result);
    }

    // Line has attribute
    [Fact]
    public void LineHasAttribute_True() {
        var lines = new[] {
            "",
            "    [test]",
            "    [additional(info = 37)]",
            "    public SoundEffect Boom(int radius) {"
        };
        var result = LineHasAttribute(lines, 3, "test");
        Assert.True(result);
    }

    [Fact]
    public void LineHasAttribute_False() {
        var lines = new[] {
            "",
            "    [test]",
            "    [additional(info = 37)]",
            "    public SoundEffect Boom(int radius) {"
        };
        var result = LineHasAttribute(lines, 3, "none");
        Assert.False(result);
    }

    // Is public field
    [Fact]
    public void ParsePublicField_IfVariable_ThenNotNull() {
        var line = "public override int MyVariable";
        var result = ParsePublicField(line);
        Assert.NotNull(result);
    }

    [Fact]
    public void ParsePublicField_IfProperty_ThenNotNull() {
        var line = "public override int MyProperty { get; set; }";
        var result = ParsePublicField(line);
        Assert.NotNull(result);
    }

    [Fact]
    public void ParsePublicField_IfFunction_ThenNull() {
        var line = "public int MyFunction()";
        var result = ParsePublicField(line);
        Assert.Null(result);
    }

    [Fact]
    public void ParsePublicField_IfClass_ThenNull() {
        var line = "public class MyClass";
        var result = ParsePublicField(line);
        Assert.Null(result);
    }

    [Fact]
    public void ParsePublicVirtualField_IfNotVirtual_ThenNull() {
        var line = "public int MyProperty { get; set; }";
        var result = ParsePublicVirtualField(line);
        Assert.Null(result);
    }

    [Fact]
    public void ParsePublicVirtualField_IfVirtual_ThenNotNull() {
        var line = "public virtual int MyProperty { get; set; }";
        var result = ParsePublicVirtualField(line);
        Assert.NotNull(result);
    }

    // Is constructor
    [Fact]
    public void ParseConstructor_IfParameterlessConstructor_ThenEmptyString() {
        var line = "public MyClass()";
        var result = ParseConstructor(new[] { line }, 0, "MyClass");
        Assert.Equal("", result);
    }

    [Fact]
    public void ParseConstructor_IfConstructor_ThenParameters() {
        var line = "public MyClass(long? id, IEnumerable<Stuff> stuffs) {";
        var result = ParseConstructor(new[] { line }, 0, "MyClass");
        Assert.Equal("long? id, IEnumerable<Stuff> stuffs", result);
    }

    [Fact]
    public void ParseConstructor_IfConstructorWithBase_ThenParameters() {
        var line = "public MyClass(int one, int two) : base(one, two, 3) {";
        var result = ParseConstructor(new[] { line }, 0, "MyClass");
        Assert.Equal("int one, int two", result);
    }

    [Fact]
    public void ParseConstructor_IfConstructorWithParamsOnMultipleLines_ThenParameters() {
        var lines = new[] {
            "    public MyClass(",
            "        int one,",
            "        int two) ",
            "    {"
        };
        var result = ParseConstructor(lines, 0, "MyClass");
        AssertHelper.AreEqualModuloWhitespace("int one, int two", result);
    }

    [Fact]
    public void ParseConstructor_IfFunction_ThenNull() {
        var line = "public int MyFunction()";
        var result = ParseConstructor(new[] { line }, 0, "MyFunction");
        Assert.Null(result);
    }
}