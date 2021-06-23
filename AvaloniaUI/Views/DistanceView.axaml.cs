
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvaloniaUI.Views
{
    public partial class DistanceView : UserControl
    {
        public DistanceView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}