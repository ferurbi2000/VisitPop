using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisitPop.Application.Interfaces;

namespace VisitPop.Infrastructure.Shared.Services
{
    public class DateTimeService : IDateTimeService
    {
        //public DateTime NowUtc => DateTime.UtcNow;
        public DateTime NowUtc => DateTime.Now;
    }
}
