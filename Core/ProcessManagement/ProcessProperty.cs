using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Naflim.ControlLibrary.WPF.Core.ProcessManagement
{
    /// <summary>
    /// 业务成员
    /// </summary>
    public class ProcessProperty
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="key">键</param>
        public ProcessProperty(string key)
        {
            Key = key;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="property">成员信息</param>
        /// <param name="key">键</param>
        /// <remarks>业务成员直接设定为指定属性</remarks>
        public ProcessProperty(object source, PropertyInfo property, string key)
        {
            Source = source;
            Property = property;
            Key = key;

            GetMethod = () => property.GetValue(Source);
            SetMethod = v => property.SetValue(Source, v);
        }

        /// <summary>
        /// 如何获取数据
        /// </summary>
        public Func<object?>? GetMethod { get; set; }

        /// <summary>
        /// 如何设置数据
        /// </summary>
        public Action<object>? SetMethod { get; set; }

        /// <summary>
        /// 数据源
        /// </summary>
        public object? Source { get; }

        /// <summary>
        /// 成员信息
        /// </summary>
        public PropertyInfo? Property { get; }

        /// <summary>
        /// 键
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// 设置业务成员Get方法
        /// </summary>
        /// <param name="func">Get方法</param>
        public void SetProces4Get(Func<object> func)
        {
            GetMethod = func;
        }

        /// <summary>
        /// 设置业务成员Set方法
        /// </summary>
        /// <param name="action">Set方法</param>
        public void SetProces4Set(Action<object> action)
        {
            SetMethod = action;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns>数据</returns>
        public object? Get()
        {
            var val = GetMethod?.Invoke();
            return val;
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="value">数据</param>
        public void Set(object value)
        {
            SetMethod?.Invoke(value);
        }
    }
}
