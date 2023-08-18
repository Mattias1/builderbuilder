using MattyControls;

namespace BuilderBuilder.UI;

internal class MainForm : MattyForm {
  private static readonly Size MIN_SIZE = new(220, 150);
  private static readonly Size DEFAULT_SIZE = new(700, 450);

  public MainForm() : base(MIN_SIZE, DEFAULT_SIZE, Settings.Get) {
    Text = "BuilderBuilder";
    Icon = Properties.Resources.EyesHybrid;

    AddUserControl(new MainControl());
    ShowUserControl<MainControl>();
  }
}
