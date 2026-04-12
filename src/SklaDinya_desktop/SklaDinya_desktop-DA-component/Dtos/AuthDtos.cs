namespace SklaDinya_desktop_DA_component.Dtos;

public record LoginRequest(
    string Username,
    string Password);

public record RegistrationRequest(
    string Username,
    string Password,
    string Name,
    string? Email);
