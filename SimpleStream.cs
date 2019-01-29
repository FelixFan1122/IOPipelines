using System;
using System.IO;
using System.Text;

namespace IOPipelines
{
    public class SimpleStream
    {
        private const int BufferSize = 256;

        public void WriteToReadFromSinglePipe()
        {
            using (var stream = new MemoryStream())
            {
                WriteData(stream);
                stream.Position = 0;
                ReadData(stream);
            }
        }

        private void ReadData(Stream stream)
        {
            int bytesRead;
            var buffer = new byte[BufferSize];
            do
            {
                bytesRead = stream.Read(buffer, 0, BufferSize);
                if (bytesRead > 0)
                {
                    Console.Write(Encoding.ASCII.GetString(buffer, 0, bytesRead));
                }
            } while (bytesRead > 0);
        }

        private void WriteData(Stream stream)
        {
            var bytes = Encoding.ASCII.GetBytes("hello, world!");
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();
        }
    }
}