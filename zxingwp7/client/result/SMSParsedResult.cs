/*
* Copyright 2008 ZXing authors
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*      http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;
using System.Text;

namespace zxingwp7.client.result
{
    /// <author>  Sean Owen
    /// </author>
    /// <author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source 
    /// </author>
    public sealed class SMSParsedResult : ParsedResult
    {
        private readonly String body;
        private readonly String number;
        private readonly String smsURI;
        private readonly String subject;
        private readonly String title;
        private readonly String via;

        public SMSParsedResult(String smsURI, String number, String via, String subject, String body, String title)
            : base(ParsedResultType.SMS)
        {
            this.smsURI = smsURI;
            this.number = number;
            this.via = via;
            this.subject = subject;
            this.body = body;
            this.title = title;
        }

        public String SMSURI
        {
            get
            {
                return smsURI;
            }
        }

        public String Number
        {
            get
            {
                return number;
            }
        }

        public String Via
        {
            get
            {
                return via;
            }
        }

        public String Subject
        {
            get
            {
                return subject;
            }
        }

        public String Body
        {
            get
            {
                return body;
            }
        }

        public String Title
        {
            get
            {
                return title;
            }
        }

        public override String DisplayResult
        {
            get
            {
                var result = new StringBuilder(100);
                maybeAppend(number, result);
                maybeAppend(via, result);
                maybeAppend(subject, result);
                maybeAppend(body, result);
                maybeAppend(title, result);
                return result.ToString();
            }
        }


    }
}