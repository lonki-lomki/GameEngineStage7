using System.Drawing;
using GameEngineStage7.Core;

namespace GameEngineStage7.Entities
{
    /// <summary>
    /// Класс реализующий взрыв
    /// </summary>
    public class Explosion : Entity
    {
        /// <summary>
        /// Время жизни объекта
        /// </summary>
        private int timeToLive = 2000;


        public Explosion()
        {
        }

        public Explosion(string id) : base(id)
        {
        }

        public Explosion(string id, GameData gd) : base(id, gd)
        {
        }

        public override void Render(Graphics g)
        {
            //base.Render(g);
            if (timeToLive <= 0)
            {
                // Пометить объект для уничтожения
                SetDestroyed(true);
                // Зарисовать место взрыва прозрачным цветом
                g.FillEllipse(new SolidBrush(Color.FromArgb(0, 0, 0, 0)), GetPosition().X - 50 + gd.camera.Geometry.X, GetPosition().Y - 50 + gd.camera.Geometry.Y, 100, 100);
            }
            else
            {
                g.FillEllipse(Brushes.Red, GetPosition().X - 50 + gd.camera.Geometry.X, GetPosition().Y - 50 + gd.camera.Geometry.Y, 100, 100);
            }
        }

        public override void Update(int delta)
        {
            base.Update(delta);
            // Уменьшить время жизни
            timeToLive -= delta;

        }
    }
}
