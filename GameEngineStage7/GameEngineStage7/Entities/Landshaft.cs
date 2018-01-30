using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using GameEngineStage7.Core;
using GameEngineStage7.Utils;

namespace GameEngineStage7.Entities
{
    /// <summary>
    /// Класс для работы с ландшафтом
    /// </summary>
    public class Landshaft : Entity
    {
        /// <summary>
        /// Количество пикселей в изображении
        /// </summary>
        private int pixelCount;

        /// <summary>
        /// 
        /// </summary>
        private Rectangle rect;

        /// <summary>
        /// Глубина цвета для пикселя (32 = ARGB)
        /// </summary>
        private int colorDepth = 32;

        /// <summary>
        /// Количество байт на цвет (32/4)
        /// </summary>
        private int step = 4;

        private int width;

        private int height;

        /// <summary>
        /// Массив для хранения байтов изображения
        /// </summary>
        byte[] pixels;


        public Landshaft()
        {
        }

        public Landshaft(string id) : base(id)
        {
        }

        public Landshaft(string id, GameData gd) : base(id, gd)
        {

            width = gd.camera.Geometry.Width;
            height = gd.camera.Geometry.Height;

            // Создание ландшафта с использованием шума Перлина
            Perlin p = new Perlin(CONFIG.PERLIN_MATRIX_SIZE, CONFIG.PERLIN_MATRIX_SIZE, CONFIG.PERLIN_OCTAVES);
            // получить карту высот со значениями из диапазона [0, 1]
            float[,] map = p.getMap();
            int[] landshaft = new int[CONFIG.LANDSHAFT_LEN];
            Random rnd = new Random();

            // Выбрать строку, из которой брать данные
            int randomLine = rnd.Next(CONFIG.PERLIN_MATRIX_SIZE);
            // Перегрузить данные в отдельный массив с нормализацией по высоте области отображения
            for (int i = 0; i < CONFIG.LANDSHAFT_LEN; i++)
            {
                landshaft[i] = (int)(map[randomLine, i] * 2 * height / 3);
            }

            // Создать замкнутую ломаную линию по данным из массива точек
            gd.gp = new GraphicsPath();
            Point pt, pt1, pt2 = new Point(0, 0);
            int tile_size = width / (CONFIG.LANDSHAFT_LEN - 2);

            for (int i = 0; i < CONFIG.LANDSHAFT_LEN - 1; i++)
            {
                pt1 = new Point(i * tile_size, height - landshaft[i]);
                pt2 = new Point((i + 1) * tile_size, height - landshaft[i + 1]);
                gd.gp.AddLine(pt1, pt2);
            }

            // Нарисовать еще две прямые линии
            pt = new Point(CONFIG.LANDSHAFT_LEN * tile_size, height);
            gd.gp.AddLine(pt2, pt);
            pt1 = new Point(0, height);
            gd.gp.AddLine(pt, pt1);

            // Замкнуть котур
            gd.gp.CloseFigure();

            // Отрисовка полученной фигуры в графический файл
            gd.bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            Graphics gg = Graphics.FromImage(gd.bmp);
            gg.FillPath(Brushes.Black, gd.gp);
            gg.Dispose();
            
            SetImage(gd.bmp);

            // Настройка параметров для хранения изображения как массива байтов
            pixelCount = width * height;
            pixels = new byte[pixelCount * step];
            rect = new Rectangle(0, 0, width, height);

        }

        public override void Render(Graphics g)
        {
            // Отрисовка объектов внутри картинки ландшафта
            Graphics gg = Graphics.FromImage(GetImage());
            // Установить атрибут, который позволяет использовать прозрачность при рисовании на картинке
            gg.CompositingMode = CompositingMode.SourceCopy;

            foreach (Entity ent in gd.world.objects)
            {
                if (ent.GetLayer() == 1)
                {
                    ent.Render(gg);
                }
            }

            // Отрисовка изображения
            base.Render(g);
            
        }

        public override void Update(int delta)
        {
            base.Update(delta);

            Reload();
        }

        /// <summary>
        /// Тестирование попиксельной перезагрузки карты
        /// </summary>
        public void Reload()
        {
            // Преобразование изображения в пиксельный массив
            Bitmap bmp = new Bitmap(GetImage());
            BitmapData bitmapData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);
            IntPtr iptr = bitmapData.Scan0;
            Marshal.Copy(iptr, pixels, 0, pixels.Length);

            /*
            // Сохранить массив пикселей в файл
            MemoryStream ms = new MemoryStream(pixels);
            FileStream fs = new FileStream("pixels.bin", FileMode.OpenOrCreate);
            ms.CopyTo(fs);
            fs.Flush();
            fs.Close();
            */


            //RemoveColor(Color.FromArgb(255, 255, 1, 1));

            byte a1, r1, g1, b1;
            byte a2, r2, g2, b2;

            // Цикл по всем столбцам исходного изображения
            for (int i = 0; i < width; i++)
            {
                // Цикл по пикселям в столбце
                for (int j = height - 1; j >= 0; j--)
                {
                    //Color c1 = GetPixel(i, j);  // Верхий пиксель
                    //Color c2 = GetPixel(i, j + 1);  // Нижний пиксель
                    ////GetPixel(i, j, out r1, out g1, out b1, out a1);  // Верхий пиксель
                    int index = (j * width + i) * step;
                    a1 = pixels[index + 3];
                    //r1 = pixels[index + 2];
                    //g1 = pixels[index + 1];
                    //b1 = pixels[index];
                    ////GetPixel(i, j + 1, out r2, out g2, out b2, out a2);  // Нижний пиксель
                    int index2 = ((j + 1) * width + i) * step;
                    if (index2 >= 0 && (index2+step) < pixels.Length)
                    {
                        a2 = pixels[index2 + 3];
                        //r2 = pixels[index2 + 2];
                        //g2 = pixels[index2 + 1];
                        //b2 = pixels[index2];
                    } else
                    {
                        a2 = 255;
                        //r2 = 255;
                        //g2 = 255;
                        //b2 = 255;
                    }
                    // Проверить, что ниже верхнего пикселя - пусто
                    //if (c1.A == 255 && c2.A != 255)
                    if (a1 == 255 && a2 != 255)
                    {
                        // Переместить пиксель на строку вниз, вместо себя оставить пустой пиксель
                        //SetPixel(i, j, 0, 0, 0, 0);
                        pixels[index + 3] = 0;
                        //pixels[index + 2] = 0;
                        //pixels[index + 1] = 0;
                        //pixels[index] = 0;
                        //SetPixel(i, j + 1, r1, g1, b1, a1);
                        pixels[index2 + 3] = a1;
                        //pixels[index2 + 2] = r1;
                        //pixels[index2 + 1] = g1;
                        //pixels[index2] = b1;
                    }
                }
            }
            


            // Обратное преобразование пиксельного массива в изображение
            Marshal.Copy(pixels, 0, iptr, pixels.Length);
            bmp.UnlockBits(bitmapData);

            SetImage(bmp);
        }

        /// <summary>
        /// Получить значение цвета пикселя из массива пикселей
        /// </summary>
        /// <param name="x">координата Х пикселя</param>
        /// <param name="y">координата У пикселя</param>
        /// <returns>цвет пикселя по данным координатам</returns>
        public Color GetPixel(int x, int y)
        {
            int index = (y * width + x) * step;
            // Проверки выхода за пределы изображения
            if ((index+step) >= pixels.Length)
            {
                return Color.White;
            }
            if (index < 0)
            {
                return Color.White;
            }

            return Color.FromArgb(pixels[index + 3], pixels[index + 2], pixels[index + 1], pixels[index]);
        }

        public void GetPixel(int x, int y, out byte r, out byte g, out byte b, out byte a)
        {
            int index = (y * width + x) * step;
            // Проверки выхода за пределы изображения
            if ((index + step) >= pixels.Length)
            {
                r = 255;
                g = 255;
                b = 255;
                a = 255;
                return;
            }
            if (index < 0)
            {
                r = 255;
                g = 255;
                b = 255;
                a = 255;
                return;
            }
            a = pixels[index + 3];
            r = pixels[index + 2];
            g = pixels[index + 1];
            b = pixels[index];
            return;
        }

        /// <summary>
        /// Установить цвет пикселя в массиве пикселей
        /// </summary>
        /// <param name="x">координата Х пикселя</param>
        /// <param name="y">координата Х пикселя</param>
        /// <param name="r">red компонента</param>
        /// <param name="g">green компонента</param>
        /// <param name="b">blue компонента</param>
        /// <param name="a">alpha компонента</param>
        public void SetPixel(int x, int y, byte r, byte g, byte b, byte a)
        {
            int index = (y * width + x) * step;
            // Проверки выхода за пределы изображения
            if ((index + step) >= pixels.Length)
            {
                return;
            }
            if (index < 0)
            {
                return;
            }

            pixels[index] = b;
            pixels[index + 1] = g;
            pixels[index + 2] = r;
            pixels[index + 3] = a;
        }

        /// <summary>
        /// Удалить из массива пикселей пиксель с данным цветом (заменить на прозрачный пиксель)
        /// </summary>
        /// <param name="color">цвет удаляемых пикселей</param>
        public void RemoveColor(Color color)
        {
            for (int i = 0; i < pixels.Length; i+=step)
            {
                if (pixels[i] == color.B && pixels[i+1] == color.G && pixels[i+2] == color.R)
                {
                    pixels[i] = 0;
                    pixels[i + 1] = 0;
                    pixels[i + 2] = 0;
                    pixels[i + 3] = 0;
                }
            }
        }

    }
}
