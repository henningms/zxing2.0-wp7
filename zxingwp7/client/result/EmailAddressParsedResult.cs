/*
* Copyright 2007 ZXing authors
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
    public sealed class EmailAddressParsedResult : ParsedResult
    {
        private readonly String body;
        private readonly String emailAddress;
        private readonly String mailtoURI;
        private readonly String subject;

        internal EmailAddressParsedResult(String emailAddress, String subject, String body, String mailtoURI)
            : base(ParsedResultType.EMAIL_ADDRESS)
        {
            this.emailAddress = emailAddress;
            this.subject = subject;
            this.body = body;
            this.mailtoURI = mailtoURI;
        }

        public String EmailAddress
        {
            get
            {
                return emailAddress;
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

        public String MailtoURI
        {
            get
            {
                return mailtoURI;
            }
        }

        public override String DisplayResult
        {
            get
            {
                var result = new StringBuilder(30);
                maybeAppend(emailAddress, result);
                maybeAppend(subject, result);
                maybeAppend(body, result);
                return result.ToString();
            }
        }


    }
}