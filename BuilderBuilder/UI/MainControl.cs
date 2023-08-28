using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Declarative;
using AvaloniaExtensions;

namespace BuilderBuilder.UI;

internal class MainControl : CanvasComponentBase {
  private Settings Settings => GetSettings<Settings>();

  private TextBox _tbInput = null!;
  private TextBox _tbOutput = null!;
  private ExtendedComboBox<Framework> _cbFramework = null!;

  protected override void InitializeControls() {
    var btnSettings = AddButton("Settings", _ => SwitchToComponent<SettingsControl>()).TopLeftInPanel();
    _cbFramework = AddComboBox(Frameworks.All, OnFrameworkChange).TopRightInPanel();

    _tbInput = AddMultilineTextBox().OnTextChanged(OnInputChange).XLeftInPanel().YBelow(btnSettings)
        .StretchDownInPanel().StretchFractionRightInPanel(1, 2);
    _tbOutput = AddMultilineTextBox().RightOf(_tbInput).StretchDownInPanel().StretchRightInPanel();
    AddLabel("Input:", _tbInput).XRightOf(btnSettings).YAbove(_tbInput);
    AddLabel("Output:", _tbOutput).Above(_tbOutput);
  }

  protected override void OnLoaded(RoutedEventArgs e) {
    base.OnLoaded(e);
    _cbFramework.SelectedIndex = Frameworks.IndexOf(Settings.SelectedFramework);
    _tbInput.Focus();
  }

  protected override void OnGotFocus(GotFocusEventArgs e) {
    base.OnGotFocus(e);
    _tbInput.Focus();
  }

  private void OnFrameworkChange(SelectedItemChangedEventArgs<Framework> e) {
    Settings.SelectedFramework = e.SelectedItem;

    OnInputChange(e);
  }

  private void OnInputChange(RoutedEventArgs e) {
    _tbOutput.Text = BuildBuilder(_tbInput.Text ?? "", Settings);
  }

  private static string BuildBuilder(string input, Settings settings) {
    try {
      var framework = settings.SelectedFramework;
      var entity = framework.Parser.Parse(input);
      return framework.Compiler.Compile(entity, settings);
    } catch (Exception e) {
      return $"Cannot parse.\nError: {e.Message}.";
    }
  }
}
