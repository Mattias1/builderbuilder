using System.Linq;
using MattyControls;

namespace BuilderBuilder
{
    class Settings : SettingsSingleton
    {
        protected override string Name => "BuilderBuilder";

        public static Settings Get => SettingsSingleton.GetSingleton<Settings>();

        public Framework SelectedFramework {
            get { return Frameworks.FromSlug(this.get("selected-framework", Frameworks.All.First().Slug)); }
            set { this.set("selected-framework", value.Slug); }
        }
    }
}
