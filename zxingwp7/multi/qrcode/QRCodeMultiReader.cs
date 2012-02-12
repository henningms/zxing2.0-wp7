/*
* Copyright 2009 ZXing authors
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
using zxingwp7.multi.qrcode.detector;
using zxingwp7.qrcode;

namespace zxingwp7.multi.qrcode
{
    /// <summary> This implementation can detect and decode multiple QR Codes in an image.
    /// 
    /// </summary>
    /// <author>  Sean Owen
    /// </author>
    /// <author>  Hannes Erven
    /// </author>
    /// <author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source 
    /// </author>
    public sealed class QRCodeMultiReader : QRCodeReader, MultipleBarcodeReader
    {

        private static readonly Result[] EMPTY_RESULT_ARRAY = new Result[0];

        #region MultipleBarcodeReader Members

        public Result[] decodeMultiple(BinaryBitmap image)
        {
            return decodeMultiple(image, null);
        }

        #endregion

        public Result[] decodeMultiple(BinaryBitmap image, Dictionary<DecodeHintType, Object> hints)
        {
            var results = new List<Result>(10);

            DetectorResult[] detectorResult = new MultiDetector(image.BlackMatrix).detectMulti(hints);

            foreach (DetectorResult t in detectorResult)
            {
                try
                {
                    DecoderResult decoderResult = Decoder.decode(t.Bits);
                    ResultPoint[] points = t.Points;
                    var result = new Result(decoderResult.Text, decoderResult.RawBytes, points, BarcodeFormat.QR_CODE);

                    if (decoderResult.ByteSegments != null)
                    {
                        result.putMetadata(ResultMetadataType.BYTE_SEGMENTS, decoderResult.ByteSegments);
                    }
                    if (decoderResult.ECLevel != null)
                    {
                        result.putMetadata(ResultMetadataType.ERROR_CORRECTION_LEVEL, decoderResult.ECLevel.ToString());
                    }
                    results.Add(result);
                }
                catch (ReaderException)
                {
                    // ignore and continue 
                }
            }
            if ((results.Count == 0))
            {
                return EMPTY_RESULT_ARRAY;
            }

            var resultArray = new Result[results.Count];
            for (int i = 0; i < results.Count; i++)
            {
                resultArray[i] = (Result) results[i];
            }
            return resultArray;
        }
    }
}