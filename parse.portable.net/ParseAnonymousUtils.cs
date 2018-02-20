/*
 /*
 * Copyright (c) 2015-present, Parse, LLC.
 * All rights reserved.
 *
 * This source code is licensed under the BSD-style license found in the
 * LICENSE file in the root directory of this source tree. An additional grant
 * of patent rights can be found in the PATENTS file in the same directory.
 */

using Newtonsoft.Json;

namespace parse.portable.net
{
    public static class ParseAnonymousUtils
    {
        public class ParseAnonClass
        {
            [JsonProperty("authData")]
            public AuthData AuthData { get; set; }
        }

        public class AuthData
        {
            [JsonProperty("anonymous")]
            public Anonymous Anonymous { get; set; }
        }

        public class Anonymous
        {
            [JsonProperty("id")]
            public string Id { get; set; }
        }
    }
}
