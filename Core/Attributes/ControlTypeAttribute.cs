using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naflim.ControlLibrary.WPF.Core.Attributes
{
    /// <summary>
    /// 控件类型
    /// </summary>
    public enum ControlType
    {
        /// <summary>
        /// 默认
        /// </summary>
        Default,

        /// <summary>
        /// 按钮
        /// </summary>
        Button,

        /// <summary>
        /// 文本框
        /// </summary>
        TextBox,

        /// <summary>
        /// 下拉框
        /// </summary>
        ComboBox,

        /// <summary>
        /// 多选框
        /// </summary>
        CheckBox,

        /// <summary>
        /// 单选按钮列表
        /// </summary>
        RadioList,

        /// <summary>
        /// 按钮列表
        /// </summary>
        ButtonList,

        /// <summary>
        /// 颜色选择框
        /// </summary>
        PopupColor,

        /// <summary>
        /// 下拉数据
        /// </summary>
        ComBox,

        /// <summary>
        /// 超链接
        /// </summary>
        Hyperlink,

        /// <summary>
        /// 自动
        /// </summary>
        Auto,
    }

    /// <summary>
    /// 声明属性在UI的呈现方式
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ControlTypeAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="controlType">控件类型</param>
        public ControlTypeAttribute(ControlType controlType = ControlType.Default)
        {
            ControlType = controlType;
        }

        /// <summary>
        /// 控件类型
        /// </summary>
        /// <remarks>决定数据呈现模式</remarks>
        public ControlType ControlType { get; set; }
    }
}
