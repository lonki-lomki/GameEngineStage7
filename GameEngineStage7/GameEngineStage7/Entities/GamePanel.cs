﻿using System.Drawing;
using GameEngineStage7.Core;

namespace GameEngineStage7.Entities
{
    public class GamePanel : Entity
    {
        private string strPower = "Power:";
        private int strPowerLen = 0;

        private string strAngle = "Angle:";
        private int strAngleLen = 0;

        private string placeHolder = "00000";
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
            fnt = new Font("Arial", 13, FontStyle.Bold);
        }

        public override void Render(Graphics g)
        {
            float posX;

            //base.Render(g);
            g.DrawImage(GetImage(), GetPosition().X, GetPosition().Y, GetSize().Width, GetSize().Height);

            // Одноразовый расчет параметров
            if (strPowerLen == 0)
            {
                strPowerLen = (int)g.MeasureString(strPower, fnt).Width;
            }
            if (strAngleLen == 0)
            {
                strAngleLen = (int)g.MeasureString(strAngle, fnt).Width;
            }
            if (placeHolderLen == 0)
            {
                placeHolderLen = (int)g.MeasureString(placeHolder, fnt).Width;
            }


            // Вывести надписи на панели
            posX = GetPosition().X + 8;
            g.DrawString(strPower, fnt, Brushes.Black, posX, GetPosition().Y + 6);
            posX += strPowerLen;
            //g.DrawString(placeHolder, fnt, Brushes.Black, posX, GetPosition().Y + 4);
            g.DrawString(gd.currentTank.Power.ToString(), fnt, Brushes.Black, posX, GetPosition().Y + 6);
            posX += placeHolderLen;
            g.DrawString(strAngle, fnt, Brushes.Black, posX, GetPosition().Y + 6);
            posX += strAngleLen;
            //g.DrawString(placeHolder, fnt, Brushes.Black, posX, GetPosition().Y + 6);
            g.DrawString(gd.currentTank.Angle.ToString(), fnt, Brushes.Black, posX, GetPosition().Y + 6);
            posX += placeHolderLen * 2;
            g.DrawString(gd.currentTank.Name, fnt, new SolidBrush(gd.currentTank.Color), posX, GetPosition().Y + 6);
            posX += placeHolderLen * 5;
            g.DrawString("Baby Missile", fnt, Brushes.Black, posX, GetPosition().Y + 6);
        }
    }
}
