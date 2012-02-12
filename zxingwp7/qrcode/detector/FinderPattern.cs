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

namespace zxingwp7.qrcode.detector
{
    /// <summary> <p>Encapsulates a finder pattern, which are the three square patterns found in
    /// the corners of QR Codes. It also encapsulates a count of similar finder patterns,
    /// as a convenience to the finder's bookkeeping.</p>
    /// 
    /// </summary>
    /// <author>  Sean Owen
    /// </author>
    /// <author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source 
    /// </author>
    public sealed class FinderPattern : ResultPoint
    {

        private readonly float estimatedModuleSize;
        private int count;

        internal FinderPattern(float posX, float posY, float estimatedModuleSize) : base(posX, posY)
        {
            this.estimatedModuleSize = estimatedModuleSize;
            count = 1;
        }

        public float EstimatedModuleSize
        {
            get
            {
                return estimatedModuleSize;
            }
        }

        internal int Count
        {
            get
            {
                return count;
            }
        }

        internal void incrementCount()
        {
            count++;
        }

        /// <summary> <p>Determines if this finder pattern "about equals" a finder pattern at the stated
        /// position and size -- meaning, it is at nearly the same center with nearly the same size.</p>
        /// </summary>
        internal bool aboutEquals(float moduleSize, float i, float j)
        {
            if (Math.Abs(i - Y) <= moduleSize && Math.Abs(j - X) <= moduleSize)
            {
                float moduleSizeDiff = Math.Abs(moduleSize - estimatedModuleSize);
                return moduleSizeDiff <= 1.0f || moduleSizeDiff/estimatedModuleSize <= 1.0f;
            }
            return false;
        }
    }
}