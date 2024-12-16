using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naflim.ControlLibrary.WPF.Core.ProcessManagement
{
    /// <summary>
    /// 业务模型
    /// </summary>
    /// <remarks>
    /// 可在模型下绑定动态业务
    /// </remarks>
    public class ProcessModel : ViewModelBase
    {
        /// <summary>
        /// 构造业务中介者以实现业务绑定
        /// </summary>
        public ProcessModel()
        {
            ProcessMediator = new ProcessMediator(this);
            ProcessMediator.ProcessCombing();
        }

        /// <summary>
        /// 为刷新数据源使用的属性
        /// </summary>
        [Display(AutoGenerateField = false)]
        public bool RefreshBindModel => true;

        /// <summary>
        /// 流程中介者
        /// </summary>
        protected ProcessMediator ProcessMediator { get; }

        /// <summary>
        /// 设置附属业务
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="get">get方法</param>
        /// <param name="set">set法</param>
        public void SetProcess(string key, Func<object> get, Action<object> set)
        {
            ProcessMediator.SetProces4Get(key, get);
            ProcessMediator.SetProces4Set(key, set);
        }

        /// <summary>
        /// 设置附属业务的get方法
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="func">get方法</param>
        public void SetProces4Get(string key, Func<object> func)
        {
            ProcessMediator.SetProces4Get(key, func);
        }

        /// <summary>
        /// 设置附属业务的set方法
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="action">set方法</param>
        public void SetProces4Set(string key, Action<object> action)
        {
            ProcessMediator.SetProces4Set(key, action);
        }

        /// <summary>
        /// 调用指定业务get方法
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public object GetProcessData(string key)
        {
            return ProcessMediator.Get(key);
        }

        /// <summary>
        /// 调用指定业务set方法
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="data">数据</param>
        public void SetProcessData(string key, object data)
        {
            ProcessMediator.Set(key, data);
            OnPropertyChanged();
        }

        /// <summary>
        /// 刷新数据源
        /// </summary>
        public void RefreshProcessData()
        {
            OnPropertyChanged(nameof(RefreshBindModel));
        }

        /// <summary>
        /// 刷新数据源
        /// </summary>
        /// <param name="propertyName">属性名</param>
        public void RefreshProcessData(string propertyName)
        {
            var processOriginators = ProcessMediator.ProcessOriginators;
            if (processOriginators.ContainsKey(propertyName))
            {
                var originator = processOriginators[propertyName];
                if (originator.Value != originator.GetValue())
                {
                    originator.RefreshValue();
                    OnPropertyChanged(nameof(RefreshBindModel));
                }
            }
            else
            {
                OnPropertyChanged(nameof(RefreshBindModel));
            }
        }
    }
}
