using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naflim.ControlLibrary.WPF.Core.ResourceLibrary
{
    /// <summary>
    /// 资源库
    /// </summary>
    public class ResourceLibrary
    {
        private readonly Dictionary<object, Dictionary<string, object>> library;

        private object? currentUser;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type">类型</param>
        public ResourceLibrary(Type type)
        {
            Type = type;
            library = new Dictionary<object, Dictionary<string, object>>();
        }

        /// <summary>
        /// 资源类型
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// 用户数量
        /// </summary>
        public int UserCount => library.Count;

        /// <summary>
        /// 获取所属资源字典
        /// </summary>
        /// <param name="owner">所有者</param>
        /// <returns>资源字典</returns>
        public Dictionary<string, object>? GetResourcesDictionary(object owner)
        {
            if (library.ContainsKey(owner))
            {
                return library[owner];
            }

            return null;
        }

        /// <summary>
        /// 获取资源
        /// </summary>
        /// <param name="owner">所有者</param>
        /// <param name="key">键</param>
        /// <returns>资源</returns>
        public object? GetValue(object owner, string key)
        {
            if (library.ContainsKey(owner) && library[owner].ContainsKey(key))
            {
                return library[owner][key];
            }

            return null;
        }

        /// <summary>
        /// 添加资源
        /// </summary>
        /// <param name="owner">所有者</param>
        /// <param name="key">键</param>
        /// <param name="value">资源</param>
        public void AddResource(object owner, string key, object value)
        {
            if (!library.ContainsKey(owner))
            {
                library[owner] = new Dictionary<string, object>();
            }

            library[owner][key] = value;
            currentUser = owner;
        }

        /// <summary>
        /// 删除其下所属资源
        /// </summary>
        /// <param name="owner">所有者</param>
        /// <returns>结果</returns>
        public bool RemoveResource(object owner)
        {
            var result = library.Remove(owner);
            if (owner == currentUser)
            {
                currentUser = library.Keys.LastOrDefault();
            }

            return result;
        }

        /// <summary>
        /// 获取当前资源
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>资源</returns>
        /// <remarks>为UI组件提供接口，外部组件禁止使用</remarks>
        internal object? GetValue(string key)
        {
            if(currentUser == null)
                return null;

            return GetValue(currentUser, key);
        }
    }
}
