using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MISA.ApplicationCore.Interfaces
{
    public interface IBaseRepository<TEntity>
    {
        /// <summary>
        /// Lấy toàn bộ dữ liệu
        /// </summary>
        /// <returns></returns>
        /// createdby ngochtb(01/12/2020)
        IEnumerable<TEntity> GetEntities();
        /// <summary>
        /// Lấy dữ liệu theo storeName
        /// </summary>
        /// <param name="storeName"></param>
        /// <returns></returns>
        /// createdby ngochtb(01/12/2020)
        IEnumerable<TEntity> GetEntities(string storeName);
        /// <summary>
        /// Lấy dữ liệu theo Id
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        /// createdby ngochtb(01/12/2020)

        TEntity GetEntityById(Guid entityId);
        int Add(TEntity entity);
        int Update(TEntity entity);
        int Delete(Guid entityId);
        /// <summary>
        /// Lấy dữ liệu theo 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        TEntity GetEntityByProperty(TEntity entity, PropertyInfo property);
    }
}
