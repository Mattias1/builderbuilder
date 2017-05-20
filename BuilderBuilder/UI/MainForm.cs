using System.Windows.Forms;
using System.Drawing;
using MattyControls;

namespace BuilderBuilder
{
    class MainForm : MattyForm
    {
        private static Size MIN_SIZE = new Size(220, 150);
        private static Size DEFAULT_SIZE = new Size(700, 500);

        public MainForm() : base(MIN_SIZE, DEFAULT_SIZE, Settings.Get) {
            this.Text = "BuilderBuilder";
            this.Icon = Properties.Resources.EyesHybrid;

            this.AddUserControl(new MainControl());
            this.ShowUserControl<MainControl>();
        }
    }
}
