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
