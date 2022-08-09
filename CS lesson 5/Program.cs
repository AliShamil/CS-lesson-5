using System;
using System.Linq;

namespace CS_lesson_5;

enum GenderType { Male, Female }


abstract class Animal
{
    public string? Nickname { get; set; }
    public virtual int Age { get; set; }
    public GenderType Gender { get; set; }
    public virtual int Energy { get; set; }
    public double Price { get; set; }
    public int MealQuantity { get; set; } = 0;

    protected Animal(string? nickname, int age, GenderType gender, double price)
    {
        Nickname=nickname;
        Age=age;
        Gender=gender;
        Price=price;

    }

    public abstract void Eat();

    public abstract void Sleep();

    public abstract void Play();

    public override string ToString()
    {
        return @$"
Nickname: {Nickname}
Age: {Age} years old
Gender: {Gender}
Energy: {Energy}
Price: {Price} AZN";

    }
}


class Dog : Animal
{
    private int maxEnergy = 150;
    private int maxAge;

    public override int Age
    {
        get => base.Age;
        set
        {
            if (Age >  maxAge)
                throw new Exception($"{Nickname} is died!");

            if (Age >= maxAge - 10)
                Price-=50;

            if (Price < 0)
                Price = 0;
            base.Age = value;

        }
    }


    public override int Energy
    {
        get => base.Energy;
        set
        {
            if (value > 0 && value <= maxEnergy)
                base.Energy=value;

            else if (value > maxEnergy)
            {
                ++Age;
                Price+=15;
                Energy=maxEnergy;
            }

            else if (value <= 0)
                Sleep();

        }
    }

    public Dog(string? nickname, int age, GenderType gender, double price)
        : base(nickname, age, gender, price)
    {
        maxAge = Random.Shared.Next(age, 29);
        Energy = Random.Shared.Next(1, maxEnergy);
    }


    public override void Eat()
    {
        ++MealQuantity;
        Energy+=20;

    }

    public override void Sleep()
    {
        Console.WriteLine("Sleeping......");
        Energy = maxEnergy;
        Thread.Sleep(1000);
    }

    public override void Play()
    {
        Energy -= 10;
    }
}


class Cat : Animal
{
    private int maxEnergy = 100;
    private int maxAge;

    public override int Age
    {
        get => base.Age;
        set
        {
            if (Age >  maxAge)
                throw new Exception($"{Nickname} is died!");

            if (Age >= maxAge - 7)
                Price-=30;

            base.Age = value;

        }
    }


    public override int Energy
    {
        get => base.Energy;
        set
        {
            if (value > 0 && value <= maxEnergy)
                base.Energy=value;

            else if (value > maxEnergy)
            {
                ++Age;
                Price+=15;
                Energy=maxEnergy;
            }

            else if (value <= 0)
                Sleep();

        }
    }

    public Cat(string? nickname, int age, GenderType gender, double price)
        : base(nickname, age, gender, price)
    {
        maxAge = Random.Shared.Next(age, 38);
        Energy = Random.Shared.Next(1, maxEnergy);
    }


    public override void Eat()
    {
        ++MealQuantity;
        Energy+=10;

    }

    public override void Sleep()
    {
        Console.WriteLine("Sleeping......");
        Energy = maxEnergy;
        Thread.Sleep(1000);
    }

    public override void Play()
    {
        Energy -= 5;
    }
}


class PetShop
{
    public string? Name { get; set; } = "Empty";

    public Animal[] animals;


    public PetShop(string? name)
    {
        Name=name;
    }


    public void Add(Animal element)
    {
        if (animals == null)
        {
            animals = new Animal[0];
        }

        Animal[] newArray = new Animal[animals.Length + 1];
        int i;
        for (i = 0; i < animals.Length; i++)
        {
            newArray[i] = animals[i];
        }
        newArray[i] = element;
        animals = newArray;
    }


    public void RemoveByNick(string? nickname)
    {
        bool isFind = false;
        if (animals == null)
            return;

        foreach (var animal in animals)
        {
            if (animal.Nickname == nickname)
            {
                animals = animals.Where(e => e != animal).ToArray();
                isFind = true;
            }
        }

        if (!isFind)
            Console.WriteLine("Your choice is not found!");

    }


    public double calculatePrice()
    {
        double price = 0;
        foreach (var animal in animals)
        {
            price+= animal.Price;
        }
        return price;
    }


    public int calculateMealQuantity()
    {
        int count = 0;
        foreach (var animal in animals)
        {
            count+= animal.MealQuantity;
        }
        return count;
    }


}






internal class Program
{
    static void Main()
    {
        PetShop petshop = new PetShop("AliShop");


        petshop.Add(new Dog("Ceka", 2, GenderType.Male, 100));
        petshop.Add(new Dog("Toplan", 10, GenderType.Female, 500));
        petshop.Add(new Dog("Rex", 7, GenderType.Male, 350));
        petshop.Add(new Dog("Chop", 3, GenderType.Male, 150));
        petshop.Add(new Cat("Milo", 1, GenderType.Female, 50));
        petshop.Add(new Cat("Oliver", 4, GenderType.Male, 200));
        petshop.Add(new Cat("Charlie", 6, GenderType.Female, 300));
        petshop.Add(new Cat("Leo", 11, GenderType.Male, 250));


        Console.WriteLine($"Welcome to {petshop.Name}");
        bool game = false;
        while (!game)
        {
            int count = 0;
            string? choice;
            bool selection = false;

            foreach (var animal in petshop.animals)
            {
                Console.WriteLine($@"
{++count}.   {animal}");
            }

            Console.Write("\nPls Choose pet for game: ");
            choice = Console.ReadLine();

            if (!int.TryParse(choice, out int index))
            {
                Console.Clear();
                continue;
            }
            --index; 


            while (!selection)
            {
                if (index >= petshop.animals.Length)
                {
                    Console.Clear();
                    selection = true;
                    break;
                }
                bool petGame = false;
                bool thread = false;    
                Task.Run(() =>
                {
                    while (!thread)
                    {
                        Console.Clear();
                        Console.WriteLine(petshop.animals[index]);
                        petshop.animals[index].Play();
                        Thread.Sleep(5000);
                    }
                });

                while (!petGame)
                {

                    Console.Write($@"1. Eat
2. Sleep
3. Back
4. Exit

Enter your choice: ");
    

                    switch (Console.ReadLine())
                    {
                        case "1":
                            try
                            {
                            petshop.animals[index].Eat();

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                Thread.Sleep(1500);
                                petshop.RemoveByNick(petshop.animals[index].Nickname);
                                selection = true;
                                thread = true;
                                petGame = true;
                            }
                            break;

                        case "2":
                            petshop.animals[index].Sleep();
                            break;

                        case "3":
                            selection = true;
                            thread = true;
                            petGame = true;
                            Console.Clear();
                            break;

                        case "4":
                            selection = true;
                            thread = true;
                            petGame = true;
                            game = true;
                            Console.Clear();
                            Console.WriteLine($@"Total animal price: {petshop.calculatePrice()} AZN
Meal Quantity: {petshop.calculateMealQuantity()}");
                            break;

                    }

                }
            }
        }

    }
}