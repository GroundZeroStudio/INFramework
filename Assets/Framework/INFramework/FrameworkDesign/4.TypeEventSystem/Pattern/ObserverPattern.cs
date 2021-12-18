using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INFrameworkDesign.Example
{
	public class ObserverPattern : MonoBehaviour
	{
		/// <summary>
		/// 观察者
		/// </summary>
		abstract class Observer
        {
			public abstract void Update();
        }

		/// <summary>
		/// 主题
		/// </summary>
		abstract class Subject
        {
			List<Observer> m_Observers = new List<Observer>();

			public void Attach(Observer rObserver)
            {
				m_Observers.Add(rObserver);
            }

			public void Detach(Observer rObserver)
            {
				m_Observers.Remove(rObserver);
            }

			public void Notify()
            {
				m_Observers.ForEach(o => o.Update());
            }
        }

		class ConcreteSubject : Subject
        {
			protected string m_SubjectState = null;

			public void SetState(string rState)
            {
				m_SubjectState = rState;
				Notify();
            }

			public string GetState()
            {
				return m_SubjectState;
            }
        }

        class ConcreteObserver : Observer
        {
			private ConcreteSubject m_Subject = null;

			public ConcreteObserver(ConcreteSubject rSubject)
            {
				m_Subject = rSubject;
            }

            public override void Update()
            {
				Debug.Log("ConcreteObserver.Update");
				Debug.Log("ConcreteObserver:Subject 当前主题" + m_Subject.GetState());
            }
        }

        private void Start()
        {
			var subject = new ConcreteSubject();
			var observer = new ConcreteObserver(subject);
			subject.Attach(observer);
			subject.SetState("测试");
        }
    }
}

