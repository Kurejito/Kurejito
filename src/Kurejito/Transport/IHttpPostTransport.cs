using System;

namespace Kurejito.Transport {
	/// <summary>Defines behaviour for a class that can submit requests via HTTP/HTTPS POST and return the response as text.</summary>
	public interface IHttpPostTransport {
		/// <summary>Submits the specified <paramref name="postData"/> to the specified <paramref name="uri"/> and return the 
		/// response body as a string.</summary>
		/// <param name="uri">The URI to which the data should be posted, including the http:// or https:// protocol prefix.</param>
		/// <param name="postData">The encoded data to POST to the specified URL.</param>
		/// <returns>A string containing the raw response body from the remote server.</returns>
		string Post(Uri uri, string postData);
	}
}
