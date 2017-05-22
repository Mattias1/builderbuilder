using System.Linq;
using MattyControls;

namespace BuilderBuilder
{
    class Settings : SettingsSingleton
    {
        protected override string Name => "BuilderBuilder";

        public static Settings Get => GetSingleton<Settings>();

        public Framework SelectedFramework {
            get { return Frameworks.FromSlug(get("selected-framework", Frameworks.All.First().Slug)); }
            set { set("selected-framework", value.Slug); }
        }
    }
}
