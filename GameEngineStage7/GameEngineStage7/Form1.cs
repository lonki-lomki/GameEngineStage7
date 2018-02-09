using GameEngineStage7.Core;
using GameEngineStage7.Scenes;
using GameEngineStage7.Utils;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GameEngineStage7
{
    public partial class Form1 : Form
    {

        // TODO: все текстовые строки перед началом игры преобразовать в картинки и положить в менеджер ресурсов
        // TODO: прозрачность работает, но Render для Entity должно учитывать коорднаты камеры
        // TODO: попробовать формат изображения Format8bppIndexed - должно работать в 4 раза быстрее

        // TODO: сделать трехцветный градиент, радиальный градиент
        // TODO: сделать текстуру по алгоритму Перлина (для заливки взрыва Nuke)
        // TODO: сделать класс Button для отобрежения визуальной кнопки

        // TODO: запрет повторного выстрела, если предыдущий еще не окончен
        // TODO: настроить параметры физики
        // TODO: осыпание земли по окончанию раунда
        // TODO: падение танка по окончанию раунда
        // TODO: при взрыве пройти по списку танков и передать количество пунктов урона
        // TODO: когда снаряд достигает самой нижней строки экрана и в пределах экрана по координате Х - активировать взрыв
        // TODO: поворот корпуса танка при переходе через 90 градусов

        

        private string old_title;	// Оригинальный текст в заголовке окна

        private Timer timer = new Timer();

        // Счётчик количества тиков
        private long tickCount = 0;
        // Для определения длины интервала времени в тиках
        private long saveTickCount = 0;

        /// <summary>
        /// Игровые данные
        /// </summary>
        private GameData gd;

        public Form1()
        {
            //this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Размер окна программы
            Width = CONFIG.WIND_WIDTH;
            Height = CONFIG.WIND_HEIGHT;
            //this.BackColor = Color.Magenta;

            // Настройки окна программы
            KeyPreview = true;
            DoubleBuffered = true;

            Logger log = new Logger("Log.txt");

            gd = GameData.Instance;
            gd.log = log;

            // Получить доступ к ресурсам, встроенным в проект
            //gd.myAssembly = Assembly.GetExecutingAssembly();

            // Начальные параметры для обработки интервалов по таймеру
            tickCount = Environment.TickCount; //GetTickCount();
            saveTickCount = tickCount;

            // Настройки таймера
            timer.Enabled = true;
            timer.Tick += new EventHandler(OnTimer);
            timer.Interval = 20;
            timer.Start();

            // Создать физический мир
            gd.world = new PhysWorld(log);

            old_title = this.Text;

            // Получить геометрию области рисования
            gd.clientRectangle = ClientRectangle;

            // Инициализация менеджера ресурсов
            gd.rm = ResourceManager.Instance;

            gd.rm.AddElementAsImage("background", Gradient.GetImage(Color.Black, Color.Black, ClientRectangle.Width, ClientRectangle.Height, 0));
            gd.backgroundImage = gd.rm.GetImage("background");

            // Создание и настройка камеры
            gd.camera = new Camera(new Rectangle(CONFIG.START_X, CONFIG.START_Y, ClientRectangle.Width - CONFIG.START_X*2, ClientRectangle.Height - CONFIG.PANEL_HEIGHT - CONFIG.START_X*2));

            // Создать стартовую сцену игры
            GameScene gs = new GameScene(GameData.GameState.Level, gd);
            //MainMenuScene scene = new MainMenuScene(GameData.GameState.MainMenu, gd);
            gd.curScene = gs;

            gd.curScene.Init();

            gd.sceneChange = true;

            

        }

        ///////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Обработка событий таймера
        /// </summary>
        /// <param name="obj">Object.</param>
        /// <param name="ea">Ea.</param>
        ///////////////////////////////////////////////////////////////////////
        private void OnTimer(object obj, EventArgs ea)
        {
            int delta;

            // Новое значение времени
            tickCount = Environment.TickCount;

            delta = (int)(tickCount - saveTickCount);

            if (delta == 0)
            {
                // А вдруг!
                return;
            }

            // Вычислить FPS
            float fps = 1000 / delta;

            // Вывести сообщение в заголовке окна
            this.Text = old_title + " : " + fps + " FPS"; // + (string)luaVersion;

            // Проверить флаг смены сцены
            if (gd.sceneChange == true)
            {
                // Удалить все объекты из физ. мира
                gd.world.objects.Clear();

                // Перенести "живые" объекты из текущей сцены в физический мир
                foreach (Entity ent in gd.curScene.objects)
                {
                    if (ent.IsDestroyed() == false)
                    {
                        gd.world.Add(ent);
                    }
                }
                // Сбросить флаг
                gd.sceneChange = false;
            }

            // Обновить мир
            gd.world.Update(delta);

            // Обновить игровую сцену
            gd.curScene.Update(delta);

            // Проверить актуальность объектов (убрать со сцены уничтоженные объекты)
            for (int i = gd.world.objects.Count - 1; i >= 0; i--)
            {
                if (gd.world.objects[i].IsDestroyed())
                {
                    // Удалить из "мира"
                    gd.world.objects.RemoveAt(i);
                }
            }

            saveTickCount = tickCount;

            Invalidate(false);
        }

        ///////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Обработка событий перерисовки содержимого окна
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        ///////////////////////////////////////////////////////////////////////
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;


            // Вывести фоновое изображение, если оно есть
            if (gd.backgroundImage != null)
            {
                //g.DrawImage(gd.backgroundImage, 0.0f, 0.0f);
            }

            // Вызвать метод отображения текущей сцены
            gd.curScene.Render(g);

        }

        ///////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Обработка нажатых клавиш
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        ///////////////////////////////////////////////////////////////////////
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // Вызвать обработчик нажатий клавиш текущей сцены
            gd.curScene.KeyDown(sender, e);
        }

        ///////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Обработка отпущенных клавиш
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        ///////////////////////////////////////////////////////////////////////
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            // Вызвать обработчик отпусканий клавиш текущей сцены
            gd.curScene.KeyUp(sender, e);
        }

        ///////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Обработка событий нажатия клавиши мыши
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        ///////////////////////////////////////////////////////////////////////
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            gd.curScene.MouseDown(sender, e);
            /*
            // Левая кнопка
            if (e.Button == MouseButtons.Left)
            {
                foreach (Entity ent in gd.world.objects)
                {
                    ent.OnLeftMouseButtonClick(e);
                }
            }

            // Правая кнопка
            if (e.Button == MouseButtons.Right)
            {
                foreach (Entity ent in gd.world.objects)
                {
                    ent.OnRightMouseButtonClick(e);
                }
            }
            */
        }

    }
}
