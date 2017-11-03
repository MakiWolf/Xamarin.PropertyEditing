using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Xamarin.PropertyEditing.Windows
{
	internal class MenuButton
		: Button
	{
		public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register (
			nameof(Header), typeof(object), typeof(MenuButton), new PropertyMetadata (default(object)));

		public object Header
		{
			get { return (object) GetValue (HeaderProperty); }
			set { SetValue (HeaderProperty, value); }
		}

		public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register (
			nameof(HeaderTemplate), typeof(DataTemplate), typeof(MenuButton), new PropertyMetadata (default(DataTemplate)));

		public DataTemplate HeaderTemplate
		{
			get { return (DataTemplate) GetValue (HeaderTemplateProperty); }
			set { SetValue (HeaderTemplateProperty, value); }
		}

		protected override void OnMouseDown (MouseButtonEventArgs e)
		{
			if (ContextMenu == null)
				return;

			ContextMenu.PlacementTarget = this;
			ContextMenu.Placement = PlacementMode.Bottom;
			ContextMenu.IsOpen = true;
			e.Handled = true;

			base.OnMouseDown (e);
		}
	}
}