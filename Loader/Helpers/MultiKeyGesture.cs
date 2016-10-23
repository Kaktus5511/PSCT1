using Loader.Helpers.Converters;
using System;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Loader.Helpers
{
	[TypeConverter(typeof(MultiKeyGestureConverter))]
	public class MultiKeyGesture : InputGesture
	{
		private readonly static TimeSpan maximumDelay;

		private readonly string displayString;

		private readonly KeySequence[] keySequences;

		private int currentKeyIndex;

		private int currentSequenceIndex;

		private DateTime lastKeyPress;

		public string DisplayString
		{
			get
			{
				return this.displayString;
			}
		}

		public KeySequence[] KeySequences
		{
			get
			{
				return this.keySequences;
			}
		}

		static MultiKeyGesture()
		{
			MultiKeyGesture.maximumDelay = TimeSpan.FromSeconds(1);
		}

		public MultiKeyGesture(params KeySequence[] sequences) : this(MultiKeyGesture.GetKeySequencesString(sequences), sequences)
		{
		}

		public MultiKeyGesture(string displayString, params KeySequence[] sequences)
		{
			if (sequences == null)
			{
				throw new ArgumentNullException("sequences");
			}
			if (sequences.Length == 0)
			{
				throw new ArgumentException("At least one sequence must be specified.", "sequences");
			}
			this.displayString = displayString;
			this.keySequences = new KeySequence[(int)sequences.Length];
			sequences.CopyTo(this.keySequences, 0);
		}

		private static string GetKeySequencesString(params KeySequence[] sequences)
		{
			if (sequences == null)
			{
				throw new ArgumentNullException("sequences");
			}
			if (sequences.Length == 0)
			{
				throw new ArgumentException("At least one sequence must be specified.", "sequences");
			}
			StringBuilder builder = new StringBuilder();
			builder.Append(sequences[0].ToString());
			for (int i = 1; i < (int)sequences.Length; i++)
			{
				builder.Append(string.Concat(", ", sequences[i]));
			}
			return builder.ToString();
		}

		private static bool IsDefinedKey(Key key)
		{
			if (key < Key.None)
			{
				return false;
			}
			return key <= Key.OemClear;
		}

		private static bool IsModifierKey(Key key)
		{
			if (key == Key.LeftCtrl || key == Key.RightCtrl || key == Key.LeftShift || key == Key.RightShift || key == Key.LeftAlt || key == Key.RightAlt || key == Key.LWin)
			{
				return true;
			}
			return key == Key.RWin;
		}

		public override bool Matches(object targetElement, InputEventArgs inputEventArgs)
		{
			KeyEventArgs args = inputEventArgs as KeyEventArgs;
			if (args == null || args.IsRepeat)
			{
				return false;
			}
			Key key = (args.Key != Key.System ? args.Key : args.SystemKey);
			if (!MultiKeyGesture.IsDefinedKey(key))
			{
				return false;
			}
			KeySequence currentSequence = this.keySequences[this.currentSequenceIndex];
			Key currentKey = currentSequence.Keys[this.currentKeyIndex];
			if (MultiKeyGesture.IsModifierKey(key))
			{
				return false;
			}
			if (this.currentSequenceIndex != 0 && (DateTime.Now - this.lastKeyPress) > MultiKeyGesture.maximumDelay)
			{
				this.ResetState();
				return false;
			}
			if (currentSequence.Modifiers != args.KeyboardDevice.Modifiers)
			{
				this.ResetState();
				return false;
			}
			if (currentKey != key)
			{
				this.ResetState();
				return false;
			}
			this.currentKeyIndex = this.currentKeyIndex + 1;
			if (this.currentKeyIndex == (int)this.keySequences[this.currentSequenceIndex].Keys.Length)
			{
				this.currentSequenceIndex = this.currentSequenceIndex + 1;
				this.currentKeyIndex = 0;
			}
			if (this.currentSequenceIndex == (int)this.keySequences.Length)
			{
				this.ResetState();
				inputEventArgs.Handled = true;
				return true;
			}
			this.lastKeyPress = DateTime.Now;
			inputEventArgs.Handled = true;
			return false;
		}

		private void ResetState()
		{
			this.currentSequenceIndex = 0;
			this.currentKeyIndex = 0;
		}
	}
}