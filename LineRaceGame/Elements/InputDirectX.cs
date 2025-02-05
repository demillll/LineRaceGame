using System;
using SharpDX;
using SharpDX.DirectInput;

namespace LineRaceGame
{
	public class InputDirectX : IDisposable
	{// Экземпляр объекта "прямого ввода"
		public DirectInput _directInput;

		// Поля и свойства, связанные с клавиатурой
		private Keyboard _keyboard;

		private KeyboardState _keyboardState;
		public KeyboardState KeyboardState { get => _keyboardState; }

		private bool _keyboardUpdated;
		public bool KeyboardUpdated { get => _keyboardUpdated; }

		private bool _keyboardAcquired;

		// В конструкторе создаем все объекты и пробуем получить доступ к устройствам
		public InputDirectX(IntPtr hwnd)
		{
			_directInput = new DirectInput();

			_keyboard = new Keyboard(_directInput);
			_keyboard.Properties.BufferSize = 16;
			AcquireKeyboard();
			_keyboardState = new KeyboardState();
		}

		// Получение доступа к клавиатуре
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


		// Обновление состояния клавиатуры
		public void UpdateKeyboardState()
		{
			// Если доступ не был получен, пробуем здесь
			if (!_keyboardAcquired) AcquireKeyboard();

			// Пробуем обновить состояние
			ResultDescriptor resultCode = ResultCode.Ok;
			try
			{
				_keyboard.GetCurrentState(ref _keyboardState);
				// Успех
				_keyboardUpdated = true;
			}
			catch (SharpDXException e)
			{
				resultCode = e.Descriptor;
				// Отказ
				_keyboardUpdated = false;
			}

			// В большинстве случаев отказ из-за потери фокуса ввода
			// Устанавливаем соответствующий флаг, чтобы в следующем кадре попытаться получить доступ
			if (resultCode == ResultCode.InputLost || resultCode == ResultCode.NotAcquired)
				_keyboardAcquired = false;
		}

		// Освобождение выделенных нам неуправляемых ресурсов
		public void Dispose()
		{
			Utilities.Dispose(ref _keyboard);
			Utilities.Dispose(ref _directInput);
		}
	}
}
