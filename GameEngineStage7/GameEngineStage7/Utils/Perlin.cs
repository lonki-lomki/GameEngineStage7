using System;
using System.Drawing;

namespace GameEngineStage7.Utils
{
    /// <summary>
    /// Реализация алгоритма генерации шума Перлина
    /// </summary>
    public class Perlin
    {
        /// <summary>
        /// Ширина результирующего изображения
        /// </summary>
        int width;

        /// <summary>
        /// Высота результирующего изображения
        /// </summary>
        int height;

        /// <summary>
        /// Количество октав
        /// </summary>
        int octaves;

        /// <summary>
        /// Массив октав - составляющих игорового изображения
        /// </summary>
        float[][,] perlinMap;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="width">ширина массива</param>
        /// <param name="height">высота массива</param>
        /// <param name="octaves">количество октав</param>
        public Perlin(int width, int height, int octaves)
        {
            this.width = width;
            this.height = height;
            this.octaves = octaves;

            // Случайное число для получения разных вариантов ландшафта
            Random rnd = new Random();

            perlinMap = new float[octaves][,];

            // Цикл генерации октав 
            for (int i = 0; i < octaves; i++)
            {
                perlinMap[i] = new float[width / (int)(Math.Pow(2, i)), height / (int)(Math.Pow(2, i))];
                PerlinNoiseInitialize(i, rnd.Next(1000));
                PerlinSmoothing(i);
            }
        }

        /// <summary>
        /// Генератор псевдо-случайных чисел для генерации шума Перлина
        /// </summary>
        /// <param name="x">Координата Х</param>
        /// <param name="y">Координата У</param>
        /// <returns></returns>
        private float Noise(int x, int y)
        {
            int n = x + y * 57;
            n = (n << 13) ^ n;
            return Math.Abs((1.0f - ((n * (n * n * 15731 + 789221) + 1376312589) & 0x7fffffff) / 1073741824.0f));
        }

        /// <summary>
        /// Заполнение матрицы
        /// </summary>
        /// <param name="octave">номер октавы</param>
        /// <param name="rnd">Случайное число</param>
        private void PerlinNoiseInitialize(int octave, int rnd)
        {
            for (int i = 0; i < width / (int)(Math.Pow(2, octave)); i++)
                for (int j = 0; j < height / (int)(Math.Pow(2, octave)); j++)
                    perlinMap[octave][i, j] = Noise(i + rnd, j + rnd);
        }

        /// <summary>
        /// Сглаживание данных в сгенерированной матрице.
        /// Используется метод сглаживания по квадрату
        /// </summary>
        /// <param name="octave">номер октавы</param>
        private void PerlinSmoothing(int octave)
        {
            for (int i = 0; i < width / (int)(Math.Pow(2, octave)) - 1; i++)
                for (int j = 0; j < height / (int)(Math.Pow(2, octave)) - 1; j++)
                {
                    {
                        float round = (perlinMap[octave][i, j] + perlinMap[octave][i + 1, j] + perlinMap[octave][i + 1, j + 1] + perlinMap[octave][i, j + 1]) / 4;
                        perlinMap[octave][i, j] = (perlinMap[octave][i, j] + round) / 2;
                        perlinMap[octave][i + 1, j] = (perlinMap[octave][i + 1, j] + round) / 2;
                        perlinMap[octave][i, j + 1] = (perlinMap[octave][i, j + 1] + round) / 2;
                        perlinMap[octave][i + 1, j + 1] = (perlinMap[octave][i + 1, j + 1] + round) / 2;
                    }
                }
        }

        /// <summary>
        /// Получить финишную матрицу с картой высот (случайные значения в диапазоне от 0 до 1)
        /// </summary>
        /// <returns>двумерный массив случайных значений, распределённых по правилу шума Перлина</returns>
        public float[,] getMap()
        {
            float[,] finish = new float[width, height];
            float max = 0.0f;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    finish[i, j] = 0.0f;
                    // Цикл суммирования октав с учётом весовых коэффициентов
                    for (int n = 0; n < octaves; n++)
                    {
                        finish[i, j] += perlinMap[n][i / (int)Math.Pow(2, n), j / (int)Math.Pow(2, n)] / (float)Math.Pow(2, octaves - n - 1);
                    }
                    // Определить максимальное значение в массиве
                    if (finish[i, j] > max)
                    {
                        max = finish[i, j];
                    }
                }
            }

            // Нормализовать значения финального массива, если надо
            if (max > 1.0f)
            {
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        finish[i, j] /= max;
                    }
                }
            }

            // Сгладить значения результата
            for (int i = 0; i < width - 1; i++)
                for (int j = 0; j < height - 1; j++)
                {
                    {
                        float round = (finish[i, j] + finish[i + 1, j] + finish[i + 1, j + 1] + finish[i, j + 1]) / 4;
                        finish[i, j] = (finish[i, j] + round) / 2;
                        finish[i + 1, j] = (finish[i + 1, j] + round) / 2;
                        finish[i, j + 1] = (finish[i, j + 1] + round) / 2;
                        finish[i + 1, j + 1] = (finish[i + 1, j + 1] + round) / 2;
                    }
                }

            return finish;
        }

        /// <summary>
        /// Записать на диск одну из октав
        /// </summary>
        /// <param name="octave">номер октавы</param>
        public void WriteToFileOctave(int octave)
        {
            Color c = new Color();

            Bitmap bmp = new Bitmap(width / (int)(Math.Pow(2, octave)), height / (int)(Math.Pow(2, octave)));
            for (int i = 0; i < width / (int)(Math.Pow(2, octave)); i++)
            {
                for (int j = 0; j < height / (int)(Math.Pow(2, octave)); j++)
                {
                    c = Color.FromArgb((int)(perlinMap[octave][i, j] * 255), (int)(perlinMap[octave][i, j] * 255), (int)(perlinMap[octave][i, j] * 255));
                    bmp.SetPixel(i, j, c);
                }
            }
            bmp.Save("out" + octave + ".bmp");
        }

        /// <summary>
        /// Собрать все октавы в один выходной файл
        /// </summary>
        public void WriteToFile()
        {
            float[,] finish;

            finish = getMap();

            // Вывести результат в файл
            Bitmap bmp = new Bitmap(width, height);
            Color c = new Color();

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    c = Color.FromArgb((int)(finish[i, j] * 255), (int)(finish[i, j] * 255), (int)(finish[i, j] * 255));
                    bmp.SetPixel(i, j, c);
                }
            }
            bmp.Save("out.bmp");
        }
    }
}
