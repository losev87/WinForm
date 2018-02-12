//
// Encog(tm) Core v3.3 - .Net Version
// http://www.heatonresearch.com/encog/
//
// Copyright 2008-2014 Heaton Research, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//   
// For more information on Heaton Research copyrights, licenses 
// and trademarks visit:
// http://www.heatonresearch.com/copyright
//
using System;

namespace Encog.Bot.DataUnits
{
    /// <summary>
    /// A data unit that holds code.
    /// </summary>
    public class CodeDataUnit : DataUnit
    {
        /// <summary>
        /// The code for this data unit.
        /// </summary>
        private String _code;

        /// <summary>
        /// The code for this data unit.
        /// </summary>
        public String Code
        {
            get { return _code; }
            set { _code = value; }
        }

        /// <summary>
        /// This object as a string.
        /// </summary>
        /// <returns>This object as a string.</returns>
        public override String ToString()
        {
            return _code;
        }
    }
}
