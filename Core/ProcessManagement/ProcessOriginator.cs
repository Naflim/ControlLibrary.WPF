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
    /// 业务发起人
    /// </summary>
    public class ProcessOriginator
    {
        private readonly Dictionary<Type, ProcessProperty> processPropertys;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="source">所属源</param>
        /// <param name="info">所属人信息</param>
        public ProcessOriginator(object source, PropertyInfo info)
        {
            Source = source;
            PropertyInfo = info;
            Name = info.Name;
            Value = info.GetValue(source);
            processPropertys = GetProcessProperty(info);
        }

        /// <summary>
        /// 所属源
        /// </summary>
        public object Source { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 值
        /// </summary>
        public object? Value { get; private set; }

        /// <summary>
        /// 所属人信息
        /// </summary>
        public PropertyInfo PropertyInfo { get; }

        /// <summary>
        /// 获取成员所关联的业务成员
        /// </summary>
        /// <param name="property">属性信息</param>
        /// <returns>关联成员</returns>
        /// <remarks>
        /// 如果数据源中存在逻辑链接指定的成员，将此成员加入业务成员中
        /// 如不存在此成员，在业务成员中预留此成员位置等待外部实现
        /// </remarks>
        public Dictionary<Type, ProcessProperty> GetProcessProperty(PropertyInfo property)
        {
            Dictionary<Type, ProcessProperty> data = new Dictionary<Type, ProcessProperty>();
            foreach (var processAttribut in property.GetCustomAttributes())
            {
                if (typeof(LogicalLinkAttribute).IsAssignableFrom(processAttribut.GetType()))
                {
                    LogicalLinkAttribute internalLink = (LogicalLinkAttribute)processAttribut;
                    ProcessProperty processProperty = GetProcessProperty(internalLink.Key);
                    data[processAttribut.GetType()] = processProperty;
                }
            }

            return data;
        }

        /// <summary>
        /// 获取业务成员
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>业务成员</returns>
        /// <remarks>
        /// 如果数据源中存在逻辑链接指定的成员，将此成员加入业务成员中
        /// 如不存在此成员，在业务成员中预留此成员位置等待外部实现
        /// </remarks>
        public ProcessProperty GetProcessProperty(string key)
        {
            PropertyInfo? property = Source.GetType().GetProperty(key);
            if (property == null)
            {
                return new ProcessProperty(key);
            }

            return new ProcessProperty(Source, property, key);
        }

        /// <summary>
        /// 获取所有所属业务成员
        /// </summary>
        /// <returns>所属业务成员</returns>
        public ProcessProperty[] GetAllProcessPropertys()
        {
            return processPropertys.Values.ToArray();
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <returns>值</returns>
        public object? GetValue()
        {
            return PropertyInfo.GetValue(Source);
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        public void RefreshValue()
        {
            Value = PropertyInfo.GetValue(Source);
        }
    }
}
