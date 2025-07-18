namespace Nebx.Verdict;

public class Verdict<T> : IVerdict
{
    public T Value { get; internal init; } = default!;
    public bool IsSuccess { get; internal init; }
    public string Message { get; internal set; } = string.Empty;
    public Dictionary<string, string>? Errors { get; internal set; }
    public Dictionary<string, object>? Metadata { get; internal set; }

    public object? GetValue() => Value;
    public IReadOnlyDictionary<string, object>? GetMetadata() => Metadata;
    public IReadOnlyDictionary<string, string>? GetErrors() => Errors;

    internal Verdict()
    {
    }

    public static implicit operator Verdict<T>(Verdict verdict) => new Verdict<T>()
    {
        Errors = verdict.Errors != null
            ? new Dictionary<string, string>(verdict.Errors, StringComparer.OrdinalIgnoreCase)
            : null,
        Metadata = verdict.Metadata != null
            ? new Dictionary<string, object>(verdict.Metadata, StringComparer.OrdinalIgnoreCase)
            : null,
        Message = verdict.Message,
        IsSuccess = verdict.IsSuccess,
        Value = default(T)!,
    };

    public static implicit operator Verdict(Verdict<T> verdict) => new Verdict()
    {
        Errors = verdict.Errors != null
            ? new Dictionary<string, string>(verdict.Errors, StringComparer.OrdinalIgnoreCase)
            : null,
        Metadata = verdict.Metadata != null
            ? new Dictionary<string, object>(verdict.Metadata, StringComparer.OrdinalIgnoreCase)
            : null,
        Message = verdict.Message,
        IsSuccess = verdict.IsSuccess,
    };

    public static implicit operator T(Verdict<T> verdict) => verdict.Value;
    public static implicit operator Verdict<T>(T value) => new Verdict<T>() { Value = value, IsSuccess = true };

    public static Verdict<T> Success(T value) => new Verdict<T>() { Value = value, IsSuccess = true };
    public static Verdict<T> Failed(string message) => new Verdict<T>() { Message = message, IsSuccess = false };

    public Verdict<T> WithErrors(IReadOnlyDictionary<string, string> errors)
    {
        if (IsSuccess) throw new InvalidOperationException("The result is successful");
        if (errors.Count == 0) return this;

        var merged = new Dictionary<string, string>(Errors ?? new Dictionary<string, string>(),
            StringComparer.OrdinalIgnoreCase);
        foreach (var kvp in errors) merged[kvp.Key] = kvp.Value;

        Errors = merged;
        return this;
    }

    public Verdict<T> WithMetadata(Dictionary<string, object> metadata)
    {
        if (metadata.Count == 0) return this;

        var merged = new Dictionary<string, object>(Metadata ?? new Dictionary<string, object>(),
            StringComparer.OrdinalIgnoreCase);
        foreach (var kvp in metadata) merged[kvp.Key] = kvp.Value;

        Metadata = merged;
        return this;
    }

    public Verdict<T> SetMessage(string message)
    {
        if (!string.IsNullOrWhiteSpace(Message)) return this;
        
        Message = message;
        return this;
    }
}

public class Verdict : Verdict<Verdict>
{
    public static Verdict Success() => new Verdict() { IsSuccess = true };
    public static Verdict<T> Success<T>(T value) => new Verdict<T>() { Value = value, IsSuccess = true };
    public new static Verdict Failed(string message) => new Verdict() { Message = message, IsSuccess = false };

    public new Verdict WithErrors(IReadOnlyDictionary<string, string> errors)
    {
        if (IsSuccess) throw new InvalidOperationException("The result is successful");
        if (errors.Count == 0) return this;

        var merged = new Dictionary<string, string>(Errors ?? new Dictionary<string, string>(),
            StringComparer.OrdinalIgnoreCase);
        foreach (var kvp in errors) merged[kvp.Key] = kvp.Value;

        Errors = merged;
        return this;
    }

    public new Verdict WithMetadata(Dictionary<string, object> metadata)
    {
        if (metadata.Count == 0) return this;

        var merged = new Dictionary<string, object>(Metadata ?? new Dictionary<string, object>(),
            StringComparer.OrdinalIgnoreCase);
        foreach (var kvp in metadata) merged[kvp.Key] = kvp.Value;

        Metadata = merged;
        return this;
    }

    public new Verdict SetMessage(string message)
    {
        if (!string.IsNullOrWhiteSpace(Message)) return this;

        Message = message;
        return this;
    }
}