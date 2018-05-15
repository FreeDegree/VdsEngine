using System;
using System.IO;
using System.Xml.Serialization;
using VdsEngine;

namespace CSharpAssembly
{

	public class TestCall:MonoBehaviour
	{
		//public Window CreateTestWindow()
		//{
		//    TestWindow testWindow = new TestWindow();
		//    return testWindow;
		//}
		public void doAction(ref string pstr)
		{ }

		private void doAction()
		{
			TestTransmitParameter param = new TestTransmitParameter();
//			param.PropertyInt = 1;
			param.PropertyString = "Test";
//			param.PropertyEnum = TestTransmitParameter.TestEnum.TT1;

			MemoryStream ms = new MemoryStream();
			XmlSerializer bf = new XmlSerializer(typeof(TestTransmitParameter));
			XmlSerializerNamespaces snsp = new XmlSerializerNamespaces ();
			snsp.Add ("","");
			bf.Serialize(ms, param,snsp);


			ms.Seek(0, SeekOrigin.Begin);
			long size = ms.Length;
			byte[] cache = new byte[size];
			int count = ms.Read(cache, 0, 1);
			while (count < size)
			{
				cache[count++] = Convert.ToByte(ms.ReadByte());
			}
//			string result = Convert.ToBase64String(cache, 0, cache.Length);
//			System.Console.Write(Environment.StackTrace);
			string result = System.Text.Encoding.Default.GetString(cache);
			System.Console.Write(result);

////			string test = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<TestTransmitParameter xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\n  <PropertyInt>1</PropertyInt>\n  <PropertyString>Test</PropertyString>\n</TestTransmitParameter>";
////			string test1 = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<TestTransmitParameter xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\n<PropertyString>Test</PropertyString>\n<PropertyInt>1</PropertyInt>\n</TestTransmitParameter>";
//			string test2 = "<?xml version=\"1.0\" encoding=\"utf-8\"?><TestTransmitParameter><PropertyInt>test</PropertyInt><PropertyString>Test</PropertyString></TestTransmitParameter>";
////			string test3 = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<TestTransmitParameter>\n  <PropertyInt>1</PropertyInt>\n</TestTransmitParameter>";
////		
//			try
//			{
//				MemoryStream ms1 = new MemoryStream(System.Text.Encoding.Default.GetBytes(test2));
//				ms1.Seek(0, SeekOrigin.Begin);
//				XmlSerializer bf1 = new XmlSerializer(typeof(TestTransmitParameter));
//				object obj1 = bf1.Deserialize(ms1);
//				TestTransmitParameter tobj = (TestTransmitParameter)obj1;
//			}
//			catch 
//			{
//				System.Console.Write("Deserialize error!!");
//			}
		}
	}
}