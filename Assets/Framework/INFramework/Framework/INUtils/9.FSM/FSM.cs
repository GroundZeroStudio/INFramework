/****************************************************
    文件：FSM.cs
    作者：TA94
    功能：简易有限状态机
*****************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace INFramework
{
    public class FSM
    {
        public delegate void FSMTranslationCallfunc();

        /// <summary>
        /// 跳转类
        /// </summary>
        public class FSMTranslation
        {
            public FSMState                 fromState;
            public string                   name;
            public FSMState                 toState;
            public FSMTranslationCallfunc   callfunc;

            public FSMTranslation(FSMState fromState, string name, FSMState toState, FSMTranslationCallfunc callfunc)
            {
                this.fromState = fromState;
                this.name = name;
                this.toState = toState;
                this.callfunc = callfunc;
            }
        }

        /// <summary>
        /// 状态类
        /// </summary>
        public class FSMState 
        {
            /// <summary>
            /// 状态名字
            /// </summary>
            private string name;
            public string Name => name; 

            public FSMState(string name)
            {
                this.name = name;
            }
            /// <summary>
            // 
            /// </summary>
            public Dictionary<string, FSMTranslation> TranslationDict = new Dictionary<string, FSMTranslation>();
        }

        private FSMState mCurState;
        /// <summary>
        /// 当前状态
        /// </summary>
        public FSMState CurState => this.mCurState;

        Dictionary<string, FSMState> StateDict = new Dictionary<string, FSMState>();

        public void AddState(FSMState state)
        {
            StateDict[state.Name] = state;
        }
        /// <summary>
        /// 添加转移条件
        /// </summary>
        /// <param name="translation"></param>
        public void AddTranslation(FSMTranslation translation)
        {
            StateDict[translation.fromState.Name].TranslationDict[translation.name] = translation;
        }

        /// <summary>
        /// 启动状态机
        /// </summary>
        /// <param name="state"></param>
        public void Start(FSMState state)
        {
            mCurState = state;
        }

        public void HandleEvent(string name)
        {
            if(mCurState != null && mCurState.TranslationDict.ContainsKey(name))
            {
                Debug.Log("formState:" + mCurState.Name);
                mCurState.TranslationDict[name].callfunc();
                mCurState = mCurState.TranslationDict[name].toState;

                Debug.LogWarning("toState:" + mCurState.Name);
            }
        }
    }
}


