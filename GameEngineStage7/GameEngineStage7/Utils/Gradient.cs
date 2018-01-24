using System.Drawing;

namespace GameEngineStage7.Utils
{
    /// <summary>
    /// Класс формирования текстуры в виде градиента (плавный переход из одного цвета в другой)
    /// </summary>
    public class Gradient
    {
        /// <summary>
        /// Запретить создание єкземпляра класса
        /// </summary>
        private Gradient()
        {
        }

        /// <summary>
        /// Создание нового градиента
        /// </summary>
        /// <param name="c1">первый цвет градиента</param>
        /// <param name="c2">второй цвет градиента</param>
        /// <param name="w">ширина градиента</param>
        /// <param name="h">высота градиента</param>
        /// <param name="angle">угол поворота градиента (только 2 значения: 0 - горизонтальный градиент, 90 - вертикальный градиент)</param>
        public static Image GetImage(Color c1, Color c2, int w, int h, int angle)
        {
            Bitmap bmp = new Bitmap(w, h);
            Graphics gr = Graphics.FromImage(bmp);
            Pen pen = new Pen(Color.LightGray, 1);

            // Разница в цветах по компонентам
            float diffR = c2.R - c1.R;
            float diffG = c2.G - c1.G;
            float diffB = c2.B - c1.B;

            // Выбор направления градиента
            if (angle == 0)
            {
                // Цикл по строкам прямоугольной текстуры
                for (int i = 0; i < h; i++)
                {
                    pen.Color = Color.FromArgb(c1.R + (int)(diffR * i / h), c1.G + (int)(diffG * i / h), c1.B + (int)(diffB * i / h));
                    gr.DrawLine(pen, 0, i, w, i);
                }
            }
            if (angle == 90)
            {
                // Цикл по строкам прямоугольной текстуры
                for (int i = 0; i < w; i++)
                {
                    pen.Color = Color.FromArgb(c1.R + (int)(diffR * i / w), c1.G + (int)(diffG * i / w), c1.B + (int)(diffB * i / w));
                    gr.DrawLine(pen, i, 0, i, h);
                }
            }

            return bmp;
        }
    }
}
