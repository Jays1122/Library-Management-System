using MediatR;

namespace Backend.Features.Auth.Queries
{
    public class LoginUserQuery : IRequest<string>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
