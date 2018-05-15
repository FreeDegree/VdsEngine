using System.Collections.Generic;

namespace CSharpAssembly
{
	public class TestTransmitParameter //: GTransmitParameterBase
	{
		public enum TestEnum
		{
			TT0,
			TT1,
			TT2,
		}
		//public class TestClassParameter
		//{
		//	public int PropertyInt
		//	{
		//		get;
		//		set;
		//	}
		//	public List<int> PropertyIntList
		//	{
		//		get;
		//		set;
		//	}
		//	public int PropertyString
		//	{
		//		get;
		//		set;
		//	}
		//	public List<string> PropertyStringList
		//	{
		//		get;
		//		set;
		//	}
		//}
		public TestEnum PropertyEnum
		{
			get;
			set;
		}
		public int PropertyInt
		{
			get;
			set;
		}
		//public List<int> PropertyIntList
		//{
		//	get;
		//	set;
		//}
		public string PropertyString
		{
			get;
			set;
		}
		//public List<string> PropertyStringList
		//{
		//	get;
		//	set;
		//}
		//public TestClassParameter PropertyClass
		//{
		//	get;
		//	set;
		//}
		//public List<TestClassParameter> PropertyClassList
		//{
		//	get;
		//	set;
		//}
	}
}