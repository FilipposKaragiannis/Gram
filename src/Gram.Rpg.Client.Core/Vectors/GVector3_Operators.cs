namespace Gram.Rpg.Client.Core
{
    public partial struct GVector3
    {
        public static GVector3 operator +(GVector3 a, GVector3 b)
        {
            return new GVector3(a.x + b.x, a.y + b.y, a.z + b.z);
        }
        
        public static GVector3 operator /(GVector3 v, float d)
        {
            return new GVector3(v.x / d, v.y / d, v.z / d);
        }

        public static GVector3 operator *(GVector3 v, float m)
        {
            return new GVector3(v.x * m, v.y * m, v.z * m);
        }

        public static GVector3 operator *(float m, GVector3 v)
        {
            return new GVector3(v.x * m, v.y * m, v.z * m);
        }

        public static GVector3 operator -(GVector3 a, GVector3 b)
        {
            return new GVector3(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static GVector3 operator -(GVector3 a)
        {
            return new GVector3(-a.x, -a.y, -a.z);
        }
        
        public static bool operator ==(GVector3 a, GVector3 b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(GVector3 a, GVector3 b)
        {
            return !a.Equals(b);
        }

        public bool Equals(GVector3 other)
        {
            return x.Equals(other.x) && y.Equals(other.y) && z.Equals(other.z);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is GVector3 other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = x.GetHashCode();
                hashCode = (hashCode * 397) ^ y.GetHashCode();
                hashCode = (hashCode * 397) ^ z.GetHashCode();
                return hashCode;
            }
        }
    }
}
