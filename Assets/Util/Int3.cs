
public struct Int3
{
    public readonly int x, y, z;

    public Int3(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public static Int3 operator+(Int3 one, Int3 two)
    {
        return new Int3(one.x + two.x, one.y + two.y, one.z + two.z);
    }

    public static Int3 operator *(Int3 one, int scalar)
    {
        return new Int3(one.x * scalar, one.y * scalar, one.z * scalar);
    }

    public static implicit operator bool(Int3? int3) => int3 != null;
}

