using Loader.Helpers.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace Loader.Helpers
{
	public class MultiKeyBinding : InputBinding
	{
		[TypeConverter(typeof(MultiKeyGestureConverter))]
		public override InputGesture Gesture
		{
			get
			{
				return base.Gesture;
			}
			set
			{
				if (!(value is Loader.Helpers.MultiKeyGesture))
				{
					throw new ArgumentException(string.Concat(new object[] { "The type ", value.GetType(), " is not a valid ", typeof(Loader.Helpers.MultiKeyGesture), " type" }), "value");
				}
				base.Gesture = value;
			}
		}

		public IEnumerable<KeySequence> KeySequences
		{
			get
			{
				return this.MultiKeyGesture.KeySequences;
			}
			set
			{
				base.Gesture = new Loader.Helpers.MultiKeyGesture(value.ToArray<KeySequence>());
			}
		}

		private Loader.Helpers.MultiKeyGesture MultiKeyGesture
		{
			get
			{
				return base.Gesture as Loader.Helpers.MultiKeyGesture;
			}
		}

		public MultiKeyBinding()
		{
			base.Gesture = new Loader.Helpers.MultiKeyGesture(new KeySequence[] { new KeySequence(ModifierKeys.None, new Key[1]) });
		}

		public MultiKeyBinding(ICommand command, Loader.Helpers.MultiKeyGesture gesture) : base(command, gesture)
		{
			base.Gesture = new Loader.Helpers.MultiKeyGesture(new KeySequence[] { new KeySequence(ModifierKeys.None, new Key[1]) });
		}

		public MultiKeyBinding(ICommand command, params KeySequence[] sequences) : base(command, new Loader.Helpers.MultiKeyGesture(sequences))
		{
			base.Gesture = new Loader.Helpers.MultiKeyGesture(new KeySequence[] { new KeySequence(ModifierKeys.None, new Key[1]) });
		}
	}
}