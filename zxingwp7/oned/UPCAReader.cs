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
using System.Collections.Generic;
using System.Text;
using zxingwp7.common;

namespace zxingwp7.oned
{
    /// <summary> <p>Implements decoding of the UPC-A format.</p>
    /// 
    /// </summary>
    /// <author>  dswitkin@google.com (Daniel Switkin)
    /// </author>
    /// <author>  Sean Owen
    /// </author>
    /// <author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source 
    /// </author>
    public sealed class UPCAReader : UPCEANReader
    {

        private readonly UPCEANReader ean13Reader = new EAN13Reader();

        internal override BarcodeFormat BarcodeFormat
        {
            get
            {
                return BarcodeFormat.UPC_A;
            }
        }

        public override Result decodeRow(int rowNumber, BitArray row, int[] startGuardRange,
                                         Dictionary<DecodeHintType, Object> hints)
        {
            return maybeReturnResult(ean13Reader.decodeRow(rowNumber, row, startGuardRange, hints));
        }

        public override Result decodeRow(int rowNumber, BitArray row, Dictionary<DecodeHintType, Object> hints)
        {
            return maybeReturnResult(ean13Reader.decodeRow(rowNumber, row, hints));
        }

        public override Result decode(BinaryBitmap image)
        {
            return maybeReturnResult(ean13Reader.decode(image));
        }

        public override Result decode(BinaryBitmap image, Dictionary<DecodeHintType, Object> hints)
        {
            return maybeReturnResult(ean13Reader.decode(image, hints));
        }

        protected internal override int decodeMiddle(BitArray row, int[] startRange, StringBuilder resultString)
        {
            return ean13Reader.decodeMiddle(row, startRange, resultString);
        }

        private static Result maybeReturnResult(Result result)
        {
            String text = result.Text;
            if (text[0] == '0')
            {
                return new Result(text.Substring(1), null, result.ResultPoints, BarcodeFormat.UPC_A);
            }
            else
            {
                throw ReaderException.Instance;
            }
        }
    }
}