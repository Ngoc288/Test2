using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.ApplicationCore.Enums
{
    /// <summary>
    /// MISACode xác định trạng thái của validate
    /// </summary>
    public enum MISACode
    {
        /// <summary>
        /// Dữ liệu hợp lệ
        /// </summary>
        IsValid=100,

        /// <summary>
        /// Dữ liệu không hợp lệ
        /// </summary>
        NotValid=900,

        /// <summary>
        /// Dữ liệu thành công
        /// </summary>
        Success=200,
        Exception=500,
    }
    /// <summary>
    /// Xác định trạng thái của Object
    /// </summary>
    public enum EntityState
    {
        AddNew=1,
        Update=2,
        Delete=3,
    }

    /// <summary>
    /// Xác định giới tính
    /// </summary>
    public enum Gender
    {
        /// <summary>
        /// Female là nữ
        /// </summary>
        Female,
        /// <summary>
        /// Male là nam
        /// </summary>
        Male,
        /// <summary>
        /// Other là khác
        /// </summary>
        Other,
    }

    /// <summary>
    /// Xác định tình trạng công việc
    /// </summary>
    public enum WorkStatus
    {
        /// <summary>
        /// Đã nghỉ việc
        /// </summary>
        Resign,
        /// <summary>
        /// Đang làm việc
        /// </summary>
        Working,
        /// <summary>
        /// Đang thử việc
        /// </summary>
        TrailWork,
        /// <summary>
        /// Đã nghỉ hưu
        /// </summary>
        Retired,
    }


}
