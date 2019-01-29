using System;

namespace IOPipelines
{
    class Program
    {
        static void Main(string[] args)
        {
            new SimpleStream().WriteToReadFromSinglePipe();
            Console.ReadKey();
        }
    }
}
