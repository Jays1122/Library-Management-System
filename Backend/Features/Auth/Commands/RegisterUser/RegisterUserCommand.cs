using MediatR;

namespace Backend.Features.Auth.Commands.RegisterUser
{
    public class RegisterUserCommand : IRequest<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = "Member"; // Default Member
    }
}
