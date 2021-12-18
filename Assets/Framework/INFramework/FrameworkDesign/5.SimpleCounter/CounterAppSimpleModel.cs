using System;
using UniRx;

namespace INFrameworkDesign.Example
{
	public class CounterAppSimpleModel
	{
		public IntReactiveProperty Count = new IntReactiveProperty(0);
	}
}

