using System;
using System.Security.Cryptography;
using System.Text;

namespace Rhovlyn.Engine.Security
{
	public static class Hash
	{
		public static byte[] GetHashMD5 ( byte[] bytes )
		{
			HashAlgorithm algorithm = MD5.Create();
			return algorithm.ComputeHash(bytes);
		}

		public static string GetHashMD5Hex( byte[] bytes )
		{
			StringBuilder sb = new StringBuilder();
			foreach (var b in GetHashMD5(bytes))
				sb.Append(b.ToString("X2"));

			return sb.ToString();
		}

		public static byte[] GetHashSHA1( byte[] bytes )
		{
			HashAlgorithm algorithm = SHA1.Create();
			return algorithm.ComputeHash(bytes);
		}

		public static string GetHashSHA1Hex( byte[] bytes )
		{
			StringBuilder sb = new StringBuilder();
			foreach (var b in GetHashSHA1(bytes))
				sb.Append(b.ToString("X2"));

			return sb.ToString();
		}

	}
}

