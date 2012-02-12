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
    /// <summary> Parses a "geo:" URI result, which specifies a location on the surface of
    /// the Earth as well as an optional altitude above the surface. See
    /// <a href="http://tools.ietf.org/html/draft-mayrhofer-geo-uri-00">
    /// http://tools.ietf.org/html/draft-mayrhofer-geo-uri-00</a>.
    /// 
    /// </summary>
    /// <author>  Sean Owen
    /// </author>
    /// <author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source 
    /// </author>
    internal sealed class GeoResultParser : ResultParser
    {
        private GeoResultParser()
        {
        }

        public static GeoParsedResult parse(Result result)
        {
            String rawText = result.Text;
            if (rawText == null || (!rawText.StartsWith("geo:") && !rawText.StartsWith("GEO:")))
            {
                return null;
            }
            // Drop geo, query portion

            int queryStart = rawText.IndexOf('?', 4);
            String geoURIWithoutQuery = queryStart < 0 ? rawText.Substring(4) : rawText.Substring(4, (queryStart) - (4));
            int latitudeEnd = geoURIWithoutQuery.IndexOf(',');
            if (latitudeEnd < 0)
            {
                return null;
            }

            int longitudeEnd = geoURIWithoutQuery.IndexOf(',', latitudeEnd + 1);
            double latitude, longitude, altitude;
            try
            {
                latitude = Double.Parse(geoURIWithoutQuery.Substring(0, (latitudeEnd) - (0)));
                if (longitudeEnd < 0)
                {
                    longitude = Double.Parse(geoURIWithoutQuery.Substring(latitudeEnd + 1));
                    altitude = 0.0;
                }
                else
                {
                    longitude =
                        Double.Parse(geoURIWithoutQuery.Substring(latitudeEnd + 1, (longitudeEnd) - (latitudeEnd + 1)));
                    altitude = Double.Parse(geoURIWithoutQuery.Substring(longitudeEnd + 1));
                }
            }
            catch (FormatException)
            {
                return null;
            }
            return new GeoParsedResult(rawText.StartsWith("GEO:") ? "geo:" + rawText.Substring(4) : rawText, latitude,
                                       longitude, altitude);
        }
    }
}