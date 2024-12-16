using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naflim.ControlLibrary.WPF.Core.Attributes
{
    /// <summary>
    /// 逻辑链接
    /// </summary>
    /// <remarks>
    /// 链接对象属性或逻辑资源库为字段绑定动态数据源
    /// 使用类型必须继承ProcessModel
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property)]
    public class LogicalLinkAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="key">键</param>
        public LogicalLinkAttribute(string key)
        {
            Key = key;
        }

        /// <summary>
        /// 键
        /// </summary>
        /// <remarks>
        /// 可以输入属性名来链接属性
        /// 也可以自定义键名来外部实现
        /// </remarks>
        public string Key { get; set; }
    }
}
