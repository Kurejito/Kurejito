using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Kurejito.Transport {
	public class HttpTransport : IHttpPostTransport {

		private IWebProxy proxy;

		/// <summary>Initialize a new HTTP transport.</summary>
		/// <remarks>This implementation is based on <see cref="System.Net.WebClient" />.</remarks>
		public HttpTransport() { }

		/// <summary>Allows you to pass in a web proxy - e.g. new WebProxy("localhost", 8888) if you're running Fiddler locally - for debugging HTTP traffic</summary>
		public HttpTransport(IWebProxy proxy) {
			this.proxy = proxy;
		}

		/// <summary>POSTs the supplied data to the specified URL using the application/x-www-form-urlencoded content type, 
		/// and return the response as an ASCII-encoded string.</summary>
		/// <param name="url">A string containing the full URL including protocol of the remote resource.</param>
		/// <param name="postData">A string containing the data to be submitted, encoded as HTTP POST form data.</param>
		/// <returns>The response body from the remote server as an ASCII-encoded string.</returns>
		public string Post(Uri uri, string postData) {
			using (var web = new WebClient()) {
				if (proxy != default(WebProxy)) web.Proxy = proxy;
				web.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
				byte[] requestData = Encoding.ASCII.GetBytes(postData);
				byte[] responseData = web.UploadData(uri, "POST", requestData);
				string response = Encoding.ASCII.GetString(responseData);
				return (response);
			}
		}
	}
}