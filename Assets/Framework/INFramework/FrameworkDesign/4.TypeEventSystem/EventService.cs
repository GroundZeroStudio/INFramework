using System;
using System.Collections.Generic;

namespace INFrameworkDesign
{
	public class EventService
	{
		private List<Action> m_UnRegisterEventActions = new List<Action>();

		public void Register<T>(Action<T> rOnReceive)
        {
            TypeEventSystem.Register<T>(rOnReceive);
            this.m_UnRegisterEventActions.Add(() =>
            {
                TypeEventSystem.UnRegister<T>(rOnReceive);
            });
        }

		public void UnRegister<T>(Action<T> rOnReceive)
        {
            TypeEventSystem.UnRegister<T>(rOnReceive);
        }

		public void Send<T>(T rEventKey)
        {
            TypeEventSystem.Send<T>(rEventKey);
        }

		public void UnRegisterAll()
        {
            m_UnRegisterEventActions.ForEach(action => action());
            m_UnRegisterEventActions.Clear();
        }
	}
}

