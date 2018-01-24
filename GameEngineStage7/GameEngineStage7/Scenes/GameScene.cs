using GameEngineStage7.Core;
using GameEngineStage7.Entities;
using GameEngineStage7.Utils;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace GameEngineStage7.Scenes
{
    /// <summary>
    /// Класс, описывающий сцену, на которой будет проходить основная игра
    /// </summary>
    public class GameScene : Scene
    {

        public GameScene(GameData.GameState ID, GameData gd) : base(ID, gd)
        {

        }

        /// <summary>
        /// Инициализация сцены
        /// </summary>
        public override void Init()
        {
            base.Init();

            //Cursor.Hide();

            // Загрузить ресурсы, необходимые для данной сцены
            gd.rm.Clear();

            gd.worldImage = new Bitmap(CONFIG.WIND_WIDTH, CONFIG.WIND_HEIGHT, PixelFormat.Format32bppPArgb);

            //gd.rm.AddElementAsImage("box", Box.GetBox(100, 100, true));

            // Создать и загружить фон для раунда
            gd.rm.AddElementAsImage("backofround", Gradient.GetImage(Color.Blue, Color.Green, gd.clientRectangle.Width, CONFIG.WIND_HEIGHT, 0));
            gd.backOfRound = new ImageObject("backofround", gd);
            gd.backOfRound.SetImage(gd.rm.GetImage("backofround"));
            gd.backOfRound.SetLayer(0);
            gd.backOfRound.SetPosition(0.0f, 0.0f);
            // Добавить объект на сцену
            objects.Add(gd.backOfRound);

            // Создать игровую панель
            gd.gamePanel = new GamePanel("gamePanel", gd);
            gd.gamePanel.SetImage(Box.GetBox(gd.clientRectangle.Width-1 , CONFIG.PANEL_HEIGHT, false));
            gd.gamePanel.SetPosition(0.0f, 0.0f);



            // Создать объект - тайловую карту и загрузить данные из файла
            //gd.map = Map.Load(@"Resources\Level_001.tmx");

            // Загрузить отдельные спрайты в менеджер ресурсов как самостоятельные изображения (для ускорения отображения)
            //Tileset ts = gd.map.Tilesets["Tiles"];

            //int tileCount = ts.FirstTileID;
            // Загрузить базовое изображение
            //gd.rm.AddElementAsImage(ts.Image, @"Resources\" + ts.Image);

            // Создать объект - карту спрайтов
            //gd.ss = new SpriteSheet(gd.rm.GetImage(ts.Image), ts.TileWidth, ts.TileHeight, ts.Spacing, ts.Margin);

            // Цикл по спрайтам внутри матрицы спрайтов
            /*
            for (int j = 0; j < ts.ImageHeight / ts.TileHeight; j++)
            {
                for (int i = 0; i < ts.ImageWidth / ts.TileWidth; i++)
                {
                    // Добавить этот спрайт в хранилище и наименованием tileset-<порядковый номер спрайта>
                    // TODO: как быть, если наборов тайлов несколько? Как вести нумерацию?
                    gd.rm.AddElementAsImage("tileset-" + tileCount, gd.ss.GetSprite(i, j));
                    tileCount++;
                }
            }
            */

            // Добавить объект на сцену
            //objects.Add(gd.tmo);

            // Загрузить спрайт игрока
            //gd.rm.AddElementAsImage("Player1", @"Resources\player.png");

            // Создать объект ГГ           
            //gd.player = new Player("Player1", gd);
            //gd.player.SetPosition(120.0f, 120.0f);
            //gd.player.SetImage(gd.rm.GetImage("Player1"));
            //gd.player.SetLayer(2);
            //gd.player.SetGravity(false);
            //gd.player.SetEngine(false);

            // Добавить игрока на сцену
            //objects.Add(gd.player);


        }

        public override void KeyDown(object sender, KeyEventArgs e)
        {
            base.KeyDown(sender, e);

            if (e.KeyCode == Keys.Escape)
            {
                Application.Exit();
            }

            /*
            // Временно: сдвиг камеры над игровым полем
            if (e.KeyCode == Keys.W)
            {
                Rectangle rect = tmo.ViewPort;
                tmo.ViewPort = new Rectangle(rect.X, rect.Y - 5, rect.Width, rect.Height);
            }
            if (e.KeyCode == Keys.S)
            {
                Rectangle rect = tmo.ViewPort;
                tmo.ViewPort = new Rectangle(rect.X, rect.Y + 5, rect.Width, rect.Height);
            }
            if (e.KeyCode == Keys.A)
            {
                Rectangle rect = tmo.ViewPort;
                tmo.ViewPort = new Rectangle(rect.X - 5, rect.Y, rect.Width, rect.Height);
            }
            if (e.KeyCode == Keys.D)
            {
                Rectangle rect = tmo.ViewPort;
                tmo.ViewPort = new Rectangle(rect.X + 5, rect.Y, rect.Width, rect.Height);
            }
            */
            /*
            if (e.KeyCode == Keys.D)
            {
                PointF p = gd.player.GetPosition();
                gd.player.SetPosition(p.X + 5, p.Y);
            }
            */
        }

        public override void Render(Graphics g)
        {
            base.Render(g);

            // Вывод игровой панели
            gd.gamePanel.Render(g);

            // Вывод того, что виидит камера
            gd.camera.Render(g);

        }

        public override void Update(int delta)
        {
            base.Update(delta);

            // Проверка нажатых клавиш
            if (gd.PressedKeys.Contains(Keys.Escape))
            {
                Application.Exit();
            }

            PointF velocity = new PointF(0.0f, 0.0f);

            if (gd.PressedKeys.Contains(Keys.D))
            {
                velocity.X += 50.0f;
            }
            if (gd.PressedKeys.Contains(Keys.A))
            {
                velocity.X -= 50.0f;
            }
            if (gd.PressedKeys.Contains(Keys.S))
            {
                velocity.Y += 50.0f;
            }
            if (gd.PressedKeys.Contains(Keys.W))
            {
                velocity.Y -= 50.0f;
            }

            // Применить к игроку посчитанную скорость
            //gd.player.SetVelocity(velocity.X, velocity.Y);

            // Сдвинуть камеру, чтобы ГГ был по центру экрана
            //Rectangle rect = gd.tmo.ViewPort;
            //gd.tmo.ViewPort = new Rectangle((int)gd.player.GetPosition().X - rect.Width / 2 - CONFIG.START_X, (int)gd.player.GetPosition().Y - rect.Height / 2 - CONFIG.START_Y, rect.Width, rect.Height);

        }
    }
}
