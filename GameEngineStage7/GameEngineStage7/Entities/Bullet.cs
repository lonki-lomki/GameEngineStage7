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
            base.Update(delta);
        }
    }
}
