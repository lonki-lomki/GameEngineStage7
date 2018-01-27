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
        public Landshaft()
        {
        }

        public Landshaft(string id) : base(id)
        {
        }

        public Landshaft(string id, GameData gd) : base(id, gd)
        {

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
                landshaft[i] = (int)(map[randomLine, i] * 2 * gd.camera.Geometry.Height / 3);
            }

            // Создать замкнутую ломаную линию по данным из массива точек
            gd.gp = new GraphicsPath();
            Point pt, pt1, pt2 = new Point(0, 0);
            int tile_size = gd.camera.Geometry.Width / (CONFIG.LANDSHAFT_LEN - 2);

            for (int i = 0; i < CONFIG.LANDSHAFT_LEN - 1; i++)
            {
                pt1 = new Point(i * tile_size, gd.camera.Geometry.Height - landshaft[i]);
                pt2 = new Point((i + 1) * tile_size, gd.camera.Geometry.Height - landshaft[i + 1]);
                gd.gp.AddLine(pt1, pt2);
            }

            // Нарисовать еще две прямые линии
            pt = new Point(CONFIG.LANDSHAFT_LEN * tile_size, gd.camera.Geometry.Height);
            gd.gp.AddLine(pt2, pt);
            pt1 = new Point(0, gd.camera.Geometry.Height);
            gd.gp.AddLine(pt, pt1);

            // Замкнуть котур
            gd.gp.CloseFigure();

            // Отрисовка полученной фигуры в графический файл
            gd.bmp = new Bitmap(gd.camera.Geometry.Width, gd.camera.Geometry.Height, PixelFormat.Format32bppArgb);
            Graphics gg = Graphics.FromImage(gd.bmp);
            //gg.Clear(Color.Transparent);
            gg.FillPath(Brushes.Black, gd.gp);
            //gd.bmp.MakeTransparent(Color.White);
            // TODO: чтение попиксельно изображения (для последующей обработки удобнее сделать развертку ???????)
            //Color c = gd.bmp.GetPixel(0, 0);
            //Color cc = Color.FromArgb(254, 0, 0, 0);
            gg.Dispose();

            //gd.bmp.Save("test.png", ImageFormat.Png);

            SetImage(gd.bmp);


        }

        public override void Render(Graphics g)
        {
            base.Render(g);
        }

        public override void Update(int delta)
        {
            base.Update(delta);
        }

        /// <summary>
        /// Тестирование попиксельной перезагрузки карты
        /// </summary>
        public void Reload()
        {
            Bitmap bmp = new Bitmap(GetImage());
            int pixelCount = bmp.Width * bmp.Height;
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            int depth = 32;
            BitmapData bitmapData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);
            int step = depth / 8;
            byte[] pixels = new byte[pixelCount * step];
            IntPtr iptr = bitmapData.Scan0;
            Marshal.Copy(iptr, pixels, 0, pixels.Length);

            MemoryStream ms = new MemoryStream(pixels);

            FileStream fs = new FileStream("pixels.bin", FileMode.OpenOrCreate);
            ms.CopyTo(fs);
            fs.Flush();
            fs.Close();


            /*
            // Конвертация изображения в байтовый массив
            // !!! Получился файл формата PNG!!!
            ImageConverter converter = new ImageConverter();
            byte[] bytes = (byte[])converter.ConvertTo(GetImage(), typeof(byte[]));

            //bytes[0] = 255;
            //bytes[1] = 255;
            //bytes[2] = 255;
            //bytes[3] = 255;

            // Обратная конвертация из байтового массива в изображение
            Bitmap bmp;
            MemoryStream ms = new MemoryStream(bytes);

            FileStream fs = new FileStream("bytes.bin", FileMode.OpenOrCreate);
            ms.CopyTo(fs);
            fs.Flush();
            fs.Close();

            //bmp = new Bitmap(ms);

            //SetImage(bmp);

            ms.Close();
            */

            //Bitmap bmp = new Bitmap(GetImage());
            /*
            // Цикл по всем столбцам исходного изображения
            for (int i = 0; i < bmp.Width; i++)
            {
                // Цикл по пикселям в столбце
                for (int j = bmp.Height-1; j >= 0; j--)
                {
                    Color c = bmp.GetPixel(i, j);
                    
                    if (c == Color.Black)
                    {
                        c = Color.White;
                    };
                    if (c == Color.White)
                    {
                        c = Color.Black;
                    }
                    
                    //bmp.SetPixel(i, j, Color.White);
                }
            }

            SetImage(bmp);
            */
        }
    }
}
