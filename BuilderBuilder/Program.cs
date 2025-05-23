﻿using Avalonia;
using Avalonia.Markup.Declarative;
using AvaloniaExtensions;
using BuilderBuilder.UI;

namespace BuilderBuilder;

internal static class Program {
  private static readonly Size MIN_SIZE = new(350, 200);
  private static readonly Size DEFAULT_SIZE = new(700, 450);

  private static void Main(string[] args) {
    AvaloniaExtensionsApp.Init().StartDesktopApp(() => ExtendedWindow.Init<MainControl>("BuilderBuilder")
      .AddLazyComponent<SettingsControl>()
      .WithSettingsFile<Settings>("./builderbuilder.json")
      .WithSize(DEFAULT_SIZE, MIN_SIZE)
      .Icon(AssetExtensions.LoadWindowIcon("Resources/EyesHybrid.ico")));
  }
}
