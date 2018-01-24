using System.Drawing;
using GameEngineStage7.Core;

namespace GameEngineStage7.Entities
{
    public class GamePanel : Entity
    {
        private string strPower = "Power:";
        private int strPowerLen = 0;

        private string strAngle = "Angle:";
        private int strAngleLen = 0;

        private string placeHolder = "000";
        private int placeHolderLen = 0;

        private Font fnt;

        public GamePanel()
        {
        }

        public GamePanel(string id) : base(id)
        {
        }

        public GamePanel(string id, GameData gd) : base(id, gd)
        {
            fnt = new Font("Arial", 15, FontStyle.Bold);
        }

        public override void Render(Graphics g)
        {
            base.Render(g);

            // Одноразовый расчет параметров
            if (strPowerLen == 0)
            {
                strPowerLen = (int)g.MeasureString(strPower, fnt).Width;
            }

            // Вывести надписи на панели
            g.DrawString(strPower, fnt, Brushes.Black, GetPosition().X + 4, GetPosition().Y + 4);
            g.DrawString(placeHolder, fnt, Brushes.Black, GetPosition().X + 4 + strPowerLen, GetPosition().Y + 4);
        }
    }
}
