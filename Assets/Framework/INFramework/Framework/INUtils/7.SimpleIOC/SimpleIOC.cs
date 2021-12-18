using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace INFramework
{

    public class SimpleIOCInject : Attribute
    {

    }

    public class SimpleIOC : ISimpleIOC
    {
        HashSet<Type> RegisterationsType = new HashSet<Type>();
        //所有实例
        Dictionary<Type, object> mInstances = new Dictionary<Type, object>();
        //依赖关系
        Dictionary<Type, Type> mDependencies = new Dictionary<Type, Type>();

        public void Register<T>()
        {
            RegisterationsType.Add(typeof(T));
        }

        public void RegisterInstance(object instance)
        {
            Type type = instance.GetType();
            mInstances.Add(type, instance);
        }

        public void RegisterInstance<T>(object instance)
        {
            Type type = typeof(T);
            mInstances.Add(type, instance);
        }

        public void Register<TBase, TConcrete>() where TConcrete : TBase
        {
            var baseType = typeof(TBase);
            var concreteType = typeof(TConcrete);
            mDependencies.Add(baseType, concreteType);
        }


        public T Resolve<T>() where T : class
        {
            Type type = typeof(T);
            return Resolve(type) as T;
        }

        object Resolve(Type type)
        {
            if (mInstances.ContainsKey(type))
            {
                return mInstances[type];
            }

            if (mDependencies.ContainsKey(type))
            {
                return Activator.CreateInstance(mDependencies[type]);
            }

            if (RegisterationsType.Contains(type))
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }

        public void Inject(object obj)
        {
            //获取有SimpleIOCInject特性的属性
            var propertyInfos = obj.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(SimpleIOCInject), true).Any());
            foreach (var propertyInfo in propertyInfos)
            {
                var instance = Resolve(propertyInfo.PropertyType);
                if(instance != null)
                {
                    propertyInfo.SetValue(obj, instance);
                }
                else
                {
                    Debug.LogErrorFormat("不能获取类型为{0}的对象", propertyInfo.PropertyType);
                }
            }
        }

        public void Clear()
        {
            RegisterationsType.Clear();
            mInstances.Clear();
            mDependencies.Clear();
        }
    }
}

