// <copyright file="HttpRequest.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace Mmm.Iot.Common.Services.Http
{
    public class HttpRequest : IHttpRequest, IDisposable
    {
        private readonly MediaTypeHeaderValue defaultMediaType = new MediaTypeHeaderValue("application/json");
        private readonly Encoding defaultEncoding = new UTF8Encoding();
        private readonly HttpRequestMessage requestContent = new HttpRequestMessage();
        private bool disposedValue = false;

        public HttpRequest()
        {
        }

        public HttpRequest(Uri uri)
        {
            this.Uri = uri;
        }

        public HttpRequest(string uri)
        {
            this.SetUriFromString(uri);
        }

        public Uri Uri { get; set; }

        public HttpHeaders Headers => this.requestContent.Headers;

        public MediaTypeHeaderValue ContentType { get; private set; }

        public HttpRequestOptions Options { get; } = new HttpRequestOptions();

        public HttpContent Content => this.requestContent.Content;

        public void AddHeader(string name, string value)
        {
            if (!this.Headers.TryAddWithoutValidation(name, value))
            {
                if (name.ToLowerInvariant() != "content-type")
                {
                    throw new ArgumentOutOfRangeException(name, "Invalid header name");
                }

                this.ContentType = new MediaTypeHeaderValue(value);
            }
        }

        public void SetUriFromString(string uri)
        {
            this.Uri = new Uri(uri);
        }

        public void SetContent(string content)
        {
            this.SetContent(content, this.defaultEncoding, this.defaultMediaType);
        }

        public void SetContent(string content, Encoding encoding)
        {
            this.SetContent(content, encoding, this.defaultMediaType);
        }

        public void SetContent(string content, Encoding encoding, string mediaType)
        {
            this.SetContent(content, encoding, new MediaTypeHeaderValue(mediaType));
        }

        public void SetContent(string content, Encoding encoding, MediaTypeHeaderValue mediaType)
        {
            this.requestContent.Content = new StringContent(content, encoding, mediaType.MediaType);
            this.ContentType = mediaType;
        }

        public void SetContent(StringContent stringContent)
        {
            this.requestContent.Content = stringContent;
            this.ContentType = stringContent.Headers.ContentType;
        }

        public void SetContent<T>(T sourceObject)
        {
            this.SetContent(sourceObject, this.defaultEncoding, this.defaultMediaType);
        }

        public void SetContent<T>(T sourceObject, Encoding encoding)
        {
            this.SetContent(sourceObject, encoding, this.defaultMediaType);
        }

        public void SetContent<T>(T sourceObject, Encoding encoding, string mediaType)
        {
            this.SetContent(sourceObject, encoding, new MediaTypeHeaderValue(mediaType));
        }

        public void SetContent<T>(T sourceObject, Encoding encoding, MediaTypeHeaderValue mediaType)
        {
            var content = JsonConvert.SerializeObject(sourceObject, Formatting.None);
            this.requestContent.Content = new StringContent(content, encoding, mediaType.MediaType);
            this.ContentType = mediaType;
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.requestContent.Dispose();
                }

                this.disposedValue = true;
            }
        }
    }
}