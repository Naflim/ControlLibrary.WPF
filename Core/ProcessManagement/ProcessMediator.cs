using Naflim.ControlLibrary.WPF.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Naflim.ControlLibrary.WPF.Core.ProcessManagement
{
    /// <summary>
    /// 流程中介者
    /// </summary>
    public class ProcessMediator
    {
        private readonly Dictionary<string, ProcessProperty> processPropertys;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="source">数据源</param>
        public ProcessMediator(object source)
        {
            Source = source;
            ProcessOriginators = new Dictionary<string, ProcessOriginator>();
            processPropertys = new Dictionary<string, ProcessProperty>();
        }

        /// <summary>
        /// 数据源
        /// </summary>
        public object Source { get; set; }

        /// <summary>
        /// 业务发起人
        /// </summary>
        public Dictionary<string, ProcessOriginator> ProcessOriginators { get; }

        /// <summary>
        /// 流程梳理（业务绑定）
        /// </summary>
        public void ProcessCombing()
        {
            foreach (PropertyInfo property in Source.GetType().GetProperties())
            {
                if (HasLogicalLink(property))
                {
                    ProcessOriginators[property.Name] = new ProcessOriginator(Source, property);
                    foreach (var processProperty in ProcessOriginators[property.Name].GetAllProcessPropertys())
                    {
                        processPropertys[processProperty.Key] = processProperty;
                    }
                }
            }
        }

        /// <summary>
        /// 是否存在逻辑链接
        /// </summary>
        /// <param name="property">属性信息</param>
        /// <returns>结果</returns>
        public bool HasLogicalLink(PropertyInfo property)
        {
            foreach (var processAttribut in property.GetCustomAttributes())
            {
                if (typeof(LogicalLinkAttribute).IsAssignableFrom(processAttribut.GetType()))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 设置业务成员Get方法
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="func">Get方法</param>
        public void SetProces4Get(string key, Func<object> func)
        {
            if (processPropertys.ContainsKey(key))
            {
                processPropertys[key].SetProces4Get(func);
            }
        }

        /// <summary>
        /// 设置业务成员Set方法
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="action">Set方法</param>
        public void SetProces4Set(string key, Action<object> action)
        {
            if (processPropertys.ContainsKey(key))
            {
                processPropertys[key].SetProces4Set(action);
            }
        }

        /// <summary>
        /// 调用业务成员Get方法
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public object? Get(string key)
        {
            if (processPropertys.ContainsKey(key))
            {
                return processPropertys[key].Get();
            }

            return null;
        }

        /// <summary>
        /// 调用业务成员Set方法
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void Set(string key, object value)
        {
            if (processPropertys.ContainsKey(key))
            {
                processPropertys[key].Set(value);
            }
        }

        /// <summary>
        /// 是否存在此业务成员
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>结果</returns>
        public bool ContainsKey(string key)
        {
            return processPropertys.ContainsKey(key);
        }
    }
}
