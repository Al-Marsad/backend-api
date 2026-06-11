using System.Threading.Channels;
using BL.Queue.Interfaces;

namespace BL.Queue
{
    public class IncidentClassificationQueue : IIncidentClassificationQueue
    {
        private readonly Channel<string> _channel = Channel.CreateBounded<string>(
            new BoundedChannelOptions(capacity: 500)
            {
                FullMode = BoundedChannelFullMode.Wait,
                SingleReader = true,
                SingleWriter = false
            });

        public void Enqueue(string incidentId)
        {
            if (!_channel.Writer.TryWrite(incidentId))
                throw new InvalidOperationException(
                    $"Classification queue is full. Could not enqueue incident {incidentId}.");
        }

        public IAsyncEnumerable<string> ReadAllAsync(CancellationToken ct) =>
            _channel.Reader.ReadAllAsync(ct);
    }
}
