// 
// Outdoor Tracker - Copyright(C) 2017 Meinard Jean-Richard
//  
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//  
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//  
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Linq;
using System.Windows.Input;

using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace OutdoorTracker.Controls
{
    [TemplatePart(Name = GridViewPartName, Type = typeof(GridView))]
    public class ColorPickerControl : Control
    {
        public static readonly DependencyProperty ColorSelectedCommandProperty = DependencyProperty.Register(
            "ColorSelectedCommand", typeof(ICommand), typeof(ColorPickerControl), new PropertyMetadata(default(ICommand)));

        public ICommand ColorSelectedCommand
        {
            get { return (ICommand)GetValue(ColorSelectedCommandProperty); }
            set { SetValue(ColorSelectedCommandProperty, value); }
        }

        public const string GridViewPartName = "PART_GridView";

        private static readonly ColorItem Yellow = new ColorItem("Yellow", 0xFFFFFF00);
        private static readonly ColorItem BananaYellow = new ColorItem("BananaYellow", 0xFFFFE135);
        private static readonly ColorItem LaserLemon = new ColorItem("LaserLemon", 0xFFFFFF66);
        private static readonly ColorItem Jasmine = new ColorItem("Jasmine", 0xFFF8DE7E);
        private static readonly ColorItem Green = new ColorItem("Green", 0xFF008000);
        private static readonly ColorItem Emerald = new ColorItem("Emerald", 0xFF008A00);
        private static readonly ColorItem GreenYellow = new ColorItem("GreenYellow", 0xFFADFF2F);
        private static readonly ColorItem Lime = new ColorItem("Lime", 0xFF00FF00);
        private static readonly ColorItem Chartreuse = new ColorItem("Chartreuse", 0xFF7FFF00);
        private static readonly ColorItem LimeGreen = new ColorItem("LimeGreen", 0xFF32CD32);
        private static readonly ColorItem SpringGreen = new ColorItem("SpringGreen", 0xFF00FF7F);
        private static readonly ColorItem LightGreen = new ColorItem("LightGreen", 0xFF90EE90);
        private static readonly ColorItem MediumSeaGreen = new ColorItem("MediumSeaGreen", 0xFF3CB371);
        private static readonly ColorItem MediumSpringGreen = new ColorItem("MediumSpringGreen", 0xFF00FA9A);
        private static readonly ColorItem Olive = new ColorItem("Olive", 0xFF808000);
        private static readonly ColorItem SeaGreen = new ColorItem("SeaGreen", 0xFF2E8B57);
        private static readonly ColorItem Red = new ColorItem("Red", 0xFFFF0000);
        private static readonly ColorItem OrangeRed = new ColorItem("OrangeRed", 0xFFFF4500);
        private static readonly ColorItem DarkOrange = new ColorItem("DarkOrange", 0xFFFF8C00);
        private static readonly ColorItem Orange = new ColorItem("Orange", 0xFFFFA500);
        private static readonly ColorItem ImperialRed = new ColorItem("ImperialRed", 0xFFED2939);
        private static readonly ColorItem Maroon = new ColorItem("Maroon", 0xFF800000);
        private static readonly ColorItem Brown = new ColorItem("Brown", 0xFFA52A2A);
        private static readonly ColorItem Chocolate = new ColorItem("Chocolate", 0xFFD2691E);
        private static readonly ColorItem Coral = new ColorItem("Coral", 0xFFFF7F50);
        private static readonly ColorItem Crimson = new ColorItem("Crimson", 0xFFDC143C);
        private static readonly ColorItem DarkSalmon = new ColorItem("DarkSalmon", 0xFFE9967A);
        private static readonly ColorItem DeepPink = new ColorItem("DeepPink", 0xFFFF1493);
        private static readonly ColorItem Firebrick = new ColorItem("Firebrick", 0xFFB22222);
        private static readonly ColorItem HotPink = new ColorItem("HotPink", 0xFFFF69B4);
        private static readonly ColorItem IndianRed = new ColorItem("IndianRed", 0xFFCD5C5C);
        private static readonly ColorItem LightCoral = new ColorItem("LightCoral", 0xFFF08080);
        private static readonly ColorItem LightPink = new ColorItem("LightPink", 0xFFFFB6C1);
        private static readonly ColorItem LightSalmon = new ColorItem("LightSalmon", 0xFFFFA07A);
        private static readonly ColorItem Magenta = new ColorItem("Magenta", 0xFFFF00FF);
        private static readonly ColorItem MediumVioletRed = new ColorItem("MediumVioletRed", 0xFFC71585);
        private static readonly ColorItem Orchid = new ColorItem("Orchid", 0xFFDA70D6);
        private static readonly ColorItem PaleVioletRed = new ColorItem("PaleVioletRed", 0xFFDB7093);
        private static readonly ColorItem Salmon = new ColorItem("Salmon", 0xFFFA8072);
        private static readonly ColorItem SandyBrown = new ColorItem("SandyBrown", 0xFFF4A460);
        private static readonly ColorItem Navy = new ColorItem("Navy", 0xFF000080);
        private static readonly ColorItem Indigo = new ColorItem("Indigo", 0xFF4B0082);
        private static readonly ColorItem MidnightBlue = new ColorItem("MidnightBlue", 0xFF191970);
        private static readonly ColorItem Blue = new ColorItem("Blue", 0xFF0000FF);
        private static readonly ColorItem Purple = new ColorItem("Purple", 0xFF800080);
        private static readonly ColorItem BlueViolet = new ColorItem("BlueViolet", 0xFF8A2BE2);
        private static readonly ColorItem CornflowerBlue = new ColorItem("CornflowerBlue", 0xFF6495ED);
        private static readonly ColorItem Cyan = new ColorItem("Cyan", 0xFF00FFFF);
        private static readonly ColorItem DarkCyan = new ColorItem("DarkCyan", 0xFF008B8B);
        private static readonly ColorItem DarkSlateBlue = new ColorItem("DarkSlateBlue", 0xFF483D8B);
        private static readonly ColorItem DeepSkyBlue = new ColorItem("DeepSkyBlue", 0xFF00BFFF);
        private static readonly ColorItem DodgerBlue = new ColorItem("DodgerBlue", 0xFF1E90FF);
        private static readonly ColorItem LightBlue = new ColorItem("LightBlue", 0xFFADD8E6);
        private static readonly ColorItem LightSeaGreen = new ColorItem("LightSeaGreen", 0xFF20B2AA);
        private static readonly ColorItem LightSkyBlue = new ColorItem("LightSkyBlue", 0xFF87CEFA);
        private static readonly ColorItem LightSteelBlue = new ColorItem("LightSteelBlue", 0xFFB0C4DE);
        private static readonly ColorItem Mauve = new ColorItem("Mauve", 0xFF76608A);
        private static readonly ColorItem MediumSlateBlue = new ColorItem("MediumSlateBlue", 0xFF7B68EE);
        private static readonly ColorItem RoyalBlue = new ColorItem("RoyalBlue", 0xFF4169E1);
        private static readonly ColorItem SlateBlue = new ColorItem("SlateBlue", 0xFF6A5ACD);
        private static readonly ColorItem SlateGray = new ColorItem("SlateGray", 0xFF708090);
        private static readonly ColorItem SteelBlue = new ColorItem("SteelBlue", 0xFF4682B4);
        private static readonly ColorItem Teal = new ColorItem("Teal", 0xFF008080);
        private static readonly ColorItem Turquoise = new ColorItem("Turquoise", 0xFF40E0D0);
        private static readonly ColorItem DarkGrey = new ColorItem("DarkGrey", 0xFFA9A9A9);
        private static readonly ColorItem LightGray = new ColorItem("LightGray", 0xFFD3D3D3);

        private static readonly ColorItem[] AllColors = { Yellow, BananaYellow, LaserLemon, Jasmine, Green, Emerald, GreenYellow, Lime, Chartreuse, LimeGreen, SpringGreen, LightGreen, MediumSeaGreen, MediumSpringGreen, Olive, SeaGreen, Red, OrangeRed, DarkOrange, Orange, ImperialRed, Maroon, Brown, Chocolate, Coral, Crimson, DarkSalmon, DeepPink, Firebrick, HotPink, IndianRed, LightCoral, LightPink, LightSalmon, Magenta, MediumVioletRed, Orchid, PaleVioletRed, Salmon, SandyBrown, Navy, Indigo, MidnightBlue, Blue, Purple, BlueViolet, CornflowerBlue, Cyan, DarkCyan, DarkSlateBlue, DeepSkyBlue, DodgerBlue, LightBlue, LightSeaGreen, LightSkyBlue, LightSteelBlue, Mauve, MediumSlateBlue, RoyalBlue, SlateBlue, SlateGray, SteelBlue, Teal, Turquoise, DarkGrey, LightGray };
        private GridView _gridView;

        protected override void OnApplyTemplate()
        {
            if (_gridView != null)
            {
                _gridView.SelectionChanged -= OnSelectionChanged;
                _gridView = null;
            }
            _gridView = GetTemplateChild(GridViewPartName) as GridView;
            if (_gridView != null)
            {
                _gridView.ItemsSource = AllColors;
                ColorChanged();
                _gridView.SelectionChanged += OnSelectionChanged;
            }
        }

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            "Color", typeof(Color), typeof(ColorPickerControl), new PropertyMetadata(Red, ColorChangedCallback));

        private static void ColorChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            ColorPickerControl colorPickerControl = dependencyObject as ColorPickerControl;
            if (colorPickerControl != null)
            {
                colorPickerControl.ColorChanged();
            }
        }

        private void ColorChanged()
        {
            if (_gridView == null)
            {
                return;
            }
            Color color = Color;
            ColorItem existingColor = AllColors.FirstOrDefault(c => c.Color == color);
            if (existingColor != null)
            {
                _gridView.SelectedItem = existingColor;
            }
        }

        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            ColorItem colorItem = _gridView.SelectedItem as ColorItem;
            ICommand colorSelectedCommand = ColorSelectedCommand;

            if (colorItem != null)
            {
                Color = colorItem.Color;
                OnColorSelected(colorItem);
                if (colorSelectedCommand != null && colorSelectedCommand.CanExecute(colorItem.Color))
                {
                    colorSelectedCommand.Execute(colorItem.Color);
                }
            }
        }

        public event EventHandler<EventArgs> ColorSelected;

        protected virtual void OnColorSelected(ColorItem colorItem)
        {
            ColorSelected?.Invoke(this, EventArgs.Empty);
        }

        protected class ColorItem
        {
            public ColorItem(string name, uint colorValue)
            {
                Name = name;
                Color = Color.FromArgb(
                    (byte)((colorValue & 0xFF000000) >> 24),
                    (byte)((colorValue & 0x00FF0000) >> 16),
                    (byte)((colorValue & 0x0000FF00) >> 8),
                    (byte)(colorValue & 0x000000FF)
                );
                ColorBrush = new SolidColorBrush(Color);
            }

            public Color Color { get; }

            public string Name { get; }

            public Brush ColorBrush { get; }
        }
    }
}