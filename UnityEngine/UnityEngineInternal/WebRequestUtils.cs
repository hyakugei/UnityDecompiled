using System;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEngineInternal
{
	internal static class WebRequestUtils
	{
		private static Regex domainRegex = new Regex("^\\s*\\w+(?:\\.\\w+)+(\\/.*)?$");

		[RequiredByNativeCode]
		internal static string RedirectTo(string baseUri, string redirectUri)
		{
			Uri uri;
			if (redirectUri[0] == '/')
			{
				uri = new Uri(redirectUri, UriKind.Relative);
			}
			else
			{
				uri = new Uri(redirectUri, UriKind.RelativeOrAbsolute);
			}
			string absoluteUri;
			if (uri.IsAbsoluteUri)
			{
				absoluteUri = uri.AbsoluteUri;
			}
			else
			{
				Uri baseUri2 = new Uri(baseUri, UriKind.Absolute);
				Uri uri2 = new Uri(baseUri2, uri);
				absoluteUri = uri2.AbsoluteUri;
			}
			return absoluteUri;
		}

		internal static string MakeInitialUrl(string targetUrl, string localUrl)
		{
			string result;
			if (targetUrl.StartsWith("jar:file://"))
			{
				result = targetUrl;
			}
			else if (targetUrl.StartsWith("blob:http"))
			{
				result = targetUrl;
			}
			else
			{
				Uri uri = new Uri(localUrl);
				Uri uri2 = null;
				if (targetUrl[0] == '/')
				{
					uri2 = new Uri(uri, targetUrl);
				}
				if (uri2 == null && WebRequestUtils.domainRegex.IsMatch(targetUrl))
				{
					targetUrl = uri.Scheme + "://" + targetUrl;
				}
				FormatException ex = null;
				try
				{
					if (uri2 == null && targetUrl[0] != '.')
					{
						uri2 = new Uri(targetUrl);
					}
				}
				catch (FormatException ex2)
				{
					ex = ex2;
				}
				if (uri2 == null)
				{
					try
					{
						uri2 = new Uri(uri, targetUrl);
					}
					catch (FormatException)
					{
						throw ex;
					}
				}
				if (targetUrl.StartsWith("file://", StringComparison.OrdinalIgnoreCase))
				{
					result = ((!targetUrl.Contains("%")) ? targetUrl : WWWTranscoder.URLDecode(targetUrl, Encoding.UTF8));
				}
				else
				{
					result = ((!targetUrl.Contains("%")) ? uri2.AbsoluteUri : uri2.OriginalString);
				}
			}
			return result;
		}
	}
}
