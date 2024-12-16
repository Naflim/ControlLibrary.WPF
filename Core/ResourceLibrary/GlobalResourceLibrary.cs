using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naflim.ControlLibrary.WPF.Core.ResourceLibrary
{
    /// <summary>
    /// 全局资源库
    /// </summary>
    /// <remarks>用于UI模型对外界数据的查阅绑定</remarks>
    public class GlobalResourceLibrary
    {
        private static GlobalResourceLibrary? instance;

        private readonly Dictionary<Type, ResourceLibrary> library;

        private GlobalResourceLibrary()
        {
            library = new Dictionary<Type, ResourceLibrary>
        {
            {
                typeof(GlobalResourceLibrary),
                new ResourceLibrary(typeof(GlobalResourceLibrary))
            },
        };
        }

        /// <summary>
        /// 资源库单例
        /// </summary>
        public static GlobalResourceLibrary Instance
        {
            get
            {
                instance ??= new GlobalResourceLibrary();

                return instance;
            }
        }

        /// <summary>
        /// 全局资源字典
        /// </summary>
        /// <remarks>
        /// 此字典为整个应用程序提供服务
        /// </remarks>
        public ResourceLibrary GlobalResource => library[typeof(GlobalResourceLibrary)];

        /// <summary>
        /// 获取资源库
        /// </summary>
        /// <param name="type">资源类型</param>
        /// <returns>资源库</returns>
        public ResourceLibrary? GetResourceLibrary(Type type)
        {
            if (library.ContainsKey(type))
            {
                return library[type];
            }

            return null;
        }

        /// <summary>
        /// 获取资源所属值
        /// </summary>
        /// <param name="owner">所有者</param>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public object? GetValue(object owner, string key)
        {
            ResourceLibrary? resourceLibrary = GetResourceLibrary(owner.GetType());
            if (resourceLibrary == null)
            {
                return null;
            }

            return resourceLibrary.GetValue(owner, key);
        }

        /// <summary>
        /// 添加资源
        /// </summary>
        /// <param name="owner">所有者</param>
        /// <param name="key">键</param>
        /// <param name="value">资源</param>
        public void AddResource(object owner, string key, object value)
        {
            if (library.ContainsKey(owner.GetType()))
            {
                library[owner.GetType()].AddResource(owner, key, value);
            }
            else
            {
                library[owner.GetType()] = new ResourceLibrary(owner.GetType());
                library[owner.GetType()].AddResource(owner, key, value);
            }
        }

        /// <summary>
        /// 删除所属资源
        /// </summary>
        /// <param name="owner">所有者</param>
        /// <returns>结果</returns>
        public bool RemoveResource(object owner)
        {
            if (owner.GetType() == typeof(GlobalResourceLibrary))
            {
                return false;
            }

            if (library.ContainsKey(owner.GetType()))
            {
                if (library[owner.GetType()].UserCount == 1)
                {
                    return library.Remove(owner.GetType());
                }

                return library[owner.GetType()].RemoveResource(owner);
            }

            return false;
        }

        /// <summary>
        /// 用户组件直接通过Key获取值
        /// </summary>
        /// <param name="type">资源类型</param>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        /// <remarks>为UI组件提供接口，外部组件禁止使用</remarks>
        internal object? GetValueByControl(Type type, string key)
        {
            ResourceLibrary? resourceLibrary = GetResourceLibrary(type);
            if (resourceLibrary == null)
            {
                return null;
            }

            return resourceLibrary.GetValue(key);
        }
    }
}
