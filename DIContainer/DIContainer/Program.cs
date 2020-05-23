using DIContainer.Container.Service;
using System;

namespace DIContainer
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
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

        Random rnd = new Random();
        public Test()
        {
            number = rnd.Next(0, 10);
        }

        public int GetRandomNumber() => number;
    }
}
