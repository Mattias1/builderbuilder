# builderbuilder
Generate code for my unit-test builders.

## Example
This tool will take an entity, for example:
```
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
```
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
```
