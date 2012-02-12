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

using zxingwp7.common;

namespace zxingwp7.qrcode.encoder
{
    internal sealed class BlockPair
    {

        private readonly ByteArray dataBytes;

        private readonly ByteArray errorCorrectionBytes;

        internal BlockPair(ByteArray data, ByteArray errorCorrection)
        {
            dataBytes = data;
            errorCorrectionBytes = errorCorrection;
        }

        public ByteArray DataBytes
        {
            get
            {
                return dataBytes;
            }
        }

        public ByteArray ErrorCorrectionBytes
        {
            get
            {
                return errorCorrectionBytes;
            }
        }
    }
}