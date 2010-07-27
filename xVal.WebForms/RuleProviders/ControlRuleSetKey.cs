using System;
namespace xVal.WebForms.RuleProviders
{
    public class ControlRuleSetKey : IComparable<ControlRuleSetKey>, IEquatable<ControlRuleSetKey>
    {
        /// <summary>
        /// Gets or sets the name of the field.
        /// </summary>
        /// <value>The name of the field.</value>
        public string FieldName { get; set; }

        /// <summary>
        /// Gets or sets the control ID.
        /// </summary>
        /// <value>The control ID.</value>
        public string ControlID { get; set; }

        #region IComparable<ControlRuleSetKey> Members

        public int CompareTo(ControlRuleSetKey other)
        {
            if (other == null)
            {
                return -1;
            }

            return (FieldName + "|" + ControlID).CompareTo(other.FieldName + "|" + other.ControlID);
        }

        #endregion

        public override bool Equals(object other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            if (!(other is ControlRuleSetKey))
            {
                return false;
            }

            return Equals((ControlRuleSetKey)other);
        }

        public override int GetHashCode()
        {
            return (FieldName ?? String.Empty).GetHashCode() ^ (ControlID ?? String.Empty).GetHashCode();
        }

        #region IEquatable<ControlRuleSetKey> Members

        public bool Equals(ControlRuleSetKey other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            return CompareTo(other) == 0;
        }

        #endregion
    }
}