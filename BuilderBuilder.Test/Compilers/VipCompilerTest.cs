using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace BuilderBuilder.Test
{
    [TestClass]
    public class VipCompilerTest
    {
        private VipCompiler Compiler => new VipCompiler();

        [TestMethod]
        public void Compile_Example() {
            var input = new BuilderEntity();
            input.Name = "ExampleEntity";
            input.Fields.Add(new Field("long?", "Id"));
            input.Fields.Add(new Field("string", "Name"));
            input.Fields.Add(new Field("Brother", "Twin", Field.InverseHandlingType.OneToOne));
            input.Fields.Add(new Field("Parent", "Mom", Field.InverseHandlingType.ManyToOne));
            input.Fields.Add(new Field("List<Child>", "Kids", Field.InverseHandlingType.OneToMany));
            input.Fields.Add(new Field("List<Parent>", "Parents", Field.InverseHandlingType.ManyToMany));

            string result = Compiler.Compile(input);

            Assert.AreEqual(TrimWhitespace(ExampleOutput), TrimWhitespace(result));
        }

        private string ExampleOutput {
            get => @"
                using ...

                namespace Declaratiegeneratie.WebApplication.VIPLive.Test. ...
                {
                    public class ExampleEntityTestHelper
                    {
                        public static ExampleEntityBuilder Builder()
                        {
                            return new ExampleEntityBuilder();
                        }


                        public class ExampleEntityBuilder
                        {
                            private ExampleEntity _exampleEntity;

                            public ExampleEntityBuilder() : this(new ExampleEntity()) { }

                            public ExampleEntityBuilder(ExampleEntity exampleEntity)
                            {
                                _exampleEntity = exampleEntity;
                            }

                            public ExampleEntityBuilder WithId(long? id)
                            {
                                _exampleEntity.Id = id;
                                return this;
                            }

                            public ExampleEntityBuilder WithName(string name)
                            {
                                _exampleEntity.Name = name;
                                return this;
                            }

                            public ExampleEntityBuilder WithTwin(Brother twin)
                            {
                                _exampleEntity.Twin = twin;
                                twin.ExampleEntity = _exampleEntity;
                                return this;
                            }

                            public ExampleEntityBuilder WithMom(Parent mom)
                            {
                                _exampleEntity.Mom = mom;
                                mom.ExampleEntitys.Add(_exampleEntity);
                                return this;
                            }

                            public ExampleEntityBuilder WithKids(List<Child> kids)
                            {
                                _exampleEntity.Kids = kids;
                                foreach (var obj in kids)
                                {
                                    obj.ExampleEntity = _exampleEntity;
                                }
                                return this;
                            }

                            public ExampleEntityBuilder WithParents(List<Parent> parents)
                            {
                                _exampleEntity.Parents = parents;
                                parents.ExampleEntitys.Add(_exampleEntity);
                                return this;
                            }

                            public ExampleEntity Build()
                            {
                                return _exampleEntity;
                            }

                            public ExampleEntity Persist(DeclaratiegeneratieZorggroepenDbTest context)
                            {
                                SaveToDatabase(context);
                                return Build();
                            }

                            private void SaveToDatabase(DeclaratiegeneratieZorggroepenDbTest context)
                            {
                                context.SaveToDatabase(_exampleEntity);
                            }
                        }
                    }
                }
            ";
        }

        private string TrimWhitespace(string s) {
            return s.Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("\t", "");
        }
    }
}
