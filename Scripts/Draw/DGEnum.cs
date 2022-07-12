namespace extDebug.Gizmos
{
    public enum RenderPipeline
    {
        Unknown,
        Legacy,
        HDRP,
        URP
    }
    
    public enum CommandType
    {
        Matrix,
        Color,
        Line,
        Cube
    }

    internal enum ZTest
    {
        Never,
        Less,
        Equal,
        LEqual,
        Greater,
        NotEqual,
        GEqual,
        Always,
    }
}