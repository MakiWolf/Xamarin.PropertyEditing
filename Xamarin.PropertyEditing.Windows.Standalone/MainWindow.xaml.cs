using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Xamarin.PropertyEditing.Drawing;
using Xamarin.PropertyEditing.Tests;

namespace Xamarin.PropertyEditing.Windows.Standalone
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow ()
		{
			InitializeComponent ();
			this.panel.TargetPlatform = new TargetPlatform {
				SupportsCustomExpressions = true,
				SupportsMaterialDesign = true,
				GroupedTypes = new Dictionary<Type, string> {
					{ typeof(CommonBrush), "Brush" }
				}
			};
			this.panel.EditorProvider = new MockEditorProvider ();
			this.panel.ResourceProvider = new MockResourceProvider ();
		}

		private async void Button_Click (object sender, RoutedEventArgs e)
		{
			var mockedControl = sender as IMockedControl;
			object inspectedObject;
			if (mockedControl == null || mockedControl.MockedControl == null) {
				inspectedObject = sender;
			} else {
				inspectedObject = mockedControl.MockedControl;
				if (mockedControl is MockedSampleControlButton mockedButton) {
					IObjectEditor editor = await this.panel.EditorProvider.GetObjectEditorAsync (inspectedObject);
					await mockedButton.SetBrushInitialValueAsync (editor, new CommonSolidBrush (20, 120, 220, 240, "sRGB"));
					await mockedButton.SetMaterialDesignBrushInitialValueAsync (editor, new CommonSolidBrush (0x65, 0x1F, 0xFF, 200));
					await mockedButton.SetReadOnlyBrushInitialValueAsync (editor, new CommonSolidBrush (240, 220, 15, 190));
				}
			}

			if (this.panel.SelectedItems.Contains (inspectedObject))
				this.panel.SelectedItems.Remove (inspectedObject);
			else
				this.panel.SelectedItems.Add (inspectedObject);
		}

		private void Theme_Click (object sender, RoutedEventArgs e)
		{
			if (e.Source is RadioButton rb) {
				switch (rb.Content.ToString()) {
				case "Dark Theme":
				PropertyEditorPanel.ThemeManager.Theme = PropertyEditing.Themes.PropertyEditorTheme.Dark;
					break;
				case "Light Theme":
					PropertyEditorPanel.ThemeManager.Theme = PropertyEditing.Themes.PropertyEditorTheme.Light;
					break;
				default:
					PropertyEditorPanel.ThemeManager.Theme = PropertyEditing.Themes.PropertyEditorTheme.None;
					break;
				}
			}
		}
	}
}
