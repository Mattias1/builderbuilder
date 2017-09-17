using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace BuilderBuilder.Test
{
    [TestClass]
    public class PlainCsClassParserTest
    {
        private Parser Parser => new PlainCsClassParser();

        [TestMethod]
        public void Parse_Example() {
            BuilderEntity result = Parser.Parse(ExampleInput);

            AssertHelper.AssertBuilderEntity(result, "ExampleEntity",
                ("long?", "Id"), ("My_class_123", "My_name_123"), ("IEnumerable<Stuff>", "Stuffs"));
        }

        private string ExampleInput {
            get => @"
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
    }
}
