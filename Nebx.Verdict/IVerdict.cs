namespace Nebx.Verdict;

public interface IVerdict
{
    public object? GetValue();
    public IReadOnlyDictionary<string, object>? GetMetadata();
    public IReadOnlyDictionary<string, string>? GetErrors();
    public bool IsSuccess { get; }
    public string Message { get; }
}