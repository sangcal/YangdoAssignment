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
    public class PersonService : IBasicService<Person>
    {
        // Declare DI Object
        private readonly IRepository<Person> personRepository;

        public PersonService(Repository<Person> personRepository)
        {
            // Inject Dependent Object
            // This DI Object will play roles relevant to DB connected
            this.personRepository = personRepository;
        }

        // Retrieve single data by id
        public Person GetById(int id)
        {
            // according to the situations, refering object can be included
            // refering object is included by default, if necessary you can add parameter in accordance with the situations
            return this.GetList(x => x.PersonId == id).Include(x => x.TimeSheets).SingleOrDefault();
        }

        // Retrieve multiple data by filters
        public IQueryable<Person> GetList(Expression<Func<Person, bool>> predicate = null)
        {
            // according to the situations, refering object can be included
            // refering object is included by default, if necessary you can add parameter in accordance with the situations
            return personRepository.GetList(predicate);
        }

        // Retrieve multiple data by filters handed over from frontend
        public IQueryable<Person> GetListByFilters(Dictionary<string, string> filters)
        {
            IQueryable<Person> tempResult = this.GetList();

            foreach (var filter in filters)
            {
                switch (filter.Key.ToUpper())
                {
                    case "EMAIL":
                        tempResult = tempResult.Where(x => x.Email.Contains(filter.Value));
                        break;

                    case "FIRSTNAME":
                        tempResult = tempResult.Where(x => x.FirstName.Contains(filter.Value));
                        break;

                    case "LASTNAME":
                        tempResult = tempResult.Where(x => x.LastName.Contains(filter.Value));
                        break;

                    case "PHONE":
                        tempResult = tempResult.Where(x => x.Phone.Contains(filter.Value));
                        break;

                    case "DOB":
                        tempResult = tempResult.Where(x => x.DOB == Convert.ToDateTime(filter.Value));
                        break;

                    default:
                        break;
                }
            }

            tempResult = tempResult.Include(x => x.TimeSheets);

            return tempResult;
        }

        // Delete entity
        public void Delete(Person entity)
        {
            personRepository.Delete(entity);
        }

        // Insert entity
        public void Insert(Person entity)
        {
            personRepository.Insert(entity);
        }


        // Update entity
        public void Update(Person entity)
        {
            personRepository.Update(entity);
        }
        
    }
}
