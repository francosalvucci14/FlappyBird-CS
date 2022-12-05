using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Serialization;
//using CreateReports.Manager;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace FlappyBirdCS.Manager
{
    /// <summary>
    /// FUNZIONE CHE PERMETTE LA SCRITTURA SUL FILE DI LOG.
    /// Il parametro LEVEL può essere settato accedendo al file XML che contiene tutte le costanti.
    /// </summary>
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public class LOLIB : IDisposable
    {
        [XmlInclude(typeof(Int32[,]))]
        [XmlInclude(typeof(Int32[,,]))]
        [XmlInclude(typeof(Int32[]))]
        public static string SerializeToString(object obj)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());

            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, obj);

                return writer.ToString();
            }
        }
        #region Queue Impl.

        /// <summary>
        /// Classe Tipizzata per la memorizzazione dei log nella Coda.
        /// </summary>
        class QueueParam
        {
            public string Code;
            public string message;
            public int lv;

            public QueueParam(string Code, int lv, string message)
            {
                this.Code = Code;
                this.message = message;
                this.lv = lv;
            }
        }

        /// <summary>
        /// Coda a Dimensione Fissa
        /// </summary>
        class LimitedQueue<T> : Queue<T>
        {
            /// <summary>
            /// Dimensione Coda
            /// </summary>
            public int Limit { get; set; }

            /// <summary>
            /// Costruisce la Coda memorizzando la Dimensione della Coda stessa
            /// </summary>
            /// <param name="limit"></param>
            public LimitedQueue(int limit)
                : base()
            {
                this.Limit = limit;
            }

            /// <summary>
            /// Accoda un messaggio, scodando l'ultimo se si è raggiunta la dimensione
            /// </summary>
            /// <param name="item"></param>
            public new void Enqueue(T item)
            {
                if (this.Count >= this.Limit)
                {
                    this.Dequeue();
                }
                base.Enqueue(item);
            }
        }
        public string ToJson(object value)
        {
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            return JsonConvert.SerializeObject(value, Newtonsoft.Json.Formatting.Indented, settings);
        }

        /// <summary>
        /// Dictionary di Code per la memorizzazione dei log
        /// </summary>
        class DictionaryQueue : Dictionary<string, LimitedQueue<QueueParam>>
        {
            /// <summary>
            /// Accoda messaggio di log nella Coda identificata dal 'Code'
            /// </summary>
            /// <param name="Code"></param>
            /// <param name="message"></param>
            /// <param name="lv"></param>
            /// 

            public void Enqueue(string Code, int lv, string message)
            {
                Enqueue(new QueueParam(Code, lv, message));
            }

            /// <summary>
            /// Accoda messaggio di log nella Coda identificata dal 'Code'
            /// </summary>
            /// <param name="p"></param>
            public void Enqueue(QueueParam p)
            {
                if (!this.ContainsKey(p.Code))
                    this.Add(p.Code, new LimitedQueue<QueueParam>(QUEUE_LOG_SIZE));

                this[p.Code].Limit = QUEUE_LOG_SIZE;

                this[p.Code].Enqueue(p);
            }

            /// <summary>
            /// Scoda il messaggio di log dalla Coda identificata dal 'Code'
            /// </summary>
            /// <param name="Code"></param>
            /// <returns></returns>
            public QueueParam Dequeue(string Code)
            {
                this[Code].Limit = QUEUE_LOG_SIZE;

                return this[Code].Dequeue();
            }
        }

        #endregion

        #region Queue Variables

        /// <summary>
        /// Valore del paramentro 'QUEUE_SIZE'
        /// </summary>
        public static string PARAM_QUEUE_SIZE
        {
            get
            {
                ResourceFileManager resourceFileManager;
                resourceFileManager = ResourceFileManager.Instance;
                resourceFileManager.SetResources();
                return resourceFileManager.getConfigData("QUEUE_SIZE");
            }
        }

        /// <summary>
        /// Valore del paramentro 'QUEUE_LOG_ON_ERROR'
        /// </summary>
        public static string PARAM_QUEUE_LOG_ON_ERROR
        {
            get
            {
                ResourceFileManager resourceFileManager;
                resourceFileManager = ResourceFileManager.Instance;
                resourceFileManager.SetResources();
                return resourceFileManager.getConfigData("QUEUE_LOG_ON_ERROR");
            }
        }

        /// <summary>
        /// Valore del paramentro 'QUEUE_LOG_ALL'
        /// </summary>
        public static string PARAM_QUEUE_LOG_ALL
        {
            get
            {
                ResourceFileManager resourceFileManager;
                resourceFileManager = ResourceFileManager.Instance;
                resourceFileManager.SetResources();
                return resourceFileManager.getConfigData("QUEUE_LOG_ALL");
            }
        }

        /// <summary>
        /// Grandezza di default della Coda
        /// </summary>
        public const int QUEUE_SIZE_DEFAULT = 50;

        /// <summary>
        /// Flag che stabilisce se Scodare tutti i log
        /// </summary>
        static int QUEUE_LOG_SIZE
        {
            get
            {
                int size = string.IsNullOrEmpty(PARAM_QUEUE_SIZE)
                    ? QUEUE_SIZE_DEFAULT
                    : Int32.Parse(PARAM_QUEUE_SIZE);

                return size;
            }
        }

        /// <summary>
        /// Flag che stabilisce se Scodare i messaggi di log in caso di Errore (3)
        /// </summary>
        static bool QUEUE_LOG_ON_ERROR
        {
            get
            {
                return !string.IsNullOrEmpty(PARAM_QUEUE_LOG_ON_ERROR)
                && (PARAM_QUEUE_LOG_ON_ERROR.ToLower().Trim().Equals("true")
                    || PARAM_QUEUE_LOG_ON_ERROR.ToLower().Trim().Equals("1"))
                ;
            }
        }

        /// <summary>
        /// Flag che stabilisce se Scodare tutti i log
        /// </summary>
        static bool QUEUE_LOG_ALL
        {
            get
            {
                return !string.IsNullOrEmpty(PARAM_QUEUE_LOG_ALL)
                && (PARAM_QUEUE_LOG_ALL.ToLower().Trim().Equals("true")
                    || PARAM_QUEUE_LOG_ALL.ToLower().Trim().Equals("1"))
                ;
            }
        }

        /// <summary>
        /// Istanza della CODA
        /// </summary>
        static DictionaryQueue QUEUE = new DictionaryQueue();

        #endregion

        #region JS-On Helpers

        /// <summary>
        /// Converter per i Byte Array
        /// </summary>
        class ByteArrayConverter : JavaScriptConverter
        {
            public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
            {
                return new byte[] { };
            }

            public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
            {
                return new Dictionary<string, object> { { "byte[]", "[length=" + ((byte[])obj).Length + "]" } };
            }

            public override IEnumerable<Type> SupportedTypes
            {
                get { return new[] { typeof(byte[]) }; }
            }
        }

        /// <summary>
        /// Istanza del Serializer
        /// </summary>
        static JavaScriptSerializer serializer = initJavaScriptSerializer();

        /// <summary>
        /// Inizializza il Serializer
        /// </summary>
        /// <returns></returns>
        private static JavaScriptSerializer initJavaScriptSerializer()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(new JavaScriptConverter[] { new ByteArrayConverter() });
            return serializer;
        }

        /// <summary>
        /// Serializza un Oggetto
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static object ser(object obj)
        {
            if (obj == null)
                return null;

            return serializer.Serialize(obj);
        }

        /// <summary>
        /// Serializza un Array di Oggetti
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static object[] serArray(object[] args)
        {
            if (args == null)
                return null;

            object[] result = new object[args.Length];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = ser(args[i]);
            }

            return result;
        }

        #endregion

        public const int LEVEL_INFO = 3;
        public const int LEVEL_DEBUG = 2;
        public const int LEVEL_ERROR = 1;
        public const int LEVEL_QUEUE = 0;

        public static readonly string DATETIME_FORMAT = "yyyy/MM/dd HH:mm:ss.fff";
        public static readonly string DATETIME_FILE_FORMAT = "yyyyMMddHHmmssfff";

        public static string TEMPLATE_PATH
        {
            get
            {
                ResourceFileManager resourceFileManager;
                resourceFileManager = ResourceFileManager.Instance;
                resourceFileManager.SetResources();
                return resourceFileManager.getConfigData("TEMPLATEPATH");
            }
        }

        public static string LOG_PATH
        {
            get
            {
                ResourceFileManager resourceFileManager;
                resourceFileManager = ResourceFileManager.Instance;
                resourceFileManager.SetResources();
                return resourceFileManager.getConfigData("LOGPATH");
            }
        }
        

        public static string IMAGES_PATH
        {
            get
            {
                ResourceFileManager resourceFileManager;
                resourceFileManager = ResourceFileManager.Instance;
                resourceFileManager.SetResources();
                return resourceFileManager.getConfigData("IMAGESPATH");
            }
        }

        public static int LOG_LEVEL
        {
            get
            {
                ResourceFileManager resourceFileManager;
                resourceFileManager = ResourceFileManager.Instance;
                resourceFileManager.SetResources();
                return int.Parse(resourceFileManager.getConfigData("LEVEL"));
            }
        }

        public object FileUtil { get; private set; }

        #region Dispose Impl.

        bool mDisposed = false;
        public int lErr = 0;

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool bDispoing)
        {
            if (mDisposed)
                return;

            if (bDispoing)
            {
                Console.WriteLine("sto chiamando il metodo Dispose per il gestore del log...");
                //Supressing the Finalization method call
                GC.SuppressFinalize(this);
            }
            mDisposed = true;
        }

        #endregion

        /// <summary>
        /// GENERA UN CODICE CHE SERVE A CREARE UN NOME UNIVOCO PER IL FILE DI LOG.
        /// </summary>
        /// <param name="Appname"></param>
        /// <returns></returns>
        static public string CodeGen(string Appname)
        {
            //DateTime data = DateTime.Now;
            //StringBuilder code = new StringBuilder();

            //if (data.Day.ToString().Length < 2) code.Append("0" + data.Day.ToString());
            //else code.Append(data.Day.ToString());

            //if (data.Month.ToString().Length < 2) code.Append("0" + data.Month.ToString());
            //else code.Append(data.Month.ToString());

            //if (data.Year.ToString().Length < 2) code.Append("0" + data.Year.ToString());
            //else code.Append(data.Year.ToString());

            //if (data.Hour.ToString().Length < 2) code.Append("0" + data.Hour.ToString());
            //else code.Append(data.Hour.ToString());

            //if (data.Minute.ToString().Length < 2) code.Append("0" + data.Minute.ToString());
            //else code.Append(data.Minute.ToString());

            //if (data.Second.ToString().Length < 2) code.Append("0" + data.Second.ToString());
            //else code.Append(data.Second.ToString());

            Guid id = Guid.NewGuid();
            return Appname + "_" + id.ToString();
        }
        /// <summary>
        /// Restituiscelo StackFrame da dove parte la chiamata al log.
        /// </summary>
        /// <returns></returns>
        public void RemoveFileLog(string Code)
        {
            Manager.FileUtil.RemoveFile(LOG_PATH + Code + ".log");
        }
        public void RenameFileLog(string oldName, string newName)
        {
            Manager.FileUtil.RenameFile(LOG_PATH + oldName + ".log", LOG_PATH + newName + ".log");
        }
        /// <summary>
        /// Restituiscelo StackFrame da dove parte la chiamata al log.
        /// </summary>
        /// <returns></returns>
        private StackFrame getCallFrame()
        {
            int i = 2;
            StackFrame frame = new StackTrace().GetFrame(i);
            while (frame.GetMethod().DeclaringType.Equals(this.GetType()))
            {
                frame = new StackTrace().GetFrame(++i);
            }
            return frame;
        }

        /// <summary>
        /// Scrive il messaggio nel file
        /// </summary>
        /// <param name="Code">id file</param>
        /// <param name="logMessage">message</param>
        private void Write(string Code, string logMessage)
        {
            Manager.FileUtil.WriteLineAppend(LOG_PATH, Code + ".log", logMessage);
            ///Manager.FileUtil.WriteLineAppend(LOG_PATH_SETUP, Code + ".log", logMessage);
        }
        /* DISABILTATO IL 23/10/2020
        /// <summary>
        /// Scrive il messaggio nel file
        /// </summary>
        /// <param name="Code">id file</param>
        /// <param name="logMessage">message</param>
        private void WriteSetup(string Code, string logMessage)
        {
            //Manager.FileUtil.WriteLineAppend(LOG_PATH, Code + ".log", logMessage);
            Manager.FileUtil.WriteLineAppend(LOG_PATH_SETUP, Code + ".log", logMessage);
        }*/

        /// <summary>
        /// Scrive il messaggio di log in base al Livello
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="lv"></param>
        /// <param name="message"></param>
        private void Write(string Code, int lv, string message)
        {
            string Priority = null;

            switch (lv)
            {
                case LEVEL_QUEUE:
                case LEVEL_ERROR:
                    Priority = " ERROR :      ";

                    break;
                case LEVEL_DEBUG:
                    Priority = " WARNING :    ";

                    break;
                case LEVEL_INFO:
                    Priority = " INFORMATION: ";

                    break;
            }

            string logMessage = Priority + "|" + Environment.MachineName.ToString() + "|" + DateTime.Now.ToString(DATETIME_FORMAT) + "|" + message;

            Write(Code, logMessage);
            // WriteSetup(Code,logMessage);
        }

        /* DISABILTATO IL 23/10/2020
        /// <summary>
        /// Scrive il messaggio di log in base al Livello
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="lv"></param>
        /// <param name="message"></param>
        private void WriteSetup(string Code, int lv, string message)
        {
            string Priority = null;

            switch (lv)
            {
                case LEVEL_QUEUE:
                case LEVEL_ERROR:
                    Priority = " ERROR :      ";

                    break;
                case LEVEL_DEBUG:
                    Priority = " WARNING :    ";

                    break;
                case LEVEL_INFO:
                    Priority = " INFORMATION: ";

                    break;
            }

            string logMessage = Priority + "|" + Environment.MachineName.ToString() + "|" + DateTime.Now.ToString(DATETIME_FORMAT) + "|" + message;

            //Write(Code, logMessage);
            WriteSetup(Code, logMessage);
        }*/

        /// <summary>
        /// Scrive gli elementi della coda
        /// </summary>
        /// <param name="code"></param>
        private void WriteQueue(string code)
        {
            if (!QUEUE.ContainsKey(code) || QUEUE[code].Count == 0)
                return;

            Write(code, "---- Writing Queue ==> " + QUEUE[code].Count + " elements ----");
            //WriteSetup(code, "---- Writing Queue ==> " + QUEUE[code].Count + " elements ----");
            for (QueueParam item = null; QUEUE[code].Count > 0;)
            {
                item = QUEUE.Dequeue(code);
                Write(item.Code, item.lv, item.message);
                //WriteSetup(item.Code, item.lv, item.message);
            }
        }

        /* DISABILTATO IL 23/10/2020
        /// <summary>
        /// Scrive gli elementi della coda
        /// </summary>
        /// <param name="code"></param>
        private void WriteQueueSetup(string code)
        {
            if (!QUEUE.ContainsKey(code) || QUEUE[code].Count == 0)
                return;

            //Write(code, "---- Writing Queue ==> " + QUEUE[code].Count + " elements ----");
            WriteSetup(code, "---- Writing Queue ==> " + QUEUE[code].Count + " elements ----");
            for (QueueParam item = null; QUEUE[code].Count > 0;)
            {
                item = QUEUE.Dequeue(code);
                Write(item.Code, item.lv, item.message);
                WriteSetup(item.Code, item.lv, item.message);
            }
        }*/

        /// <summary>
        /// Scrive il messaggio di log se il livello specificato è
        /// minore o uguale al parametro LEVEL.
        /// Questo metodo mantiene la compatibilità con quello precedente
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="lv"></param>
        /// <param name="message"></param>
        public void WriteOnLog(string Code, string message, int lv)
        {
            WriteOnLog(Code, lv, message);
        }

        /* DISABILTATO IL 23/10/2020
        /// <summary>
        /// Scrive il messaggio di log se il livello specificato è
        /// minore o uguale al parametro LEVEL.
        /// Questo metodo mantiene la compatibilità con quello precedente
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="lv"></param>
        /// <param name="message"></param>
        public void WriteOnLogSetup(string Code, string message, int lv)
        {
            WriteOnLogSetup(Code, lv, message);
        }*/

        /// <summary>
        /// Scrive il messaggio di log se il livello specificato è
        /// minore o uguale al parametro LEVEL
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="message"></param>
        /// <param name="lv"></param>
        public void WriteOnLog(string Code, int lv, string message)
        {
            message = MethodInfo() + message;

            if (lv < LOG_LEVEL || lv == LOG_LEVEL)
            {
                if ((QUEUE_LOG_ON_ERROR && lv == LEVEL_ERROR)
                    || lv == LEVEL_QUEUE)
                {
                    if (QUEUE_LOG_ALL)
                    {
                        foreach (String code in QUEUE.Keys)
                        {
                            WriteQueue(code);
                        }
                    }
                    else
                    {
                        WriteQueue(Code);
                    }
                }

                Write(Code, lv, message);

            }
            /* Disabilitato da Elio 14/05/2015
            else if (LOG_LEVEL <= LEVEL_INFO)
            {
                QUEUE.Enqueue(Code, lv, message);
            }*/
        }

        /* DISABILTATO IL 23/10/2020
        /// <summary>
        /// Scrive il messaggio di log se il livello specificato è
        /// minore o uguale al parametro LEVEL
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="message"></param>
        /// <param name="lv"></param>
        public void WriteOnLogSetup(string Code, int lv, string message)
        {
            message = MethodInfo() + message;

            if (lv < LOG_LEVEL || lv == LOG_LEVEL)
            {
                if ((QUEUE_LOG_ON_ERROR && lv == LEVEL_ERROR)
                    || lv == LEVEL_QUEUE)
                {
                    if (QUEUE_LOG_ALL)
                    {
                        foreach (String code in QUEUE.Keys)
                        {
                            WriteQueueSetup(code);
                        }
                    }
                    else
                    {
                        WriteQueueSetup(Code);
                    }
                }


                WriteSetup(Code, lv, message);
            }
            /* Disabilitato da Elio 14/05/2015
            else if (LOG_LEVEL <= LEVEL_INFO)
            {
                QUEUE.Enqueue(Code, lv, message);
            }*/
        //}

        /// <summary>
        /// Scrive il messaggio di log includendo la firma del metodo
        /// </summary>
        /// <param name="methodParamArray">parametri della firma</param>
        /// <param name="Code"></param>
        /// <param name="message"></param>
        /// <param name="lv"></param>
        public void WriteOnLog(object[] methodParamArray, string Code, int lv, string message)
        {
            WriteOnLog(Code, lv, MethodParams(methodParamArray) + " | " + message);
        }

        /* DISABILTATO IL 23/10/2020
        /// <summary>
        /// Scrive il messaggio di log includendo la firma del metodo
        /// </summary>
        /// <param name="methodParamArray">parametri della firma</param>
        /// <param name="Code"></param>
        /// <param name="message"></param>
        /// <param name="lv"></param>
        public void WriteOnLogSetup(object[] methodParamArray, string Code, int lv, string message)
        {
            WriteOnLogSetup(Code, lv, MethodParams(methodParamArray) + " | " + message);
        }*/

        /// <summary>
        /// Scrive il messaggio di log includendo la firma del metodo
        /// </summary>
        /// <param name="methodParam">parametro della firma</param>
        /// <param name="Code"></param>
        /// <param name="message"></param>
        /// <param name="lv"></param>
        public void WriteOnLog(object methodParam, string Code, int lv, string message)
        {
            WriteOnLog(Code, lv, MethodParams(methodParam) + " | " + message);
        }

        /* DISABILTATO IL 23/10/2020
        /// <summary>
        /// Scrive il messaggio di log includendo la firma del metodo
        /// </summary>
        /// <param name="methodParam">parametro della firma</param>
        /// <param name="Code"></param>
        /// <param name="message"></param>
        /// <param name="lv"></param>
        public void WriteOnLogSetup(object methodParam, string Code, int lv, string message)
        {
            WriteOnLogSetup(Code, lv, MethodParams(methodParam) + " | " + message);
        }*/

        /// <summary>
        /// Scrive il messaggio di log formattato
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="messageFormat"></param>
        /// <param name="arg"></param>
        /// <param name="lv"></param>
        public void WriteOnLog(string Code, int lv, string messageFormat, object arg)
        {
            WriteOnLog(Code, lv, string.Format(messageFormat, ser(arg)));
        }

        /* DISABILTATO IL 23/10/2020
        /// <summary>
        /// Scrive il messaggio di log formattato
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="messageFormat"></param>
        /// <param name="arg"></param>
        /// <param name="lv"></param>
        public void WriteOnLogSetup(string Code, int lv, string messageFormat, object arg)
        {
            WriteOnLogSetup(Code, lv, string.Format(messageFormat, ser(arg)));
        }*/

        /// <summary>
        /// Scrive il messaggio di log formattato
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="messageFormat"></param>
        /// <param name="args"></param>
        /// <param name="lv"></param>
        public void WriteOnLog(string Code, int lv, string messageFormat, params object[] args)
        {
            WriteOnLog(Code, lv, string.Format(messageFormat, serArray(args)));
        }

        /* DISABILTATO IL 23/10/2020
        /// <summary>
        /// Scrive il messaggio di log formattato
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="messageFormat"></param>
        /// <param name="args"></param>
        /// <param name="lv"></param>
        public void WriteOnLogSetup(string Code, int lv, string messageFormat, params object[] args)
        {
            WriteOnLogSetup(Code, lv, string.Format(messageFormat, serArray(args)));
        }*/

        /// <summary>
        /// Scrive il messaggio di log formattato includendo la firma del metodo
        /// </summary>
        /// <param name="methodParamArray"></param>
        /// <param name="Code"></param>
        /// <param name="lv"></param>
        /// <param name="messageFormat"></param>
        /// <param name="arg"></param>
        public void WriteOnLog(object[] methodParamArray, string Code, int lv, string messageFormat, object arg)
        {
            WriteOnLog(Code, lv, MethodParams(methodParamArray) + " | " + string.Format(messageFormat, ser(arg)));
        }

        /* DISABILTATO IL 23/10/2020
        /// <summary>
        /// Scrive il messaggio di log formattato includendo la firma del metodo
        /// </summary>
        /// <param name="methodParamArray"></param>
        /// <param name="Code"></param>
        /// <param name="lv"></param>
        /// <param name="messageFormat"></param>
        /// <param name="arg"></param>
        public void WriteOnLogSetup(object[] methodParamArray, string Code, int lv, string messageFormat, object arg)
        {
            WriteOnLogSetup(Code, lv, MethodParams(methodParamArray) + " | " + string.Format(messageFormat, ser(arg)));
        }*/

        /// <summary>
        /// Scrive il messaggio di log formattato includendo la firma del metodo
        /// </summary>
        /// <param name="methodParamArray"></param>
        /// <param name="Code"></param>
        /// <param name="lv"></param>
        /// <param name="messageFormat"></param>
        /// <param name="args"></param>
        public void WriteOnLog(object[] methodParamArray, string Code, int lv, string messageFormat, params object[] args)
        {
            WriteOnLog(Code, MethodParams(methodParamArray) + " | " + string.Format(messageFormat, serArray(args)), lv);
        }

        /* DISABILTATO IL 23/10/2020
        /// <summary>
        /// Scrive il messaggio di log formattato includendo la firma del metodo
        /// </summary>
        /// <param name="methodParamArray"></param>
        /// <param name="Code"></param>
        /// <param name="lv"></param>
        /// <param name="messageFormat"></param>
        /// <param name="args"></param>
        public void WriteOnLogSetup(object[] methodParamArray, string Code, int lv, string messageFormat, params object[] args)
        {
            WriteOnLogSetup(Code, MethodParams(methodParamArray) + " | " + string.Format(messageFormat, serArray(args)), lv);
        }*/

        /// <summary>
        /// Scrive il messaggio di log formattato includendo la firma del metodo
        /// </summary>
        /// <param name="methodParam"></param>
        /// <param name="Code"></param>
        /// <param name="lv"></param>
        /// <param name="messageFormat"></param>
        /// <param name="arg"></param>
        public void WriteOnLog(object methodParam, string Code, int lv, string messageFormat, object arg)
        {
            WriteOnLog(Code, MethodParams(methodParam) + " | " + string.Format(messageFormat, ser(arg)), lv);
        }

        /* DISABILTATO IL 23/10/2020
        /// <summary>
        /// Scrive il messaggio di log formattato includendo la firma del metodo
        /// </summary>
        /// <param name="methodParam"></param>
        /// <param name="Code"></param>
        /// <param name="lv"></param>
        /// <param name="messageFormat"></param>
        /// <param name="arg"></param>
        public void WriteOnLogSetup(object methodParam, string Code, int lv, string messageFormat, object arg)
        {
            WriteOnLogSetup(Code, MethodParams(methodParam) + " | " + string.Format(messageFormat, ser(arg)), lv);
        }*/

        /// <summary>
        /// Scrive il messaggio di log formattato includendo la firma del metodo
        /// </summary>
        /// <param name="methodParam"></param>
        /// <param name="Code"></param>
        /// <param name="lv"></param>
        /// <param name="messageFormat"></param>
        /// <param name="args"></param>
        public void WriteOnLog(object methodParam, string Code, int lv, string messageFormat, params object[] args)
        {
            WriteOnLog(Code, MethodParams(methodParam) + " | " + string.Format(messageFormat, serArray(args)), lv);
        }

        /* DISABILTATO IL 23/10/2020
        /// <summary>
        /// Scrive il messaggio di log formattato includendo la firma del metodo
        /// </summary>
        /// <param name="methodParam"></param>
        /// <param name="Code"></param>
        /// <param name="lv"></param>
        /// <param name="messageFormat"></param>
        /// <param name="args"></param>
        public void WriteOnLogSetup(object methodParam, string Code, int lv, string messageFormat, params object[] args)
        {
            WriteOnLogSetup(Code, MethodParams(methodParam) + " | " + string.Format(messageFormat, serArray(args)), lv);
        }*/

        /// <summary>
        /// Restituisce il nome del metodo
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        private string MethodInfo()
        {
            StackFrame frame = getCallFrame();
            StringBuilder method = new StringBuilder();
            method.Append(" ");
            method.Append(frame.GetMethod().DeclaringType.FullName);
            method.Append(".");
            method.Append(frame.GetMethod().Name);
            //method.Append("[");
            //method.Append(frame.GetFileLineNumber());
            //method.Append(",");
            //method.Append(frame.GetFileColumnNumber());
            //method.Append("] ");
            method.Append(" | ");
            return method.ToString();
        }

        /// <summary>
        /// Costruisce una stringa contenente il nome del parametro ed il rispettivo valore.
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private string MethodParams(object param)
        {
            return MethodParams(new object[] { param });
        }

        /// <summary>
        /// Costruisce una stringa contenente i nomi dei parametri ed i rispettivi valori.
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="paramArray"></param>
        /// <returns></returns>
        private string MethodParams(object[] paramArray)
        {
            StackFrame frame = getCallFrame();
            StringBuilder param = new StringBuilder();

            int iParam = 0;
            foreach (ParameterInfo pi in frame.GetMethod().GetParameters())
            {
                if (param.Length > 0)
                    param.Append(", ");

                param.Append(pi.Name);
                param.Append("={");
                param.Append(iParam++);
                param.Append("}");
            }

            //param.Insert(0, "(");
            //param.Append(")");

            if (paramArray.GetType().Equals(new object[0].GetType()))
                return string.Format(param.ToString(), serArray(paramArray));
            else
                return string.Format(param.ToString(), ser(paramArray));
        }

        /// <summary>
        /// Scrive nel Log il messaggio di errore e lo Stack Trace
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="lv"></param>
        /// <param name="e"></param>
        public void WriteErrorOnLog(string Code, int lv, Exception e)
        {
            WriteErrorOnLog(Code, lv, e, null);
        }

        /* DISABILTATO IL 23/10/2020
        /// <summary>
        /// Scrive nel Log il messaggio di errore e lo Stack Trace
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="lv"></param>
        /// <param name="e"></param>
        public void WriteErrorOnLogSetup(string Code, int lv, Exception e)
        {
            WriteErrorOnLogSetup(Code, lv, e, null);
        }*/

        /// <summary>
        /// Scrive nel Log il messaggio di errore e lo Stack Trace
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="lv"></param>
        /// <param name="e"></param>
        /// <param name="message"></param>
        public void WriteErrorOnLog(string Code, int lv, Exception e, string message)
        {
            string error = string.Format("Error={0}", e.Message);

            if (!string.IsNullOrEmpty(message))
            {
                error = message + " | " + error;
            }

            WriteOnLog(Code, lv, error);
            WriteOnLog(Code, lv, e.StackTrace);
        }

        /* DISABILTATO IL 23/10/2020
        /// <summary>
        /// Scrive nel Log il messaggio di errore e lo Stack Trace
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="lv"></param>
        /// <param name="e"></param>
        /// <param name="message"></param>
        public void WriteErrorOnLogSetup(string Code, int lv, Exception e, string message)
        {
            string error = string.Format("Error={0}", e.Message);

            if (!string.IsNullOrEmpty(message))
            {
                error = message + " | " + error;
            }

            WriteOnLogSetup(Code, lv, error);
            WriteOnLogSetup(Code, lv, e.StackTrace);
        }
        */

    }
}
