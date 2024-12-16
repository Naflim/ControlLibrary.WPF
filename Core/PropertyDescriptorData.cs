using Naflim.ControlLibrary.WPF.Core.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naflim.ControlLibrary.WPF.Core
{
    /// <summary>
    /// 属性数据
    /// </summary>
    public class PropertyDescriptorData
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="property">属性</param>
        public PropertyDescriptorData(PropertyDescriptor property)
        {
            Property = property;
            PropertyName = property.Name;
            PropertyType = property.PropertyType;
            Attributes = property.Attributes;

            if (Attributes.OfType<LogicalLinkAttribute>().Any())
            {
                LogicalLink = Attributes[typeof(LogicalLinkAttribute)] as LogicalLinkAttribute;
            }

            if (Attributes.OfType<DisplayFormatAttribute>().Any())
            {
                DisplayFormat = Attributes[typeof(DisplayFormatAttribute)] as DisplayFormatAttribute;
            }

            if (Attributes.OfType<DisplayAttribute>().Any())
            {
                Display = Attributes[typeof(DisplayAttribute)] as DisplayAttribute;
            }


            if (Attributes.OfType<ControlTypeAttribute>().Any())
            {
                ControlType = (Attributes[typeof(ControlTypeAttribute)] as ControlTypeAttribute)?.ControlType ?? ControlType.Default;
            }
        }

        /// <summary>
        /// 属性的抽象
        /// </summary>
        public PropertyDescriptor Property { get; set; }

        /// <summary>
        /// 属性名
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// 属性类型
        /// </summary>
        public Type PropertyType { get; }

        /// <summary>
        /// 属性上绑定的标签
        /// </summary>
        public AttributeCollection Attributes { get; }

        /// <summary>
        /// 逻辑链接
        /// </summary>
        public LogicalLinkAttribute? LogicalLink { get; }

        /// <summary>
        /// 如何显示数据字段以及如何设置数据字段的格式。
        /// </summary>
        public DisplayFormatAttribute? DisplayFormat { get; set; }

        /// <summary>
        /// 显示
        /// </summary>
        public DisplayAttribute? Display { get; set; }

        /// <summary>
        /// 控件类型
        /// </summary>
        public ControlType ControlType { get; }
    }
}