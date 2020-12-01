using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Enums;
using MISA.ApplicationCore.Interfaces;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MISA.Infarstructure
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity>, IDisposable where TEntity : BaseEntity
    {

        #region DECLARE
        IConfiguration _configuration;
        string _connectionString = string.Empty;
        protected IDbConnection _dbConnection = null;
        protected string _tableName;
        #endregion


        public BaseRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("MISACukCukConnectionString");
            _dbConnection = new MySqlConnection(_connectionString);
            _tableName = typeof(TEntity).Name;
        }
        /// <summary>
        /// Thực hiện thêm 1 bản ghi
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Số lượng bản ghi</returns>
        /// createdby ngochtb(1/12/2020)
        public int Add(TEntity entity)
        {
            var rowAffects = 0;
            _dbConnection.Open();
            using (var transaction = _dbConnection.BeginTransaction())
            {
                try
                {
                    var parameters = MappingDbType(entity);
                    // Thực hiện thêm khách hàng:
                    rowAffects = _dbConnection.Execute($"Proc_Insert{_tableName}", parameters, commandType: CommandType.StoredProcedure);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }
            // Trả về kết quả (số bản ghi thêm mới được)
            return rowAffects;
        }
        /// <summary>
        /// Thực hiện xóa 1 bản ghi
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns>Số bản ghi</returns>
        /// createdby ngochtb(1/12/2020)
        public int Delete(Guid entityId)
        {
            var res = 0;
            _dbConnection.Open();
            using (var transaction = _dbConnection.BeginTransaction())
            {
                res = _dbConnection.Execute($"DELETE FROM {_tableName} WHERE {_tableName}Id = '{entityId.ToString()}'", commandType: CommandType.Text);
                transaction.Commit();
            }
            return res;
        }

        /// <summary>
        /// Lấy danh sách bản ghi
        /// </summary>
        /// <returns>danh sách bản ghi</returns>
        /// createdby ngochtb(1/12/2020)
        public virtual IEnumerable<TEntity> GetEntities()
        {
            // Kết nối tới CSDL:
            // Khởi tạo các commandText:
            var entities = _dbConnection.Query<TEntity>($"Proc_Get{_tableName}", commandType: CommandType.StoredProcedure);
            // Trả về dữ liệu:
            return entities;
        }
        /// <summary>
        /// Lấy danh sách bản ghi theo stroreName
        /// </summary>
        /// <param name="storeName"></param>
        /// <returns>danh sách bản ghi</returns>
        /// createdby ngochtb(1/12/2020)

        public IEnumerable<TEntity> GetEntities(string storeName)
        {
            // Kết nối tới CSDL:
            // Khởi tạo các commandText:
            var entities = _dbConnection.Query<TEntity>($"{storeName}", commandType: CommandType.StoredProcedure);
            // Trả về dữ liệu:
            return entities;
        }

        /// <summary>
        /// Lấy bản ghi theo ID
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns>Bản ghi</returns>
        /// createdby ngochtb(1/12/2020)
        /// 
        public IEnumerable<TEntity> GetEntityById(Guid entityId)
        {
            // Kết nối tới CSDL:
            // Khởi tạo các commandText:
            var entity = _dbConnection.Query<TEntity>($"Proc_Get{_tableName}ById", commandType: CommandType.StoredProcedure);
            // Trả về về dữ liệu:
            return entity;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public TEntity GetEntityByProperty(TEntity entity, PropertyInfo property)
        {
            var propertyName = property.Name;
            var propertyValue = property.GetValue(entity);
            var keyValue = entity.GetType().GetProperty($"{_tableName}Id").GetValue(entity);
            var query = string.Empty;
            if (entity.EntityState == EntityState.AddNew)
                query = $"SELECT * FROM {_tableName} WHERE {propertyName} = '{propertyValue}'";
            else if (entity.EntityState == EntityState.Update)
                query = $"SELECT * FROM {_tableName} WHERE {propertyName} = '{propertyValue}' AND {_tableName}Id <> '{keyValue}'";
            else
                return null;
            var entityReturn = _dbConnection.Query<TEntity>(query, commandType: CommandType.Text).FirstOrDefault();
            return entityReturn;
        }

        /// <summary>
        /// Cập nhật bản ghi
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>số bản ghi</returns>
        /// createdby ngochtb(1/12/2020)
        public int Update(TEntity entity)
        {
            // Khởi tạo kết nối với Db:
            var parameters = MappingDbType(entity);
            // Thực thi commandText:
            var rowAffects = _dbConnection.Execute($"Proc_Update{_tableName}", parameters, commandType: CommandType.StoredProcedure);
            // Trả về kết quả (số bản ghi thêm mới được)
            return rowAffects;
        }
        /// <summary>
        /// Khởi tạo kiểu dữ liệu 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        private DynamicParameters MappingDbType(TEntity entity)
        {
            var properties = entity.GetType().GetProperties();
            var parameters = new DynamicParameters();
            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(entity);
                var propertyType = property.PropertyType;
                if (propertyType == typeof(Guid) || propertyType == typeof(Guid?))
                {
                    parameters.Add($"@{propertyName}", propertyValue, DbType.String);
                }
                else if (propertyType == typeof(bool) || propertyType == typeof(bool?))
                {
                    var dbValue = ((bool)propertyValue == true ? 1 : 0);
                    parameters.Add($"@{propertyName}", dbValue, DbType.Int32);
                }
                else
                {
                    parameters.Add($"@{propertyName}", propertyValue);
                }

            }
            return parameters;
        }


        public void Dispose()
        {
            if (_dbConnection.State == ConnectionState.Open)
                _dbConnection.Close();
        }

        TEntity IBaseRepository<TEntity>.GetEntityById(Guid entityId)
        {
            throw new NotImplementedException();
        }
    }
}
