using System;
using System.Collections.Generic;
using System.Drawing;
using GameEngineStage7.Core;
using GameEngineStage7.Utils;

namespace GameEngineStage7.Entities
{
    public class Tank : Entity
    {
        private int power;

        private Angle angle_;

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

        /// <summary>
        /// Тип танка (игрока)
        /// </summary>
        private GameData.TankTypes tankType;


        public Tank()
        {
        }

        public Tank(string id) : base(id)
        {
        }

        public Tank(string id, GameData gd) : base(id, gd)
        {
            // TODO: ВРЕМЕННО!!!! Позже размеры будут браться из картинки
            SetSize(16, 16);
        }

        /// <summary>
        /// Мощность выстрела
        /// </summary>
        public int Power
        {
            get => power;
            set
            {
                power = value;
                if (power < 0)
                {
                    power = 0;
                }
                if (power > maxPower)
                {
                    power = maxPower;
                }
            }
        }

        /// <summary>
        /// Угол поворота ствола (0-90 градусов, с учетом поворота корпуса танка при переходе через 90 градусов)
        /// </summary>
        public Angle Angle { get => angle_; set => angle_ = value; }

        public string Name { get => name; set => name = value; }
        public Color Color { get => color; set => color = value; }
        public string WeaponName { get => weaponName; set => weaponName = value; }
        public int MaxPower { get => maxPower; set => maxPower = value; }
        public float Battery { get => battery; set => battery = value; }
        public int Parachutes { get => parachutes; set => parachutes = value; }
        public int Shields { get => shields; set => shields = value; }
        public int MagDeflectors { get => magDeflectors; set => magDeflectors = value; }
        public int Money { get => money; set => money = value; }
        public GameData.TankTypes TankType { get => tankType; set => tankType = value; }

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
                Color c = bmp.GetPixel((int)GetPosition().X + (int)GetSize().Width / 2, i + (int)GetSize().Height / 2);
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

        /// <summary>
        /// Произвести выстрел
        /// </summary>
        public void Fire()
        {
            // Создать снаряд с начальными параметрами
            Bullet b = new Bullet("bullet", gd);
            b.SetPosition(GetPosition().X + GetSize().Width / 2, GetPosition().Y);
            b.SetLayer(2);
            b.SetGravity(true);
            // Скорость по X = V * cos(alpha)
            //             Y = V * sin(alpha)
            b.SetVelocity(Power * (float)(Math.Cos(angle_.Value * Math.PI / 180)), 0 - Power * (float)(Math.Sin(angle_.Value * Math.PI / 180)));
            b.SetSize(3.0f, 3.0f);
            gd.curScene.objects.Add(b);
            gd.world.Add(b);
        }

        public override void Render(Graphics g)
        {
            //base.Render(g);
            // TODO: отображение танка в зависимости от угла поворота дула: 0-90 - направление дула вправа, 91-180 - направление дула влево
            g.FillRectangle(new SolidBrush(color), GetPosition().X + gd.camera.Geometry.X, GetPosition().Y + gd.camera.Geometry.Y, GetSize().Width, GetSize().Height / 2);
            // Отображение дула
            double x = GetSize().Width / 2 * Math.Cos(angle_.Value * Math.PI / 180);
            double y = GetSize().Width / 2 * Math.Sin(angle_.Value * Math.PI / 180);
            g.DrawLine(new Pen(color, 2), GetPosition().X + gd.camera.Geometry.X + GetSize().Width / 2, GetPosition().Y + gd.camera.Geometry.Y, GetPosition().X + gd.camera.Geometry.X + GetSize().Width / 2 + (int)x, GetPosition().Y + gd.camera.Geometry.Y - (int)y);
        }

        public override void Update(int delta)
        {
            base.Update(delta);
        }
    }
}
