using System;
using System.Globalization;

namespace toofz.NecroDancer.Data
{
    public sealed class DisplayString : IEquatable<DisplayString>
    {
        public static DisplayString Empty => new DisplayString();

        // Required for Entity Framework
        private DisplayString() : this(null) { }

        public DisplayString(string value)
        {
            if (value != null)
            {
                var parts = value.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2)
                {
                    Id = int.Parse(parts[0], CultureInfo.InvariantCulture);
                    Text = parts[1];
                }
                else
                {
                    Text = value;
                }
            }
        }

        public int Id { get; set; }
        public string Text { get; set; }

        public override string ToString()
        {
            return Text;
        }

        #region Custom Equality

        public override bool Equals(object obj)
        {
            return Equals(obj as DisplayString);
        }

        public bool Equals(DisplayString other)
        {
            if (ReferenceEquals(other, null))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            if (GetType() != other.GetType())
                return false;
            return Text == other.Text;
        }

        public override int GetHashCode()
        {
            return Text.GetHashCode();
        }

        #endregion
    }
}
