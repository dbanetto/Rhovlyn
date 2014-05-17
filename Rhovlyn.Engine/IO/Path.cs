using System;
using System.IO;
using System.Net;
using System.Net.Mime;
using Rhovlyn.Engine.Security;
using System.Text;

namespace Rhovlyn.Engine.IO
{
	public static class Path
	{
		static string cache_path = "Content/cache/"; 
		static int cache_timeout = 10; //Minutes

		public static bool AllowWebResouces { get; set; }
		public static bool AllowWebResoucesCaching { get; set; }
		public static int WebResoucesCacheTimeOut { 
			get { return cache_timeout; }
			set {
				cache_timeout = value;
				CheckCacheTimeOutAll();
			}
		}
		public static string WebResoucesCachePath
		{
			get { return cache_path; } 
			set
			{
				cache_path = value;
				if (cache_path[cache_path.Length-1] != '/')
					cache_path += '/';

				if (!Directory.Exists(cache_path))
				{
					Directory.CreateDirectory(cache_path);
				}
				CheckCacheTimeOutAll();
			}
		}

		public static Stream ResolvePath ( string path )
		{
			if ( AllowWebResouces && (path.StartsWith("http://") || path.StartsWith("https://")) )
			{
				if (AllowWebResoucesCaching)
				{
					var c_path = GetCachePath(path);
					CheckCacheTimeOut(c_path);
					if (File.Exists(c_path))
					{
						return new FileStream(c_path, FileMode.Open);
					}
					else
					{
						CacheFile(((HttpWebRequest)WebRequest.Create(path)).GetResponse().GetResponseStream(), c_path);
						return new FileStream(c_path, FileMode.Open);
					}
				}
				return ((HttpWebRequest)WebRequest.Create(path)).GetResponse().GetResponseStream();
			} else if (AllowWebResouces && path.StartsWith("ftp://"))
			{
				if (AllowWebResoucesCaching)
				{
					var c_path = GetCachePath(path);
					if (File.Exists(c_path))
					{
						return new FileStream(c_path, FileMode.Open);
					}
					else
					{
						CacheFile(((FtpWebRequest)WebRequest.Create(path)).GetResponse().GetResponseStream(), c_path);
						return new FileStream(c_path, FileMode.Open);
					}
				}
				return ((FtpWebRequest)WebRequest.Create(path)).GetResponse().GetResponseStream();
			}
			else if (File.Exists(path))
			{
				return new FileStream(path, FileMode.Open);
			}
			throw new IOException(path + " could not be resloved");
		}

		public static void CacheFile ( Stream data , string cachepath)
		{
			using (var fs = new BinaryWriter( new FileStream(cachepath, FileMode.Create)))
			{
				using (var reader = new BinaryReader(data))
				{
					try {
					while (reader.BaseStream.CanRead)
						fs.Write( reader.ReadByte() );
					} catch (EndOfStreamException ex ) { //Expected Error, everthing will be fine* 
					}
					fs.Flush();
				}
			}
		}

		public static string GetCachePath (string url )
		{
			var name = System.IO.Path.GetFileName(url);
			name = Hash.GetHashMD5Hex(Encoding.UTF8.GetBytes(url)) + "-" + name;
			return  cache_path + name;
		}

		public static void CheckCacheTimeOutAll ()
		{
			foreach (var file in Directory.EnumerateFiles(WebResoucesCachePath))
			{
				CheckCacheTimeOut( file , WebResoucesCacheTimeOut);
			}
		}

		public static void CheckCacheTimeOut (string file)
		{
			CheckCacheTimeOut(file, WebResoucesCacheTimeOut);
		}

		public static void CheckCacheTimeOut (string file , long minutes)
		{
			if (!File.Exists(file))
				return;

			FileInfo info = new FileInfo(file);
			var past = DateTime.Now.Subtract(info.LastWriteTime);
			if ( past.TotalMinutes > minutes )
			{
				File.Delete(file);
			}
		}
	}
}

