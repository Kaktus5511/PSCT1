using Caliburn.Micro;
using Loader.Helpers.Converters;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Loader.Helpers
{
	public class KeyTrigger : TriggerBase<UIElement>
	{
		public readonly static DependencyProperty KeyProperty;

		public readonly static DependencyProperty ModifiersProperty;

		public System.Windows.Input.Key Key
		{
			get
			{
				return (System.Windows.Input.Key)base.GetValue(KeyTrigger.KeyProperty);
			}
			set
			{
				base.SetValue(KeyTrigger.KeyProperty, value);
			}
		}

		public ModifierKeys Modifiers
		{
			get
			{
				return (ModifierKeys)base.GetValue(KeyTrigger.ModifiersProperty);
			}
			set
			{
				base.SetValue(KeyTrigger.ModifiersProperty, value);
			}
		}

		static KeyTrigger()
		{
			KeyTrigger.KeyProperty = DependencyProperty.Register("Key", typeof(System.Windows.Input.Key), typeof(KeyTrigger), null);
			KeyTrigger.ModifiersProperty = DependencyProperty.Register("Modifiers", typeof(ModifierKeys), typeof(KeyTrigger), null);
		}

		public KeyTrigger()
		{
		}

		private static ModifierKeys GetActualModifiers(System.Windows.Input.Key key, ModifierKeys modifiers)
		{
			if (key == System.Windows.Input.Key.LeftCtrl || key == System.Windows.Input.Key.RightCtrl)
			{
				modifiers = modifiers | ModifierKeys.Control;
				return modifiers;
			}
			if (key == System.Windows.Input.Key.LeftAlt || key == System.Windows.Input.Key.RightAlt)
			{
				modifiers = modifiers | ModifierKeys.Alt;
				return modifiers;
			}
			if (key == System.Windows.Input.Key.LeftShift || key == System.Windows.Input.Key.RightShift)
			{
				modifiers = modifiers | ModifierKeys.Shift;
			}
			return modifiers;
		}

		private void OnAssociatedObjectKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == this.Key && Keyboard.Modifiers == KeyTrigger.GetActualModifiers(e.Key, this.Modifiers))
			{
				base.InvokeActions(e);
			}
		}

		protected override void OnAttached()
		{
			base.OnAttached();
			base.AssociatedObject.KeyDown += new KeyEventHandler(this.OnAssociatedObjectKeyDown);
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
			base.AssociatedObject.KeyDown -= new KeyEventHandler(this.OnAssociatedObjectKeyDown);
		}

		public static void Register()
		{
			Func<DependencyObject, string, System.Windows.Interactivity.TriggerBase> createTrigger = Parser.CreateTrigger;
			Parser.CreateTrigger = (DependencyObject target, string triggerText) => {
				if (triggerText == null)
				{
					return ConventionManager.GetElementConvention(target.GetType()).CreateTrigger();
				}
				string[] splits = triggerText.Replace("[", string.Empty).Replace("]", string.Empty).Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
				if (splits[0] == "Key")
				{
					return new KeyTrigger()
					{
						Key = (System.Windows.Input.Key)Enum.Parse(typeof(System.Windows.Input.Key), splits[1], true)
					};
				}
				if (splits[0] != "Gesture")
				{
					return createTrigger(target, triggerText);
				}
				MultiKeyGesture mkg = (MultiKeyGesture)(new MultiKeyGestureConverter()).ConvertFrom(splits[1]);
				return new KeyTrigger()
				{
					Modifiers = mkg.KeySequences[0].Modifiers,
					Key = mkg.KeySequences[0].Keys[0]
				};
			};
		}
	}
}