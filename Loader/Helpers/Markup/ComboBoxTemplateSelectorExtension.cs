using Loader.Helpers;
using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Loader.Helpers.Markup
{
	public class ComboBoxTemplateSelectorExtension : MarkupExtension
	{
		public DataTemplate DropdownItemsTemplate
		{
			get;
			set;
		}

		public DataTemplateSelector DropdownItemsTemplateSelector
		{
			get;
			set;
		}

		public DataTemplate SelectedItemTemplate
		{
			get;
			set;
		}

		public DataTemplateSelector SelectedItemTemplateSelector
		{
			get;
			set;
		}

		public ComboBoxTemplateSelectorExtension(DataTemplate dropdown, DataTemplate selected)
		{
			this.DropdownItemsTemplate = dropdown;
			this.SelectedItemTemplate = selected;
		}

		public ComboBoxTemplateSelectorExtension()
		{
		}

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return new ComboBoxTemplateSelector()
			{
				SelectedItemTemplate = this.SelectedItemTemplate,
				SelectedItemTemplateSelector = this.SelectedItemTemplateSelector,
				DropdownItemsTemplate = this.DropdownItemsTemplate,
				DropdownItemsTemplateSelector = this.DropdownItemsTemplateSelector
			};
		}
	}
}