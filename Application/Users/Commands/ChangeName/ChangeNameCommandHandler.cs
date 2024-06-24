using ErrorOr;
using MediatR;
using PizzaApi.Application.Common.Interfaces.Repositories;
using PizzaApi.Domain.Users;

namespace PizzaApi.Application.Users.Commands.ChangeName
{
    public class ChangeNameCommandHandler : IRequestHandler<ChangeNameCommand, ErrorOr<User>>
    {
        private readonly IUsersRepository _usersRepository;

        public ChangeNameCommandHandler(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task<ErrorOr<User>> Handle(ChangeNameCommand command, CancellationToken cancellationToken)
        {
            User user = await _usersRepository.FindAsync(command.UserId)
                ?? throw new InvalidOperationException($"User with id ({command.UserId}) not found");

            user.Name = command.NewName;

            return user;
        }
    }
}
