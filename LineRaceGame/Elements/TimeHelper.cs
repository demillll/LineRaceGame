using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace LineRaceGame
{
	public static class TimeHelper
	{
		// Таймер
		private static Stopwatch _watch;

		// Счетчик кадров
		private static int _counter = 0;
		// Количество кадров за прошлую секунду
		private static int _fps = 0;
		public static int FPS { get => _fps; }

		// Момент времени при прошлом обновлении значения FPS
		private static long _previousFPSMeasurementTime;

		// Количество тиков на момент прошлого кадра
		private static long _previousTicks;

		// Текущее время в секундах
		private static float _time;
		public static float Time { get => _time; }

		// Сколько времени прошло с прошлого кадра
		private static float _dT;
		public static float dT { get => _dT; }

		// В конструкторе создаем экземпляр таймера и выполняем сброс
		static TimeHelper()
		{
			_watch = new Stopwatch();
			Reset();
		}

		// Обновление подсчитываемых значений
		// Должен вызываться в начале каждого кадра
		public static void Update()
		{
			// Текущее значение счетчика тиков
			long ticks = _watch.Elapsed.Ticks;
			// Вычисляем текущее время и интервал между текущим и прошлым кадрами
			_time = (float)ticks / TimeSpan.TicksPerSecond;
			_dT = (float)(ticks - _previousTicks) / TimeSpan.TicksPerSecond;
			// Запоминаем текущее значение счетчика тиков для вычислений в будущем кадре
			_previousTicks = ticks;

			// Инкремент счетчика кадров
			_counter++;
			// Если истекла секунда, то обновляем значение FPS и фиксируем момент времени для отсчета следующей секунды
			if (_watch.ElapsedMilliseconds - _previousFPSMeasurementTime >= 1000)
			{
				_fps = _counter;
				_counter = 0;
				_previousFPSMeasurementTime = _watch.ElapsedMilliseconds;
			}
		}

		// Сброс таймера и счетчиков
		public static void Reset()
		{
			_watch.Reset();
			_counter = 0;
			_fps = 0;
			_watch.Start();
			_previousFPSMeasurementTime = _watch.ElapsedMilliseconds;
			_previousTicks = _watch.Elapsed.Ticks;
		}
	}
}