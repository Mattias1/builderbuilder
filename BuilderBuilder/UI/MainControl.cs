using System;
using MattyControls;

namespace BuilderBuilder
{
    class MainControl : MattyUserControl
    {
        private Db _dbFramework;
        private RichTb _tbInput, _tbOutput;

        public MainControl() {
            _dbFramework = new Db(this);
            _dbFramework.SelectedIndexChanged += OnFrameworkChange;
            foreach (var framework in Frameworks.All) {
                _dbFramework.Items.Add(framework.Name);
            }
            _dbFramework.SelectedIndex = Frameworks.IndexOf(Settings.Get.SelectedFramework);
            _dbFramework.SelectedIndexChanged += OnInputChange;

            _tbInput = new RichTb(this);
            _tbInput.Multiline = true;
            _tbInput.TextChanged += OnInputChange;
            _tbInput.AddLabel("Input:", false);

            _tbOutput = new RichTb(this);
            _tbOutput.Multiline = true;
            _tbOutput.AddLabel("Output:", false);
        }

        public override void OnResize() {
            _dbFramework.PositionTopRightInside(this);

            _tbInput.PositionBelow(_dbFramework);
            _tbInput.StretchDownInside(this);

            ControlHelpers.StretchControlsHorizontallyInside(this, _tbInput, _tbOutput);
            _tbInput.Label.PositionAbove(_tbInput);
            _tbOutput.Label.PositionAbove(_tbOutput);
        }

        public override void OnShow() {
            _tbInput.Select();
        }

        private void OnFrameworkChange(object o, EventArgs e) {
            Settings.Get.SelectedFramework = Frameworks.All[_dbFramework.SelectedIndex];
        }

        private void OnInputChange(object o, EventArgs e) {
            _tbOutput.Text = BuildBuilder(_tbInput.Text, Settings.Get.SelectedFramework);
        }

        public static string BuildBuilder(string input, Framework framework) {
            BuilderEntity entity = framework.Parser.Parse(input);
            return framework.Compiler.Compile(entity);
        }
    }
}
