using System;
using System.Text;
using System.Windows.Input;

namespace Loader.Helpers
{
	public class KeySequence
	{
		private readonly Key[] keys;

		private readonly ModifierKeys modifiers;

		public Key[] Keys
		{
			get
			{
				return this.keys;
			}
		}

		public ModifierKeys Modifiers
		{
			get
			{
				return this.modifiers;
			}
		}

		public KeySequence(ModifierKeys modifiers, params Key[] keys)
		{
			if (keys == null)
			{
				throw new ArgumentNullException("keys");
			}
			if ((int)keys.Length < 1)
			{
				throw new ArgumentException("At least 1 key should be provided", "keys");
			}
			this.keys = new Key[(int)keys.Length];
			keys.CopyTo(this.keys, 0);
			this.modifiers = modifiers;
		}

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			if (this.modifiers != ModifierKeys.None)
			{
				if ((this.modifiers & ModifierKeys.Control) != ModifierKeys.None)
				{
					builder.Append("Ctrl+");
				}
				if ((this.modifiers & ModifierKeys.Alt) != ModifierKeys.None)
				{
					builder.Append("Alt+");
				}
				if ((this.modifiers & ModifierKeys.Shift) != ModifierKeys.None)
				{
					builder.Append("Shift+");
				}
				if ((this.modifiers & ModifierKeys.Windows) != ModifierKeys.None)
				{
					builder.Append("Windows+");
				}
			}
			builder.Append(this.keys[0]);
			for (int i = 1; i < (int)this.keys.Length; i++)
			{
				builder.Append(string.Concat("+", this.keys[i]));
			}
			return builder.ToString();
		}
	}
}