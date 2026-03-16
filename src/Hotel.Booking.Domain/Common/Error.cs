using System.Text.Json.Serialization;

namespace Hotel.Booking.Domain.Common
{
    public record Error
    {
        public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);
        public static readonly Error NullValue = new(
            "General.Null",
            "Null value was provided",
            ErrorType.Failure);

        public Error(string code, string description, ErrorType type, string? identifier = null)
        {
            Code = code;
            Description = description;
            Type = type;
            Identifier = identifier;
        }

        public string Code { get; }

        public string Description { get; }

        public ErrorType Type { get; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Identifier { get; }

        public static Error Failure(string code, string description) =>
            new(code, description, ErrorType.Failure);

        public static Error NotFound(string code, string description) =>
            new(code, description, ErrorType.NotFound);

        public static Error Problem(string code, string description, string? identifier = null) =>
            new(code, description, ErrorType.Problem, identifier);

        public static Error Conflict(string code, string description) =>
            new(code, description, ErrorType.Conflict);
        public static Error UnprocessableEntity(string code, string description) =>
            new(code, description, ErrorType.UnprocessableEntity);
    }
}
