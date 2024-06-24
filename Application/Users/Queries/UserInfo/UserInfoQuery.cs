﻿using ErrorOr;
using MediatR;
using PizzaApi.Domain.Users;

namespace PizzaApi.Application.Users.Queries
{
    public record UserInfoQuery(Guid Id) : IRequest<ErrorOr<User>>;
}