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

namespace zxingwp7.client.result
{
    /// <summary> Parses a "tel:" URI result, which specifies a phone number.
    /// 
    /// </summary>
    /// <author>  Sean Owen
    /// </author>
    /// <author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source 
    /// </author>
    internal sealed class TelResultParser : ResultParser
    {
        private TelResultParser()
        {
        }

        public static TelParsedResult parse(Result result)
        {
            String rawText = result.Text;
            if (rawText == null || (!rawText.StartsWith("tel:") && !rawText.StartsWith("TEL:")))
            {
                return null;
            }
            // Normalize "TEL:" to "tel:"
            String telURI = rawText.StartsWith("TEL:") ? "tel:" + rawText.Substring(4) : rawText;
            // Drop tel, query portion

            int queryStart = rawText.IndexOf('?', 4);
            String number = queryStart < 0 ? rawText.Substring(4) : rawText.Substring(4, (queryStart) - (4));
            return new TelParsedResult(number, telURI, null);
        }
    }
}