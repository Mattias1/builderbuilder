BuilderBuilder
===============
NOTE - THIS README IS OUT OF DATE - TODO

A little tool to generate code for my unit-test builders.
It's not perfect as you have to fix the usings and namespaces and such yourselves, but it should save quite some typing.


Prerequisite
-------------
It expects the `AbstractBuilder` and `AbstractEntityBuilder` base classes and an `IdGenerator` to exist in your project.
You can copy the ones in this repo if you want.


Example
--------
This tool will take an entity, for example:
``` csharp
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

        [ManyToMany]
        public virtual IEnumerable<Stuff> Stuffs { get; set; }

        public virtual int IgnoreMe()
        {
            return 42;
        }
    }
}
```

And turn it into a builder:
``` csharp
using ...

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

            public ExampleEntityBuilder WithStuffs(IEnumerable<Stuff> stuffs)
            {
                Item.Stuffs = stuffs;
                stuffs.ExampleEntitys.Add(Item);
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
```


Setup development environment
------------------------------
Install the dotnet 7 SDK and run with `dotnet run --project BuilderBuilder`.
