using Via.EventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Aggregates.Event.Values;

public class ViaEventTitle : ValueObject
{
    internal string Value { get; }

    private ViaEventTitle(string value) => Value = value;

    public static OperationResult<ViaEventTitle> Create(string title)
    {
        var validation = Validate(title);

        if (validation.IsFailure)
        {
            return validation.OperationErrors;
        }

        return new ViaEventTitle(title);
    }

    private static OperationResult<string> Validate(string title)
    {
        if (string.IsNullOrWhiteSpace(title) || title.Length < 3 || title.Length > 75)
        {
            return new OperationError(ErrorCode.InvalidInput, "Title must be between 3 and 75 characters long.");
        }

        return OperationResult<string>.Success(title);
    }
    
    internal static ViaEventTitle Default() => new("Working Title");

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}