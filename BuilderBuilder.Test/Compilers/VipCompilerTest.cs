using BuilderBuilder.Compilers;
using Xunit;

namespace BuilderBuilder.Test.Compilers;

public class VipCompilerTest
{
    private VipCompiler Compiler => new();

    [Fact]
    public void Compile_PersistExample() {
        var input = new BuilderEntity(persistable: true)
        {
            Name = "ExampleEntity"
        };
        input.Fields.Add(new Field("long?", "Id"));
        input.Fields.Add(new Field("string", "Name"));
        input.Fields.Add(new Field("Brother", "Twin", Field.InverseHandlingType.OneToOne));
        input.Fields.Add(new Field("Parent", "Mom", Field.InverseHandlingType.ManyToOne));
        input.Fields.Add(new Field("List<Child>", "Kids", Field.InverseHandlingType.OneToMany));
        input.Fields.Add(new Field("List<Parent>", "Parents", Field.InverseHandlingType.ManyToMany));

        var result = Compiler.Compile(input);

        AssertHelper.AssertMultilineStringEq(PersistExampleOutput, result);
    }

    private string PersistExampleOutput =>
        @"                using ...

                namespace VipLive.WebApplication.VIPLive.Test. ...
                {
                    public class ExampleEntityTestHelper
                    {
                        public static ExampleEntityBuilder Builder()
                        {
                            return new ExampleEntityBuilder();
                        }


                        public class ExampleEntityBuilder : AbstractEntityBuilder<ExampleEntity>
                        {
                            public ExampleEntityBuilder() : base() { }

                            public ExampleEntityBuilder(ExampleEntity exampleEntity) : base(exampleEntity) { }

                            public ExampleEntityBuilder WithId(long? id)
                            {
                                Item.Id = id;
                                return this;
                            }

                            public ExampleEntityBuilder WithName(string name)
                            {
                                Item.Name = name;
                                return this;
                            }

                            public ExampleEntityBuilder WithTwin(Brother twin)
                            {
                                Item.Twin = twin;
                                twin.ExampleEntity = Item;
                                return this;
                            }

                            public ExampleEntityBuilder WithMom(Parent mom)
                            {
                                Item.Mom = mom;
                                mom.ExampleEntitys.Add(Item);
                                return this;
                            }

                            public ExampleEntityBuilder WithKids(List<Child> kids)
                            {
                                Item.Kids = kids;
                                foreach (var obj in kids)
                                {
                                    obj.ExampleEntity = Item;
                                }
                                return this;
                            }

                            public ExampleEntityBuilder WithParents(List<Parent> parents)
                            {
                                Item.Parents = parents;
                                parents.ExampleEntitys.Add(Item);
                                return this;
                            }

                            public override ExampleEntity AutoBuild()
                            {
                                if (Item.Id is null)
                                {
                                    WithId(IdGenerator.Next());
                                }
                                return Build();
                            }
                        }
                    }
                }
            ";

    [Fact]
    public void Compile_NonPersistExample() {
        var input = new BuilderEntity(persistable: false)
        {
            Name = "ExampleEntity"
        };
        input.Fields.Add(new Field("string", "Name"));
        input.Fields.Add(new Field("Brother", "Twin"));
        input.Fields.Add(new Field("Parent", "Mom"));
        input.Fields.Add(new Field("List<Child>", "Kids"));
        input.Fields.Add(new Field("List<Parent>", "Parents"));

        var result = Compiler.Compile(input);

        AssertHelper.AssertMultilineStringEq(NonPersistExampleOutput, result);
    }

    private string NonPersistExampleOutput =>
        @"                using ...

                namespace VipLive.WebApplication.VIPLive.Test. ...
                {
                    public class ExampleEntityTestHelper
                    {
                        public static ExampleEntityBuilder Builder()
                        {
                            return new ExampleEntityBuilder();
                        }


                        public class ExampleEntityBuilder : AbstractBuilder<ExampleEntity>
                        {
                            public ExampleEntityBuilder() : base() { }

                            public ExampleEntityBuilder(ExampleEntity exampleEntity) : base(exampleEntity) { }

                            public ExampleEntityBuilder WithName(string name)
                            {
                                Item.Name = name;
                                return this;
                            }

                            public ExampleEntityBuilder WithTwin(Brother twin)
                            {
                                Item.Twin = twin;
                                return this;
                            }

                            public ExampleEntityBuilder WithMom(Parent mom)
                            {
                                Item.Mom = mom;
                                return this;
                            }

                            public ExampleEntityBuilder WithKids(List<Child> kids)
                            {
                                Item.Kids = kids;
                                return this;
                            }

                            public ExampleEntityBuilder WithParents(List<Parent> parents)
                            {
                                Item.Parents = parents;
                                return this;
                            }
                        }
                    }
                }
            ";
}