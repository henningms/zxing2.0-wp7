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

namespace zxingwp7.client.result
{
    /// <summary> Represents the type of data encoded by a barcode -- from plain text, to a
    /// URI, to an e-mail address, etc.
    /// 
    /// </summary>
    /// <author>  Sean Owen
    /// </author>
    /// <author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source 
    /// </author>
    public sealed class ParsedResultType
    {

        public static readonly ParsedResultType ADDRESSBOOK = new ParsedResultType("ADDRESSBOOK");

        public static readonly ParsedResultType EMAIL_ADDRESS = new ParsedResultType("EMAIL_ADDRESS");

        public static readonly ParsedResultType PRODUCT = new ParsedResultType("PRODUCT");

        public static readonly ParsedResultType URI = new ParsedResultType("URI");

        public static readonly ParsedResultType TEXT = new ParsedResultType("TEXT");

        public static readonly ParsedResultType ANDROID_INTENT = new ParsedResultType("ANDROID_INTENT");

        public static readonly ParsedResultType GEO = new ParsedResultType("GEO");

        public static readonly ParsedResultType TEL = new ParsedResultType("TEL");

        public static readonly ParsedResultType SMS = new ParsedResultType("SMS");

        public static readonly ParsedResultType CALENDAR = new ParsedResultType("CALENDAR");
        // "optional" types

        public static readonly ParsedResultType NDEF_SMART_POSTER = new ParsedResultType("NDEF_SMART_POSTER");

        public static readonly ParsedResultType MOBILETAG_RICH_WEB = new ParsedResultType("MOBILETAG_RICH_WEB");

        public static readonly ParsedResultType ISBN = new ParsedResultType("ISBN");


        private readonly String name;

        private ParsedResultType(String name)
        {
            this.name = name;
        }

        public override String ToString()
        {
            return name;
        }
    }
}