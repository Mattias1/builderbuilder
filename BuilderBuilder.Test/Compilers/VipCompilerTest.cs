using BuilderBuilder.Compilers;
using Xunit;

namespace BuilderBuilder.Test.Compilers;

public class VipCompilerTest
{
    private readonly VipCompiler _compiler = new();

    [Fact]
    public void Compile_PersistExample()
    {
        var input = new BuilderEntity(persistable: true)
        {
            Name = "ExampleEntity",
            Fields =
            {
                new Field("long?", "Id"),
                new Field("string", "Name"),
                new Field("Brother", "Twin", Field.InverseHandlingType.OneToOne),
                new Field("Parent", "Mom", Field.InverseHandlingType.ManyToOne),
                new Field("List<Child>", "Kids", Field.InverseHandlingType.OneToMany),
                new Field("List<Parent>", "Parents", Field.InverseHandlingType.ManyToMany)
            }
        };

        var result = _compiler.Compile(input);

        AssertHelper.AssertMultilineStringEq(PersistExampleOutput, result);
    }

    private const string PersistExampleOutput = @"                using ...

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
    public void Compile_NonPersistExample()
    {
        var input = new BuilderEntity(persistable: false)
        {
            Name = "ExampleEntity",
            Fields =
            {
                new Field("string", "Name"),
                new Field("Brother", "Twin"),
                new Field("Parent", "Mom"),
                new Field("List<Child>", "Kids"),
                new Field("List<Parent>", "Parents")
            }
        };

        var result = _compiler.Compile(input);

        AssertHelper.AssertMultilineStringEq(NonPersistExampleOutput, result);
    }

    private const string NonPersistExampleOutput = @"                using ...

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