using GameEngineStage7.Core;
using GameEngineStage7.Entities;
using GameEngineStage7.Utils;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GameEngineStage7.Scenes
{
    /// <summary>
    /// Класс, описывающий сцену, на которой будет проходить основная игра
    /// </summary>
    public class GameScene : Scene
    {
        // Переменная для отслеживания состояния падения танков
        //private bool isFallingTanks = false;

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

            // Танки
            gd.tanks = new List<Tank>();

            // Создать танк игрока
            Tank tank = new Tank("Player", gd, GameData.TankTypes.Player, Color.ForestGreen);
            tank.SetPosition(300, 32);
            tank.SetLayer(2);
            tank.MaxPower = 1000;
            tank.Power = 200;
            tank.Angle = new Angle(60);
            tank.Name = "Player";
            tank.Landing();
            gd.tanks.Add(tank);
            // Добавить объект на сцену
            objects.Add(tank);

            gd.currentTank = tank;

            // Создать танк бота
            tank = new Tank("Bot", gd, GameData.TankTypes.Tosser, Color.Red);
            tank.SetPosition(900, 32);
            tank.SetLayer(2);
            tank.MaxPower = 1000;
            tank.Power = 200;
            tank.Angle = new Angle(60);
            tank.Name = "Bot";
            tank.Landing();
            gd.tanks.Add(tank);
            // Добавить объект на сцену
            objects.Add(tank);

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
                //gd.landshaft.LandFall(true);
                gd.currentTank.Landing2();
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
            bool flag;

            // GameOver
            if (gd.gameFlow == GameData.GameFlow.GameOver)
            {
                // Создать сцену GameOver
                GameOverScene gameover = new GameOverScene(GameData.GameState.GameOver, gd);
                gd.curScene = gameover;

                gd.curScene.Init();

                gd.sceneChange = true;
                return;
            }


            base.Update(delta);
            // Ландшафт - это отдельный объект и обрабатывается отдельно
            gd.landshaft.Update(delta);

            // Выстрел бота
            if (gd.currentTank.TankType != GameData.TankTypes.Player && gd.gameFlow == GameData.GameFlow.Aiming)
            {
                gd.currentTank.BotFire();
                // Сменить этап игрового цикла на выстрел и полет снаряда
                gd.gameFlow = GameData.GameFlow.Firing;
            }

            // Все танки упали, фиксируем урон от падения
            if (gd.gameFlow == GameData.GameFlow.TankCrash)
            {
                flag = false;
                foreach (Entity e in gd.curScene.objects.ToArray())
                {
                    if (e.GetType().Name == "Tank")
                    {
                        if (((Tank)e).FixDamage() == true)
                        {
                            // Танк уничтожен
                            flag = true;
                        }
                    }
                }
                // Перевод в следующий статус
                if (flag == true)
                {
                    // Танк взорвался
                    gd.gameFlow = GameData.GameFlow.Explosion;
                }
                else
                {
                    // Больше никто не взорвался, следующий выстрел
                    // Передать ход следующему танку
                    // TODO: КОСТЫЛЬ!!!! Надо переделать!!!
                    int idx = gd.tanks.IndexOf(gd.currentTank);
                    idx++;
                    if (idx >= gd.tanks.Count)
                    {
                        idx = 0;
                    }
                    gd.currentTank = gd.tanks[idx];
                    gd.gameFlow = GameData.GameFlow.Aiming;
                }
            }

            // Падение файлов после взрывов и осыпание земли обрабатывает здесь
            if (gd.gameFlow == GameData.GameFlow.Tankfall)
            {
                flag = false;
                // Падение танков должно быть с анимацией, поэтому растягиваем падение на несколько кадров
                foreach (Entity e in gd.curScene.objects)
                {
                    //gd.log.Write(e.GetType().Name);
                    if (e.GetType().Name == "Tank")
                    {
                        if (((Tank)e).Landing2() == true)
                        {
                            // Танк продолжает падение
                            flag = true;
                        }
                    }
                }
                if (flag == false)
                {
                    // Все танки упали, переходим в следующий статус
                    gd.gameFlow = GameData.GameFlow.TankCrash;
                }
            }

            // Посчитать количество живых танков.
            // TODO: КОСТЫЛЬ только для двух танков
            if (gd.gameFlow != GameData.GameFlow.TankExplosion)
            {
                flag = false;
                Tank tank = new Tank();
                foreach (Tank t in gd.tanks)
                {
                    if (t.IsDestroyed() == true)
                    {
                        flag = true;
                    }
                    else
                    {
                        tank = t;
                    }
                }
                if (flag == true)
                {
                    // Game Over. Победил танк tank
                    gd.currentTank = tank;
                    gd.gameFlow = GameData.GameFlow.TankExplosion;
                }
            }
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
