using System.Text.Json.Serialization;

namespace BuilderBuilder;

public class Settings {
  public const int DEFAULT_NR_OF_SPACES = 4;
  public const string DEFAULT_NAMESPACE = "<insert namespace>";
  public const bool DEFAULT_USE_EGYPTIAN_BRACES = false;
  public const bool DEFAULT_UNDERSCORE_ABSTRACT = false;
  public const bool DEFAULT_INCLUDE_USING_DOTS = true;
  public string SelectedFrameworkString { get; set; } = Frameworks.All[0].Slug;

  [JsonIgnore]
  public Framework SelectedFramework {
    get => Frameworks.FromSlug(SelectedFrameworkString);
    set => SelectedFrameworkString = value.Slug;
  }

  /// <summary>
  /// The number of spaces to indent with. Use 0 for tabs.
  /// </summary>
  public int NrOfSpaces { get; set; } = DEFAULT_NR_OF_SPACES;

  public string Namespace { get; set; } = DEFAULT_NAMESPACE;

  public bool EgyptianBracesIndentStyle { get; set; } = DEFAULT_USE_EGYPTIAN_BRACES;

  public bool UnderscoreAbstract { get; set; } = DEFAULT_UNDERSCORE_ABSTRACT;

  public bool IncludeUsingDots { get; set; } = DEFAULT_INCLUDE_USING_DOTS;
}
