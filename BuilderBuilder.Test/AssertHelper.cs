using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BuilderBuilder.Test
{
    static class AssertHelper
    {
        public static void AssertBuilderEntity(BuilderEntity entityResult, string name, params (string type, string name)[] fields) {
            var fieldsWithInverseType = fields.Select(t => (t.type, t.name, Field.InverseHandlingType.None)).ToArray();
            AssertBuilderEntity(entityResult, name, fieldsWithInverseType);
        }

        public static void AssertBuilderEntity(BuilderEntity entityResult, string name, params (string type, string name, Field.InverseHandlingType inverse)[] fields) {
            Assert.AreEqual(name, entityResult.Name);

            var expectedFields = fields.Select(t => new Field(t.type, t.name, t.inverse));

            AssertHelper.ListEq(entityResult.Fields, f => $"{f.Type} {f.Name} {f.InverseHandling}", expectedFields);
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
            List<T> shouldHaves = Missing(result, expected);
            List<T> shouldntHaves = new List<T>();
            if (result.Count() + shouldHaves.Count != expected.Count()) {
                shouldntHaves = Missing(expected, result);
            }

            if (shouldHaves.Count > 0 || shouldntHaves.Count > 0) {
                Assert.Fail(ErrorList("Should haves", shouldHaves) + ". " + ErrorList("Shouldn't haves", shouldntHaves));
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
