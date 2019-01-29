using System;

namespace IOPipelines
{
    class Program
    {
        static void Main(string[] args)
        {
            new SimplePipe().WriteToReadFromSinglePipe();
            Console.ReadKey();
        }
    }
}
