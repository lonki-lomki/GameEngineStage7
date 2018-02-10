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
                // Перевод игрового цикла в режим взрыва
                //gd.gameFlow = GameData.GameFlow.Explosion;

                // Зарисовать место взрыва прозрачным цветом
                g.FillEllipse(new SolidBrush(Color.FromArgb(0, 0, 0, 0)), GetPosition().X - GetSize().Width / 2, GetPosition().Y - GetSize().Height / 2, GetSize().Width, GetSize().Height);
            }
            else
            {
                g.FillEllipse(Brushes.Red, GetPosition().X - GetSize().Width / 2, GetPosition().Y - GetSize().Height / 2, GetSize().Width, GetSize().Height);
                //g.FillRectangle(Brushes.White, GetPosition().X + gd.camera.Geometry.X, GetPosition().Y + gd.camera.Geometry.Y, 2, 2);
            }
        }

        public override void Update(int delta)
        {
            base.Update(delta);
            // Уменьшить время жизни
            timeToLive -= delta;

            // Настало время нанести урон танкам
            if (gd.gameFlow == GameData.GameFlow.Damage)
            {
                // TODO: цикл по всем танкам и нанесение урона, если танк попал в зону поражения
                foreach(Entity e in gd.curScene.objects)
                {
                    //gd.log.Write(e.GetType().Name);
                    if (e.GetType().Name == "Tank")
                    {
                        // TODO: определить дистанцию от танка до эпицентра взрыва
                        ((Tank)e).OnDamage(0);
                    }
                }
                // Пометить объект для уничтожения
                SetDestroyed(true);

                gd.gameFlow = GameData.GameFlow.Landfall;
            }

            // Проверить, что процесс визуализации взрыва завершен
            if (timeToLive < 0 && gd.gameFlow == GameData.GameFlow.Explosion)
            {
                gd.gameFlow = GameData.GameFlow.Damage;
            }
        }
    }
}
