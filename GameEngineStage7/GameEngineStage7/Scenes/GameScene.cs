using GameEngineStage7.Core;
using GameEngineStage7.Entities;
using GameEngineStage7.Utils;
using System.Drawing;
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

            // Геометрия области рисования (камеры)
            int width = gd.clientRectangle.Width - CONFIG.START_X * 2;
            int height = gd.clientRectangle.Height - CONFIG.PANEL_HEIGHT - CONFIG.START_X * 2;

            //Cursor.Hide();

            // Загрузить ресурсы, необходимые для данной сцены
            gd.rm.Clear();

            //gd.worldImage = new Bitmap(CONFIG.WIND_WIDTH, CONFIG.WIND_HEIGHT, PixelFormat.Format32bppPArgb);
            //gd.worldImage = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            //gd.rm.AddElementAsImage("box", Box.GetBox(100, 100, true));

            // Создать и загружить фон для раунда
            gd.rm.AddElementAsImage("backofround", Gradient.GetImage(Color.Blue, Color.Yellow, width, height, 0));
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

            // Создать Землю!
            gd.landshaft = new Landshaft("landshaft", gd);
            gd.landshaft.SetLayer(1);
            gd.landshaft.SetPosition(0.0f, 0.0f);
            // Добавить объект на сцену
            //objects.Add(gd.landshaft);

            // Создать танк
            gd.currentTank = new Tank("tank", gd, GameData.TankTypes.Player, Color.ForestGreen);
            gd.currentTank.SetPosition(500, 32);
            gd.currentTank.SetLayer(2);
            gd.currentTank.MaxPower = 1000;
            gd.currentTank.Power = 200;
            gd.currentTank.Angle = new Angle(60);
            gd.currentTank.Name = "Username";
            //gd.currentTank.Color = Color.ForestGreen;
            gd.currentTank.Landing();
            //gd.tank.SetGravity(true);
            // Добавить объект на сцену
            objects.Add(gd.currentTank);


            // Запуск игрового процесса
            // Первый этап - Прицеливание
            gd.gameFlow = GameData.GameFlow.Aiming;
            





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

            // Остальные клавиши обрабатываем только во время прицеливания
            if (gd.gameFlow != GameData.GameFlow.Aiming)
            {
                return;
            }

            if (e.KeyCode == Keys.R)
            {
                gd.landshaft.LandFall(true);
            }

            // Клавиша PageUp - увеличение мощности выстрела
            if (e.KeyCode == Keys.PageUp)
            {
                gd.currentTank.Power += 25;
            }

            // Клавиша PageDown - уменьшение мощности выстрела
            if (e.KeyCode == Keys.PageDown)
            {
                gd.currentTank.Power -= 25;
            }

            // Клавиша Up - увеличение мощности выстрела
            if (e.KeyCode == Keys.Up)
            {
                gd.currentTank.Power += 5;
            }

            // Клавиша Down - уменьшение мощности выстрела
            if (e.KeyCode == Keys.Down)
            {
                gd.currentTank.Power -= 5;
            }

            // Клавиша Left - вращение дула против часовой стрелки
            if (e.KeyCode == Keys.Left)
            {
                gd.currentTank.Angle.Value += 5;
            }

            // Клавиша Right - вращение дула по часовой стрелке
            if (e.KeyCode == Keys.Right)
            {
                gd.currentTank.Angle.Value -= 5;
            }

            // Пробел или Enter - произвести выстрел текущим оружием с текущими углом и мощностью
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space)
            {
                gd.currentTank.Fire();
                // Сменить этап игрового цикла на выстрел и полет снаряда
                gd.gameFlow = GameData.GameFlow.Firing;
            }

        }

        public override void Render(Graphics g)
        {
            base.Render(g);

            // Вывод игровой панели
            gd.gamePanel.Render(g);

            // Вывод того, что видит камера
            gd.camera.Render(g);

            //g.DrawImage(gd.bmp, 32, 32);

        }

        public override void Update(int delta)
        {
            base.Update(delta);
            gd.landshaft.Update(delta);
        }

        public override void MouseDown(object sender, MouseEventArgs e)
        {
            base.MouseDown(sender, e);
            /*
            if (e.Button == MouseButtons.Left)
            {
                Explosion expl = new Explosion("explosion", gd);
                expl.SetPosition(e.X - gd.camera.Geometry.X * 2, e.Y - gd.camera.Geometry.Y * 2);   // перевести из координат экрана в координаты камеры минус размер панели (этот размер будет добавлен при отрисовке)
                expl.SetLayer(1);
                objects.Add(expl);
                gd.world.Add(expl);
            }
            if (e.Button == MouseButtons.Right)
            {

            }
            */
        }
    }
}
