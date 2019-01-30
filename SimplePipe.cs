using System;
using System.IO.Pipelines;
using System.Text;
using System.Threading.Tasks;

namespace IOPipelines
{
    public class SimplePipe
    {
        public async void WriteToReadFromSinglePipeAsync()
        {
            var pipe = new Pipe();
            await WriteDataAsync(pipe.Writer);
            pipe.Writer.Complete();
            await ReadDataAsync(pipe.Reader); 
        }

        private async ValueTask ReadDataAsync(PipeReader reader)
        {
            while (true)
            {
                var result = await reader.ReadAsync();
                var buffer = result.Buffer;
                if (buffer.IsEmpty && result.IsCompleted)
                {
                    break;
                }

                foreach (var memory in buffer)
                {
                    var span = Encoding.ASCII.GetString(memory.Span);
                    Console.Write(span);
                }

                reader.AdvanceTo(buffer.End);
            }
        }

        private async ValueTask WriteDataAsync(PipeWriter writer)
        {
            var memory = writer.GetMemory(20);
            var bytes = Encoding.ASCII.GetBytes("hello, world!", memory.Span);
            writer.Advance(bytes);
            await writer.FlushAsync();
        }
    }
}