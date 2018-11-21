using System;
using System.Collections.Generic;

namespace Something.Something.TestHelpers
{
    public class IdGenerator
    {
        public enum Type { Unspecified };

        private static IdGenerator idGenerator;

        private Dictionary<Type, int> currentIds;
        private object lockObj = new object();

        private static IdGenerator Get => idGenerator ?? (idGenerator = new IdGenerator());

        public static int Next() => Next(Type.Unspecified);
        public static int Next(Type type) => Get.CalculateNext(type);

        public static Guid NextGuid() => NextGuid(Type.Unspecified);
        public static Guid NextGuid(Type type) => Get.CalculateNextGuid(type);

        private IdGenerator() {
            var types = (Type[])Enum.GetValues(typeof(Type));
            currentIds = new Dictionary<Type, int>(types.Length);
            foreach (Type type in types) {
                currentIds[type] = 0;
            }
        }

        public int CalculateNext(Type type) {
            lock (lockObj) {
                currentIds[type]++;
                return currentIds[type];
            }
        }

        public Guid CalculateNextGuid(Type type) {
            lock (lockObj) {
                currentIds[type]++;
                return new Guid(currentIds[type], 0, 0, new byte[8]);
            }
        }
    }
}
