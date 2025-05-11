BuilderBuilder
===============
A little tool to generate code for my unit test builders.
It's not perfect as you have to fix the usings and such yourselves, but it should save quite some
typing.
The reason I use a copy/paste app, instead of something like an IDE plugin or a source generator is
that this way it's IDE independent and the resulting builder is easy to modify.


Prerequisite
-------------
For the NHibernate builders it expects the `AbstractBuilder` and `AbstractEntityBuilder` base classes
and an `IdGenerator` to exist in your project. You can copy the examples from this repo if you want.


Example
--------
This tool will take an entity, for example:
``` csharp
using ...;

namespace SomeApplication.Entities;

public sealed class User : AggregateRoot<User, UserId> {
    public override UserId Id { get; }
    public List<Stuff>? Things { get; }

    public User(UserId id, List<Stuff>? things) {
        Id = id;
        Things = things;
    }

    public bool SomeMethod() => true;
}
```

And turn it into a builder:
``` csharp
using ...;

namespace SomeApplication.Test.TestHelpers;

public class UserTestHelper {
    public static UserBuilder Builder() {
        return new UserBuilder();
    }


    public class UserBuilder {
        private UserId? _id;
        private List<Stuff>? _things;

        public UserBuilder WithId(UserId id) {
            _id = id;
            return this;
        }

        public UserBuilder WithThings(List<Stuff>? things) {
            _things = things;
            return this;
        }

        public User Build() {
            if (_id is null) {
                throw new InvalidOperationException("Id is not nullable");
            }

            return new User(_id, _things);
        }
    }
}
```


Setup development environment
------------------------------
Install the dotnet 8 SDK and run with `dotnet run --project BuilderBuilder`.


Build a release
----------------
To build a release, run `./build-release.sh`.
