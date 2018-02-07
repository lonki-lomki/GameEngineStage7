using System;
using System.Drawing;
using GameEngineStage7.Core;

namespace GameEngineStage7.Entities
{
    /// <summary>
    /// Класс, описывающий снаряд, летящий в физическом мире
    /// </summary>
    public class Bullet : Entity
    {
        public Bullet()
        {
        }

        public Bullet(string id) : base(id)
        {
        }

        public Bullet(string id, GameData gd) : base(id, gd)
        {
        }

        public override void Render(Graphics g)
        {
            //base.Render(g);
            //g.FillRectangle(Brushes.White, GetPosition().X + gd.camera.Geometry.X, GetPosition().Y + gd.camera.Geometry.Y, GetSize().Width, GetSize().Height / 2);
            g.FillEllipse(Brushes.White, GetPosition().X + gd.camera.Geometry.X, GetPosition().Y + gd.camera.Geometry.Y, GetSize().Width, GetSize().Height);
        }

        public override void Update(int delta)
        {
            Color c;

            base.Update(delta);

            // Проверка коллизии с ландшафтом
            Bitmap bmp = new Bitmap(gd.landshaft.GetImage());

            try
            {
                c = bmp.GetPixel((int)GetPosition().X, (int)GetPosition().Y);
            }
            catch (Exception e)
            {
                // TODO: это выход за пределы картинки, надо пометить особым кодом, например, прозрачностью 254
                c = Color.FromArgb(255, 0, 0, 0);
            }

            // Проверить коллизию с землей
            if (c.A == 255)
            {
                // Коснулись земли
                // Уничтожить снаряд и создать объект-взрыв
                SetDestroyed(true);
                Explosion expl = new Explosion("explosion", gd);
                expl.SetPosition(GetPosition().X, GetPosition().Y);   // перевести из координат экрана в координаты камеры минус размер панели (этот размер будет добавлен при отрисовке)
                expl.SetLayer(1);
                gd.curScene.objects.Add(expl);
                gd.world.Add2(expl);

            }

            bmp.Dispose();
        }
    }
}
