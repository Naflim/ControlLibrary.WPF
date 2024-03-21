using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Naflim.ControlLibrary.WPF.Controls.TreeView
{
    /// <summary>
    /// 树型构件视图模型
    /// </summary>
    public interface ITreeViewModel
    {
        /// <summary>
        /// 标题
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// 是否以选中
        /// </summary>
        bool? IsChecked { get; set; }

        /// <summary>
        /// 图片数据源
        /// </summary>
        ImageSource? ImageSource { get; set; }

        /// <summary>
        /// 父节点
        /// </summary>
        ITreeViewModel? ParentNode { get; set; }

        /// <summary>
        /// 是否展开
        /// </summary>
        bool IsExpanded { get; set; }

        /// <summary>
        /// 子节点
        /// </summary>
        IEnumerable<ITreeViewModel> ChildNodes { get; set; }
    }
}
