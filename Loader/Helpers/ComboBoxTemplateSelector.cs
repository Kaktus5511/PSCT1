using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Loader.Helpers
{
	public class ComboBoxTemplateSelector : DataTemplateSelector
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

		public ComboBoxTemplateSelector()
		{
		}

		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			DataTemplate dropdownItemsTemplate;
			DependencyObject parent = container;
			while (parent != null && !(parent is ComboBoxItem) && !(parent is ComboBox))
			{
				parent = VisualTreeHelper.GetParent(parent);
			}
			if (parent is ComboBoxItem)
			{
				dropdownItemsTemplate = this.DropdownItemsTemplate;
				if (dropdownItemsTemplate == null)
				{
					DataTemplateSelector dropdownItemsTemplateSelector = this.DropdownItemsTemplateSelector;
					if (dropdownItemsTemplateSelector == null)
					{
						return null;
					}
					dropdownItemsTemplate = dropdownItemsTemplateSelector.SelectTemplate(item, container);
				}
			}
			else
			{
				dropdownItemsTemplate = this.SelectedItemTemplate;
				if (dropdownItemsTemplate == null)
				{
					DataTemplateSelector selectedItemTemplateSelector = this.SelectedItemTemplateSelector;
					if (selectedItemTemplateSelector == null)
					{
						return null;
					}
					return selectedItemTemplateSelector.SelectTemplate(item, container);
				}
			}
			return dropdownItemsTemplate;
		}
	}
}