﻿using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Declarative;
using AvaloniaExtensions;

namespace BuilderBuilder.UI;

internal class SettingsControl : CanvasComponentBase {
  private const int LABEL_WIDTH = 110;

  private Settings Settings => GetSettings<Settings>();

  private TextBox _tbNamespace = null!;
  private RadioButton _rbSpaces = null!, _rbTabs = null!;
  private TextBox _tbNumberOfSpaces = null!;
  private RadioButton _rbEgyptianBraces = null!, _rbNormalBraces = null!;

  protected override void InitializeControls() {
    _tbNamespace = AddTextBox().TopLeftInPanel();
    _rbSpaces = AddRadio("indent", "Spaces").Below();
    _rbTabs = AddRadio("indent", "Tabs").Below();
    _rbNormalBraces = AddRadio("braces", "Normal (Allman)").Below();
    _rbEgyptianBraces = AddRadio("braces", "Egyptian (K&R)").Below();

    InsertLabelLeftOf("Namespace:", _tbNamespace, LABEL_WIDTH);
    InsertLabelLeftOf("Indent size:", _rbSpaces, LABEL_WIDTH);
    InsertLabelLeftOf("Brace style:", _rbNormalBraces, LABEL_WIDTH);
    _rbTabs.XAlignLeft(_rbSpaces);
    _rbEgyptianBraces.XAlignLeft(_rbNormalBraces);

    _tbNumberOfSpaces = AddTextBox().OnTextChanged(OnNumberOfSpacesTextChanged).RightOf(_rbSpaces);
    _tbNamespace.StretchRightInPanel();

    AddButton("Cancel", OnCancelClick).With(b => b.IsCancel = true).BottomRightInPanel();
    AddButton("Ok", OnOkClick).With(b => b.IsDefault = true).LeftOf();
  }

  protected override void OnInitialized() {
    base.OnInitialized();
    LoadSettings();
  }

  private void OnNumberOfSpacesTextChanged(TextChangedEventArgs e) {
    if (int.TryParse(_tbNumberOfSpaces.Text, out int nrOfSpaces) && nrOfSpaces > 0) {
      _rbSpaces.IsChecked = true;
    }
  }

  private void OnCancelClick(RoutedEventArgs e) {
    LoadSettings();
    SwitchToComponent<MainControl>();
  }

  private void OnOkClick(RoutedEventArgs e) {
    SaveSettings();
    SwitchToComponent<MainControl>();
  }

  private void LoadSettings() {
    _tbNamespace.Text = Settings.Namespace;
    _rbSpaces.IsChecked = Settings.NrOfSpaces > 0;
    _rbTabs.IsChecked = Settings.NrOfSpaces == 0;
    _tbNumberOfSpaces.Text = Settings.NrOfSpaces > 0 ? Settings.NrOfSpaces.ToString() : "";
    _rbNormalBraces.IsChecked = !Settings.EgyptianBracesIndentStyle;
    _rbEgyptianBraces.IsChecked = Settings.EgyptianBracesIndentStyle;
  }

  private void SaveSettings() {
    Settings.Namespace = _tbNamespace.Text ?? Settings.DEFAULT_NAMESPACE;
    if (!int.TryParse(_tbNumberOfSpaces.Text, out int nrOfSpaces)) {
      nrOfSpaces = Settings.DEFAULT_NR_OF_SPACES;
    }
    Settings.NrOfSpaces = _rbTabs.IsChecked == true ? 0 : nrOfSpaces;
    Settings.EgyptianBracesIndentStyle = _rbEgyptianBraces.IsChecked == true;
  }
}
