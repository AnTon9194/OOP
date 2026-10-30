using System;

namespace lab6v4
{
    // Базовий клас
    public class Animal
    {
        private string _name;
        private int _age;

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public int Age
        {
            get => _age;
            set => _age = value;
        }

        public Animal(string name, int age)
        {
            _name = name;
            _age = age;
        }

        // Віртуальний метод для перевизначення
        public virtual void MakeSound()
        {
            Console.WriteLine("Animal makes an unknown sound.");
        }

        // Метод для демонстрації приховування (new)
        public string GetSpecies()
        {
            return "Unknown Animal Species";
        }
    }

    // Похідний клас Dog
    public class Dog : Animal
    {
        public string Breed { get; set; }

        // Виклик конструктора базового класу через base
        public Dog(string name, int age, string breed) : base(name, age)
        {
            Breed = breed;
        }

        // Перевизначення віртуального методу
        public override void MakeSound()
        {
            Console.WriteLine($"{Name} (Dog) says: Woof! Woof!");
        }

        // Унікальний метод класу Dog
        public void Fetch()
        {
            Console.WriteLine($"{Name} is happily fetching the ball!");
        }

        // Приховування методу базового класу за допомогою new
        public new string GetSpecies()
        {
            return $"Dog (Breed: {Breed})";
        }
    }

    // Похідний клас Cat
    public class Cat : Animal
    {
        public bool IsIndoor { get; set; }

        // Виклик конструктора базового класу через base
        public Cat(string name, int age, bool isIndoor) : base(name, age)
        {
            IsIndoor = isIndoor;
        }

        // Перевизначення віртуального методу
        public override void MakeSound()
        {
            Console.WriteLine($"{Name} (Cat) says: Meow~");
        }

        // Унікальний метод класу Cat
        public void Purr()
        {
            Console.WriteLine($"{Name} is purring softly.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== 1. Створення об'єктів та виклик унікальних методів ===");
            Dog myDog = new Dog("Rex", 3, "German Shepherd");
            Cat myCat = new Cat("Whiskers", 2, true);

            myDog.MakeSound();
            myDog.Fetch();
            
            myCat.MakeSound();
            myCat.Purr();

            Console.WriteLine("\n=== 2. Демонстрація поліморфізму (override) ===");
            // Масив посилань на базовий клас Animal, що вказують на об'єкти похідних класів
            Animal[] animals = new Animal[] { myDog, myCat };

            foreach (Animal animal in animals)
            {
                // Викликається перевизначена версія методу завдяки virtual/override
                animal.MakeSound(); 
            }

            Console.WriteLine("\n=== 3. Демонстрація різниці між override та new ===");
            
            // Виклик через посилання на похідний клас Dog
            Console.WriteLine("Виклик через посилання типу Dog:");
            Console.WriteLine($"GetSpecies(): {myDog.GetSpecies()}");

            // Виклик через посилання на базовий клас Animal
            Animal animalRef = myDog;
            Console.WriteLine("Виклик через посилання типу Animal (вказує на той самий об'єкт Dog):");
            Console.WriteLine($"GetSpecies(): {animalRef.GetSpecies()}");
            
            Console.WriteLine("\nПорівняння з override:");
            Console.WriteLine("Виклик MakeSound через Animal посилання:");
            animalRef.MakeSound(); // Викличе метод Dog.MakeSound(), бо він override
        }
    }
}
