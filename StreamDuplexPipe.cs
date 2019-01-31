using System;
using System.IO;
using System.IO.Pipelines;

namespace IOPipelines
{
    public class StreamDuplexPipe : IDuplexPipe
    {
        private Stream stream;
        private Pipe readPipe;
        private Pipe writePipe;

        public PipeReader Input => readPipe.Reader;

        public PipeWriter Output => writePipe.Writer;

        public async void ReadPipe()
        {
            Exception exception = null;
            try
            {
                while (true)
                {
                    var memory = readPipe.Writer.GetMemory(1);
                    var bytes = await stream.ReadAsync(memory);
                    readPipe.Writer.Advance(bytes);
                    if (bytes == 0)
                    {
                        break;
                    }

                    var flushResult = await readPipe.Writer.FlushAsync();
                    if (flushResult.IsCanceled || flushResult.IsCompleted)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            finally
            {
                readPipe.Writer.Complete(exception);
            }
        }

        public async void WritePipe()
        {
            Exception exception = null;
            try
            {
                while (true)
                {
                    var readResult = await writePipe.Reader.ReadAsync();
                    var buffer = readResult.Buffer;
                    if (readResult.IsCanceled || (readResult.IsCompleted && buffer.IsEmpty))
                    {
                        break;
                    }

                    foreach (var memory in buffer)
                    {
                        await stream.WriteAsync(memory);
                    }

                    writePipe.Reader.AdvanceTo(buffer.End);
                    await stream.FlushAsync();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            finally
            {
                writePipe.Reader.Complete(exception);
            }
        }
    }
}