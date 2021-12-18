using System;
using System.Linq;
using System.Reflection;


namespace INFramework
{
	public class AssemblyUtil
	{

		/// <summary>
		/// 获取第一个编辑器程序集
		/// </summary>
		/// <returns></returns>
		public static Assembly GetEditorAssembly()
        {
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			Assembly editorAssembly = assemblies.First(assembly => assembly.FullName.StartsWith("Assembly-CSharp-Editor"));
			return editorAssembly;
        } 
	}
}

