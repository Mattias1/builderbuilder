using System.Linq;

namespace BuilderBuilder
{
    class Frameworks
    {
        private static Framework[] frameworks;
        public static Framework[] All = frameworks ?? BuildFrameworks();

        private static Framework[] BuildFrameworks() {
            frameworks = new Framework[] {
                new Framework("C# NHibernate", "CsNhibernate")
            };
            return frameworks;
        }

        public static Framework FromSlug(string slug) {
            return All.First(f => f.Slug == slug);
        }

        public static int IndexOf(Framework framework) {
            for (int i = 0; i < All.Length; i++) {
                if (All[i] == framework) {
                    return i;
                }
            }
            return -1;
        }
    }

    class Framework
    {
        public string Name { get; private set; }
        public string Slug { get; private set; }

        public Framework(string name, string slug = null) {
            Name = name;
            Slug = slug ?? name;
        }
    }
}
