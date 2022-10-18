using MattyControls;

namespace BuilderBuilder;

internal class Settings : SettingsSingleton
{
    protected override string Name => "BuilderBuilder";

    public static Settings Get => GetSingleton<Settings>();

    public Framework SelectedFramework {
        get => Frameworks.FromSlug(get("selected-framework", Frameworks.All.First().Slug));
        set => set("selected-framework", value.Slug);
    }
}