using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace BuilderBuilder.Test
{
    [TestClass]
    public class NhibernateParserTest
    {
        private Parser Parser => new NhibernateParser();

        [TestMethod]
        public void Parse_Example() {
            BuilderEntity result = Parser.Parse(ExampleInput);

            AssertBuilderEntity(result, "ExampleEntity", ("long?", "Id"), ("string", "Name"), ("My_class_123", "My_name_123"));
        }

        private string ExampleInput {
            get => @"
                using ...

                namespace ...
                {
                    [Class]
                    public class ExampleEntity
                    {
                        [Id]
                        public virtual long? Id { get; set; }

                        [Property]
                        public virtual string Name { get; set; }

                        [Property]
                        public virtual My_class_123 My_name_123 { get; set; }

                        [ManyToMany]
                        public virtual IEnumerable<Stuff> Stuffs { get; set; }

                        public virtual int IgnoreMe()
                        {
                            return 42;
                        }
                    }
                }
            ";
        }

        private void AssertBuilderEntity(BuilderEntity entityResult, string name, params (string type, string name)[] fields) {
            Assert.AreEqual(name, entityResult.Name);
            Assert.AreEqual(fields.Length, entityResult.Fields.Count);

            var expectedFields = fields.Select(t => new Field(t.type, t.name)).OrderBy(f => f.Name).ToArray();
            var resultFields = entityResult.Fields.OrderBy(f => f.Name).ToArray();

            foreach ((Field expected, Field result) tuple in expectedFields.Zip(resultFields, (e, f) => (e, f))) {
                Assert.AreEqual(tuple.expected.Name, tuple.result.Name);
                Assert.AreEqual(tuple.expected.Type, tuple.result.Type);
            }
        }
    }
}
