/****************************************************
    文件：Example.cs
    作者：TA94
    日期：#CreateTime#
    功能：Nothing
*****************************************************/
using System.Linq;
using UnityEngine;

namespace INFrameworkDesign.ServiceLocator.Pattern.Example
{
    public class Example : MonoBehaviour
    {
        public class InitialContext : AbstractInitialContext
        {
            public override IService LookUp(string rName)
            {
                ////获取Example所在的Service
                //var assembly = typeof(Example).Assembly;
                //var serviceType = typeof(IService);

                //var service = assembly.GetTypes()
                //    .Where((t) => serviceType.IsAssignableFrom(t) && !t.IsAbstract)
                //    .Select(t => t.GetConstructors().First().Invoke(null))
                //    .Cast<IService>()
                //    .SingleOrDefault(s => s.Name == rName);

                IService service = null;
                if(rName == "bluetooth")
                {
                    service = new BluetoothService();
                }

                return service;
            }
        }

        public class BluetoothService : IService
        {
            public string Name => "bluetooth";

            public void Execute()
            {
                Debug.Log("蓝牙服务启动");
            }
        }

        private void Start()
        {
            //创建查找器
            var context = new InitialContext();
            //创建服务定位器
            var serviceLocator = new ServiceLocator(context);
            //获取蓝牙服务
            var bluetoothService = serviceLocator.GetService("bluetooth");
            //执行蓝牙服务
            bluetoothService.Execute();
        }
    }


}


