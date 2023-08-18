using BuilderBuilder.UI;

namespace BuilderBuilder;

internal static class Program {
  private static void Main(string[] args) {
    Settings.Get.Load();

    var main = new MainForm();

    Application.EnableVisualStyles();
    Application.Run(main);

    Settings.Get.Save();
  }
}
