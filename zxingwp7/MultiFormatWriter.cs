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
using zxingwp7.common;
using zxingwp7.oned;
using zxingwp7.qrcode;

namespace zxingwp7
{
    /// <summary> This is a factory class which finds the appropriate Writer subclass for the BarcodeFormat
    /// requested and encodes the barcode with the supplied contents.
    /// 
    /// </summary>
    /// <author>  dswitkin@google.com (Daniel Switkin)
    /// </author>
    /// <author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source 
    /// </author>
    public sealed class MultiFormatWriter : Writer
    {
        #region Writer Members

        public ByteMatrix encode(String contents, BarcodeFormat format, int width, int height)
        {
            return encode(contents, format, width, height, null);
        }

        #endregion

        public ByteMatrix encode(String contents, BarcodeFormat format, int width, int height,
                                 Dictionary<EncodeHintType, Object> hints)
        {
            if (format == BarcodeFormat.EAN_8)
            {
                return new EAN8Writer().encode(contents, format, width, height, hints);
            }
            else if (format == BarcodeFormat.EAN_13)
            {
                return new EAN13Writer().encode(contents, format, width, height, hints);
            }
            else if (format == BarcodeFormat.QR_CODE)
            {
                return new QRCodeWriter().encode(contents, format, width, height, hints);
            }
            else
            {
                throw new ArgumentException("No encoder available for format " + format);
            }
        }
    }
}