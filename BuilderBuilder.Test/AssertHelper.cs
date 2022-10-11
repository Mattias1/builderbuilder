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

        var expectedFields = fields.Select(t => new Field(t.type, t.name, t.inverse));

        ListEq(entityResult.Fields, f => $"{f.Type} {f.Name} {f.InverseHandling}", expectedFields);
    }

    private static void ListEq<T, TC>(IEnumerable<T> result, Func<T, TC> compareBy, IEnumerable<T> expected) {
        ListEq(result.Select(compareBy), expected.Select(compareBy));
    }

    private static void ListEq<T>(IEnumerable<T> result, IEnumerable<T> expected) {
        var actualArr = result as T[] ?? result.ToArray();
        var expectedArr = expected as T[] ?? expected.ToArray();
        var shouldHaves = Missing(actualArr, expectedArr);
        var shouldntHaves = new List<T>();
        if (actualArr.Length + shouldHaves.Count != expectedArr.Length) {
            shouldntHaves = Missing(expectedArr, actualArr);
        }

        if (shouldHaves.Count > 0 || shouldntHaves.Count > 0) {
            Assert.Fail(ErrorList("Should haves", shouldHaves) + ". " + ErrorList("Shouldn't haves", shouldntHaves));
        }
    }

    private static List<T> Missing<T>(IEnumerable<T> checkList, IEnumerable<T> source) {
        var sourceHashSet = new HashSet<T>(checkList);
        return source.Where(b => !sourceHashSet.Contains(b)).ToList();
    }

    private static string ErrorList<T>(string description, IEnumerable<T> list) {
        var arr = list as T[] ?? list.ToArray();
        if (!arr.Any()) {
            return "";
        }
        var errors = string.Join(", ", arr.Select(i => i.ToString()));
        return $"{description}: {errors}";
    }

    public static void AssertMultilineStringEq(string expected, string actual) {
        var split = new[] { '\n' };
        var el = expected.Split(split).Select(TrimWhitespace);
        var al = actual.Split(split).Select(TrimWhitespace);

        ListEq(al, el);
    }

    public static void AreEqualModuloWhitespace(string expected, string actual) {
        Assert.Equal(TrimWhitespace(expected), TrimWhitespace(actual));
    }

    private static string TrimWhitespace(string s) {
        return s.Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("\t", "");
    }
}