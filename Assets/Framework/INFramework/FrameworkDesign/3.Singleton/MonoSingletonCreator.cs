using System.Linq;
using UnityEngine;

namespace INFrameworkDesign
{
	public static class MonoSingletonCreator
	{
		public static T CreateMonoSingleton<T>() where T:MonoBehaviour, ISingleton
        {
			//尝试获取场景内的T脚本
			var instance = Object.FindObjectOfType<T>();
            //如果存在则直接返回
            if (instance != null)
            {
                instance.OnSingletonInit();
                return instance;
            }

            var info = typeof(T);

            instance = info.GetCustomAttributes(false)
                .Cast<MonoSingletonPath>()
                .Select(path => CreateSingletonWithPath<T>(path.PathInHierachy, true))
                .FirstOrDefault();

            if (!instance)
            {
                var gameObject = new GameObject(typeof(T).Name);
                Object.DontDestroyOnLoad(gameObject);
                instance = gameObject.AddComponent<T>();
            }

            instance.OnSingletonInit();
            return instance;
        }

        /// <summary>
        /// 根据Path区创建Singleton
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rPath"></param>
        /// <param name="bDontDestroy"></param>
        /// <returns></returns>
        private static T CreateSingletonWithPath<T>(string rPath, bool bDontDestroy) where T : MonoBehaviour
        {
            var gameObj = GetOrCreateGameObjectWithPath(rPath, true, bDontDestroy);
            if (!gameObj)
            {
                gameObj = new GameObject("Singleton of" + typeof(T).Name);
                if (bDontDestroy)
                {
                    Object.DontDestroyOnLoad(gameObj);
                }
            }
            return gameObj.AddComponent<T>();
        }

        private static GameObject GetOrCreateGameObjectWithPath(string rPath, bool bBuild, bool bDontDestroy)
        {
            if (string.IsNullOrEmpty(rPath))
            {
                return null;
            }

            var subPath = rPath.Split('/');
            if(subPath.Length == 0)
            {
                return null;
            }

            return GetOrCreateGameObjectWithPathArray(null, subPath, 0, bBuild, bDontDestroy);
        }

        /// <summary>
        /// 递归找到叶子GameObject接地那
        /// </summary>
        /// <param name="rParentGameObj"></param>
        /// <param name="rSubPath"></param>
        /// <param name="nIndex"></param>
        /// <param name="bBuild"></param>
        /// <param name="bDontDestroy"></param>
        /// <returns></returns>
        private static GameObject GetOrCreateGameObjectWithPathArray(GameObject rParentGameObj, string[] rSubPath, int nIndex, bool bBuild, bool bDontDestroy)
        {
            while (true)
            {
                GameObject currentGameObj = null;
                if (!rParentGameObj)
                {
                    currentGameObj = GameObject.Find(rSubPath[nIndex]);
                }
                else
                {
                    var child = rParentGameObj.transform.Find(rSubPath[nIndex]);
                    if(child != null)
                    {
                        currentGameObj = child.gameObject;
                    }
                }

                if (!currentGameObj)
                {
                    if (bBuild)
                    {
                        currentGameObj = new GameObject(rSubPath[nIndex]);
                        if(rParentGameObj != null)
                        {
                            currentGameObj.transform.SetParent(rParentGameObj.transform);
                        }
                        if(bDontDestroy && nIndex == 0)
                        {
                            Object.DontDestroyOnLoad(currentGameObj);
                        }
                    }
                }

                if (!currentGameObj)
                {
                    return null;
                }

                //如果是叶子节点则直接返回
                if (++nIndex == rSubPath.Length) return currentGameObj;
                rParentGameObj = currentGameObj;
            }
        }
	}
}

