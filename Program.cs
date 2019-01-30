using System;

namespace IOPipelines
{
    class Program
    {
        static void Main(string[] args)
        {
            // new SimpleStream().WriteToReadFromSinglePipe();
            new SimplePipe().WriteToReadFromSinglePipeAsync();
            Console.ReadKey();
        }
    }
}
