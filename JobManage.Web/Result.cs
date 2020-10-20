using JobManage.Core;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace JobManage.Web
{
    public class Result
    {
        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static JsonResult Success(ApiStatusCode status)
        {
            int code = (int)status;
            ApiResult result = new ApiResult(true, code, code.GetEnumDescription(typeof(ApiStatusCode)));
            return new JsonResult(result);
        }

        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="status"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static JsonResult Success(ApiStatusCode status, object data)
        {
            int code = (int)status;
            ApiResult result = new ApiResult(true, code, code.GetEnumDescription(typeof(ApiStatusCode)), data);
            return new JsonResult(result);
        }

        /// <summary>
        /// 成功,包含状态消息、业务数据、扩展数据
        /// </summary>
        /// <param name="code">自定义状态码</param>
        /// <param name="data">业务数据</param>
        /// <param name="exted">扩展数据</param>
        /// <returns></returns>
        public static JsonResult Success(ApiStatusCode status, object data, object exted)
        {
            int code = (int)status;
            ApiResult result = new ApiResult(true, code, code.GetEnumDescription(typeof(ApiStatusCode)), data, exted);
            return new JsonResult(result);
        }

        /// <summary>
        /// 返回自定义错误
        /// </summary>
        /// <param name="code">自定义状态码</param>
        /// <returns></returns>
        public static JsonResult Error(ApiStatusCode status)
        {
            int code = (int)status;
            ApiResult result = new ApiResult(false, code, code.GetEnumDescription(typeof(ApiStatusCode)));
            return new JsonResult(result);
        }

        /// <summary>
        /// 返回程序错误
        /// </summary>
        /// <param name="code">自定义状态码</param>
        /// <returns></returns>
        public static JsonResult Error(ApiStatusCode status, string exception)
        {
            int code = (int)status;
            ApiResult result = new ApiResult(false, code, exception);
            return new JsonResult(result);
        }
    }

    public enum ApiStatusCode
    {
        [Description("成功")]
        succ = 10000,
        [Description("失败")]
        error = 10001
    }
}
