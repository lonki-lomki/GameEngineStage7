using System.Drawing;

namespace GameEngineStage7.Utils
{
    /// <summary>
    /// Рисование панели (выпуклой или вогнутой)
    /// </summary>
    public class Box
    {

        /// <summary>
        /// Запретить создание єкземпляра класса
        /// </summary>
        private Box()
        {
        }

        /// <summary>
        /// Рисование панели с заданными размерами и видом (нажата или отжата)
        /// </summary>
        /// <param name="width">ширина панели</param>
        /// <param name="height">высота панели</param>
        /// <param name="isEmboss">true - если панель вдавлена, false - если выпуклая</param>
        /// <returns>изображение панели</returns>
        public static Image GetBox(int width, int height, bool isEmboss)
        {
            Bitmap bmp = new Bitmap(width, height);
            Graphics gr = Graphics.FromImage(bmp);
            //Pen pen = new Pen(Color.LightGray, 1);
            Pen upColor;
            Pen downColor;
            Brush fillColor = Brushes.LightGray;
            int borderSize = 5;

            // Выбор вида панели
            if (isEmboss == true)
            {
                // нажата (вдавлена)
                upColor = Pens.Black;
                downColor = Pens.White;
            }
            else
            {
                // отжата (выпуклая)
                upColor = Pens.White;
                downColor = Pens.Black;
            }

            // Заливка фона
            gr.FillRectangle(fillColor, 0, 0, width, height);

            // Выделение бордюров
            for (int i = 0; i < borderSize; i++)
            {
                gr.DrawLine(upColor, i, i, width - i, i);
                gr.DrawLine(upColor, i, i, i, height - i);
                gr.DrawLine(downColor, i, height-i, width - i, height - i);
                gr.DrawLine(downColor, width - i, i, width - i, height - i);
            }

            return bmp;
        }

    }
}
