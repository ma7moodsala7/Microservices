using Common.Models;
using MediatR;

namespace IdentityService.Application.Features.Auth.Commands;

public record RegisterUserCommand : IRequest<Result>
{
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
    public string PhoneNumber { get; init; } = null!;
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
}
