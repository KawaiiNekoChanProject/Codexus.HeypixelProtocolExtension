namespace Codexus.HeypixelExtension.utils;

public static class FloodgateFormId
{
    public static short GetFormId(byte[] data)
    {
        return (short)((data[0] << 8) | data[1]);
    }

    public static byte[] ToFormId(short value)
    {
        return
        [
            (byte)((value >> 8) & 0xFF),
            (byte)(value & 0xFF)
        ];
    }
}