using IOIT.Identity.Application.Common.Interfaces;
using System;

namespace IOIT.Identity.Infrastructure.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.UtcNow;
    }
}
