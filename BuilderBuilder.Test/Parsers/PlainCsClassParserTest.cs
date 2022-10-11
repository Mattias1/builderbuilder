using BuilderBuilder.Parsers;
using Xunit;

namespace BuilderBuilder.Test.Parsers;

public class PlainCsClassParserTest
{
    private Parser Parser => new PlainCsClassParser();

    [Fact]
    public void Parse_Example() {
        var result = Parser.Parse(ExampleInput);

        AssertHelper.AssertBuilderEntity(result, "ExampleEntity", false,
            ("long?", "Id"),
            ("My_class_123", "My_name_123"),
            ("IEnumerable<Stuff>", "Stuffs"));
    }

    private string ExampleInput =>
        @"
                using ...

                namespace ...
                {
                    public class ExampleEntity
                    {
                        public long? Id;

                        [JsonProperty(""Name_123"")]
                        public My_class_123 My_name_123 { get; set; }

                        public virtual IEnumerable<Stuff> Stuffs { get; set; }

                        public virtual int IgnoreMe()
                        {
                            return 42;
                        }
                    }
                }
            ";
}