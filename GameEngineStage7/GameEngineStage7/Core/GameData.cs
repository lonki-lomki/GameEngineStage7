using GameEngineStage7.Utils;
using System.Collections.Generic;
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

        public HashSet<Keys> PressedKeys = new HashSet<Keys>();

        /////////////////////////////////////////////////////////

        public Scene curScene = null;

        public bool sceneChange = false;


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
