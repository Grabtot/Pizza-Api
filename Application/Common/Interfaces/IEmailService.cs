﻿namespace PizzaApi.Application.Common.Interfaces
{
    public interface IEmailService
    {
        Task Send(string toAddress, string subject, string? body = "");
    }
}