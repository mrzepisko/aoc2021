using System;
using System.Linq;

namespace AdventOfCode2021 {
    class Program {
        static Day[] days;

        static void Main(string[] args) {
            days = typeof(Day).Assembly
                .GetTypes()
                .Where(t => typeof(Day).IsAssignableFrom(t))
                .Where(t => !t.IsAbstract)
                .OrderBy(t => t.Name)
                .Select(t => (Day) Activator.CreateInstance(t))
                .ToArray();

            int id;
            do {
                id = MainLoop();
            } while (id > 0);
            Console.Clear();
        }

        static int MainLoop() {
            int id = 0;
            do {
                do {
                    if (id > 0) {
                        int dayId = 0;
                        do {
                            dayId = DayLoop(days[id - 1]);
                        } while (dayId > 0);
                    }
                    WriteConsoleHeader();
                    for (int i = 0; i < days.Length; i++) {
                        Console.WriteLine(string.Format("\t{0}. {1}", i + 1, days[i].Name));
                    }
                    Console.Write("\n\nSelect day: ");
                } while (!int.TryParse(Console.ReadLine(), out id));
            } while (id > 0);
            return id;
        }

        static int DayLoop(Day day) {
            WriteConsoleHeader();
            Console.Write("\n\n");
            Console.WriteLine(day.Name);
            Console.Write("Input data: (default none): ");
            //test data
            string data = Console.ReadLine();
            if (data.Length > 0) {
                return DayLoopTest(day, data);
            }

            day.Init();
            day.PreTest();
            Console.WriteLine("Part one: {0}", day.PartOne());
            Console.WriteLine("Part two: {0}", day.PartTwo());
            Console.ReadLine();
            return 0;
        }

        static int DayLoopTest(Day day, string input) {
            day.ParseInput(input);
            day.PreTest();
            Console.WriteLine("Part one: {0}", day.PartOne());
            Console.WriteLine("Part two: {0}", day.PartTwo());
            Console.ReadLine();
            return 0;
        }

        static void WriteConsoleHeader() {
            Console.Clear();
            Console.Write(InputResource.HelloWorld);
        }
    }
}