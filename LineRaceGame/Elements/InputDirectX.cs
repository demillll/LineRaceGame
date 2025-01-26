using System;
using SharpDX;
using SharpDX.DirectInput;

namespace LineRaceGame
{
	public class InputDirectX : IDisposable
	{
		private DirectInput _directInput;
		private Keyboard _keyboard;
		private KeyboardState _keyboardState;
		public KeyboardState KeyboardState => _keyboardState;

		private bool _keyboardUpdated;
		public bool KeyboardUpdated => _keyboardUpdated;

		private bool _keyboardAcquired;

		public InputDirectX(IntPtr hwnd)
		{
			_directInput = new DirectInput();

			// Инициализация клавиатуры
			_keyboard = new Keyboard(_directInput)
			{
				Properties = { BufferSize = 16 }
			};

			AcquireKeyboard();
			_keyboardState = new KeyboardState();
		}

		private void AcquireKeyboard()
		{
			try
			{
				_keyboard.Acquire();
				_keyboardAcquired = true;
			}
			catch (SharpDXException e)
			{
				if (e.ResultCode.Failure)
					_keyboardAcquired = false;
			}
		}

		public void UpdateKeyboardState()
		{
			if (!_keyboardAcquired) AcquireKeyboard();

			try
			{
				_keyboard.GetCurrentState(ref _keyboardState);
				_keyboardUpdated = true;
			}
			catch (SharpDXException e)
			{
				if (e.ResultCode == ResultCode.InputLost || e.ResultCode == ResultCode.NotAcquired)
					_keyboardAcquired = false;

				_keyboardUpdated = false;
			}
		}

		public void Dispose()
		{
			Utilities.Dispose(ref _keyboard);
			Utilities.Dispose(ref _directInput);
		}
	}
}
