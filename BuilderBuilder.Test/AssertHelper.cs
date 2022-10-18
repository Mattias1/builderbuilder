using Xunit;

namespace BuilderBuilder.Test;

internal static class AssertHelper
{
    public static void AssertBuilderEntity(
        BuilderEntity entityResult, string name, bool persistable, params (string type, string name)[] fields
    ) {
        var fieldsWithInverseType = fields.Select(t => (t.type, t.name, Field.InverseHandlingType.None)).ToArray();
        AssertBuilderEntity(entityResult, name, persistable, fieldsWithInverseType);
    }

    public static void AssertBuilderEntity(BuilderEntity entityResult, string name, bool persistable,
        params (string type, string name, Field.InverseHandlingType inverse)[] fields
    ) {
        Assert.Equal(name, entityResult.Name);

        Assert.Equal(persistable, entityResult.Persistable);

        var expectedFields = fields.Select(t => new Field(t.type, t.name, t.inverse)).ToArray();

        ListEq(entityResult.Fields, f => $"{f.Type} {f.Name} {f.InverseHandling}", expectedFields);
    }

    private static void ListEq<T, TC>(IList<T> result, Func<T, TC> compareBy, IList<T> expected) {
        ListEq(result.Select(compareBy).ToArray(), expected.Select(compareBy).ToArray());
    }

    private static void ListEq<T>(IList<T> result, IList<T> expected) {
        var shouldHaves = Missing(result, expected);
        var shouldntHaves = new List<T>();
        if (result.Count + shouldHaves.Count != expected.Count) {
            shouldntHaves = Missing(expected, result);
        }

        if (shouldHaves.Count > 0 || shouldntHaves.Count > 0) {
            Assert.Fail(ErrorList("Should haves", shouldHaves) + ". " + ErrorList("Shouldn't haves", shouldntHaves));
        }
    }

    private static List<T> Missing<T>(IList<T> checkList, IList<T> source) {
        var sourceHashSet = new HashSet<T>(checkList);
        return source.Where(b => !sourceHashSet.Contains(b)).ToList();
    }

    private static string ErrorList<T>(string description, IList<T> list) {
        if (!list.Any()) {
            return "";
        }
        var errors = string.Join(", ", list.Select(i => i?.ToString()));
        return $"{description}: {errors}";
    }

    public static void AssertMultilineStringEq(string expected, string actual) {
        var split = new[] { '\n' };
        var el = expected.Split(split).Select(TrimWhitespace).ToArray();
        var al = actual.Split(split).Select(TrimWhitespace).ToArray();

        ListEq(al, el);
    }

    public static void AreEqualModuloWhitespace(string expected, string actual) {
        Assert.Equal(TrimWhitespace(expected), TrimWhitespace(actual));
    }

    private static string TrimWhitespace(string s) {
        return s.Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("\t", "");
    }
}