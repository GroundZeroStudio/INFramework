using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INFrameworkDesign.Example
{
	public class ObserverPatternWithTES : MonoBehaviour
	{
        /// <summary>
        /// 通知消息
        /// </summary>
		class NotifyEvent
        {

        }


        /// <summary>
        /// 主题
        /// </summary>
		class Subject
        {
			public void DoObserverInterestedThings()
            {
                Notify();
            }

            void Notify()
            {
                TypeEventSystem.Send(new NotifyEvent());
            }
        }

        class Observer
        {
            public Observer()
            {
                Subscribe();
            }

            void Subscribe()
            {
                TypeEventSystem.Register<NotifyEvent>(Update);
            }

            void Update(NotifyEvent rNotifyEvent)
            {
                Debug.Log("Received Notifycation");
            }

            public void Dispose()
            {
                TypeEventSystem.UnRegister<NotifyEvent>(Update);
            }
        }

        private void Start()
        {
            var subject = new Subject();
            var observer1 = new Observer();
            var observer2 = new Observer();
            subject.DoObserverInterestedThings();
        }
    }
}

