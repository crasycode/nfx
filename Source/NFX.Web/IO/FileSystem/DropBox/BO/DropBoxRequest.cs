/*<FILE_LICENSE>
* NFX (.NET Framework Extension) Unistack Library
* Copyright 2003-2014 IT Adapter Inc / 2015 Aum Code LLC
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
</FILE_LICENSE>*/


/* NFX by ITAdapter
 * Author: Alexey Miheev <mihadev@yandex.ru>
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace NFX.Web.IO.FileSystem.DropBox.BO
{
    public class DropBoxRequest
    {
        #region Private Properties

        private const int DefaultRequestTimeout = 30*1000; // 30 sec
        private int _requestTimeout;
        private readonly HttpRequestMessage _httpMessage = new HttpRequestMessage();
        public static int DefaultFileMaxSizeBytes = 157286400; // 150 Mb
        private readonly Dictionary<string, string> _parametrs = new Dictionary<string, string>();

        #endregion

        #region Public Properties

        public string ContentSourcePath { get; set; }

        public Stream Content { get; set; }

        public StreamContent StreamContent
        {
            get { return CreateHttpContentStream(); }
        }

        public bool IsLargeContent
        {
            get
            {
                if (Content == null)
                    return false;
                return Content.Length > DefaultFileMaxSizeBytes;
            }
        }

        public string RequestUrl
        {
            get { return _httpMessage.RequestUri.AbsoluteUri; }
            set { _httpMessage.RequestUri = new Uri(value); }
        }

        public HttpMethod MethodName
        {
            get { return _httpMessage.Method; }
            set { _httpMessage.Method = value; }
        }

        public string AuthorizationToken
        {
            get
            {
                AuthenticationHeaderValue headerValue = _httpMessage.Headers.Authorization;
                if (headerValue == null)
                    return null;
                return headerValue.Parameter;
            }
            set
            {
                if (value == null)
                    throw new ArgumentException("Value cannot be empty or null", "value");
                _httpMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", value);
            }
        }

        public long ContentLength
        {
            get
            {
                if (Content == null)
                    return 0;
                return Content.Length;
            }
        }

        public int RequestTimeout
        {
            get
            {
                if(_requestTimeout <= 0)
                    return DefaultRequestTimeout;
                return _requestTimeout;
            }
            set { _requestTimeout = value; }
        }

        #endregion

        #region Public Methods

        public void AddParameter(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("All value must be not empty", "key");

            if (string.IsNullOrEmpty(value))
                return;

            _parametrs.Add(key, value);
        }

        public void ChageParameter(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("All value must be not empty", "key");

            if (string.IsNullOrEmpty(value))
                return;

            _parametrs[key] = value;
        }

        public void AddHeader(string key, string value)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
                throw new ArgumentException("All value must be not empty", "key/value");

            _httpMessage.Headers.Add(key, new[] {value});
        }

        public HttpRequestMessage CreateHttpRequestMessage()
        {
            string requests = PrepareRequestParameters();
            HttpRequestMessage message = new HttpRequestMessage(MethodName
                                             , new Uri(string.Format("{0}?{1}", _httpMessage.RequestUri, requests)));
            _httpMessage.Headers.ForEach(h => message.Headers.Add(h.Key, h.Value));

            return message;
        }

        #endregion

        #region Private Methods


        private string PrepareRequestParameters()
        {
            if (_parametrs.Count == 0)
                return null;
            IEnumerable<string> listOfParametrs = from kvp in _parametrs
                select string.Format("{0}={1}", kvp.Key, kvp.Value);
            return string.Join("&", listOfParametrs);
        }

        private StreamContent CreateHttpContentStream()
        {
            if(Content == null)
                return new StreamContent(new MemoryStream());

            Content.Position = 0;
            StreamContent sc = new StreamContent(Content, (int)ContentLength);
            return sc;
        }

        #endregion
    }
}