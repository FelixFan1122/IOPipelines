using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Tasks;

namespace IOPipelines
{
    public class StreamDuplexPipeApp
    {
        public async ValueTask<Message> GetNextMessageAsync(PipeReader reader, CancellationToken cancellationToken = default)
        {
            while (true)
            {
                var readResult = await reader.ReadAsync(cancellationToken);
                if (readResult.IsCanceled)
                {
                    ThrowCanceled();
                }

                var buffer = readResult.Buffer;
                if (TryParseFrame(buffer, out Message nextMessage, out SequencePosition consumed))
                {
                    reader.AdvanceTo(consumed);
                    return nextMessage;
                }

                reader.AdvanceTo(buffer.Start, buffer.End);
                if (readResult.IsCompleted)
                {
                    ThrowEof();
                }
            }
        }

        public ValueTask<bool> Write(Message message, PipeWriter writer)
        {
            var span = writer.GetSpan();
            var bytes = WriteMessage(message, span);
            writer.Advance(bytes);
            return FlushAsync(writer);
        }

        private static async ValueTask<bool> FlushAsync(PipeWriter writer)
        {
            var flushResult = await writer.FlushAsync();
            return !(flushResult.IsCanceled || flushResult.IsCompleted);
        }

        private Message ReadMessage(ReadOnlySequence<byte> payload)
        {
            throw new NotImplementedException();
        }

        private void ThrowCanceled()
        {
            throw new NotImplementedException();
        }

        private void ThrowEof()
        {
            throw new NotImplementedException();
        }

        private bool TryParseFrame(ReadOnlySequence<byte> buffer, out Message nextMessage, out SequencePosition consumed)
        {
            var eol = buffer.PositionOf((byte)'\n');
            if (eol == null)
            {
                nextMessage = default;
                consumed = default;
                return false;
            }

            consumed = buffer.GetPosition(1, eol.Value);
            var payload = buffer.Slice(0, eol.Value);
            nextMessage = ReadMessage(payload);
            return true;
        }

        private int WriteMessage(Message message, Span<byte> span)
        {
            throw new NotImplementedException();
        }
    }
}