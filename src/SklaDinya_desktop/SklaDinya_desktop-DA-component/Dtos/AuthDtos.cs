namespace SklaDinya_desktop_DA_component.Dtos;

internal record LoginRequest(
    string Username,
    string Password);

internal record RegistrationRequest(
    string  Username,
    string  Password,
    string  Name,
    string? Email);
