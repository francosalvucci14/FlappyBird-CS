using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBirdCS.Manager
{
    public class FileUtil
    {
        public static void WriteLineAppend(string fileName, string line)
        {
            WriteLineAppend(null, fileName, line);
        }
        public static bool MaterializeFile(string sPathStart, byte[] byteArr)
        {
            bool bReturn = false;
            System.IO.FileStream _FileStream =
                new System.IO.FileStream(sPathStart, System.IO.FileMode.Create,
                                 System.IO.FileAccess.Write);
            // Writes a block of bytes to this stream using data from
            // a byte array.
            _FileStream.Write(byteArr, 0, byteArr.Length);

            // close file stream
            _FileStream.Close();
            bReturn = true;
            return bReturn;
        }
        public static string GetUniquePath(string sPathStart = "")
        {
            string uniquePath;
            do
            {
                Guid guid = Guid.NewGuid();
                string uniqueSubFolderName = guid.ToString();
                if (sPathStart != "")
                    uniquePath = sPathStart + uniqueSubFolderName;
                else
                    uniquePath = Path.GetTempPath() + uniqueSubFolderName;
            } while (Directory.Exists(uniquePath));
            Directory.CreateDirectory(uniquePath);
            return uniquePath;
        }

        public static void RenameFile(string oldFileName, string newFileName)
        {
            try
            {
                File.Move(oldFileName, newFileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void RemoveFile(string fileName)
        {
            try
            {
                File.Delete(fileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void WriteLineAppend(string path, string fileName, string line)
        {


            string file = !string.IsNullOrEmpty(path)
                ? path + "\\" + fileName
                : fileName;


            if (!string.IsNullOrEmpty(path) && !File.Exists(path))
                System.IO.Directory.CreateDirectory(path);

            if (!File.Exists(file))
            {

                // Creo il file in cui scrivere
                using (StreamWriter sw = new StreamWriter(file))
                {
                    try
                    {
                        sw.WriteLine(line);
                        //svuoto il buffer            
                        sw.Flush();

                        //chiudo lo stream
                        sw.Close();
                    }
                    catch (Exception e)
                    {
                        if (sw != null)
                        {
                            //svuoto il buffer 
                            sw.Flush();
                            //chiudo lo stream
                            sw.Close();
                        }

                        throw e;
                    }
                }
            }
            else
                //in questo caso aggiungo i messaggi in coda
                using (StreamWriter sw = new StreamWriter(file, true))
                {
                    try
                    {
                        sw.WriteLine(line);
                        //svuoto il buffer            
                        sw.Flush();
                        //chiudo lo stream
                        sw.Close();
                    }
                    catch (Exception e)
                    {
                        if (sw != null)
                        {
                            //svuoto il buffer 
                            sw.Flush();
                            //chiudo lo stream
                            sw.Close();
                        }

                        throw e;
                    }

                }


        }
        public static byte[] ReadFully(string file, int initialLength = 0)
        {
            byte[] ret = null;
            // If we've been passed an unhelpful initial length, just
            // use 32K.
            if (initialLength < 1)
            {
                initialLength = 32768;
            }
            FileStream s = new FileStream(file, FileMode.Open, FileAccess.Read);
            using (s)
            {

                byte[] buffer = new byte[initialLength];
                long read = 0;

                int chunk;
                while ((chunk = s.Read(buffer, int.Parse(read.ToString()), buffer.Length - int.Parse(read.ToString()))) > 0)
                {
                    read += chunk;

                    // If we've reached the end of our buffer, check to see if there's
                    // any more information
                    if (read == buffer.Length)
                    {
                        int nextByte = s.ReadByte();

                        // End of stream? If so, we're done
                        if (nextByte == -1)
                        {
                            return buffer;
                        }

                        // Nope. Resize the buffer, put in the byte we've just
                        // read, and continue
                        byte[] newBuffer = new byte[buffer.Length * 2];
                        Array.Copy(buffer, newBuffer, buffer.Length);
                        newBuffer[read] = (byte)nextByte;
                        buffer = newBuffer;
                        read++;
                    }
                }
                // Buffer is now too big. Shrink it.
                ret = new byte[read];
                Array.Copy(buffer, ret, read);
            }

            return ret;
        }

        public static void ReadAllToString(string path, out string sContent)
        {
            sContent = string.Empty;
            using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    sContent += line;
                }
            }
        }
    }
}
