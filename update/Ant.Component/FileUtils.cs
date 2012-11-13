using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace Ant.Component
{
    public class FileUtils
    {
        public static int ListenPort = 9560;
        public static int PackageSize = 1024 * 32;
        private static byte[] mNullData = new Byte[4096 * 1000];
        public const string PRIVATE_FILE = "private.data";
        public const string PUBLIC_FILE = "public.data"; 
        public static bool CreateFile(string name, long size)
        {
            if (System.IO.File.Exists(name ))
                return false;
            using (System.IO.FileStream stream = System.IO.File.Open(name , FileMode.Create, FileAccess.Write))
            {
                while (size > 0)
                {
                    if (size > mNullData.Length)
                    {
                        stream.Write(mNullData, 0, mNullData.Length);
                        size -= mNullData.Length;
                    }
                    else
                    {
                        stream.Write(mNullData, 0, Convert.ToInt32(size));
                        size = 0;
                    }
                }
            }
            return true;
        }
        public static int GetFilePackages(long filesize)
        {
            int count;
            if (filesize % PackageSize > 0)
            {
                count = Convert.ToInt32(filesize / PackageSize) + 1;
            }
            else
            {
                count = Convert.ToInt32(filesize / PackageSize);
            }

            return count;
        }
        public static byte[] FileRead(string filename, int index, int size)
        {
            using (Smark.Core.ObjectEnter oe = new Smark.Core.ObjectEnter(filename))
            {
                byte[] resutl = null;
                long length = (long)index * (long)size + size;
                using (System.IO.FileStream stream = System.IO.File.OpenRead(filename))
                {
                    if (length > stream.Length)
                    {
                        resutl = new byte[stream.Length - ((long)index * (long)size)];
                    }
                    else
                    {
                        resutl = new byte[size];
                    }
                    stream.Seek((long)index * (long)size, System.IO.SeekOrigin.Begin);
                    stream.Read(resutl, 0, resutl.Length);
                }
                return resutl;
            }
        }
        public static void FileWrite(string filename, int index, int size, byte[] data)
        {
            using (Smark.Core.ObjectEnter oe = new Smark.Core.ObjectEnter(filename))
            {
                using (System.IO.FileStream stream = System.IO.File.OpenWrite(filename))
                {
                    stream.Seek((long)index * (long)size, System.IO.SeekOrigin.Begin);
                    stream.Write(data, 0, data.Length);
                    stream.Flush();
                }
            }

        }
    }
}
