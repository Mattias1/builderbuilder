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

    public static void ListEq<T, C>(IEnumerable<T> result, Func<T, C> compareBy, params T[] expected) {
        ListEq(result, compareBy, expected.AsEnumerable());
    }
    public static void ListEq<T, C>(IEnumerable<T> result, Func<T, C> compareBy, params C[] expected) {
        ListEq(result, compareBy, expected.AsEnumerable());
    }
    public static void ListEq<T>(IEnumerable<T> result, params T[] expected) {
        ListEq(result, expected.AsEnumerable());
    }

    public static void ListEq<T, C>(IEnumerable<T> result, Func<T, C> compareBy, IEnumerable<T> expected) {
        ListEq(result.Select(compareBy), expected.Select(compareBy));
    }
    public static void ListEq<T, C>(IEnumerable<T> result, Func<T, C> compareBy, IEnumerable<C> expected) {
        ListEq(result.Select(compareBy), expected);
    }
    public static void ListEq<T>(IEnumerable<T> result, IEnumerable<T> expected) {
        var shouldHaves = Missing(result, expected);
        var shouldntHaves = new List<T>();
        if (result.Count() + shouldHaves.Count != expected.Count()) {
            shouldntHaves = Missing(expected, result);
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
        if (!list.Any()) {
            return "";
        }
        var errors = string.Join(", ", list.Select(i => i.ToString()));
        return $"{description}: {errors}";
    }

    public static void AssertMultilineStringEq(string expected, string actual) {
        var split = new char[] { '\n' };
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