
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvaloniaUI.Views
{
    public partial class WarehouseView : UserControl
    {
        public WarehouseView()
        {
            InitializeComponent();
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}