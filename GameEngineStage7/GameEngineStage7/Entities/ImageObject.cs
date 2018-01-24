using GameEngineStage7.Core;
using System.Drawing;

namespace GameEngineStage7.Entities
{
    /// <summary>
    /// Класс, описывающий визуальный объект, отображающийся как изображение
    /// </summary>
    public class ImageObject : Entity
    {
        public ImageObject()
        {
        }

        public ImageObject(string id) : base(id)
        {
        }

        public ImageObject(string id, GameData gd) : base(id, gd)
        {
        }

        public override void Render(Graphics g)
        {
            //base.Render(g);
            g.DrawImage(GetImage(), position);
        }
    }
}
