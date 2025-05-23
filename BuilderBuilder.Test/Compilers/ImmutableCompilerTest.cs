﻿using BuilderBuilder.Compilers;
using Xunit;

namespace BuilderBuilder.Test.Compilers;

public class ImmutableCompilerTest {
  private readonly ImmutableCompiler _compiler = new();

  [Fact]
  public void Compile_NonPersistExample() {
    var input = new BuilderEntity(persistable: false) {
        Name = "ExampleEntity",
        Fields = {
            new Field("long?", "Id"),
            new Field("string", "Name"),
            new Field("Brother", "Twin"),
            new Field("Parent", "Mom"),
            new Field("List<Child>", "Kids"),
            new Field("List<Parent>", "Parents")
        }
    };

    var result = _compiler.Compile(input, new Settings() {
        Namespace = "ExampleProject.Test.TestHelpers",
        NrOfSpaces = 2,
        EgyptianBracesIndentStyle = true
    });

    AssertHelper.AssertMultilineStringEq(NonPersistExampleOutput, result);
  }

  private const string NonPersistExampleOutput = @"using ...

      namespace ExampleProject.Test.TestHelpers;

      public class ExampleEntityTestHelper {
        public static ExampleEntityBuilder Builder() {
          return new ExampleEntityBuilder();
        }

        public class ExampleEntityBuilder {
          private long? _id;
          private string? _name;
          private Brother? _twin;
          private Parent? _mom;
          private List<Child>? _kids;
          private List<Parent>? _parents;

          public ExampleEntityBuilder WithId(long? id, out long? outVar) => WithOut(WithId, id, out outVar);
          public ExampleEntityBuilder WithId(long? id) {
            _id = id;
            return this;
          }

          public ExampleEntityBuilder WithName(string name, out string outVar) => WithOut(WithName, name, out outVar);
          public ExampleEntityBuilder WithName(string name) {
            _name = name;
            return this;
          }

          public ExampleEntityBuilder WithTwin(Brother twin, out Brother outVar) => WithOut(WithTwin, twin, out outVar);
          public ExampleEntityBuilder WithTwin(Brother twin) {
            _twin = twin;
            return this;
          }

          public ExampleEntityBuilder WithMom(Parent mom, out Parent outVar) => WithOut(WithMom, mom, out outVar);
          public ExampleEntityBuilder WithMom(Parent mom) {
            _mom = mom;
            return this;
          }

          public ExampleEntityBuilder WithKids(List<Child> kids, out List<Child> outVar) => WithOut(WithKids, kids, out outVar);
          public ExampleEntityBuilder WithKids(List<Child> kids) {
            _kids = kids;
            return this;
          }

          public ExampleEntityBuilder WithParents(List<Parent> parents, out List<Parent> outVar) => WithOut(WithParents, parents, out outVar);
          public ExampleEntityBuilder WithParents(List<Parent> parents) {
            _parents = parents;
            return this;
          }

          public ExampleEntity Build() {
            if (_name is null) {
              throw new InvalidOperationException(""Name is not nullable"");
            }
            if (_twin is null) {
              throw new InvalidOperationException(""Twin is not nullable"");
            }
            if (_mom is null) {
              throw new InvalidOperationException(""Mom is not nullable"");
            }
            if (_kids is null) {
              throw new InvalidOperationException(""Kids is not nullable"");
            }
            if (_parents is null) {
              throw new InvalidOperationException(""Parents is not nullable"");
            }

            return new ExampleEntity(_id, _name, _twin, _mom, _kids, _parents);
          }

          private ExampleEntityBuilder WithOut<T>(Func<T, ExampleEntityBuilder> buildFunc, T inVar, out T outVar) {
            outVar = inVar;
            return buildFunc(inVar);
          }
        }
      }
  ";
}
