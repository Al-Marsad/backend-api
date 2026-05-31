using System.Threading.Channels;
using BL.Queue.Interfaces;

namespace BL.Queue
{
    public class IncidentClassificationQueue : IIncidentClassificationQueue
    {
        // BoundedChannel = backpressure protection (won't grow unbounded)
        private readonly Channel<string> _channel = Channel.CreateBounded<string>(
            new BoundedChannelOptions(capacity: 500)
            {
                FullMode = BoundedChannelFullMode.Wait,  // block writer if full
                SingleReader = true,                         // only the worker reads
                SingleWriter = false                         // multiple controllers can write
            });

        public void Enqueue(string incidentId)
        {
            if (!_channel.Writer.TryWrite(incidentId))
                throw new InvalidOperationException(
                    $"Classification queue is full. Could not enqueue incident {incidentId}.");
        }

        // Worker iterates this — it awaits automatically when queue is empty
        public IAsyncEnumerable<string> ReadAllAsync(CancellationToken ct) =>
            _channel.Reader.ReadAllAsync(ct);
    }
}
