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
    public class TaskService : IBasicService<Task>
    {
        // Declare DI Object
        private readonly IRepository<Task> taskRepository;

        // Inject Dependent Object
        // This DI Object will play roles relevant to DB connected
        public TaskService(Repository<Task> taskRepository)
        {
            this.taskRepository = taskRepository;
        }

        // Retrieve single data by id
        public Task GetById(int id)
        {
            // according to the situations, refering object can be included
            // refering object is included by default, if necessary you can add parameter in accordance with the situations
            return this.GetList(x => x.TaskId == id).Include(x => x.TimeSheets).SingleOrDefault();
        }

        // Retrieve multiple data by filters
        public IQueryable<Task> GetList(Expression<Func<Task, bool>> predicate = null)
        {
            // according to the situations, refering object can be included
            // refering object is included by default, if necessary you can add parameter in accordance with the situations
            return taskRepository.GetList(predicate);
        }

        // Retrieve multiple data by filters handed over from frontend
        public IQueryable<Task> GetListByFilters(Dictionary<string, string> filters)
        {
            IQueryable<Task> tempResult = this.GetList();

            foreach (var filter in filters)
            {
                switch (filter.Key.ToUpper())
                {
                    case "TASKNAME":
                        tempResult = tempResult.Where(x => x.TaskName.Contains(filter.Value));
                        break;

                    case "TASKDESC":
                        tempResult = tempResult.Where(x => x.TaskDesc.Contains(filter.Value));
                        break;

                    default:
                        break;
                }
            }

            tempResult = tempResult.Include(x => x.TimeSheets);

            return tempResult;
        }

        // Delete entity
        public void Delete(Task entity)
        {
            taskRepository.Delete(entity);
        }

        // Insert entity
        public void Insert(Task entity)
        {
            taskRepository.Insert(entity);
        }


        // Update entity
        public void Update(Task entity)
        {
            taskRepository.Update(entity);
        }
    }
}
