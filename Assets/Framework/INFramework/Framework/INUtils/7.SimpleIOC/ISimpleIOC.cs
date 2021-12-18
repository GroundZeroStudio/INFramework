namespace INFramework
{
	public interface ISimpleIOC
	{
		/// <summary>
		/// 注册简单类型
		/// </summary>
		/// <typeparam name="T"></typeparam>
		void Register<T>();
		/// <summary>
		/// 注册为单例（非泛型）
		/// </summary>
		/// <param name="instance"></param>
		void RegisterInstance(object instance);
		/// <summary>
		/// 注册为单例（泛型）
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="instance"></param>
		void RegisterInstance<T>(object instance);
		/// <summary>
		/// 注册依赖
		/// </summary>
		/// <typeparam name="TBase"></typeparam>
		/// <typeparam name="TConcrete"></typeparam>
		void Register<TBase, TConcrete>() where TConcrete : TBase;
		/// <summary>
		/// 获取实例
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		T Resolve<T>() where T : class;
		/// <summary>
		/// 注入
		/// </summary>
		/// <param name="obj"></param>
		void Inject(object obj);
		/// <summary>
		/// 清空类型和实例
		/// </summary>
		void Clear();
	}
}

