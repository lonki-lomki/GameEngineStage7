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


        public Tank()
        {
        }

        public Tank(string id) : base(id)
        {
        }

        public Tank(string id, GameData gd) : base(id, gd)
        {
        }

        public override void Render(Graphics g)
        {
            base.Render(g);
        }

        public override void Update(int delta)
        {
            base.Update(delta);
        }
    }
}
