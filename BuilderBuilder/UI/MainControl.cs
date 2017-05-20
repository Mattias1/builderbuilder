using System;
using MattyControls;

namespace BuilderBuilder
{
    class MainControl : MattyUserControl
    {
        Db dbFramework;
        Tb tbInput, tbOutput;

        public MainControl() {
            this.dbFramework = new Db(this);
            foreach (var framework in Frameworks.All) {
                this.dbFramework.Items.Add(framework.Name);
            }
            this.dbFramework.SelectedIndex = Frameworks.IndexOf(Settings.Get.SelectedFramework);

            this.tbInput = new Tb(this);
            this.tbInput.Multiline = true;
            this.tbInput.AddLabel("Input:", false);

            this.tbOutput = new Tb(this);
            this.tbOutput.Multiline = true;
            this.tbOutput.AddLabel("Output:", false);
        }

        public override void OnResize() {
            this.dbFramework.PositionTopRightInside(this);

            this.tbInput.PositionBottomLeftInside(this);
            this.tbInput.StretchRightFixed(this.Width / 2 - MattyControl.Distance - this.tbInput.Width);
            this.tbInput.StretchUpTo(this.dbFramework);
            this.tbInput.Label.PositionAbove(this.tbInput);

            this.tbOutput.PositionRightOf(this.tbInput);
            this.tbOutput.StretchRightInside(this);
            this.tbOutput.StretchDownInside(this);
            this.tbOutput.Label.PositionAbove(this.tbOutput);
        }

        public override void OnShow() {
            this.tbInput.Select();
        }
    }
}
