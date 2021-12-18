using System;

namespace INFrameworkDesign
{
	[AttributeUsage(AttributeTargets.Class)]
	public class MonoSingletonPath : Attribute
	{
		private string m_PathInHierachy;

		public MonoSingletonPath(string rPathInHierachy)
        {
			m_PathInHierachy = rPathInHierachy;
        }

		public string PathInHierachy => m_PathInHierachy;
	}
}

