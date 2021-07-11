using Gram.Rpg.Client.Core.Design;

namespace Gram.Rpg.Client.Core
{
    public partial struct GVector3
    {
        public static   GVector3 one  => new GVector3(1f, 1f,  1f);
        public static   GVector3 up   => new GVector3(0f, 1f,  0f);
        public static   GVector3 down => new GVector3(0f, -1f, 0f);
        public static   GVector3 zero => new GVector3(0f, 0f,  0f);
        
        public delegate float    SetComponent(float c);

        public const float Deg2Rad = 0.01745329f;
        public const float Rad2Deg = 57.29578f;


        public GVector3(float x, float y)
        {
            this.x = x;
            this.y = y;

            z = 0;
        }

        public GVector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }


        public float x;
        public float y;
        public float z;


        [Pure]
        public GVector3 SetX(float value)
        {
            return new GVector3(value, y, z);
        }

        [Pure]
        public GVector3 SetX(SetComponent func)
        {
            return new GVector3(func(x), y, z);
        }

        [Pure]
        public GVector3 SetXZ(float x, float z)
        {
            return new GVector3(x, y, z);
        }

        [Pure]
        public GVector3 SetXZ(SetComponent fx, SetComponent fz)
        {
            return new GVector3(fx(x), y, fz(z));
        }

        [Pure]
        public GVector3 SetY(float value)
        {
            return new GVector3(x, value, z);
        }

        [Pure]
        public GVector3 SetZ(float value)
        {
            return new GVector3(x, y, value);
        }

        [Pure]
        public GVector3 SetZ(SetComponent func)
        {
            return new GVector3(x, y, func(z));
        }

        public override string ToString()
        {
            return $"({x:F3}, {y:F3}, {z:F3})";
        } 
    }
}
