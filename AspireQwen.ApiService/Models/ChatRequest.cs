using System.ComponentModel.DataAnnotations;

public sealed class ChatRequest {
    [Required]
    [MinLength(1)]
    public string Message { get; set; } = string.Empty;
}