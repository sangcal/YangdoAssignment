using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YangdoService
{
    public interface IExtendedService<T> : IBasicService<T>
    {
        // Retrieve total amount of time between timerange
        IQueryable<T> GetHoursOfTimeRange(Dictionary<string, string> filters);

        // Retrieve total amount of time between timerange by TaskId
        IQueryable<T> GetHoursOfTimeRangeByTaskId(Dictionary<string, string> filters);

        // Retrieve total amount of time between timerange by PersonId
        IQueryable<T> GetHoursOfTimeRangeByPersonId(Dictionary<string, string> filters);
    }
}
