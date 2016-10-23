using Loader.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Windows.Input;

namespace Loader.Helpers.Converters
{
	public class MultiKeyGestureConverter : TypeConverter
	{
		public readonly static MultiKeyGestureConverter DefaultConverter;

		private readonly static KeyConverter keyConverter;

		private readonly static ModifierKeysConverter modifierKeysConverter;

		static MultiKeyGestureConverter()
		{
			MultiKeyGestureConverter.DefaultConverter = new MultiKeyGestureConverter();
			MultiKeyGestureConverter.keyConverter = new KeyConverter();
			MultiKeyGestureConverter.modifierKeysConverter = new ModifierKeysConverter();
		}

		public MultiKeyGestureConverter()
		{
		}

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			ModifierKeys currentModifier;
			string str = value as string;
			if (string.IsNullOrEmpty(str))
			{
				throw base.GetConvertFromException(value);
			}
			string[] strArrays = str.Split(new char[] { ',' });
			List<KeySequence> keySequences = new List<KeySequence>();
			string[] strArrays1 = strArrays;
			for (int num = 0; num < (int)strArrays1.Length; num++)
			{
				string str1 = strArrays1[num];
				ModifierKeys modifier = ModifierKeys.None;
				List<Key> keys = new List<Key>();
				string[] keyStrings = str1.Split(new char[] { '+' });
				int modifiersCount = 0;
				while (true)
				{
					string str2 = keyStrings[modifiersCount];
					string temp = str2;
					if (str2 == null || !MultiKeyGestureConverter.TryGetModifierKeys(temp.Trim(), out currentModifier))
					{
						break;
					}
					modifiersCount++;
					modifier = modifier | currentModifier;
				}
				for (int i = modifiersCount; i < (int)keyStrings.Length; i++)
				{
					string keyString = keyStrings[i];
					if (keyString != null)
					{
						Key key = (Key)MultiKeyGestureConverter.keyConverter.ConvertFrom(keyString.Trim());
						keys.Add(key);
					}
				}
				keySequences.Add(new KeySequence(modifier, keys.ToArray()));
			}
			return new MultiKeyGesture(str, keySequences.ToArray());
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				MultiKeyGesture gesture = value as MultiKeyGesture;
				if (gesture != null)
				{
					StringBuilder builder = new StringBuilder();
					for (int i = 0; i < (int)gesture.KeySequences.Length; i++)
					{
						if (i > 0)
						{
							builder.Append(", ");
						}
						KeySequence sequence = gesture.KeySequences[i];
						if (sequence.Modifiers != ModifierKeys.None)
						{
							builder.Append((string)MultiKeyGestureConverter.modifierKeysConverter.ConvertTo(context, culture, sequence.Modifiers, destinationType));
							builder.Append("+");
						}
						builder.Append((string)MultiKeyGestureConverter.keyConverter.ConvertTo(context, culture, sequence.Keys[0], destinationType));
						for (int j = 1; j < (int)sequence.Keys.Length; j++)
						{
							builder.Append("+");
							builder.Append((string)MultiKeyGestureConverter.keyConverter.ConvertTo(context, culture, sequence.Keys[0], destinationType));
						}
					}
					return builder.ToString();
				}
			}
			throw base.GetConvertToException(value, destinationType);
		}

		private static bool TryGetModifierKeys(string str, out ModifierKeys modifier)
		{
			string upper = str.ToUpper();
			if (upper == "CONTROL" || upper == "CTRL")
			{
				modifier = ModifierKeys.Control;
				return true;
			}
			if (upper == "SHIFT")
			{
				modifier = ModifierKeys.Shift;
				return true;
			}
			if (upper == "ALT")
			{
				modifier = ModifierKeys.Alt;
				return true;
			}
			if (upper == "WINDOWS" || upper == "WIN")
			{
				modifier = ModifierKeys.Windows;
				return true;
			}
			modifier = ModifierKeys.None;
			return false;
		}
	}
}