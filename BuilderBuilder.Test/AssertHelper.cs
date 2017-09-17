using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BuilderBuilder.Test
{
    static class AssertHelper
    {
        public static void AssertBuilderEntity(BuilderEntity entityResult, string name, params (string type, string name)[] fields) {
            Assert.AreEqual(name, entityResult.Name);

            var expectedFields = fields.Select(t => new Field(t.type, t.name));

            AssertHelper.List(entityResult.Fields, f => $"{f.Type} {f.Name}", expectedFields);
        }

        public static void List<T, C>(IEnumerable<T> result, Func<T, C> compareBy, params T[] expected) {
            List(result, compareBy, expected.AsEnumerable());
        }
        public static void List<T, C>(IEnumerable<T> result, Func<T, C> compareBy, params C[] expected) {
            List(result, compareBy, expected.AsEnumerable());
        }
        public static void List<T>(IEnumerable<T> result, params T[] expected) {
            List(result, expected.AsEnumerable());
        }

        public static void List<T, C>(IEnumerable<T> result, Func<T, C> compareBy, IEnumerable<T> expected) {
            List(result.Select(compareBy), expected.Select(compareBy));
        }
        public static void List<T, C>(IEnumerable<T> result, Func<T, C> compareBy, IEnumerable<C> expected) {
            List(result.Select(compareBy), expected);
        }
        public static void List<T>(IEnumerable<T> result, IEnumerable<T> expected) {
            List<T> shouldHaves = Missing(result, expected);
            List<T> shouldntHaves = new List<T>();
            if (result.Count() + shouldHaves.Count < expected.Count()) {
                shouldntHaves = Missing(expected, result);
            }

            if (shouldHaves.Count > 0 || shouldntHaves.Count > 0) {
                Assert.Fail(ErrorList("Should haves", shouldHaves) + " " + ErrorList("Shouldn't haves", shouldntHaves));
            }
        }

        private static List<T> Missing<T>(IEnumerable<T> checkList, IEnumerable<T> source) {
            HashSet<T> sourceHashSet = new HashSet<T>(checkList);
            return source.Where(b => !sourceHashSet.Contains(b)).ToList();
        }

        private static string ErrorList<T>(string description, IEnumerable<T> list) {
            if (!list.Any()) {
                return "";
            }
            var errors = string.Join(", ", list.Select(i => i.ToString()));
            return $"{description}: {errors}";
        }
    }
}
