namespace IndependentWork1
{
    /// <summary>
    /// Клас для представлення геометричної фігури "Прямокутник".
    /// </summary>
    public class Rectangle
    {
        // Приватні поля
        private double _width;
        private double _height;

        // Властивості з get та set
        public double Width
        {
            get => _width;
            set
            {
                if (value > 0)
                    _width = value;
            }
        }

        public double Height
        {
            get => _height;
            set
            {
                if (value > 0)
                    _height = value;
            }
        }

        // Властивість тільки для читання (read-only)
        public double Area => _width * _height;

        // Конструктор з параметрами
        public Rectangle(double width, double height)
        {
            _width = width > 0 ? width : 1.0;
            _height = height > 0 ? height : 1.0;
        }

        // Методи
        public double CalculatePerimeter()
        {
            return 2 * (_width + _height);
        }

        public void PrintInfo()
        {
            Console.WriteLine($"[Прямокутник] Ширина: {_width}, Висота: {_height}, Площа: {Area:F2}, Периметр: {CalculatePerimeter():F2}");
        }
    }
}
