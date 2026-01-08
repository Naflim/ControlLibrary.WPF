using Naflim.DevelopmentKit.Algorithms.Tree;
using Naflim.DevelopmentKit.DataStructure.Tree;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Naflim.ControlLibrary.WPF.Controls.TreeView
{
    /// <summary>
    /// 多选模式
    /// </summary>
    /// <remarks>
    /// 多选模式下判断选中节点
    /// </remarks>
    public enum MultipleSelectionMode
    {
        /// <summary>
        /// 子节点全部选中则此节点选中
        /// </summary>
        AllSelection,

        /// <summary>
        /// 存在子节点选中则此节点选中
        /// </summary>
        HasSelection,
    }

    /// <summary>
    /// 搜索模式
    /// </summary>
    public enum SearchMode
    {
        /// <summary>
        /// 节点搜索
        /// </summary>
        /// <remarks>
        /// 将树的所有节点当作单一节点看待，仅搜索出符合条件的节点
        /// </remarks>
        NodeSearch,

        /// <summary>
        /// 文件夹搜索
        /// </summary>
        /// <remarks>
        /// 将树的所有节点当作文件资源管理器看待，匹配的节点是文件夹将展示文件夹下所有节点
        /// </remarks>
        FolderSearch,

        /// <summary>
        /// 叶子节点搜索
        /// </summary>
        /// <remarks>
        /// 仅搜索叶子节点，匹配的节点将展示其父节点直到根节点
        /// </remarks>
        LeafNodeSearch
    }

    /// <summary>
    /// TreeViewControl.xaml 的交互逻辑
    /// </summary>
    public partial class TreeViewControl : UserControl
    {
        /// <summary>
        /// 数据源依赖属性
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource),
                                        typeof(IEnumerable<ITreeViewModel>),
                                        typeof(TreeViewControl),
                                        new PropertyMetadata(new ITreeViewModel[0]));

        /// <summary>
        /// 显示多选框依赖属性
        /// </summary>
        public static readonly DependencyProperty ShowCheckBoxProperty =
            DependencyProperty.Register(nameof(ShowCheckBox),
                                        typeof(bool),
                                        typeof(TreeViewControl),
                                        new PropertyMetadata(false));

        /// <summary>
        /// 选中项依赖属性
        /// </summary>
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(nameof(SelectedItem),
                                        typeof(ITreeViewModel),
                                        typeof(TreeViewControl),
                                        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (d, e) =>
                                        {
                                            if (e.NewValue is not ITreeViewModel treeViewModel)
                                                return;

                                            TreeViewControl control = (TreeViewControl)d;
                                            control.LordSelectedItem(treeViewModel);
                                        }));

        /// <summary>
        /// 选中项集合依赖属性
        /// </summary>
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register(nameof(SelectedItems),
                                        typeof(IList<ITreeViewModel>),
                                        typeof(TreeViewControl),
                                        new PropertyMetadata(new List<ITreeViewModel>()));

        /// <summary>
        /// 多选模式依赖属性
        /// </summary>
        public static readonly DependencyProperty MultipleSelectionModeProperty =
            DependencyProperty.Register(nameof(MultipleSelectionMode),
                                        typeof(MultipleSelectionMode),
                                        typeof(TreeViewControl),
                                        new PropertyMetadata(MultipleSelectionMode.AllSelection));

        /// <summary>
        /// 搜索模式依赖属性
        /// </summary>
        public static readonly DependencyProperty SearchModeProperty =
            DependencyProperty.Register(nameof(SearchMode),
                                        typeof(SearchMode),
                                        typeof(TreeViewControl),
                                        new PropertyMetadata(SearchMode.NodeSearch));

        /// <summary>
        /// 显示图像依赖属性
        /// </summary>
        public static readonly DependencyProperty ShowImageProperty =
            DependencyProperty.Register(nameof(ShowImage),
                                        typeof(bool),
                                        typeof(TreeViewControl),
                                        new PropertyMetadata(false));

        /// <summary>
        /// 显示搜索面板依赖属性
        /// </summary>
        public static readonly DependencyProperty ShowSearchPanelProperty =
            DependencyProperty.Register(nameof(ShowSearchPanel),
                                        typeof(bool),
                                        typeof(TreeViewControl),
                                        new PropertyMetadata(false));

        /// <summary>
        /// 搜索文本依赖属性
        /// </summary>
        public static readonly DependencyProperty SearchTextProperty =
            DependencyProperty.Register(nameof(SearchText),
                                        typeof(string),
                                        typeof(TreeViewControl),
                                        new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// 是否为主动搜索依赖属性
        /// </summary>
        public static readonly DependencyProperty IsActiveSearchProperty =
            DependencyProperty.Register(nameof(IsActiveSearch),
                                        typeof(bool),
                                        typeof(TreeViewControl),
                                        new PropertyMetadata(false));

        public TreeViewControl()
        {
            InitializeComponent();
        }

        /// <summary>
        ///  发生时选中项更改。
        /// </summary>
        public event RoutedPropertyChangedEventHandler<ITreeViewModel?>? SelectedItemChanged;

        /// <summary>
        ///  发生时选中项集合更改。
        /// </summary>
        public event RoutedPropertyChangedEventHandler<IList<ITreeViewModel>>? SelectedItemsChanged;

        /// <summary>
        /// 数据源
        /// </summary>
        public IEnumerable<ITreeViewModel> ItemsSource
        {
            get => (IEnumerable<ITreeViewModel>)GetValue(ItemsSourceProperty);

            set => SetValue(ItemsSourceProperty, value);
        }

        /// <summary>
        /// 选中项
        /// </summary>
        public ITreeViewModel SelectedItem
        {
            get => (ITreeViewModel)GetValue(SelectedItemProperty);

            set
            {
                SetValue(SelectedItemProperty, value);
            }
        }

        /// <summary>
        /// 选中项集合
        /// </summary>
        /// <remarks>
        /// ShowCheckBox为true时启用
        /// </remarks>
        public IList<ITreeViewModel> SelectedItems
        {
            get => (IList<ITreeViewModel>)GetValue(SelectedItemsProperty);

            set => SetValue(SelectedItemsProperty, value);
        }

        /// <summary>
        /// 多选模式
        /// </summary>
        public MultipleSelectionMode MultipleSelectionMode
        {
            get => (MultipleSelectionMode)GetValue(MultipleSelectionModeProperty);

            set => SetValue(MultipleSelectionModeProperty, value);
        }

        /// <summary>
        /// 搜索模式
        /// </summary>
        public SearchMode SearchMode
        {
            get => (SearchMode)GetValue(SearchModeProperty);

            set => SetValue(SearchModeProperty, value);
        }

        /// <summary>
        /// 显示多选框
        /// </summary>
        public bool ShowCheckBox
        {
            get => (bool)GetValue(ShowCheckBoxProperty);

            set => SetValue(ShowCheckBoxProperty, value);
        }

        /// <summary>
        /// 显示图像
        /// </summary>
        public bool ShowImage
        {
            get => (bool)GetValue(ShowImageProperty);

            set => SetValue(ShowImageProperty, value);
        }

        /// <summary>
        /// 显示搜索面板
        /// </summary>
        public bool ShowSearchPanel
        {
            get => (bool)GetValue(ShowSearchPanelProperty);

            set => SetValue(ShowSearchPanelProperty, value);
        }

        /// <summary>
        /// 搜索文本
        /// </summary>
        public string SearchText
        {
            get => (string)GetValue(SearchTextProperty);

            set => SetValue(SearchTextProperty, value);
        }

        /// <summary>
        /// 是否为主动搜索
        /// </summary>
        /// <remarks>
        /// 主动搜索时每次输入搜索内容都会立即进行搜索，非主动搜索时需要按下回车键才会进行搜索
        /// </remarks>
        public bool IsActiveSearch
        {
            get => (bool)GetValue(IsActiveSearchProperty);

            set => SetValue(IsActiveSearchProperty, value);
        }

        private void SearchPanel_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!IsActiveSearch)
                return;

            string newValue = searchPanel.Text;
            if (string.IsNullOrEmpty(newValue))
            {
                Binding binding = new Binding(nameof(ItemsSource))
                {
                    Source = this,
                };
                tree.SetBinding(ItemsControl.ItemsSourceProperty, binding);
                LordSelectedItem(SelectedItem);
            }
            else
            {
                switch (SearchMode)
                {
                    case SearchMode.NodeSearch:
                        NodeSearch(newValue);
                        break;
                    case SearchMode.FolderSearch:
                        FolderSearch(newValue);
                        break;
                    case SearchMode.LeafNodeSearch:
                        LeafNodeSearch(newValue);
                        break;
                }
            }

            foreach (var item in ItemsSource)
            {
                Tree<ITreeViewModel> tree = new Tree<ITreeViewModel>(item, v => v.ChildNodes);
                tree.Root.PostorderTraversal(n =>
                {
                    if ((n.ChildNodes != null) && n.ChildNodes.Any())
                    {
                        if (n.ChildNodes.All(v => v.IsChecked == true))
                        {
                            n.IsChecked = true;
                        }
                        else if (n.ChildNodes.All(v => v.IsChecked == false))
                        {
                            n.IsChecked = false;
                        }
                        else
                        {
                            n.IsChecked = null;
                        }
                    }
                });
            }
        }

        private void SearchPanel_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(IsActiveSearch)
                return;

            // 判断是否按下回车键（兼容普通回车和小键盘回车）
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                // 阻止回车的默认行为（避免文本框换行）
                e.Handled = true;

                string newValue = searchPanel.Text;
                if (string.IsNullOrEmpty(newValue))
                {
                    Binding binding = new Binding(nameof(ItemsSource))
                    {
                        Source = this,
                    };
                    tree.SetBinding(ItemsControl.ItemsSourceProperty, binding);
                    LordSelectedItem(SelectedItem);
                }
                else
                {
                    switch (SearchMode)
                    {
                        case SearchMode.NodeSearch:
                            NodeSearch(newValue);
                            break;
                        case SearchMode.FolderSearch:
                            FolderSearch(newValue);
                            break;
                        case SearchMode.LeafNodeSearch:
                            LeafNodeSearch(newValue);
                            break;
                    }
                }

                foreach (var item in ItemsSource)
                {
                    Tree<ITreeViewModel> tree = new Tree<ITreeViewModel>(item, v => v.ChildNodes);
                    tree.Root.PostorderTraversal(n =>
                    {
                        if ((n.ChildNodes != null) && n.ChildNodes.Any())
                        {
                            if (n.ChildNodes.All(v => v.IsChecked == true))
                            {
                                n.IsChecked = true;
                            }
                            else if (n.ChildNodes.All(v => v.IsChecked == false))
                            {
                                n.IsChecked = false;
                            }
                            else
                            {
                                n.IsChecked = null;
                            }
                        }
                    });
                }
            }
        }

        private void NodeSearch(string newValue)
        {
            List<TreeViewModel> itemsSource = new List<TreeViewModel>();
            foreach (var item in ItemsSource)
            {
                TreeViewModel root = new TreeViewModel(item, false);
                NodeSearch(root, item.ChildNodes, n => n.Title.Contains(newValue));

                if (root.Title.Contains(newValue))
                {
                    itemsSource.Add(root);
                }
                else
                {
                    itemsSource.AddRange(root.ChildItems);
                }
            }

            tree.ItemsSource = itemsSource;
            LordSelectedItem(SelectedItem);
        }

        private static void NodeSearch(TreeViewModel root, IEnumerable<ITreeViewModel> nodes, Func<ITreeViewModel, bool> condition)
        {
            if ((nodes == null) || !nodes.Any())
            {
                return;
            }

            Dictionary<TreeViewModel, List<ITreeViewModel>> groups = new Dictionary<TreeViewModel, List<ITreeViewModel>>();
            groups[root] = new List<ITreeViewModel>();
            foreach (var node in nodes)
            {
                if (condition(node))
                {
                    TreeViewModel viewModel = new TreeViewModel(node, false);
                    root.ChildItems.Add(viewModel);

                    if (condition(root))
                        viewModel.ParentItem = root;
                    if (node.ChildNodes != null)
                    {
                        groups[viewModel] = node.ChildNodes.ToList();
                    }
                }
                else
                {
                    if (node.ChildNodes != null)
                    {
                        groups[root].AddRange(node.ChildNodes);
                    }
                }
            }

            foreach (var item in groups)
            {
                if (item.Value.Count > 0)
                {
                    NodeSearch(item.Key, item.Value, condition);
                }
            }
        }

        private void FolderSearch(string newValue)
        {
            List<TreeViewModel> itemsSource = new List<TreeViewModel>();
            foreach (var item in ItemsSource)
            {
                if (item.Title.Contains(newValue))
                {
                    TreeViewModel root = new TreeViewModel(item, true);
                    itemsSource.Add(root);
                }
                else
                {
                    var roots = FolderSearch(item.ChildNodes, n => n.Title.Contains(newValue));
                    itemsSource.AddRange(roots);
                }
            }

            tree.ItemsSource = itemsSource;
            LordSelectedItem(SelectedItem);
        }

        private static List<TreeViewModel> FolderSearch(IEnumerable<ITreeViewModel> nodes, Func<ITreeViewModel, bool> condition)
        {
            if ((nodes == null) || !nodes.Any())
            {
                return new List<TreeViewModel>();
            }

            List<TreeViewModel> result = new List<TreeViewModel>();
            foreach (var node in nodes)
            {
                if (condition(node))
                {
                    TreeViewModel viewModel = new TreeViewModel(node, true);
                    result.Add(viewModel);
                }
                else
                {
                    var list = FolderSearch(node.ChildNodes, condition);
                    result.AddRange(list);
                }
            }

            return result;
        }

        private void LeafNodeSearch(string newValue)
        {
            List<TreeViewModel> itemsSource = new List<TreeViewModel>();
            foreach (var item in ItemsSource)
            {
                TreeViewModel root = new TreeViewModel(item, true);
                LeafNodeSearch(root.ChildItems, n => n.Title.Contains(newValue));
                if(root.ChildItems.Count > 0)
                {
                    itemsSource.Add(root);
                }
            }

            tree.ItemsSource = itemsSource;
            LordSelectedItem(SelectedItem);
        }

        private static void LeafNodeSearch(List<TreeViewModel> nodes, Func<ITreeViewModel, bool> condition)
        {
            List<TreeViewModel> removeNodes = new List<TreeViewModel>();

            foreach (var node in nodes)
            {
                if(node.ChildItems.Count > 0)
                {
                    LeafNodeSearch(node.ChildItems, condition);
                    if (node.ChildItems.Count == 0)
                    {
                        removeNodes.Add(node);
                    }
                }
                else
                {
                    if (!condition(node))
                    {
                        removeNodes.Add(node);
                    }
                }
            }

            foreach (var item in removeNodes)
            {
                nodes.Remove(item);
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckEdit_EditValueChanged(sender, true);
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckEdit_EditValueChanged(sender, false);
        }

        private void LordSelectedItem(ITreeViewModel selectedItem)
        {
            if (selectedItem == null)
                return;

            var selectedVal = tree.SelectedValue;
            if (selectedVal is TreeViewModel model && (model.ViewModel != null))
            {
                selectedVal = model.ViewModel;
            }

            if (selectedItem == selectedVal)
            {
                return;
            }

            TreeViewItem? target = null;
            if (string.IsNullOrEmpty(searchPanel.Text))
            {
                var path = GetNodePath(selectedItem);
                if (path.Count > 0)
                {
                    target = GetTreeViewItem(tree, selectedItem, path, 0);
                }
            }
            else
            {
                var source = tree.ItemsSource.Cast<TreeViewModel>();
                TreeViewModel? selectedNode = null;

                foreach (var root in source)
                {
                    Tree<TreeViewModel> tree = new Tree<TreeViewModel>(root, r => r.ChildItems);

                    foreach (var node in tree)
                    {
                        if (node.ViewModel == selectedItem)
                        {
                            selectedNode = node;
                            break;
                        }

                    }

                    if (selectedNode != null)
                        break;
                }

                if (selectedNode != null)
                {
                    var path = GetNodePath(selectedNode);
                    if (path.Count > 0)
                    {
                        target = GetTreeViewItem(tree, selectedNode, path, 0);
                    }
                }
            }

            if (target != null)
            {
                target.IsSelected = true;
            }
        }

        private List<int> GetNodePath(ITreeViewModel node)
        {
            List<int> path = new List<int>();

            var now = node;
            while (now.ParentNode != null)
            {
                var parent = now.ParentNode;
                var child = parent.ChildNodes.ToArray();
                var index = Array.IndexOf(child, now);
                if (index == -1)
                    return new List<int>();

                path.Add(index);

                now = parent;
            }

            var roots = tree.ItemsSource.Cast<ITreeViewModel>().ToArray();
            var start = Array.IndexOf(roots, now);
            if (start == -1)
                return new List<int>();

            path.Add(start);
            path.Reverse();

            return path;
        }

        private void CheckEdit_EditValueChanged(object sender, bool? value)
        {
            if (sender is FrameworkElement element && element.DataContext is ITreeViewModel node)
            {
                if (value == null)
                {
                    return;
                }

                bool flag = (bool)value;

                Tree<ITreeViewModel> tree = new Tree<ITreeViewModel>(node, v => v.ChildNodes);
                foreach (var item in tree)
                {
                    item.IsChecked = flag;
                }

                var parent = node.ParentNode;

                while (parent != null)
                {
                    bool? isChange;
                    if (parent.ChildNodes.All(n => n.IsChecked == true))
                    {
                        isChange = true;
                    }
                    else if (parent.ChildNodes.All(n => n.IsChecked == false))
                    {
                        isChange = false;
                    }
                    else
                    {
                        isChange = null;
                    }

                    if (parent.IsChecked == isChange)
                    {
                        parent = null;
                    }
                    else
                    {
                        parent.IsChecked = isChange;
                        parent = parent.ParentNode;
                    }
                }

                var oldVal = SelectedItems.ToList();

                SelectedItems.Clear();

                if (this.tree.ItemsSource is not IEnumerable<ITreeViewModel> roots)
                    return;

                foreach (var item in roots)
                {
                    Tree<ITreeViewModel> itemTree = new Tree<ITreeViewModel>(item, v => v.ChildNodes);
                    foreach (var val in itemTree)
                    {
                        switch (MultipleSelectionMode)
                        {
                            case MultipleSelectionMode.AllSelection:
                                if (val.IsChecked == true)
                                {
                                    if (!string.IsNullOrEmpty(searchPanel.Text) && val is TreeViewModel viewModel)
                                    {
                                        Debug.Assert(viewModel.ViewModel != null);
                                        SelectedItems.Add(viewModel.ViewModel);
                                    }
                                    else
                                    {
                                        SelectedItems.Add(val);
                                    }
                                }

                                break;
                            case MultipleSelectionMode.HasSelection:
                                if ((val.IsChecked == true) || (val.IsChecked == null))
                                {
                                    if (!string.IsNullOrEmpty(searchPanel.Text) && val is TreeViewModel viewModel)
                                    {
                                        Debug.Assert(viewModel.ViewModel != null);
                                        SelectedItems.Add(viewModel.ViewModel);
                                    }
                                    else
                                    {
                                        SelectedItems.Add(val);
                                    }
                                }

                                break;
                        }
                    }
                }

                var newVal = SelectedItems.ToList();
                RoutedPropertyChangedEventArgs<IList<ITreeViewModel>>
                    args = new RoutedPropertyChangedEventArgs<IList<ITreeViewModel>>(oldVal, newVal);
                SelectedItemsChanged?.Invoke(this, args);
            }
        }

        private void Tree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            ITreeViewModel? oldVal = e.OldValue as ITreeViewModel;
            ITreeViewModel? newVal = null;

            if (e.NewValue is ITreeViewModel)
            {
                newVal = (ITreeViewModel)e.NewValue;
                if (!string.IsNullOrEmpty(searchPanel.Text) && newVal is TreeViewModel viewModel)
                {
                    Debug.Assert(viewModel.ViewModel != null);
                    SelectedItem = viewModel.ViewModel;
                    newVal = viewModel.ViewModel;
                }
                else
                {
                    SelectedItem = newVal;
                }
            }

            RoutedPropertyChangedEventArgs<ITreeViewModel?> args = new RoutedPropertyChangedEventArgs<ITreeViewModel?>(oldVal, newVal);
            SelectedItemChanged?.Invoke(this, args);
        }

        private TreeViewItem? GetTreeViewItem(ItemsControl container, ITreeViewModel item)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var data = container.DataContext;
            if ((data == item) || (data is TreeViewModel view && (view.ViewModel == item)))
            {
                return container as TreeViewItem;
            }

            if (container is TreeViewItem && !((TreeViewItem)container).IsExpanded)
            {
                container.SetValue(TreeViewItem.IsExpandedProperty, true);
            }

            container.ApplyTemplate();
            ItemsPresenter? itemsPresenter;
            if (container.Template.FindName("ItemsHost", container) is ItemsPresenter presenter)
            {
                presenter.ApplyTemplate();
                itemsPresenter = presenter;
            }
            else
            {
                itemsPresenter= FindVisualChild<ItemsPresenter>(container);
                if (itemsPresenter == null)
                {
                    container.UpdateLayout();
                    itemsPresenter = FindVisualChild<ItemsPresenter>(container);
                }
            }

            if (itemsPresenter == null)
                return null;

            var itemsHostPanel = (Panel)VisualTreeHelper.GetChild(itemsPresenter, 0);
            var virtualizingPanel = itemsHostPanel as VirtualizingPanel;
            for (int i = 0, count = container.Items.Count; i < count; i++)
            {
                TreeViewItem subContainer;
                if (virtualizingPanel != null)
                {
                    // this is the part that requires .NET 4.5+
                    virtualizingPanel.BringIndexIntoViewPublic(i);
                    subContainer = (TreeViewItem)container.ItemContainerGenerator.ContainerFromIndex(i);
                }
                else
                {
                    subContainer = (TreeViewItem)container.ItemContainerGenerator.ContainerFromIndex(i);
                    subContainer.BringIntoView();
                }

                if (subContainer != null)
                {
                    TreeViewItem? resultContainer = GetTreeViewItem(subContainer, item);
                    if (resultContainer != null)
                    {
                        return resultContainer;
                    }

                    subContainer.IsExpanded = false;
                }
            }

            return null;
        }

        private TreeViewItem? GetTreeViewItem(ItemsControl container, ITreeViewModel item, IList<int> path, int pointer)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var data = container.DataContext;
            if ((data == item) || (data is TreeViewModel view && (view.ViewModel == item)))
            {
                return container as TreeViewItem;
            }

            if (container is TreeViewItem && !((TreeViewItem)container).IsExpanded)
            {
                container.SetValue(TreeViewItem.IsExpandedProperty, true);
            }

            container.ApplyTemplate();

            ItemsPresenter? itemsPresenter;
            if (container.Template.FindName("ItemsHost", container) is ItemsPresenter presenter)
            {
                presenter.ApplyTemplate();
                itemsPresenter = presenter;
            }
            else
            {
                itemsPresenter = FindVisualChild<ItemsPresenter>(container);
                if (itemsPresenter == null)
                {
                    container.UpdateLayout();
                    itemsPresenter = FindVisualChild<ItemsPresenter>(container);
                }
            }

            if (itemsPresenter == null)
                return null;

            var itemsHostPanel = (Panel)VisualTreeHelper.GetChild(itemsPresenter, 0);
            var virtualizingPanel = itemsHostPanel as VirtualizingPanel;

            var index = path[pointer];

            if (index >= container.Items.Count)
                return null;

            TreeViewItem subContainer;
            if (virtualizingPanel != null)
            {
                // this is the part that requires .NET 4.5+
                virtualizingPanel.BringIndexIntoViewPublic(index);
                subContainer = (TreeViewItem)container.ItemContainerGenerator.ContainerFromIndex(index);
            }
            else
            {
                subContainer = (TreeViewItem)container.ItemContainerGenerator.ContainerFromIndex(index);
                subContainer.BringIntoView();
            }

            if (subContainer != null)
            {
                TreeViewItem? resultContainer = GetTreeViewItem(subContainer, item, path, ++pointer);
                if (resultContainer != null)
                {
                    return resultContainer;
                }

                subContainer.IsExpanded = false;
            }

            return null;
        }

        private T? FindVisualChild<T>(Visual visual)
            where T : Visual
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(visual); i++)
            {
                if (VisualTreeHelper.GetChild(visual, i) is Visual child)
                {
                    if (child is T item)
                    {
                        return item;
                    }

                    var val = FindVisualChild<T>(child);
                    if (val != null)
                    {
                        return val;
                    }
                }
            }

            return null;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LordSelectedItem(SelectedItem);
        }
    }
}
