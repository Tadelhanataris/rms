﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Web;

namespace RMS.Log
{
    /// <summary>
    /// 监控日志对象
    /// </summary>
    public class MonitorLog
    {
        public string ControllerName
        {
            get;
            set;
        }
        public string ActionName
        {
            get;
            set;
        }
        public DateTime ExecuteStartTime
        {
            get;
            set;
        }
        public DateTime ExecuteEndTime
        {
            get;
            set;
        }
        /// <summary>
        /// Form 表单数据
        /// </summary>
        public NameValueCollection FormCollections
        {
            get;
            set;
        }
        /// <summary>
        /// URL 参数
        /// </summary>
        public NameValueCollection QueryCollections
        {
            get;
            set;
        }
        /// <summary>
        /// 监控类型
        /// </summary>
        public enum MonitorType
        {
            Action = 1,
            View = 2
        }
        
        private string GetIP()
        {
            string ip = string.Empty;
            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"]))
                ip = Convert.ToString(System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]);
            if (string.IsNullOrEmpty(ip))
                ip = Convert.ToString(System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
            return ip;
        }
        /// <summary>
        /// 获取监控指标日志
        /// </summary>
        /// <param name="mtype"></param>
        /// <returns></returns>
        public string GetLoginfo(MonitorType mtype = MonitorType.Action)
        {
            string ActionView = "Action执行时间监控：";
            string Name = "Action";
            if (mtype == MonitorType.View)
            {
                ActionView = "View视图生成时间监控：";
                Name = "View";
            }
            string Msg = @"
      {0}
      ControllerName：{1}Controller
      {8}Name:{2}
      开始时间：{3}
      结束时间：{4}
      总 时 间：{5}秒
      Form表单数据：{6}
      URL参数：{7}
      访问来自：{9}
          ";
            return string.Format(Msg,
              ActionView,
              ControllerName,
              ActionName,
              ExecuteStartTime,
              ExecuteEndTime,
              (ExecuteEndTime - ExecuteStartTime).TotalSeconds,
              GetCollections(FormCollections),
              GetCollections(QueryCollections),
              Name, GetIP());
        }
        /// <summary>
        /// 获取Post 或Get 参数
        /// </summary>
        /// <param name="Collections"></param>
        /// <returns></returns>
        public string GetCollections(NameValueCollection Collections)
        {
            string Parameters = string.Empty;
            if (Collections == null || Collections.Count == 0)
            {
                return Parameters;
            }
            foreach (string key in Collections.Keys)
            {
                Parameters += string.Format("{0}={1}&", key, Collections[key]);
            }
            if (!string.IsNullOrWhiteSpace(Parameters) && Parameters.EndsWith("&"))
            {
                Parameters = Parameters.Substring(0, Parameters.Length - 1);
            }
            return Parameters;
        }
    }
}