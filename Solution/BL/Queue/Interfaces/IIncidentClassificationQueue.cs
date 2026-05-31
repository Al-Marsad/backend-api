using System.Threading.Channels;


namespace BL.Queue.Interfaces
{
    public interface IIncidentClassificationQueue
    {
        void Enqueue(string incidentId);
        IAsyncEnumerable<string> ReadAllAsync(CancellationToken ct);
    }
}
