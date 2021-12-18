/****************************************************
    文件：MissionSystem.cs
    作者：Olivia
    功能：Nothing
*****************************************************/

using UnityEngine;

namespace INFrameworkDesign.ServiceLocator.LayerdArchitectureExample
{
    public interface IMissionSystem : ISystem
    {
        void OnEvent(string rEventName);
    }

    public class MissionSystem : IMissionSystem
    {
        private int mJumpCount
        {
            get
            {
                return PlayerPrefs.GetInt("JUMP_COUNT");
            }
            set
            {
                PlayerPrefs.SetInt("JUMP_COUNT", value);
            }
        }

        public void OnEvent(string rEventName)
        {
            if(rEventName == "JUMP")
            {
                mJumpCount++;
                if(mJumpCount == 1)
                {
                    Debug.Log("第一次跳跃 任务完成");
                }

                if(mJumpCount == 5)
                {
                    Debug.Log("跳跃新手 任务完成");
                }

                if(mJumpCount == 10)
                {
                    Debug.Log("跳跃达人 任务完成");
                }
            }
        }
    }

}
