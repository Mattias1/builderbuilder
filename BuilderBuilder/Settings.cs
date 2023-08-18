using System.Text.Json.Serialization;

namespace BuilderBuilder;

internal class Settings {
  public string SelectedFrameworkString { get; set; } = Frameworks.All[0].Slug;

  [JsonIgnore]
  public Framework SelectedFramework {
    get => Frameworks.FromSlug(SelectedFrameworkString);
    set => SelectedFrameworkString = value.Slug;
  }
}
