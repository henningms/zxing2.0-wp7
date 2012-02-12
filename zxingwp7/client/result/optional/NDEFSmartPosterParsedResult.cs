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

namespace zxingwp7.client.result.optional
{
    /// <author>  Sean Owen
    /// </author>
    /// <author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source 
    /// </author>
    public sealed class NDEFSmartPosterParsedResult : ParsedResult
    {
        public const int ACTION_UNSPECIFIED = - 1;
        public const int ACTION_DO = 0;
        public const int ACTION_SAVE = 1;
        public const int ACTION_OPEN = 2;
        private readonly int action;


        private readonly String title;

        private readonly String uri;


        internal NDEFSmartPosterParsedResult(int action, String uri, String title)
            : base(ParsedResultType.NDEF_SMART_POSTER)
        {
            this.action = action;
            this.uri = uri;
            this.title = title;
        }

        public String Title
        {
            get
            {
                return title;
            }
        }

        public String URI
        {
            get
            {
                return uri;
            }
        }

        public int Action
        {
            get
            {
                return action;
            }
        }

        public override String DisplayResult
        {
            get
            {
                if (title == null)
                {
                    return uri;
                }
                else
                {
                    return title + '\n' + uri;
                }
            }
        }
    }
}