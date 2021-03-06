﻿namespace GameEngineStage7.Core
{
    class CONFIG
    {
        // Размер окна программы
        public static readonly int WIND_WIDTH = 1024;
        public static readonly int WIND_HEIGHT = 600;

        // Размер тайла
        public static readonly int TILE_SIZE = 48;

        public static readonly int PANEL_HEIGHT = 30; // Высота верхней панели

        // Координата начала игрового поля
        public static readonly int START_X = 2;
        public static readonly int START_Y = PANEL_HEIGHT + 2;

        public static readonly int LANDSHAFT_LEN = 16;  // Количество вершин ландшафта
        public static readonly int PERLIN_MATRIX_SIZE = 128;    // Размер матрицы для генерации шума Перлина
        public static readonly int PERLIN_OCTAVES = 1;  // Количество октав для генерации шума Перлина

        //public static readonly int VIEWPORT_WIDTH = 1010;
        //public static readonly int VIEWPORT_HEIGHT = 534;

        public static readonly float PHYS_GRAVITY = 500.0f; //1000.0f;//1.1f; //5.0f; // Гравитация для физ. движка

        public static readonly float MAX_ENG_POWER = 5.0f;  // Максимальная мощность двигателя

        public static readonly int FALL_DAMAGE = 10;    // Повреждение танка при падении на 1 пиксель
        public static readonly int MISSILE_DAMAGE = 500;    // Повреждение танка в эпицентре взрыва

    }
}
