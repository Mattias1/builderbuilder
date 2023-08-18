using Avalonia;
using Avalonia.Markup.Declarative;
using AvaloniaExtensions;
using BuilderBuilder.UI;

namespace BuilderBuilder;

internal static class Program {
  private static readonly Size MIN_SIZE = new(220, 150);
  private static readonly Size DEFAULT_SIZE = new(700, 450);

  private static void Main(string[] args) {
    AppBuilderExtensions.Init().StartDesktopApp(() => ExtendedWindow.Init<MainControl>("BuilderBuilder")
        .WithSettingsFile<Settings>("./builderbuilder.json")
        .WithSize(DEFAULT_SIZE, MIN_SIZE)
        .Icon(AssetExtensions.LoadWindowIcon("Resources/EyesHybrid.ico")));
  }
}
