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
