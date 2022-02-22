using System;
using System.Collections.Generic;
using System.Linq;

namespace TestGroupBy
{
    class Program
    {
        static void Main(string[] args)
        {
            var mockData = new List<TestClass>();
            mockData.Add(new TestClass("A", "B", "c", "D", "E", "X", 1000));
            mockData.Add(new TestClass("A", "B", "c", "D", "E", "X", 1000));
            mockData.Add(new TestClass("A", "B", "c", "D", "E", "X", 1000));
            mockData.Add(new TestClass("A", "B", "c", "D", "E", "Y", 1000));
            mockData.Add(new TestClass("A", "B", "c", "D", "E", "Y", 1000));
            mockData.Add(new TestClass("A", "B", "c", "D", "E", "Z", 1000));
            mockData.Add(new TestClass("AA", "BB", "CC", "DD", "EE", "Z", 1000));

            //here's how to group a few properties into a list in another object
            var viewModel = (from m in mockData
                     group new { m } by new
                     {
                         m.Property1,
                         m.Property2,
                         m.Property3,
                         m.Property4,
                         m.Property5
                     } into g
                     select new
                     {
                         g.Key.Property1,
                         g.Key.Property2,
                         g.Key.Property3,
                         g.Key.Property4,
                         g.Key.Property5,
                         Sixes = g.Select(o => new { o.m.Property6, o.m.NumericValue }).ToList(),
                         //SixesSummed = g.Select(o => new { o.m.Property6, o.m.NumericValue }).GroupBy(g => new { g.Property6 }).Select(s => new { s.Key.Property6, Total = s.Sum(o => o.NumericValue) }).ToList()
                         SixesSummed = g.GroupBy(o=> new { o.m.Property6}).Select(s => new { s.Key.Property6, Total = s.Sum(o => o.m.NumericValue) }).ToList()
                     }).ToList();


            Console.WriteLine($"Count: {viewModel.Count}");
            foreach (var a in viewModel)
            {
                Console.WriteLine($"{a.Property1},{a.Property2},{a.Property3},{a.Property4},{a.Property5} Count:{a.Sixes.Count} Sum:{a.Sixes.Sum(o => o.NumericValue)} TotalCount:{a.SixesSummed.Count} TotalSum:{a.SixesSummed.Sum(o => o.Total)}");

                Console.WriteLine("Sixes details:");
                foreach (var b in a.Sixes)
                {
                    Console.WriteLine($"{b.Property6},{b.NumericValue}");
                }
                Console.WriteLine("SixesSummed details:");
                foreach (var b in a.SixesSummed)
                {
                    Console.WriteLine($"{b.Property6},{b.Total}");
                }
            }

            //here's how to go back the other way
            Console.WriteLine("here is the flattend model:");
            var flatModel = viewModel.SelectMany(o => o.Sixes.Select(t => new TestClass(o.Property1, o.Property2, o.Property3, o.Property4, o.Property5, t.Property6, t.NumericValue)));
            foreach (var a in flatModel)
            {
                Console.WriteLine($"{a.Property1},{a.Property2},{a.Property3},{a.Property4},{a.Property5},{a.Property6},{a.NumericValue}");
            }

            Console.WriteLine("press enter to close");
            Console.ReadLine();
        }
    }

    public class TestClass
    {
        public string Property1 { get; set; }
        public string Property2 { get; set; }
        public string Property3 { get; set; }
        public string Property4 { get; set; }
        public string Property5 { get; set; }
        public string Property6 { get; set; }
        public decimal NumericValue { get; set; }

        public TestClass(string one, string two, string three, string four, string five, string six, decimal number)
        {
            Property1 = one;
            Property2 = two;
            Property3 = three;
            Property4 = four;
            Property5 = five;
            Property6 = six;
            NumericValue = number;
        }
    }
}
