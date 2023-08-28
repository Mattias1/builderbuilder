using BuilderBuilder.Parsers;
using Xunit;

namespace BuilderBuilder.Test.Parsers;

public class ImmutableParserTest {
  private Parser Parser => new ImmutableParser();

  [Fact]
  public void Parse_Example() {
    var result = Parser.Parse(ExampleInput);

    AssertHelper.AssertBuilderEntity(result, "ExampleStruct", false,
        ("long?", "Id"),
        ("My_class_123", "My_name_123"),
        ("IReadOnlyList<Stuff>", "Stuffs"));
  }

  private string ExampleInput =>
      @"
                using ...

                namespace ... {
                    public sealed readonly struct ExampleStruct {
                        public readonly long? Id;
                        public My_class_123 My_name_123 { get; }
                        public IReadOnlyList<Stuff> Stuffs { get; }
                        public SomeIgnoredThing? BecauseItsNotInTheConstructor { get; private set; }

                        public ExampleStruct(long? id, My_class_123 my_name_123) : this(id, my_name_123, new List<Stuff>().AsReadOnly()) { }

                        public ExampleStruct(long? id, My_class_123 my_name_123, IReadOnlyList<Stuff> stuffs) {
                            Id = id;
                            My_name_123 = my_name_123;
                            Stuffs = stuffs;
                        }

                        public int IgnoreMe() {
                            return 42;
                        }
                    }
                }
            ";
}
