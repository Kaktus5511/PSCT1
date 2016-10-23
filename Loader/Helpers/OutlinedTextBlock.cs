using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;

namespace Loader.Helpers
{
	[ContentProperty("Text")]
	public class OutlinedTextBlock : FrameworkElement
	{
		public readonly static DependencyProperty FillProperty;

		public readonly static DependencyProperty StrokeProperty;

		public readonly static DependencyProperty StrokeThicknessProperty;

		public readonly static DependencyProperty FontFamilyProperty;

		public readonly static DependencyProperty FontSizeProperty;

		public readonly static DependencyProperty FontStretchProperty;

		public readonly static DependencyProperty FontStyleProperty;

		public readonly static DependencyProperty FontWeightProperty;

		public readonly static DependencyProperty TextProperty;

		public readonly static DependencyProperty TextAlignmentProperty;

		public readonly static DependencyProperty TextDecorationsProperty;

		public readonly static DependencyProperty TextTrimmingProperty;

		public readonly static DependencyProperty TextWrappingProperty;

		private FormattedText formattedText;

		private Geometry textGeometry;

		public Brush Fill
		{
			get
			{
				return (Brush)base.GetValue(OutlinedTextBlock.FillProperty);
			}
			set
			{
				base.SetValue(OutlinedTextBlock.FillProperty, value);
			}
		}

		public System.Windows.Media.FontFamily FontFamily
		{
			get
			{
				return (System.Windows.Media.FontFamily)base.GetValue(OutlinedTextBlock.FontFamilyProperty);
			}
			set
			{
				base.SetValue(OutlinedTextBlock.FontFamilyProperty, value);
			}
		}

		[TypeConverter(typeof(FontSizeConverter))]
		public double FontSize
		{
			get
			{
				return (double)base.GetValue(OutlinedTextBlock.FontSizeProperty);
			}
			set
			{
				base.SetValue(OutlinedTextBlock.FontSizeProperty, value);
			}
		}

		public System.Windows.FontStretch FontStretch
		{
			get
			{
				return (System.Windows.FontStretch)base.GetValue(OutlinedTextBlock.FontStretchProperty);
			}
			set
			{
				base.SetValue(OutlinedTextBlock.FontStretchProperty, value);
			}
		}

		public System.Windows.FontStyle FontStyle
		{
			get
			{
				return (System.Windows.FontStyle)base.GetValue(OutlinedTextBlock.FontStyleProperty);
			}
			set
			{
				base.SetValue(OutlinedTextBlock.FontStyleProperty, value);
			}
		}

		public System.Windows.FontWeight FontWeight
		{
			get
			{
				return (System.Windows.FontWeight)base.GetValue(OutlinedTextBlock.FontWeightProperty);
			}
			set
			{
				base.SetValue(OutlinedTextBlock.FontWeightProperty, value);
			}
		}

		public Brush Stroke
		{
			get
			{
				return (Brush)base.GetValue(OutlinedTextBlock.StrokeProperty);
			}
			set
			{
				base.SetValue(OutlinedTextBlock.StrokeProperty, value);
			}
		}

		public double StrokeThickness
		{
			get
			{
				return (double)base.GetValue(OutlinedTextBlock.StrokeThicknessProperty);
			}
			set
			{
				base.SetValue(OutlinedTextBlock.StrokeThicknessProperty, value);
			}
		}

		public string Text
		{
			get
			{
				return (string)base.GetValue(OutlinedTextBlock.TextProperty);
			}
			set
			{
				base.SetValue(OutlinedTextBlock.TextProperty, value);
			}
		}

		public System.Windows.TextAlignment TextAlignment
		{
			get
			{
				return (System.Windows.TextAlignment)base.GetValue(OutlinedTextBlock.TextAlignmentProperty);
			}
			set
			{
				base.SetValue(OutlinedTextBlock.TextAlignmentProperty, value);
			}
		}

		public TextDecorationCollection TextDecorations
		{
			get
			{
				return (TextDecorationCollection)base.GetValue(OutlinedTextBlock.TextDecorationsProperty);
			}
			set
			{
				base.SetValue(OutlinedTextBlock.TextDecorationsProperty, value);
			}
		}

		public System.Windows.TextTrimming TextTrimming
		{
			get
			{
				return (System.Windows.TextTrimming)base.GetValue(OutlinedTextBlock.TextTrimmingProperty);
			}
			set
			{
				base.SetValue(OutlinedTextBlock.TextTrimmingProperty, value);
			}
		}

		public System.Windows.TextWrapping TextWrapping
		{
			get
			{
				return (System.Windows.TextWrapping)base.GetValue(OutlinedTextBlock.TextWrappingProperty);
			}
			set
			{
				base.SetValue(OutlinedTextBlock.TextWrappingProperty, value);
			}
		}

		static OutlinedTextBlock()
		{
			OutlinedTextBlock.FillProperty = DependencyProperty.Register("Fill", typeof(Brush), typeof(OutlinedTextBlock), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender));
			OutlinedTextBlock.StrokeProperty = DependencyProperty.Register("Stroke", typeof(Brush), typeof(OutlinedTextBlock), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender));
			OutlinedTextBlock.StrokeThicknessProperty = DependencyProperty.Register("StrokeThickness", typeof(double), typeof(OutlinedTextBlock), new FrameworkPropertyMetadata((object)1, FrameworkPropertyMetadataOptions.AffectsRender));
			OutlinedTextBlock.FontFamilyProperty = TextElement.FontFamilyProperty.AddOwner(typeof(OutlinedTextBlock), new FrameworkPropertyMetadata(new PropertyChangedCallback(OutlinedTextBlock.OnFormattedTextUpdated)));
			OutlinedTextBlock.FontSizeProperty = TextElement.FontSizeProperty.AddOwner(typeof(OutlinedTextBlock), new FrameworkPropertyMetadata(new PropertyChangedCallback(OutlinedTextBlock.OnFormattedTextUpdated)));
			OutlinedTextBlock.FontStretchProperty = TextElement.FontStretchProperty.AddOwner(typeof(OutlinedTextBlock), new FrameworkPropertyMetadata(new PropertyChangedCallback(OutlinedTextBlock.OnFormattedTextUpdated)));
			OutlinedTextBlock.FontStyleProperty = TextElement.FontStyleProperty.AddOwner(typeof(OutlinedTextBlock), new FrameworkPropertyMetadata(new PropertyChangedCallback(OutlinedTextBlock.OnFormattedTextUpdated)));
			OutlinedTextBlock.FontWeightProperty = TextElement.FontWeightProperty.AddOwner(typeof(OutlinedTextBlock), new FrameworkPropertyMetadata(new PropertyChangedCallback(OutlinedTextBlock.OnFormattedTextUpdated)));
			OutlinedTextBlock.TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(OutlinedTextBlock), new FrameworkPropertyMetadata(new PropertyChangedCallback(OutlinedTextBlock.OnFormattedTextInvalidated)));
			OutlinedTextBlock.TextAlignmentProperty = DependencyProperty.Register("TextAlignment", typeof(System.Windows.TextAlignment), typeof(OutlinedTextBlock), new FrameworkPropertyMetadata(new PropertyChangedCallback(OutlinedTextBlock.OnFormattedTextUpdated)));
			OutlinedTextBlock.TextDecorationsProperty = DependencyProperty.Register("TextDecorations", typeof(TextDecorationCollection), typeof(OutlinedTextBlock), new FrameworkPropertyMetadata(new PropertyChangedCallback(OutlinedTextBlock.OnFormattedTextUpdated)));
			OutlinedTextBlock.TextTrimmingProperty = DependencyProperty.Register("TextTrimming", typeof(System.Windows.TextTrimming), typeof(OutlinedTextBlock), new FrameworkPropertyMetadata(new PropertyChangedCallback(OutlinedTextBlock.OnFormattedTextUpdated)));
			OutlinedTextBlock.TextWrappingProperty = DependencyProperty.Register("TextWrapping", typeof(System.Windows.TextWrapping), typeof(OutlinedTextBlock), new FrameworkPropertyMetadata((object)System.Windows.TextWrapping.NoWrap, new PropertyChangedCallback(OutlinedTextBlock.OnFormattedTextUpdated)));
		}

		public OutlinedTextBlock()
		{
			this.TextDecorations = new TextDecorationCollection();
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			this.EnsureFormattedText();
			this.formattedText.MaxTextWidth = finalSize.Width;
			this.formattedText.MaxTextHeight = finalSize.Height;
			this.textGeometry = null;
			return finalSize;
		}

		private void EnsureFormattedText()
		{
			if (this.formattedText != null || this.Text == null)
			{
				return;
			}
			this.formattedText = new FormattedText(this.Text, CultureInfo.CurrentUICulture, base.FlowDirection, new Typeface(this.FontFamily, this.FontStyle, this.FontWeight, FontStretches.Normal), this.FontSize, Brushes.Black);
			this.UpdateFormattedText();
		}

		private void EnsureGeometry()
		{
			if (this.textGeometry != null)
			{
				return;
			}
			this.EnsureFormattedText();
			this.textGeometry = this.formattedText.BuildGeometry(new Point(0, 0));
		}

		protected override Size MeasureOverride(Size availableSize)
		{
			this.EnsureFormattedText();
			this.formattedText.MaxTextWidth = Math.Min(3579139, availableSize.Width);
			this.formattedText.MaxTextHeight = Math.Max(0.0001, availableSize.Height);
			return new Size(this.formattedText.Width, this.formattedText.Height);
		}

		private static void OnFormattedTextInvalidated(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			OutlinedTextBlock outlinedTextBlock = (OutlinedTextBlock)dependencyObject;
			outlinedTextBlock.formattedText = null;
			outlinedTextBlock.textGeometry = null;
			outlinedTextBlock.InvalidateMeasure();
			outlinedTextBlock.InvalidateVisual();
		}

		private static void OnFormattedTextUpdated(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			OutlinedTextBlock outlinedTextBlock = (OutlinedTextBlock)dependencyObject;
			outlinedTextBlock.UpdateFormattedText();
			outlinedTextBlock.textGeometry = null;
			outlinedTextBlock.InvalidateMeasure();
			outlinedTextBlock.InvalidateVisual();
		}

		protected override void OnRender(DrawingContext drawingContext)
		{
			this.EnsureGeometry();
			drawingContext.DrawGeometry(this.Fill, new Pen(this.Stroke, this.StrokeThickness), this.textGeometry);
		}

		private void UpdateFormattedText()
		{
			if (this.formattedText == null)
			{
				return;
			}
			this.formattedText.MaxLineCount = (this.TextWrapping == System.Windows.TextWrapping.NoWrap ? 1 : 2147483647);
			this.formattedText.TextAlignment = this.TextAlignment;
			this.formattedText.Trimming = this.TextTrimming;
			this.formattedText.SetFontSize(this.FontSize);
			this.formattedText.SetFontStyle(this.FontStyle);
			this.formattedText.SetFontWeight(this.FontWeight);
			this.formattedText.SetFontFamily(this.FontFamily);
			this.formattedText.SetFontStretch(this.FontStretch);
			this.formattedText.SetTextDecorations(this.TextDecorations);
		}
	}
}