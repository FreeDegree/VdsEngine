using System;
using System.IO;
using System.Xml.Serialization;

namespace VdsEngine
{
    public class SerializableXML
    {
        /// <summary>
        /// 参数类序列化
        /// </summary>
        /// <returns></returns>
        public string SerializeXML()
        {
            MemoryStream ms = new MemoryStream();
            XmlSerializer bf = new XmlSerializer(this.GetType());
            XmlSerializerNamespaces snsp = new XmlSerializerNamespaces();
            snsp.Add("", "");
            bf.Serialize(ms, this, snsp);
            ms.Seek(0, SeekOrigin.Begin);
            long size = ms.Length;
            byte[] cache = new byte[size];
            int count = ms.Read(cache, 0, 1);
            while (count < size)
            {
                cache[count++] = Convert.ToByte(ms.ReadByte());
            }
            //string result = Convert.ToBase64String(cache, 0, cache.Length);
            string result = System.Text.Encoding.Default.GetString(cache);
            return result;
        }
        /// <summary>
        /// 参数类反序列化
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public object DeserializeXML(string s)
        {
            //MemoryStream ms = new MemoryStream(Convert.FromBase64String(s));
            try
            {
                MemoryStream ms = new MemoryStream(System.Text.Encoding.Default.GetBytes(s));
                ms.Seek(0, SeekOrigin.Begin);
                XmlSerializer bf = new XmlSerializer(this.GetType());
                object obj = bf.Deserialize(ms);
                return obj;
            }
            catch
            {
                System.Console.Write("Deserialize error!!");
                return false;
            }
        }
        /// <summary>
        /// 测试是否可以序列化成功
        /// </summary>
        /// <returns><c>true</c>, if deserialize XM was tested, <c>false</c> otherwise.</returns>
        /// <param name="s">S.</param>
        protected bool TestDeserializeXML(string s)
        {
            //MemoryStream ms = new MemoryStream(Convert.FromBase64String(s));
            try
            {
                MemoryStream ms = new MemoryStream(System.Text.Encoding.Default.GetBytes(s));
                ms.Seek(0, SeekOrigin.Begin);
                XmlSerializer bf = new XmlSerializer(this.GetType());
                object obj = bf.Deserialize(ms);
                obj = null;
                return true;
            }
            catch
            {
                System.Console.Write("Deserialize error!!");
                return false;
            }
        }
    }
}