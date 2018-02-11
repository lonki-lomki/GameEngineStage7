using System.Drawing;
using System.Windows.Forms;
using GameEngineStage7.Core;
using GameEngineStage7.Entities;
using GameEngineStage7.Utils;

namespace GameEngineStage7.Scenes
{
    public class GameOverScene : Scene
    {
        public GameOverScene(GameData.GameState ID, GameData gd) : base(ID, gd)
        {
        }

        public override void Init()
        {
            base.Init();

            // Геометрия области рисования (камеры)
            int width = gd.clientRectangle.Width - CONFIG.START_X * 2;
            int height = gd.clientRectangle.Height - CONFIG.PANEL_HEIGHT - CONFIG.START_X * 2;

            //Cursor.Hide();

            // Загрузить ресурсы, необходимые для данной сцены
            gd.rm.Clear();

            // Создать панель
            Image box = Box.GetBox(gd.clientRectangle.Width / 4, gd.clientRectangle.Height / 4, false);
            ImageObject io = new ImageObject("box", gd);
            io.SetImage(box);
            io.SetLayer(0);
            io.SetPosition(gd.clientRectangle.Width * 3 / 8, gd.clientRectangle.Height * 3 / 8);

            objects.Add(io);

        }

        public override void KeyDown(object sender, KeyEventArgs e)
        {
            base.KeyDown(sender, e);

            if (e.KeyCode == Keys.Escape)
            {
                Application.Exit();
            }
        }

        public override void Render(Graphics g)
        {
            g.FillRectangle(Brushes.LightGray, 0, 0, gd.clientRectangle.Width, gd.clientRectangle.Height);

            base.Render(g);

            foreach(Entity e in objects)
            {
                e.Render(g);
            }

            Font fnt = new Font("Arial", 20, FontStyle.Bold);

            int strLen = (int)g.MeasureString(gd.currentTank.Name + " WINS!!!!", fnt).Width;

            g.DrawString(gd.currentTank.Name + " WINS!!!!", fnt, new SolidBrush(gd.currentTank.Color), gd.clientRectangle.Width / 2 - strLen / 2, gd.clientRectangle.Height / 2);

        }

        public override void Update(int delta)
        {
            base.Update(delta);
        }
    }
}
