namespace Codexus.HeypixelExtension.Utils;

public static class FloodgateFormId
{
    public static short GetFormId(byte[] data)
    {
        return (short)((data[0] << 8) | data[1]);
    }
    
    public static short GetFormIdFromPayload(byte[] data)
    {
        var windowRaw = new byte[2];
        windowRaw[0] = data[1];
        windowRaw[1] = data[2];
        return GetFormId(windowRaw);
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