/*----------------------------------------------------------------
 * 
 * 
 * Hikvision SDK
 * 
 * 2019-7-29 
 * 
 * daniel.zhang
 * 
 * 文件名：StreamHelper.cs
 * 
 * 文件功能描述：文件流
 * 
 * 修改记录：
 * 
----------------------------------------------------------------*/
using System;
using System.IO;
using System.Net;

namespace Com.Hikvision.Wrapper.Core.Utility
{
    public class StreamHelper
    {
        /// <summary>
        /// 解压缩，如果有使用
        /// </summary>
        /// <param name="hwResp"></param>
        /// <returns></returns>
        public static Stream Gzip(HttpWebResponse hwResp)
        {
            Stream stream1 = hwResp.GetResponseStream();

            if (hwResp.ContentEncoding.ToLower().Contains("gzip"))
            {
                stream1 = new System.IO.Compression.GZipStream(stream1, System.IO.Compression.CompressionMode.Decompress);
            }
            else if (hwResp.ContentEncoding.ToLower().Contains("deflate"))
            {
                stream1 = new System.IO.Compression.DeflateStream(stream1, System.IO.Compression.CompressionMode.Decompress);
            }
            return stream1;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Stream GetStreamFromUrl(string url)
        {
            HttpWebRequest re = (HttpWebRequest)HttpWebRequest.Create(url);
            HttpWebResponse rep = null;
            Stream oresponseStream = null;
            //try
            {
                rep = (HttpWebResponse)re.GetResponse();
                //oresponseStream = rep.GetResponseStream();
                oresponseStream = Gzip(rep);
                return oresponseStream;
            }
            //catch (Exception err)
            {
                //return null;
            }
        }

        public static MemoryStream StreamToMemoryStream(Stream instream)
        {
            MemoryStream outstream = new MemoryStream();
            const int bufferLen = 4096;
            byte[] buffer = new byte[bufferLen];
            int count = 0;
            while ((count = instream.Read(buffer, 0, bufferLen)) > 0)
            {
                outstream.Write(buffer, 0, count);
            }
            return outstream;
        }
    }
}
