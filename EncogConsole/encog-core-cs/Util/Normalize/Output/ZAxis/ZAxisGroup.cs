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
using System.Linq;

namespace Encog.Util.Normalize.Output.ZAxis
{
    /// <summary>
    /// Used to group Z-Axis fields together. Both OutputFieldZAxis and
    /// OutputFieldZAxisSynthetic fields may belong to this group. For
    /// more information see the OutputFieldZAxis class.
    /// </summary>
    [Serializable]
    public class ZAxisGroup : BasicOutputFieldGroup
    {
        /// <summary>
        /// The calculated length.
        /// </summary>
        private double _length;

        /// <summary>
        /// The multiplier, which is the value that all other values will be
        /// multiplied to become normalized.
        /// </summary>
        private double _multiplier;

        /// <summary>
        /// The vector length.
        /// </summary>
        public double Length
        {
            get { return _length; }
        }

        /// <summary>
        /// The value to multiply the other values by to normalize them.
        /// </summary>
        public double Multiplier
        {
            get { return _multiplier; }
        }

        /// <summary>
        /// Initialize this group for a new row.
        /// </summary>
        public override void RowInit()
        {
            double value = (from field in GroupedFields
                            where !(field is OutputFieldZAxisSynthetic)
                            where field.SourceField != null
                            select (field.SourceField.CurrentValue*field.SourceField.CurrentValue)).Sum();

            _length = Math.Sqrt(value);
            _multiplier = 1.0/Math.Sqrt(GroupedFields.Count);
        }
    }
}
