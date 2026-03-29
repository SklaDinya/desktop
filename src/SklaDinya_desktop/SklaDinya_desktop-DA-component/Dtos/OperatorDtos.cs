namespace SklaDinya_desktop_DA_component.Dtos;

internal record OperatorDto(
    string? Id,
    string? Username,
    string? Name,
    string? Email,
    string? Role,
    bool?   Banned,
    string? CreatedAt,
    string? UpdatedAt);

internal record OperatorCreateRequest(
    string  Username,
    string  Password,
    string  Name,
    string? Email,
    string  Role);

internal record OperatorUpdateRequest(
    string? Username,
    string? Password,
    string? Name,
    string? Email,
    string? Role,
    bool?   Banned);
