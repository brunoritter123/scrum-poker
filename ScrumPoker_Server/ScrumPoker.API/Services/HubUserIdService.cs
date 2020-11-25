using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ScrumPoker.Domain.Interfaces.Repositories;
using ScrumPoker.Domain.Models;
using System.Threading.Tasks;
using AutoMapper;
using ScrumPoker.API.Dtos;
using ScrumPoker.API.Interfaces;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;

namespace ScrumPoker.API.Services
{
    public class HubUserIdService : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            var httpContext = connection.GetHttpContext();
            return httpContext.Request.Query["participante-id"];
        }
    }
}