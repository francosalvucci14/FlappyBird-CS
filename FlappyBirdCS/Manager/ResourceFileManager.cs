using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace FlappyBirdCS.Manager
{
    public sealed class ResourceFileManager
    {
        //private const string appFilesPath = Path.GetDirectoryName(Application.ExecutablePath);
        private static volatile ResourceFileManager instance;
        private static object syncRoot = new Object();
        private const string filePathName =/*@"C:\Users\FSalvucci\source\repos\FlappyBirdCS\FlappyBirdCS\Properties\Resources.resx";*/ @"\Properties\Resources.resx";

        public XDocument _resourceManager;
        private ResourceFileManager() { }

        public void SetResources(string filename = filePathName)
        {
            if (_resourceManager == null)
            {
                _resourceManager = XDocument.Load(filename);
            }
        }
        public string getConfigData(string keyToCheck)
        {
            try
            {
                XElement result = _resourceManager.Root.Descendants("data")
                                          .Where(k => k.Attribute("name").Value == keyToCheck)
                                          .Select(k => k)
                                          .FirstOrDefault();
                return result.Element("value").FirstNode.ToString();
            }
#pragma warning disable CS0168 // La variabile 'ex' è dichiarata, ma non viene mai usata
            catch (Exception ex)
#pragma warning restore CS0168 // La variabile 'ex' è dichiarata, ma non viene mai usata
            {
                return "";
            }
        }
        public static ResourceFileManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null) { }
                        instance = new ResourceFileManager();
                    }
                }

                return instance;
            }
        }

    }
}
