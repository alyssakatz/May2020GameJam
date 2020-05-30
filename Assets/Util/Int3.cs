
public struct Int3
{
    public readonly int x, y, z;

    public Int3(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public static implicit operator bool(Int3? int3) => int3 != null;
}

