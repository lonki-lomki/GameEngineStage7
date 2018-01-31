using System.Collections.Generic;
using System.Drawing;
using GameEngineStage7.Core;

namespace GameEngineStage7.Entities
{
    public class Tank : Entity
    {
        private int power;

        private int angle;

        private string name;

        private Color color;

        private string weaponName;

        private int maxPower;

        /// <summary>
        /// Количество батарей. Дробная часть - остаток заряда неполной батареи
        /// </summary>
        private float battery;

        private int parachutes;

        private int shields;

        private int magDeflectors;

        private int money;

        private List<Weapon> weapons;


        public Tank()
        {
        }

        public Tank(string id) : base(id)
        {
        }

        public Tank(string id, GameData gd) : base(id, gd)
        {
            //Landing();
        }

        /// <summary>
        /// Приземлить танк (сдвинуть вертикально вниз до контакта с землей)
        /// </summary>
        /// <returns>true, если танк коснулся земли</returns>
        public bool Landing()
        {
            // Двигать вниз попиксельно танк до контакта с ландшафтом
            // Факт контакта определять с помощью метода gd.landshaft.GetPixel();

            Bitmap bmp = new Bitmap(gd.landshaft.GetImage());

            for (int i = (int)GetPosition().Y; i < gd.camera.Geometry.Height; i++)
            {
                //Color c = gd.landshaft.GetPixel((int)GetPosition().X, i);
                Color c = bmp.GetPixel((int)GetPosition().X, i);
                if (c.A == 255)
                {
                    // Коснулись земли - остановить цикл
                    break;
                }
                SetPosition(GetPosition().X, i);
            }

            bmp.Dispose();

            return true;
        }

        public override void Render(Graphics g)
        {
            //base.Render(g);
            //g.DrawImage(img, GetPosition().X + gd.camera.Geometry.X, GetPosition().Y + gd.camera.Geometry.Y, GetSize().Width, GetSize().Height);
            g.FillRectangle(Brushes.LightGreen, GetPosition().X + gd.camera.Geometry.X, GetPosition().Y + gd.camera.Geometry.Y, 16, 8);
        }

        public override void Update(int delta)
        {
            base.Update(delta);
        }
    }
}
