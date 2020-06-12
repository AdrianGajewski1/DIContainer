using DIContainer.Container.Service;
using System;

namespace DIContainer
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddTransient<IRandomNumber, RandomNumber>();
            services.AddTransient<ITest, Test>();
            services.AddSingleton<RandomNumber>();
            services.AddSingleton<Test>();
            var container = services.BuildContainer();


            Console.WriteLine(container.GetSingleton<Test>().GetRandomNumber());
            Console.WriteLine(container.GetSingleton<Test>().GetRandomNumber());
            Console.ReadLine();
        }
    }

    public interface ITest
    {
        int GetRandomNumber();
    }

    public class Test : ITest
    {
        int number;

        IRandomNumber randomNumbers;

        Random rnd = new Random();

        public Test(RandomNumber random)
        {
            number = rnd.Next(0, 10);
            randomNumbers = random;
            SetRandomFromInterface();
        }

        public int GetRandomNumber() => number;
        public void SetRandomFromInterface() => randomNumbers.SetRandomNumber(ref this.number);
    }

    public interface IRandomNumber
    {
        void SetRandomNumber(ref int number);
    }

    public class RandomNumber : IRandomNumber
    {
        public void SetRandomNumber(ref int number) => number = new Random().Next(10, 20);
    }
}
