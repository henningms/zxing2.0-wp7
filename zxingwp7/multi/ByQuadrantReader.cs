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

namespace zxingwp7.multi
{
    /// <summary> This class attempts to decode a barcode from an image, not by scanning the whole image,
    /// but by scanning subsets of the image. This is important when there may be multiple barcodes in
    /// an image, and detecting a barcode may find parts of multiple barcode and fail to decode
    /// (e.g. QR Codes). Instead this scans the four quadrants of the image -- and also the center
    /// 'quadrant' to cover the case where a barcode is found in the center.
    /// 
    /// </summary>
    /// <seealso cref="GenericMultipleBarcodeReader">
    /// </seealso>
    public sealed class ByQuadrantReader : Reader
    {

        private readonly Reader delegate_Renamed;

        public ByQuadrantReader(Reader delegate_Renamed)
        {
            this.delegate_Renamed = delegate_Renamed;
        }

        #region Reader Members

        public Result decode(BinaryBitmap image)
        {
            return decode(image, null);
        }

        #endregion

        public Result decode(BinaryBitmap image, Dictionary<DecodeHintType, Object> hints)
        {
            int width = image.Width;
            int height = image.Height;
            int halfWidth = width/2;
            int halfHeight = height/2;

            BinaryBitmap topLeft = image.crop(0, 0, halfWidth, halfHeight);
            try
            {
                return delegate_Renamed.decode(topLeft, hints);
            }
            catch (ReaderException)
            {
                // continue
            }

            BinaryBitmap topRight = image.crop(halfWidth, 0, halfWidth, halfHeight);
            try
            {
                return delegate_Renamed.decode(topRight, hints);
            }
            catch (ReaderException)
            {
                // continue
            }

            BinaryBitmap bottomLeft = image.crop(0, halfHeight, halfWidth, halfHeight);
            try
            {
                return delegate_Renamed.decode(bottomLeft, hints);
            }
            catch (ReaderException)
            {
                // continue
            }

            BinaryBitmap bottomRight = image.crop(halfWidth, halfHeight, halfWidth, halfHeight);
            try
            {
                return delegate_Renamed.decode(bottomRight, hints);
            }
            catch (ReaderException)
            {
                // continue
            }

            int quarterWidth = halfWidth/2;
            int quarterHeight = halfHeight/2;
            BinaryBitmap center = image.crop(quarterWidth, quarterHeight, halfWidth, halfHeight);
            return delegate_Renamed.decode(center, hints);
        }
    }
}