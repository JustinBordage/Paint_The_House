[System.Serializable]
public struct RangeFloat
{
    public float min, max;
    public float length { get { return max - min; } }

    public RangeFloat(float _min, float _max)
    {
        min = _min;
        max = _max;
    }
}