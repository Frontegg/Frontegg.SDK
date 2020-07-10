using System;
using System.IO;

namespace Frontegg.SDK.Client
{
    public class SortDirection : IEquatable<SortDirection>, IComparable<SortDirection>
    {
        private readonly string _direction;

        public static SortDirection Asc = new SortDirection("asc");
        public static SortDirection Desc = new SortDirection("desc");
        private SortDirection(string direction)
        {
            _direction = direction ?? throw new ArgumentNullException(nameof(direction));
        }
        
        public bool Equals(SortDirection other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _direction == other._direction;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((SortDirection) obj);
        }

        public override int GetHashCode()
        {
            var hashCode = (_direction != null ? _direction.GetHashCode() : 0);
            return hashCode;
        }

        public int CompareTo(SortDirection other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var directionComparison = string.Compare(_direction, other._direction, StringComparison.Ordinal);
            return directionComparison;
        }
        
        public static bool operator ==(SortDirection left, SortDirection right)
        {
            if (left is null)   
                return right is null; // null == null = true
            
            return left.Equals(right);
        }

        public static bool operator !=(SortDirection left, SortDirection right)
        {
            return !(left == right);
        }
        
        public static implicit operator string(SortDirection sortDirection) => 
            sortDirection._direction;
        
        public static explicit operator SortDirection(string value) => 
            FromValue(value);
        
        private static SortDirection FromValue(string sortDirection)
        {
            if (sortDirection == null) throw new ArgumentNullException(nameof(sortDirection));

            if (string.Equals(sortDirection, "asc", StringComparison.InvariantCultureIgnoreCase))
            {
                return SortDirection.Asc;
            }
            
            if (string.Equals(sortDirection, "desc", StringComparison.InvariantCultureIgnoreCase))
            {
                return SortDirection.Desc;
            }
            
            throw new InvalidDataException($"{sortDirection} is not a valid value, only asc and desc are supported");
        }
    }
}