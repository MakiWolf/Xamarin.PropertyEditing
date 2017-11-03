using System.Collections.Generic;
using Xamarin.PropertyEditing.Drawing;

namespace Xamarin.PropertyEditing.ViewModels
{
	internal class SolidBrushPropertyViewModel : PropertyViewModel<CommonSolidBrush>, IColorSpaced
	{
		public SolidBrushPropertyViewModel(IPropertyInfo property, IEnumerable<IObjectEditor> editors)
			: base(property, editors)
		{
			var solidBrushPropertyInfo = property as IColorSpaced;
			if (solidBrushPropertyInfo != null) {
				ColorSpaces = solidBrushPropertyInfo.ColorSpaces;
			}
		}

		public IReadOnlyList<string> ColorSpaces { get; }

		CommonColor? hue;
		public CommonColor Hue {
			get => hue.HasValue ? hue.Value : (hue = Value.Color.ToHue()).Value;
			set {
				if (!hue.Equals(value)) {
					hue = value;
					OnPropertyChanged ();
				}
			}
		}
	}
}
