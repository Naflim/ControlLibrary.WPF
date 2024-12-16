using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naflim.ControlLibrary.WPF.Core.ResourceLibrary
{
    /// <summary>
    /// 资源视图模型
    /// </summary>
    public class ResourceViewModel : ViewModelBase, IDisposable
    {
        /// <summary>
        /// 添加资源至全局资源库
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">资源</param>
        public void AddResource(string key, object value)
        {
            GlobalResourceLibrary.Instance.AddResource(this, key, value);
        }

        /// <summary>
        /// 释放资源库储存的资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                GlobalResourceLibrary.Instance.RemoveResource(this);
            }
        }
    }
}
