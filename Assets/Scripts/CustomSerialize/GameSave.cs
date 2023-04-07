using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
namespace CustomSerialize
{
    public static class GameSave
    {
        static string _localPath = Application.streamingAssetsPath + "/" + "RankList.xml";
        public static string PlayerName = "佚名";
        public static void AddXML(int kill,int second)
        {
            if (!File.Exists(_localPath) ) {
                XmlDocument xml = new XmlDocument();
                xml.CreateXmlDeclaration("1.0", "UTF-8", "");//设置xml文件编码格式为UTF-8
                XmlElement root = xml.CreateElement("Data");//创建根节点
                XmlElement info = xml.CreateElement("Info");//创建子节点
                info.SetAttribute("Name",PlayerName);//创建子节点属性名和属性值
                info.SetAttribute("Kill",kill.ToString());
                info.SetAttribute("LifeTime",second.ToString());
                root.AppendChild(info);//将子节点按照创建顺序，添加到xml
                xml.AppendChild(root);
                xml.Save(_localPath);//保存xml到路径位置
                Debug.Log("创建XML成功！");
            }
            else
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(_localPath);//加载xml文件
                XmlNode root = xml.SelectSingleNode("Data");//获取根节点
                XmlElement info = xml.CreateElement("Info");//创建新的子节点
                info.SetAttribute("Name",PlayerName);//创建子节点属性名和属性值
                info.SetAttribute("Kill",kill.ToString());
                info.SetAttribute("LifeTime",second.ToString());
                root.AppendChild(info);//将子节点按照创建顺序，添加到xml
                xml.AppendChild(root);
                xml.Save(_localPath);//保存xml到路径位置
                Debug.Log("添加XML成功！");
            }
        }

        public static List<(string, string, string)> ReadXML()
        {
            if ( File.Exists(_localPath) )
            {
                var list = Fundamental.ListPool<(string, string, string)>.Get();
                XmlDocument xml = new XmlDocument();
                xml.Load(_localPath);//加载xml文件
                XmlNodeList nodeList = xml.SelectSingleNode("Data").ChildNodes;
                foreach (XmlElement xe in nodeList) {//遍历所以子节点
                    if (xe.Name== "Info" ) {
                       list.Add((xe.GetAttribute("Name"),xe.GetAttribute("Kill"),xe.GetAttribute("LifeTime")));
                    }
                }                     
                Debug.Log("读取XML成功！"+xml.OuterXml);
                return list;
            }

            return null;
        }
    }
}