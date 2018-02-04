namespace GameEngineStage7.Utils
{
    /// <summary>
    /// Класс для упрощения работы с углом поворота пушки
    /// </summary>
    public class Angle
    {
        private int value;

        public Angle()
        {
            value = 0;
        }

        public Angle(int value)
        {
            this.value = value;
        }

        public int Value
        {
            get => value;
            set
            {
                this.value = value;
                if (this.value < 0)
                {
                    this.value = 0;
                }
                if (this.value > 180)
                {
                    this.value = 180;
                }
            }
        }

        /// <summary>
        /// Перевод в строку значение угла в градусах. Углы от 0 до 90 отображается как есть, углы от 91 до 180 отображаються как 89 - 0.
        /// </summary>
        /// <returns>текстовое представление угла в градусах</returns>
        public override string ToString()
        {
            if (value <= 90)
            {
                return value.ToString();
            }
            return (180 - value).ToString();
        }
    }
}
