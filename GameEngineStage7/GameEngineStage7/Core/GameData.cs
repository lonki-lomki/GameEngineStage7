using GameEngineStage7.Entities;
using GameEngineStage7.Utils;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GameEngineStage7.Core
{
    public class GameData
    {
        // Набор значений для определения текущего состояния игры
        public enum GameState
        {
            NotStarted,
            MainMenu,
            Level,
            LevelWin,
            GameWin,
            GameOver
        }

        // Набор значений для определения типа танка (игрока)
        public enum TankTypes
        {
            Player      = 0,
            Moron       = 1,
            Shooter     = 2,
            Poolshark   = 3,
            Tosser      = 4,
            Chooser     = 5,
            Spoiler     = 6,
            Cyborg      = 7,
            Unknown     = 8
        }

        // Порядок этапов игрового цикла
        public enum GameFlow : int
        {
            Aiming = 0,     // прицеливание
            Firing = 1,     // выстрел (полет снаряда)
            Explosion = 2,  // взрыв
            Damage = 3,     // нанесение урона танкам
            Landfall = 4,   // падение кусков земли
            Tankfall = 5,   // падение танков с нанесением урона
            TankCrash = 6,  // уничтожение подбитых танков
            // далее в цикле обработка пунктов 2, 3, 4, 5, 6 пока состояние не стабилизируется
            TankExplosion = 7,  // взрыв танка
            GameOver = 8    // окончание игры
        }

        private static GameData instance;

        public PhysWorld world;

        public Logger log;

        public ResourceManager rm;

        public Image backgroundImage;

        public HashSet<Keys> PressedKeys = new HashSet<Keys>();

        public Bitmap worldImage;   // Буфер для отображения мира (общая карта, из которой камера будет отображать некоторую часть)

        public ImageObject backOfRound;

        public GamePanel gamePanel;

        public Rectangle clientRectangle;

        /////////////////////////////////////////////////////////

        public Scene curScene = null;

        public bool sceneChange = false;

        public Camera camera;

        public Landshaft landshaft;

        // TODO: временно
        public GraphicsPath gp;
        public Bitmap bmp;
        //

        public List<Tank> tanks;

        public Tank currentTank;

        public GameFlow gameFlow;

        // Запретить new
        private GameData()
        {
        }

        /// <summary>
        /// Получить единственный экземпляр данного класса
        /// </summary>
        public static GameData Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameData();
                }
                return instance;
            }
        }

    }
}
