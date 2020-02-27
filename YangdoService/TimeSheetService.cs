using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using YangdoDAO;
using YangdoDTO;

namespace YangdoService
{
    public class TimeSheetService : IExtendedService<TimeSheet>
    {
        // Declare DI Object
        private readonly IRepository<TimeSheet> timeSheetRepository;

        // Inject Dependent Object
        // This DI Object will play roles relevant to DB connected
        public TimeSheetService(Repository<TimeSheet> timeSheetRepository)
        {
            this.timeSheetRepository = timeSheetRepository;
        }

        // Retrieve single data by id
        public TimeSheet GetById(int id)
        {
            // according to the situations, refering object can be included
            return this.GetList(x => x.TimeSheetId == id).Include(x => x.Person).Include(x => x.Task).SingleOrDefault();
        }

        // Retrieve multiple data by filters
        public IQueryable<TimeSheet> GetList(Expression<Func<TimeSheet, bool>> predicate = null)
        {
            // according to the situations, refering object can be included
            return timeSheetRepository.GetList(predicate);
        }

        // Retrieve multiple data by filters handed over from frontend
        public IQueryable<TimeSheet> GetListByFilters(Dictionary<string, string> filters)
        {
            IQueryable<TimeSheet> tempResult = this.GetList();

            foreach (var filter in filters)
            {
                /// test value => jsonFIlters: {'personid': '1', 'taskid': '1', 'time': '25/02/2020'}
                /// test value => jsonFIlters: {'personid': '1', 'time': '25/02/2020'}
                /// test value => jsonFIlters: {'taskid': '1', 'time': '25/02/2020'}
                switch (filter.Key.ToUpper())
                {
                    case "PERSONID":
                        tempResult = tempResult.Where(x => x.PersonId == int.Parse(filter.Value));
                        break;

                    case "TASKID":
                        tempResult = tempResult.Where(x => x.TaskId == int.Parse(filter.Value));
                        break;

                    // The data contained between the 'timefrom' date and the 'timeto' date will be filtered
                    case "TIME":
                        tempResult = tempResult.Where(x => (x.TimeFrom <= Convert.ToDateTime(filter.Value) && x.TimeTo >= Convert.ToDateTime(filter.Value)));
                        break;

                    default:
                        break;
                }
            }

            tempResult = tempResult.Include(x => x.Person).Include(x => x.Task);

            return tempResult;
        }

        // Delete entity
        public void Delete(TimeSheet entity)
        {
            timeSheetRepository.Delete(entity);
        }

        // Insert entity
        public void Insert(TimeSheet entity)
        {
            entity.WorkedHours = this.GetHourDiff(entity.TimeFrom, entity.TimeTo);

            timeSheetRepository.Insert(entity);
        }

        private double GetHourDiff(DateTime timeFrom, DateTime timeTo)
        {
            double hourDiff = 0;
            TimeSpan ts = timeTo - timeFrom;

            hourDiff = ts.TotalMinutes / 60;

            return Math.Round(hourDiff, 2); // rounded up to two decimal places
        }

        // Update entity
        public void Update(TimeSheet entity)
        {
            entity.WorkedHours = this.GetHourDiff(entity.TimeFrom, entity.TimeTo);

            timeSheetRepository.Update(entity);
        }

        // Retrieve total amount of time between timerange
        public IQueryable<TimeSheet> GetHoursOfTimeRange(Dictionary<string, string> filters)
        {
            StringBuilder query = new StringBuilder();
            query.Append(" select * from timesheets ");
            query.Append(" where 1=1 ");
            query.Append(" and timefrom >= @timefrom ");
            query.Append(" and timefrom <= @timeto ");
            query.Append(" and timeto >= @timefrom ");
            query.Append(" and timeto <= @timeto ");

            List<SqlParameter> sqlParameters = new List<SqlParameter>();

            foreach (var filter in filters)
            {
                sqlParameters.Add(new SqlParameter() { ParameterName = "@" + filter.Key, SqlDbType = System.Data.SqlDbType.DateTime, Value = filter.Value });
            }

            IQueryable<TimeSheet> tempResult = timeSheetRepository.GetListFromRawSql(query.ToString(), sqlParameters);

            return tempResult;
        }

        // Retrieve total amount of time between timerange by PersonId
        public IQueryable<TimeSheet> GetHoursOfTimeRangeByPersonId(Dictionary<string, string> filters)
        {
            StringBuilder query = new StringBuilder();
            query.Append(" select * from timesheets ");
            query.Append(" where 1=1 ");
            query.Append(" and personid = @personid ");
            query.Append(" and timefrom >= @timefrom ");
            query.Append(" and timefrom <= @timeto ");
            query.Append(" and timeto >= @timefrom ");
            query.Append(" and timeto <= @timeto ");

            List<SqlParameter> sqlParameters = new List<SqlParameter>();

            foreach (var filter in filters)
            {
                if (filter.Key.ToUpper().Equals("PERSONID"))
                    sqlParameters.Add(new SqlParameter() { ParameterName = "@" + filter.Key, SqlDbType = System.Data.SqlDbType.Int, Value = filter.Value });
                else
                    sqlParameters.Add(new SqlParameter() { ParameterName = "@" + filter.Key, SqlDbType = System.Data.SqlDbType.DateTime, Value = filter.Value });
            }

            IQueryable<TimeSheet> tempResult = timeSheetRepository.GetListFromRawSql(query.ToString(), sqlParameters);

            return tempResult;
        }

        // Retrieve total amount of time between timerange by TaskId
        public IQueryable<TimeSheet> GetHoursOfTimeRangeByTaskId(Dictionary<string, string> filters)
        {
            StringBuilder query = new StringBuilder();
            query.Append(" select * from timesheets ");
            query.Append(" where 1=1 ");
            query.Append(" and taskid = @taskid ");
            query.Append(" and timefrom >= @timefrom ");
            query.Append(" and timefrom <= @timeto ");
            query.Append(" and timeto >= @timefrom ");
            query.Append(" and timeto <= @timeto ");

            List<SqlParameter> sqlParameters = new List<SqlParameter>();

            foreach (var filter in filters)
            {
                if (filter.Key.ToUpper().Equals("TASKID"))
                    sqlParameters.Add(new SqlParameter() { ParameterName = "@" + filter.Key, SqlDbType = System.Data.SqlDbType.Int, Value = filter.Value });
                else
                    sqlParameters.Add(new SqlParameter() { ParameterName = "@" + filter.Key, SqlDbType = System.Data.SqlDbType.DateTime, Value = filter.Value });
            }

            IQueryable<TimeSheet> tempResult = timeSheetRepository.GetListFromRawSql(query.ToString(), sqlParameters);

            return tempResult;
        }
    }
}
