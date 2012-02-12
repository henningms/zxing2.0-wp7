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
using System.Collections.Generic;

namespace zxingwp7
{
    /// <summary> Enumerates barcode formats known to this package.
    /// 
    /// </summary>
    /// <author>  Sean Owen
    /// </author>
    /// <author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source 
    /// </author>
    public sealed class BarcodeFormat
    {
        private static readonly Dictionary<String, BarcodeFormat> VALUES = new Dictionary<String, BarcodeFormat>();

        /// <summary>QR Code 2D barcode format. </summary>

        public static readonly BarcodeFormat QR_CODE = new BarcodeFormat("QR_CODE");

        /// <summary>DataMatrix 2D barcode format. </summary>

        public static readonly BarcodeFormat DATAMATRIX = new BarcodeFormat("DATAMATRIX");

        /// <summary>UPC-E 1D format. </summary>

        public static readonly BarcodeFormat UPC_E = new BarcodeFormat("UPC_E");

        /// <summary>UPC-A 1D format. </summary>

        public static readonly BarcodeFormat UPC_A = new BarcodeFormat("UPC_A");

        /// <summary>EAN-8 1D format. </summary>

        public static readonly BarcodeFormat EAN_8 = new BarcodeFormat("EAN_8");

        /// <summary>EAN-13 1D format. </summary>

        public static readonly BarcodeFormat EAN_13 = new BarcodeFormat("EAN_13");

        /// <summary>Code 128 1D format. </summary>

        public static readonly BarcodeFormat CODE_128 = new BarcodeFormat("CODE_128");

        /// <summary>Code 39 1D format. </summary>

        public static readonly BarcodeFormat CODE_39 = new BarcodeFormat("CODE_39");

        /// <summary>ITF (Interleaved Two of Five) 1D format. </summary>

        public static readonly BarcodeFormat ITF = new BarcodeFormat("ITF");

        /// <summary>PDF417 format. </summary>
        public static readonly BarcodeFormat PDF417 = new BarcodeFormat("PDF417");

        public static List<BarcodeFormat> AllFormats
        {
            get
            {
                return new List<BarcodeFormat>{QR_CODE, DATAMATRIX, UPC_E, UPC_A, EAN_13, EAN_8, CODE_128, CODE_39, ITF, PDF417};
            }
        }

        private readonly String name;

        private BarcodeFormat(String name)
        {
            this.name = name;
            VALUES[name] = this;
        }

        public String Name
        {
            get
            {
                return name;
            }
        }

        public override String ToString()
        {
            return name;
        }

        public static BarcodeFormat ValueOf(String name)
        {
            var format = VALUES[name];
            if (format == null)
            {
                throw new ArgumentException();
            }
            return format;
        }
    }
}