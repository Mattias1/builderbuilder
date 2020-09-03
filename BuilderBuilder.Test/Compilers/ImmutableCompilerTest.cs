using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BuilderBuilder.Test
{
    [TestClass]
    public class ImmutableCompilerTest
    {
        private ImmutableCompiler Compiler => new ImmutableCompiler();

        [TestMethod]
        public void Compile_NonPersistExample() {
            var input = new BuilderEntity(persistable: false);
            input.Name = "ExampleEntity";
            input.Fields.Add(new Field("long?", "Id"));
            input.Fields.Add(new Field("string", "Name"));
            input.Fields.Add(new Field("Brother", "Twin"));
            input.Fields.Add(new Field("Parent", "Mom"));
            input.Fields.Add(new Field("List<Child>", "Kids"));
            input.Fields.Add(new Field("List<Parent>", "Parents"));

            string result = Compiler.Compile(input);

            AssertHelper.AssertMultilineStringEq(NonPersistExampleOutput, result);
        }

        private string NonPersistExampleOutput {
            get => @"                using ...

                namespace ...
                {
                    public class ExampleEntityTestHelper
                    {
                        public static ExampleEntityBuilder Builder()
                        {
                            return new ExampleEntityBuilder();
                        }


                        public class ExampleEntityBuilder
                        {
                            private long? _id;
                            private string? _name;
                            private Brother? _twin;
                            private Parent? _mom;
                            private List<Child>? _kids;
                            private List<Parent>? _parents;

                            public ExampleEntityBuilder WithId(long? id)
                            {
                                _id = id;
                                return this;
                            }

                            public ExampleEntityBuilder WithName(string name)
                            {
                                _name = name;
                                return this;
                            }

                            public ExampleEntityBuilder WithTwin(Brother twin)
                            {
                                _twin = twin;
                                return this;
                            }

                            public ExampleEntityBuilder WithMom(Parent mom)
                            {
                                _mom = mom;
                                return this;
                            }

                            public ExampleEntityBuilder WithKids(List<Child> kids)
                            {
                                _kids = kids;
                                return this;
                            }

                            public ExampleEntityBuilder WithParents(List<Parent> parents)
                            {
                                _parents = parents;
                                return this;
                            }

                            public ExampleEntity Build()
                            {
                                if (_name is null)
                                {
                                    throw new InvalidOperationException(""Name is not nullable"");
                                }
                                if (_twin is null)
                                {
                                    throw new InvalidOperationException(""Twin is not nullable"");
                                }
                                if (_mom is null)
                                {
                                    throw new InvalidOperationException(""Mom is not nullable"");
                                }
                                if (_kids is null)
                                {
                                    throw new InvalidOperationException(""Kids is not nullable"");
                                }
                                if (_parents is null)
                                {
                                    throw new InvalidOperationException(""Parents is not nullable"");
                                }

                                return new ExampleEntity(_id, _name, _twin, _mom, _kids, _parents);
                            }
                        }
                    }
                }
            ";
        }
    }
}
