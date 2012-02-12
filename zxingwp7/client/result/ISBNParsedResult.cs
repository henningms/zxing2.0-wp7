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
    /// <author>  jbreiden@google.com (Jeff Breidenbach)
    /// </author>
    /// <author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source 
    /// </author>
    public sealed class ISBNParsedResult : ParsedResult
    {

        private readonly String isbn;

        internal ISBNParsedResult(String isbn) : base(ParsedResultType.ISBN)
        {
            this.isbn = isbn;
        }

        public String ISBN
        {
            get
            {
                return isbn;
            }
        }

        public override String DisplayResult
        {
            get
            {
                return isbn;
            }
        }
    }
}