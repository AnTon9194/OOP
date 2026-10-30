using System;

namespace lab6v4
{
    // === БАЗОВИЙ КЛАС ===
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
            set => _age = value >= 0 ? value : 0;
        }

        public Animal(string name, int age)
        {
            _name = name;
            _age = age;
        }

        public virtual void MakeSound()
        {
            Console.WriteLine("Тварина видає невизначений звук.");
        }

        public string GetSpecies()
        {
            return "Біологічний вид: Тварина (Animal)";
        }
    }

    // === ПОХІДНИЙ КЛАС 1: Dog ===
    public class Dog : Animal
    {
        public string Breed { get; set; }

        public Dog(string name, int age, string breed) : base(name, age)
        {
            Breed = breed;
        }

        public override void MakeSound()
        {
            Console.WriteLine($"{Name} (Собака) голосно гавкає: Гав-гав!");
        }

        public void Fetch()
        {
            Console.WriteLine($"{Name} весело біжить за м'ячиком!");
        }

        public new string GetSpecies()
        {
            return $"Біологічний вид: Собака (Dog), Порода: {Breed}";
        }
    }

    // === ПОХІДНИЙ КЛАС 2: Cat ===
    public class Cat : Animal
    {
        public bool IsIndoor { get; set; }

        public Cat(string name, int age, bool isIndoor) : base(name, age)
        {
            IsIndoor = isIndoor;
        }

        public override void MakeSound()
        {
            Console.WriteLine($"{Name} (Кіт) ніжно нявчить: Мяу~");
        }

        public void Purr()
        {
            Console.WriteLine($"{Name} муркоче від задоволення.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== 1. СТВОРЕННЯ ОБ'ЄКТІВ ===");
            Animal genericAnimal = new Animal("Невідома істота", 5);
            Dog myDog = new Dog("Рекс", 3, "Німецька вівчарка");
            Cat myCat = new Cat("Мурчик", 2, true);

            Console.WriteLine($"{genericAnimal.Name}, Вік: {genericAnimal.Age}");
            Console.WriteLine($"{myDog.Name}, Вік: {myDog.Age}, Порода: {myDog.Breed}");
            Console.WriteLine($"{myCat.Name}, Вік: {myCat.Age}, Домашній: {myCat.IsIndoor}\n");

            Console.WriteLine("=== 2. ДЕМОНСТРАЦІЯ ПОЛІМОРФІЗМУ (override) ===");
            Animal[] zoo = new Animal[] { genericAnimal, myDog, myCat };
            foreach (Animal animal in zoo)
            {
                animal.MakeSound();
            }
            Console.WriteLine();

            Console.WriteLine("=== 3. ВИКЛИК УНІКАЛЬНИХ МЕТОДІВ ===");
            myDog.Fetch();
            myCat.Purr();
            Console.WriteLine();

            Console.WriteLine("=== 4. ДЕМОНСТРАЦІЯ РІЗНИЦІ МІЖ OVERRIDE ТА NEW ===");
            Dog dogAsDog = myDog;
            Animal dogAsAnimal = myDog;

            Console.WriteLine("--- Виклик OVERRIDE методу (MakeSound): ---");
            dogAsDog.MakeSound();
            dogAsAnimal.MakeSound(); 

            Console.WriteLine("\n--- Виклик NEW методу (GetSpecies): ---");
            Console.WriteLine($"Виклик через Dog:    {dogAsDog.GetSpecies()}");
            Console.WriteLine($"Виклик через Animal: {dogAsAnimal.GetSpecies()}");
        }
    }
}
