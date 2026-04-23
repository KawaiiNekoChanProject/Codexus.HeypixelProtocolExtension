namespace Codexus.HeypixelExtension.protocol.nbt;

public class Nbt
{
    public readonly LinkedList<NbtField> Data = [];
    public bool IsText = false;
    
    public override string ToString()
    {
        return string.Join(", ", Data);
    }
}