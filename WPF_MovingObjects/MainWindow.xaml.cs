﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_MovingObjects
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        int count = 0;

        public int Count { get; set; }

        public FrameworkElement currentElement { get; set; }

        Random rand = new Random();

        // Ссылка на передвигаемый объект
        FrameworkElement movingElement = null;

        // Координаты нажатия в передвигаемом объекте
        Point elementCoords;

        // Запускается по нажатию мыши на Canvas
        private void mainCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement currentElem = null;

            int num = rand.Next(2);

            switch (num)
            {
                case 0:
                    currentElem = new Ellipse();

                    // Задать случайный цвет контура
                    ((Ellipse)currentElem).Stroke = new SolidColorBrush(Color.FromRgb((byte)rand.Next(256), (byte)rand.Next(256), (byte)rand.Next(256)));

                    // Покрасить созданный объект случайным цветом
                    ((Ellipse)currentElem).Fill = new SolidColorBrush(Color.FromRgb((byte)rand.Next(256), (byte)rand.Next(256), (byte)rand.Next(256)));

                    break;
                case 1:
                    currentElem = new Rectangle();

                    // Задать случайный цвет контура
                    ((Rectangle)currentElem).Stroke = new SolidColorBrush(Color.FromRgb((byte)rand.Next(256), (byte)rand.Next(256), (byte)rand.Next(256)));

                    // Покрасить созданный объект случайным цветом
                    ((Rectangle)currentElem).Fill = new SolidColorBrush(Color.FromRgb((byte)rand.Next(256), (byte)rand.Next(256), (byte)rand.Next(256)));

                    break;
            }

            currentElem.Width = 80;
            currentElem.Height = 50;

            // Установить координаты для созданного элемента из координат мыши
            Point coords = e.GetPosition(globalCanvas);
            Canvas.SetLeft(currentElem, coords.X);
            Canvas.SetTop(currentElem, coords.Y);

            // Поместить созданный объект на самый нижний Z-уровень
            Canvas.SetZIndex(currentElem, 0);

            // Зарегистрировать обработчик нажатия мышью на созданном объекте
            currentElem.MouseDown += Element_MouseDown;

            // добавление на canvas
            globalCanvas.Children.Add(currentElem);
        }

        private void Element_MouseDown(object sender, MouseButtonEventArgs e)
        {
            movingElement = (FrameworkElement)sender;

            // Получить координаты мыши внутри перемещаемого объекта
            elementCoords = e.GetPosition(movingElement);

            // Поместить выбранный объект на самый верхний Z-уровень
            Canvas.SetZIndex(movingElement, 10);

            // Наложить эффект тени
            movingElement.Effect = new DropShadowEffect
            {
                // Цвет тени
                Color = new Color { A = 0, R = 0, G = 0, B = 0 },

                // Угол тени
                Direction = 300,

                // Радиус (величина тени)
                BlurRadius = 50,

                // Качество отрисовки
                RenderingBias = RenderingBias.Quality,

                // Дистанция от объекта
                ShadowDepth = 10,

                // Прозрачность тени
                Opacity = 0.8
            };

            // отметить сообщение, как обработанное
            e.Handled = true;
        }

        private void mainCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (movingElement != null)
            {
                // Текущие координаты мыши на холсте
                Point coords = e.GetPosition(globalCanvas);

                // Перемещение элемента по новым координатам мыши, с учётом места нажатия на элементе
                Canvas.SetLeft(movingElement, coords.X - elementCoords.X);  // Moving X-coord
                Canvas.SetTop(movingElement, coords.Y - elementCoords.Y);   // Moving Y-coord
            }
        }

        private void globalCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (movingElement != null)
            {
                // Отменить эффект тени
                movingElement.ClearValue(EffectProperty);

                // Поместить отпущенный объект на самый нижний Z-уровень
                Canvas.SetZIndex(movingElement, 10);

                movingElement = null;
            }
        }
    }
}
