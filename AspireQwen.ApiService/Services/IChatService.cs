public interface IChatService {
    Task<string> ChatAsync(string userMessage, CancellationToken cancellationToken);
}