using Microsoft.AspNetCore.Mvc;
using System;

namespace MISA.Web.Controllers
{
    public interface IBaseEntityController<TEntity>
    {
        IActionResult Delete(Guid id);
        IActionResult Get();
        IActionResult Get(string id);
        IActionResult Post(TEntity entity);
        IActionResult Put([FromRoute] string id, [FromBody] TEntity entity);
    }
}