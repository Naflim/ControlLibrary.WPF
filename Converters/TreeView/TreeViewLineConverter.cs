using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Shapes;

namespace Naflim.ControlLibrary.WPF.Converters.TreeView
{
    /// <summary>
    /// 树视图连线转换器
    /// </summary>
    internal class TreeViewLineConverter : IMultiValueConverter
    {
        /// <summary>
        /// 将多重绑定的输入值转换为用于绘制树视图连线的长度值，并根据是否为父项的最后一项调整目标 <see cref="Rectangle"/> 的垂直对齐方式。
        /// </summary>
        /// <param name="values">
        /// 一个包含多重绑定值的数组，方法期望数组中的具体索引含义如下：
        /// <list type="bullet">
        /// <item><description>index 0: <see cref="double"/> 表示高度（方法内部未实际使用，但保留以兼容绑定签名）</description></item>
        /// <item><description>index 2: <see cref="TreeViewItem"/> 当前项的容器</description></item>
        /// <item><description>index 3: <see cref="Rectangle"/> 用于绘制连线的矩形控件，方法会修改其 <see cref="FrameworkElement.VerticalAlignment"/></description></item>
        /// </list>
        /// 注意：方法假定上述索引处的值已存在且可转换为相应类型，否则会引发异常。</param>
        /// <param name="targetType">目标绑定类型（由绑定框架传入，方法不使用）。</param>
        /// <param name="parameter">可选的转换参数（未使用）。</param>
        /// <param name="culture">区域性信息（未使用）。</param>
        /// <returns>
        /// 如果当前项是其父容器中的最后一项，返回 <c>9.0</c>（表示固定长度）；否则返回 <see cref="double.NaN"/>（表示自动/拉伸）。
        /// 返回值通常用于绑定到控件的某个尺寸属性以影响连线的显示。
        /// </returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double height = (double)values[0];

            TreeViewItem item = (values[2] as TreeViewItem)!;
            ItemsControl ic = ItemsControl.ItemsControlFromItemContainer(item);
            bool isLastOne = ic.ItemContainerGenerator.IndexFromContainer(item) == ic.Items.Count - 1;

            Rectangle rectangle = (values[3] as Rectangle)!;
            if (isLastOne)
            {
                rectangle.VerticalAlignment = VerticalAlignment.Top;
                return 9.0;
            }
            else
            {
                rectangle.VerticalAlignment = VerticalAlignment.Stretch;
                return double.NaN;
            }
        }

        /// <summary>
        /// 本转换器不支持反向转换；调用此方法将抛出 <see cref="NotImplementedException"/>。
        /// </summary>
        /// <param name="value">目标值（未使用）。</param>
        /// <param name="targetTypes">目标类型数组（未使用）。</param>
        /// <param name="parameter">可选参数（未使用）。</param>
        /// <param name="culture">区域性信息（未使用）。</param>
        /// <returns>不返回值，始终抛出异常。</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
