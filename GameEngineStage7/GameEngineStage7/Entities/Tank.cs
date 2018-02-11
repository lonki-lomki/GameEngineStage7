using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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

        private int[] tank_bits;

        /// <summary>
        /// Накопление повреждений от падения танка
        /// </summary>
        private int fallDamage;

        public Tank()
        {
        }

        public Tank(string id) : base(id)
        {
        }

        public Tank(string id, GameData gd) : base(id, gd)
        {
        }

        public Tank(string id, GameData gd, GameData.TankTypes type, Color c) : base(id, gd)
        {
            tankType = type;
            color = c;

            // TODO: ВРЕМЕННО!!!! Позже размеры будут браться из картинки
            SetSize(24, 24);
            // Битовый массив из файла формата XBM
            tank_bits = new int[] {
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x18, 0x00, 0x00, 0x18, 0x00, 0x00, 0x3c, 0x00, 0x00, 0x7e, 0x00,
                0x00, 0xff, 0x00, 0x80, 0xff, 0x01, 0xc0, 0xff, 0x03, 0xf0, 0xff, 0x0f,
                0xf8, 0xff, 0x1f, 0x98, 0x99, 0x19, 0x98, 0x99, 0x19, 0xf0, 0xff, 0x0f };
            // Преобразовать битовый массив в изображение
            Bitmap bmp = new Bitmap(24, 24, PixelFormat.Format32bppArgb);
            // 24 точки = 3 байта на строку
            // Цикл по строкам битового массива
            for (int i = 0; i < GetSize().Width * 3; i += 3)
            {
                // Цикл по двум байтам
                for (int j = 0; j < 3; j++)
                {
                    byte b = (byte)tank_bits[i + 2 - j];
                    // Цикл по битам в байте
                    for (int k = 0; k < 8; k++)
                    {
                        if (b % 2 == 1)
                        {
                            // Есть пиксель
                            bmp.SetPixel(j * 8 + 8 - k, i / 3, color);
                        }
                        // к следующему биту
                        b /= 2;
                    }
                }
            }

            SetImage(bmp);

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
        /// Максимальная мощность выстрела, она же ХП (при уменьшении до 0 танк считается уничтоженным)
        /// </summary>
        public int MaxPower
        { 
            get => maxPower; 
            set
            {
                maxPower = value;
                if (maxPower < 0)
                {
                    maxPower = 0;
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
        public float Battery { get => battery; set => battery = value; }
        public int Parachutes { get => parachutes; set => parachutes = value; }
        public int Shields { get => shields; set => shields = value; }
        public int MagDeflectors { get => magDeflectors; set => magDeflectors = value; }
        public int Money { get => money; set => money = value; }
        public GameData.TankTypes TankType { get => tankType; set => tankType = value; }

        /// <summary>
        /// Приземлить танк (сдвинуть вертикально вниз до контакта с землей)
        /// </summary>
        public void Landing()
        {
            // Двигать вниз попиксельно танк до контакта с ландшафтом
            // Факт контакта определять с помощью метода gd.landshaft.GetPixel();

            Bitmap bmp = new Bitmap(gd.landshaft.GetImage());

            for (int i = (int)GetPosition().Y; i < gd.camera.Geometry.Height; i++)
            {
                //Color c = gd.landshaft.GetPixel((int)GetPosition().X, i);
                Color c = bmp.GetPixel((int)GetPosition().X + (int)GetSize().Width / 2, i + (int)GetSize().Height);
                SetPosition(GetPosition().X, i);
                if (c.A == 255)
                {
                    // Коснулись земли - остановить цикл
                    break;
                }
            }

            bmp.Dispose();
        }

        /// <summary>
        /// Приземлить танк (версия 2: пошаговое спускание до касания с ландшафтом)
        /// </summary>
        /// <returns>true - танк продолжает движение вниз, false - танк опустился на минимально возможную высоту</returns>
        public bool Landing2()
        {
            Bitmap bmp = new Bitmap(gd.landshaft.GetImage());

            // Проверить на нижнюю границу изображения
            if ((GetPosition().Y + (int)GetSize().Height) >= gd.camera.Geometry.Height)
            {
                // Далее опускаться некуда
                return false;
            }

            // Проверить пиксель под танком
            Color c = bmp.GetPixel((int)GetPosition().X + (int)GetSize().Width / 2, (int)GetPosition().Y + (int)GetSize().Height);
            if (c.A == 255)
            {
                // Стоим на земле
                return false;
            }

            // Двигаемся на 1 пиксель вниз
            SetPosition(GetPosition().X, GetPosition().Y + 1);
            // Добавить накопительное повреждение
            fallDamage += CONFIG.FALL_DAMAGE;

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
            b.SetPosition(GetPosition().X + GetSize().Width / 2, GetPosition().Y + GetSize().Width / 2);
            b.SetLayer(2);
            b.SetGravity(true);
            // Скорость по X = V * cos(alpha)
            //             Y = V * sin(alpha)
            b.SetVelocity(Power * (float)(Math.Cos(angle_.Value * Math.PI / 180)), 0 - Power * (float)(Math.Sin(angle_.Value * Math.PI / 180)));
            b.SetSize(3.0f, 3.0f);
            gd.curScene.objects.Add(b);
            gd.world.Add(b);
        }

        /// <summary>
        /// Попытка реализации ИИ для вражеского танка
        /// TODO: надо сделать визуализацию прицеливания
        /// </summary>
        public void BotFire()
        {
            /*
            Tank playerTank = new Tank();
            // Поиск танка игрока
            foreach(Tank t in gd.tanks)
            {
                if (t.TankType == GameData.TankTypes.Player)
                {
                    playerTank = t;
                }
            }
            */

            // Выставляем угол 90+45 = 135 градусов
            Angle = new Angle(135);

            // Выставляем рандомную мощность от 300 до 1000
            Random r = new Random();
            Power = 300 + r.Next(700);

            Fire();
        }
        
        /// <summary>
        /// Получить урон
        /// </summary>
        /// <param name="damage">количество пунктов урона</param>
        public void OnDamage(int damage)
        {
            MaxPower -= damage;
        }

        /// <summary>
        /// Зафиксировать текущий ХП танка с учетом урона от падения
        /// </summary>
        /// <returns>true - в результате раунда танк уничтожен, false - танк еще живой</returns>
        public bool FixDamage()
        {
            if (IsDestroyed() == true)
            {
                return false;
            }

            MaxPower -= fallDamage;
            fallDamage = 0;
            if (MaxPower <= 0)
            {
                // Танк уничтожен
                SetDestroyed(true);
                // Сгенерировать взрыв
                Explosion expl = new Explosion("explosion", gd);
                expl.SetPosition(GetPosition().X + GetSize().Width / 2, GetPosition().Y + GetSize().Height / 2);
                expl.SetLayer(1);
                expl.SetSize(100.0f, 100.0f);
                gd.curScene.objects.Add(expl);
                gd.world.Add2(expl);

                return true;
            }
            // Танк еще не уничтожен
            return false;
        }

        public override void Render(Graphics g)
        {
            if (IsDestroyed() == false)
            {
                base.Render(g);
                double x = GetSize().Width / 2 * Math.Cos(angle_.Value * Math.PI / 180);
                double y = GetSize().Width / 2 * Math.Sin(angle_.Value * Math.PI / 180);
                g.DrawLine(new Pen(color, 2), GetPosition().X + gd.camera.Geometry.X + GetSize().Width / 2, GetPosition().Y + gd.camera.Geometry.Y + GetSize().Height / 2, GetPosition().X + gd.camera.Geometry.X + GetSize().Width / 2 + (int)x, GetPosition().Y + gd.camera.Geometry.Y - (int)y + GetSize().Height / 2);

                /*
                // TODO: отображение танка в зависимости от угла поворота дула: 0-90 - направление дула вправа, 91-180 - направление дула влево
                g.FillRectangle(new SolidBrush(color), GetPosition().X + gd.camera.Geometry.X, GetPosition().Y + gd.camera.Geometry.Y, GetSize().Width, GetSize().Height / 2);
                // Отображение дула
                double x = GetSize().Width / 2 * Math.Cos(angle_.Value * Math.PI / 180);
                double y = GetSize().Width / 2 * Math.Sin(angle_.Value * Math.PI / 180);
                g.DrawLine(new Pen(color, 2), GetPosition().X + gd.camera.Geometry.X + GetSize().Width / 2, GetPosition().Y + gd.camera.Geometry.Y, GetPosition().X + gd.camera.Geometry.X + GetSize().Width / 2 + (int)x, GetPosition().Y + gd.camera.Geometry.Y - (int)y);
                */
            }
        }

        public override void Update(int delta)
        {
            if (IsDestroyed() == false)
            {
                base.Update(delta);
            }
        }
    }
}
