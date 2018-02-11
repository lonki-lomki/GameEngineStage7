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
            if (GetPosition().Y > 0)
            {
                g.FillEllipse(Brushes.White, GetPosition().X + gd.camera.Geometry.X, GetPosition().Y + gd.camera.Geometry.Y, GetSize().Width, GetSize().Height);
            } else
            {
                // Отображение снаряда в верхней строке камеры, если снаряд вылетел выше обзора камеры
                g.FillEllipse(Brushes.White, GetPosition().X + gd.camera.Geometry.X, gd.camera.Geometry.Y, GetSize().Width, GetSize().Height);
            }
        }

        public override void Update(int delta)
        {
            Color c;

            // Запомнить позицию снаряда до перемещения
            PointF posBefore = GetPosition();

            // Расчет новой позиции снаряда
            base.Update(delta);

            // Проверка коллизии с ландшафтом
            Bitmap bmp = new Bitmap(gd.landshaft.GetImage());

            // *** Далее надо пройти по линии между двумя точками и проверить коллизию с ландшафтом
            // Подготовка параметров для запуска алгоритма Брезенхема для движения по линии
            int x, y, dx, dy, incx, incy, pdx, pdy, es, el, err;

            // Проекции отрезка на оси Х и У
            dx = (int)(GetPosition().X - posBefore.X);
            dy = (int)(GetPosition().Y - posBefore.Y);

            // Определить направление движения по отрезку
            incx = (dx > 0) ? 1 : (dx < 0) ? -1 : 0;
            incy = (dy > 0) ? 1 : (dy < 0) ? -1 : 0;

            // Получить абсолютную величину длины проекции отрезка
            if (dx < 0) dx = -dx;
            if (dy < 0) dy = -dy;

            // Определить наклон отрезка
            if (dx > dy)
            {
                // Вдоль оси Х
                pdx = incx;
                pdy = 0;
                es = dy;
                el = dx;
            } else
            {
                // Вдоль оси Y
                pdx = 0;
                pdy = incy;
                es = dx;
                el = dy;
            }

            x = (int)posBefore.X;
            y = (int)posBefore.Y;
            err = el/2;
            // Стартовую точку пропускаем, её проверяли при прошлом выстреле
            // SetPixel(x, y)

            // Цикл по точкам отрезка от второй до последней
            for (int t = 0; t < el; t++)
            {
                err -= es;
                if (err < 0)
                {
                    err += el;
                    x += incx;
                    y += incy;
                } else
                {
                    x += pdx;
                    y += pdy;
                }

                // Обработать точку (x, y)
                // SetPixel(x, y)
                try
                {
                    c = bmp.GetPixel(x, y);
                }
                catch (Exception e)
                {
                    // это выход за пределы картинки, надо пометить особым кодом, например, прозрачностью 254
                    c = Color.FromArgb(254, 0, 0, 0);
                }
                // Проверить коллизию с землей
                if (c.A == 255)
                {
                    // Коснулись земли
                    // Уничтожить снаряд и создать объект-взрыв
                    SetDestroyed(true);
                    Explosion expl = new Explosion("explosion", gd);
                    expl.SetPosition(x, y);   // перевести из координат экрана в координаты камеры минус размер панели (этот размер будет добавлен при отрисовке)
                    expl.SetLayer(1);
                    expl.SetSize(50.0f, 50.0f);
                    gd.curScene.objects.Add(expl);
                    gd.world.Add2(expl);
                    // Перевод игрового цикла в режим взрыва
                    gd.gameFlow = GameData.GameFlow.Explosion;
                    // Выход из цикла
                    break;
                }
                // Снаряд за пределами экрана
                if (c.A == 254)
                {
                    if (y > gd.camera.Geometry.Height + gd.camera.Geometry.Y)
                    {
                        // Проверить попадание в экран по горизонтали
                        if (x < gd.camera.Geometry.X || x > gd.camera.Geometry.Width + gd.camera.Geometry.X)
                        {
                            // Уничтожить снаряд без взрыва
                            SetDestroyed(true);
                            // Перевод игрового цикла в режим падения земли
                            gd.gameFlow = GameData.GameFlow.Landfall;
                            // Выход из цикла
                            break;
                        } else
                        {
                            // Долетели до нижней строки экрана в границах камеры
                            // Уничтожить снаряд и создать объект-взрыв
                            SetDestroyed(true);
                            Explosion expl = new Explosion("explosion", gd);
                            expl.SetPosition(x, y - CONFIG.PANEL_HEIGHT);   // перевести из координат экрана в координаты камеры минус размер панели (этот размер будет добавлен при отрисовке)
                            expl.SetLayer(1);
                            expl.SetSize(50.0f, 50.0f);
                            gd.curScene.objects.Add(expl);
                            gd.world.Add2(expl);
                            // Перевод игрового цикла в режим взрыва
                            gd.gameFlow = GameData.GameFlow.Explosion;
                            // Выход из цикла
                            break;
                        }
                    }
                }
            } // цикл по точкам отрезка
            // *** Окончание алгоритма Брезенхема

            bmp.Dispose();
        }
    }
}
