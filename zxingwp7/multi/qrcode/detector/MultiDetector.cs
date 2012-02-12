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
using zxingwp7.qrcode.detector;

namespace zxingwp7.multi.qrcode.detector
{
    /// <summary> <p>Encapsulates logic that can detect one or more QR Codes in an image, even if the QR Code
    /// is rotated or skewed, or partially obscured.</p>
    /// 
    /// </summary>
    /// <author>  Sean Owen
    /// </author>
    /// <author>  Hannes Erven
    /// </author>
    /// <author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source 
    /// </author>
    public sealed class MultiDetector : Detector
    {

        private static readonly DetectorResult[] EMPTY_DETECTOR_RESULTS = new DetectorResult[0];

        public MultiDetector(BitMatrix image) : base(image)
        {
        }

        public DetectorResult[] detectMulti(Dictionary<DecodeHintType, Object> hints)
        {
            BitMatrix image = Image;
            var finder = new MultiFinderPatternFinder(image);
            FinderPatternInfo[] info = finder.findMulti(hints);

            if (info == null || info.Length == 0)
            {
                throw ReaderException.Instance;
            }

            var result = new List<DetectorResult>(10);

            foreach (FinderPatternInfo t in info)
            {
                try
                {
                    result.Add(processFinderPatternInfo(t));
                }
                catch (ReaderException)
                {
                    // ignore
                }
            }
            if ((result.Count == 0))
            {
                return EMPTY_DETECTOR_RESULTS;
            }

            var resultArray = new DetectorResult[result.Count];

            for (int i = 0; i < result.Count; i++)
            {
                resultArray[i] = result[i];
            }

            return resultArray;
        }
    }
}