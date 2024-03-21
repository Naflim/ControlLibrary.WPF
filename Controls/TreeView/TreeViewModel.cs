using Naflim.ControlLibrary.WPF.Core;
using System.Windows.Media;

namespace Naflim.ControlLibrary.WPF.Controls.TreeView
{
    /// <summary>
    /// 树型构件视图模型
    /// </summary>
    public class TreeViewModel : ViewModelBase, ITreeViewModel
    {
        private string title = string.Empty;

        private bool? isChecked = false;

        private bool isExpanded = false;

        private ImageSource? imageSource;

        private TreeViewModel? parentItem;

        private List<TreeViewModel> childItems = new List<TreeViewModel>();

        /// <summary>
        /// 无参构造
        /// </summary>
        public TreeViewModel()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="viewModel">树型构件视图模型</param>
        /// <param name="isAll">是否全部继承</param>
        internal TreeViewModel(ITreeViewModel viewModel, bool isAll = true)
        {
            Title = viewModel.Title;
            IsChecked = viewModel.IsChecked;
            IsExpanded = viewModel.IsExpanded;
            ImageSource = viewModel.ImageSource;
            ViewModel = viewModel;
            if (isAll)
            {
                ParentNode = viewModel.ParentNode;
                ChildNodes = viewModel.ChildNodes;
            }
            else
            {
                ChildItems = new List<TreeViewModel>();
            }
        }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get => title;

            set
            {
                title = value;
                OnPropertyChanged();
                if (ViewModel != null)
                {
                    ViewModel.Title = value;
                }
            }
        }

        /// <summary>
        /// 是否以选中
        /// </summary>
        public bool? IsChecked
        {
            get => isChecked;

            set
            {
                isChecked = value;
                OnPropertyChanged(nameof(IsChecked));
                if (ViewModel != null)
                {
                    ViewModel.IsChecked = value;
                }
            }
        }

        /// <summary>
        /// 图片数据源
        /// </summary>
        public ImageSource? ImageSource
        {
            get => imageSource;

            set
            {
                imageSource = value;
                OnPropertyChanged();
                if (ViewModel != null)
                {
                    ViewModel.ImageSource = value;
                }
            }
        }

        /// <summary>
        /// 父项
        /// </summary>
        public TreeViewModel? ParentItem
        {
            get => parentItem;

            set
            {
                parentItem = value;
                OnPropertyChanged(nameof(ParentNode));
            }
        }

        /// <summary>
        /// 父节点
        /// </summary>
        public ITreeViewModel? ParentNode
        {
            get => ParentItem;

            set
            {
                if (value is TreeViewModel model)
                {
                    ParentItem = model;
                    OnPropertyChanged(nameof(ParentNode));
                }
            }
        }

        /// <summary>
        /// 子项
        /// </summary>
        public List<TreeViewModel> ChildItems
        {
            get => childItems;

            set
            {
                childItems = value;
                OnPropertyChanged(nameof(ChildNodes));
            }
        }

        /// <summary>
        /// 子节点
        /// </summary>
        public IEnumerable<ITreeViewModel> ChildNodes
        {
            get => ChildItems;

            set
            {
                ChildItems = value.Where(x => x is TreeViewModel).Cast<TreeViewModel>().ToList();
                OnPropertyChanged(nameof(ChildNodes));
            }
        }

        /// <summary>
        /// 是否展开
        /// </summary>
        public bool IsExpanded
        {
            get => isExpanded;

            set
            {
                isExpanded = value;
                OnPropertyChanged(nameof(IsExpanded));
                if (ViewModel != null)
                {
                    ViewModel.IsExpanded = value;
                }
            }
        }

        /// <summary>
        /// 存储的外部视图模型
        /// </summary>
        internal ITreeViewModel? ViewModel { get; set; }
    }
}