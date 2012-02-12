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
    /// <summary> <p>Encapsulates functionality and implementation that is common to UPC and EAN families
    /// of one-dimensional barcodes.</p>
    /// 
    /// </summary>
    /// <author>  dswitkin@google.com (Daniel Switkin)
    /// </author>
    /// <author>  Sean Owen
    /// </author>
    /// <author>  alasdair@google.com (Alasdair Mackintosh)
    /// </author>
    /// <author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source 
    /// </author>
    public abstract class UPCEANReader : OneDReader
    {
        // These two values are critical for determining how permissive the decoding will be.
        // We've arrived at these values through a lot of trial and error. Setting them any higher
        // lets false positives creep in quickly.


        private static readonly int MAX_AVG_VARIANCE = (int) (PATTERN_MATCH_RESULT_SCALE_FACTOR*0.42f);


        private static readonly int MAX_INDIVIDUAL_VARIANCE = (int) (PATTERN_MATCH_RESULT_SCALE_FACTOR*0.7f);

        /// <summary> Start/end guard pattern.</summary>

        internal static readonly int[] START_END_PATTERN = new[] {1, 1, 1};

        /// <summary> Pattern marking the middle of a UPC/EAN pattern, separating the two halves.</summary>

        internal static readonly int[] MIDDLE_PATTERN = new[] {1, 1, 1, 1, 1};

        /// <summary> "Odd", or "L" patterns used to encode UPC/EAN digits.</summary>

        internal static readonly int[][] L_PATTERNS = new[]
                                                          {
                                                              new[] {3, 2, 1, 1}, new[] {2, 2, 2, 1}, new[] {2, 1, 2, 2},
                                                              new[] {1, 4, 1, 1}, new[] {1, 1, 3, 2}, new[] {1, 2, 3, 1},
                                                              new[] {1, 1, 1, 4}, new[] {1, 3, 1, 2}, new[] {1, 2, 1, 3},
                                                              new[] {3, 1, 1, 2}
                                                          };

        /// <summary> As above but also including the "even", or "G" patterns used to encode UPC/EAN digits.</summary>
        internal static int[][] L_AND_G_PATTERNS;


        private readonly StringBuilder decodeRowStringBuffer;

        static UPCEANReader()
        {
            {
                L_AND_G_PATTERNS = new int[20][];
                for (int i = 0; i < 10; i++)
                {
                    L_AND_G_PATTERNS[i] = L_PATTERNS[i];
                }
                for (int i = 10; i < 20; i++)
                {
                    int[] widths = L_PATTERNS[i - 10];
                    var reversedWidths = new int[widths.Length];
                    for (int j = 0; j < widths.Length; j++)
                    {
                        reversedWidths[j] = widths[widths.Length - j - 1];
                    }
                    L_AND_G_PATTERNS[i] = reversedWidths;
                }
            }
        }

        protected internal UPCEANReader()
        {
            decodeRowStringBuffer = new StringBuilder(20);
        }

        /// <summary> Get the format of this decoder.
        /// 
        /// </summary>
        /// <returns> The 1D format.
        /// </returns>
        internal abstract BarcodeFormat BarcodeFormat
        {
            get;
        }

        internal static int[] findStartGuardPattern(BitArray row)
        {
            bool foundStart = false;
            int[] startRange = null;
            int nextStart = 0;
            while (!foundStart)
            {
                startRange = findGuardPattern(row, nextStart, false, START_END_PATTERN);
                int start = startRange[0];
                nextStart = startRange[1];
                // Make sure there is a quiet zone at least as big as the start pattern before the barcode.
                // If this check would run off the left edge of the image, do not accept this barcode,
                // as it is very likely to be a false positive.
                int quietStart = start - (nextStart - start);
                if (quietStart >= 0)
                {
                    foundStart = row.isRange(quietStart, start, false);
                }
            }
            return startRange;
        }

        public override Result decodeRow(int rowNumber, BitArray row, Dictionary<DecodeHintType, Object> hints)
        {
            return decodeRow(rowNumber, row, findStartGuardPattern(row), hints);
        }

        /// <summary> <p>Like {@link #decodeRow(int, BitArray, java.util.Hashtable)}, but
        /// allows caller to inform method about where the UPC/EAN start pattern is
        /// found. This allows this to be computed once and reused across many implementations.</p>
        /// </summary>
        public virtual Result decodeRow(int rowNumber, BitArray row, int[] startGuardRange,
                                        Dictionary<DecodeHintType, Object> hints)
        {
            ResultPointCallback resultPointCallback = hints == null
                                                          ? null
                                                          : (ResultPointCallback)
                                                            hints[DecodeHintType.NEED_RESULT_POINT_CALLBACK];

            if (resultPointCallback != null)
            {
                resultPointCallback.foundPossibleResultPoint(
                    new ResultPoint((startGuardRange[0] + startGuardRange[1])/2.0f, rowNumber));
            }

            StringBuilder result = decodeRowStringBuffer;
            result.Length = 0;
            int endStart = decodeMiddle(row, startGuardRange, result);

            if (resultPointCallback != null)
            {
                resultPointCallback.foundPossibleResultPoint(new ResultPoint(endStart, rowNumber));
            }

            int[] endRange = decodeEnd(row, endStart);

            if (resultPointCallback != null)
            {
                resultPointCallback.foundPossibleResultPoint(new ResultPoint((endRange[0] + endRange[1])/2.0f, rowNumber));
            }


            // Make sure there is a quiet zone at least as big as the end pattern after the barcode. The
            // spec might want more whitespace, but in practice this is the maximum we can count on.
            int end = endRange[1];
            int quietEnd = end + (end - endRange[0]);
            if (quietEnd >= row.Size || !row.isRange(end, quietEnd, false))
            {
                throw ReaderException.Instance;
            }

            String resultString = result.ToString();
            if (!checkChecksum(resultString))
            {
                throw ReaderException.Instance;
            }


            float left = (startGuardRange[1] + startGuardRange[0])/2.0f;

            float right = (endRange[1] + endRange[0])/2.0f;

            return new Result(resultString, null,
                              new[] {new ResultPoint(left, rowNumber), new ResultPoint(right, rowNumber)}, BarcodeFormat);
        }

        /// <returns> {@link #checkStandardUPCEANChecksum(String)}
        /// </returns>

        protected internal virtual bool checkChecksum(String s)
        {
            return checkStandardUPCEANChecksum(s);
        }

        /// <summary> Computes the UPC/EAN checksum on a string of digits, and reports
        /// whether the checksum is correct or not.
        /// 
        /// </summary>
        /// <param name="s">string of digits to check
        /// </param>
        /// <returns> true iff string of digits passes the UPC/EAN checksum algorithm
        /// </returns>
        /// <throws>  ReaderException if the string does not contain only digits </throws>
        private static bool checkStandardUPCEANChecksum(String s)
        {
            int length = s.Length;
            if (length == 0)
            {
                return false;
            }

            int sum = 0;
            for (int i = length - 2; i >= 0; i -= 2)
            {
                int digit = s[i] - '0';
                if (digit < 0 || digit > 9)
                {
                    throw ReaderException.Instance;
                }
                sum += digit;
            }
            sum *= 3;
            for (int i = length - 1; i >= 0; i -= 2)
            {
                int digit = s[i] - '0';
                if (digit < 0 || digit > 9)
                {
                    throw ReaderException.Instance;
                }
                sum += digit;
            }
            return sum%10 == 0;
        }


        protected internal virtual int[] decodeEnd(BitArray row, int endStart)
        {
            return findGuardPattern(row, endStart, false, START_END_PATTERN);
        }

        /// <param name="row">row of black/white values to search
        /// </param>
        /// <param name="rowOffset">position to start search
        /// </param>
        /// <param name="whiteFirst">if true, indicates that the pattern specifies white/black/white/...
        /// pixel counts, otherwise, it is interpreted as black/white/black/...
        /// </param>
        /// <param name="pattern">pattern of counts of number of black and white pixels that are being
        /// searched for as a pattern
        /// </param>
        /// <returns> start/end horizontal offset of guard pattern, as an array of two ints
        /// </returns>
        /// <throws>  ReaderException if pattern is not found </throws>
        internal static int[] findGuardPattern(BitArray row, int rowOffset, bool whiteFirst, int[] pattern)
        {
            int patternLength = pattern.Length;
            var counters = new int[patternLength];
            int width = row.Size;
            bool isWhite = false;
            while (rowOffset < width)
            {
                isWhite = !row.get_Renamed(rowOffset);
                if (whiteFirst == isWhite)
                {
                    break;
                }
                rowOffset++;
            }

            int counterPosition = 0;
            int patternStart = rowOffset;
            for (int x = rowOffset; x < width; x++)
            {
                bool pixel = row.get_Renamed(x);
                if (pixel ^ isWhite)
                {
                    counters[counterPosition]++;
                }
                else
                {
                    if (counterPosition == patternLength - 1)
                    {
                        if (patternMatchVariance(counters, pattern, MAX_INDIVIDUAL_VARIANCE) < MAX_AVG_VARIANCE)
                        {
                            return new[] {patternStart, x};
                        }
                        patternStart += counters[0] + counters[1];
                        for (int y = 2; y < patternLength; y++)
                        {
                            counters[y - 2] = counters[y];
                        }
                        counters[patternLength - 2] = 0;
                        counters[patternLength - 1] = 0;
                        counterPosition--;
                    }
                    else
                    {
                        counterPosition++;
                    }
                    counters[counterPosition] = 1;
                    isWhite = !isWhite;
                }
            }
            throw ReaderException.Instance;
        }

        /// <summary> Attempts to decode a single UPC/EAN-encoded digit.
        /// 
        /// </summary>
        /// <param name="row">row of black/white values to decode
        /// </param>
        /// <param name="counters">the counts of runs of observed black/white/black/... values
        /// </param>
        /// <param name="rowOffset">horizontal offset to start decoding from
        /// </param>
        /// <param name="patterns">the set of patterns to use to decode -- sometimes different encodings
        /// for the digits 0-9 are used, and this indicates the encodings for 0 to 9 that should
        /// be used
        /// </param>
        /// <returns> horizontal offset of first pixel beyond the decoded digit
        /// </returns>
        /// <throws>  ReaderException if digit cannot be decoded </throws>
        internal static int decodeDigit(BitArray row, int[] counters, int rowOffset, int[][] patterns)
        {
            recordPattern(row, rowOffset, counters);
            int bestVariance = MAX_AVG_VARIANCE; // worst variance we'll accept
            int bestMatch = - 1;
            int max = patterns.Length;
            for (int i = 0; i < max; i++)
            {
                int[] pattern = patterns[i];
                int variance = patternMatchVariance(counters, pattern, MAX_INDIVIDUAL_VARIANCE);
                if (variance < bestVariance)
                {
                    bestVariance = variance;
                    bestMatch = i;
                }
            }
            if (bestMatch >= 0)
            {
                return bestMatch;
            }
            else
            {
                throw ReaderException.Instance;
            }
        }

        /// <summary> Subclasses override this to decode the portion of a barcode between the start
        /// and end guard patterns.
        /// 
        /// </summary>
        /// <param name="row">row of black/white values to search
        /// </param>
        /// <param name="startRange">start/end offset of start guard pattern
        /// </param>
        /// <param name="resultString">{@link StringBuffer} to append decoded chars to
        /// </param>
        /// <returns> horizontal offset of first pixel after the "middle" that was decoded
        /// </returns>
        /// <throws>  ReaderException if decoding could not complete successfully </throws>
        protected internal abstract int decodeMiddle(BitArray row, int[] startRange, StringBuilder resultString);
    }
}