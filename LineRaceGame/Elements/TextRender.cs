using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;

namespace LineRaceGame
{
	public class TextRender
	{
		public TextFormat textFormat { get; set; }
		public Brush Brush { get; private set; }
		private SharpDX.DirectWrite.Factory writeFactory = new SharpDX.DirectWrite.Factory();

		public TextRender(RenderTarget renderTarget, int size, ParagraphAlignment paragraphAlignment, TextAlignment textAlignment, Color color)
		{
			textFormat = new TextFormat(writeFactory, "Calibri", size);
			textFormat.ParagraphAlignment = paragraphAlignment;
			textFormat.TextAlignment = textAlignment;

			// Преобразование System.Drawing.Color в RawColor4
			var rawColor = new RawColor4(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);

			// Создание кисти с преобразованным цветом
			Brush = new SolidColorBrush(renderTarget, rawColor);
		}
	}
}
