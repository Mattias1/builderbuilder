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

            AssertHelper.AssertBuilderEntity(result, "ExampleEntity", true,
                ("long?", "Id", Field.InverseHandlingType.None),
                ("string", "Name", Field.InverseHandlingType.None),
                ("My_class_123", "My_name_123", Field.InverseHandlingType.None),
                ("Brother", "Twin", Field.InverseHandlingType.OneToOne),
                ("Parent", "Mom", Field.InverseHandlingType.ManyToOne),
                ("IEnumerable<Child>", "Kids", Field.InverseHandlingType.OneToMany),
                ("IEnumerable<Parent>", "Parents", Field.InverseHandlingType.ManyToMany)
            );
        }

        private string ExampleInput {
            get => @"
                using ...

                namespace ...
                {
                    [Class]
                    public class ExampleEntity
                    {
                        public const string TABLENAME = ""ExampleEntityTable"";

                        [Id]
                        public virtual long? Id { get; set; }

                        [Property]
                        public virtual string Name { get; set; }

                        [Property(Name = ""Name_123"")]
                        public virtual My_class_123 My_name_123 { get; set; }

                        [OneToOne]
                        public virtual Brother Twin { get; set; }

                        [ManyToOne]
                        public virtual Parent Mom { get; set; }

                        [OneToMany]
                        public virtual IEnumerable<Child> Kids { get; set; }

                        [ManyToMany]
                        public virtual IEnumerable<Parent> Parents { get; set; }

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
