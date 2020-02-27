using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace YangdoAPI
{
    public interface IController<T>
    {
        IActionResult GetById(int id);

        IActionResult GetList();

        IActionResult GetListByFilters(string jsonFilters);

        // Insert
        IActionResult Post(T entity);

        // Update
        IActionResult Put(T entity);

        // Delete
        IActionResult Delete(int id);
    }
}
