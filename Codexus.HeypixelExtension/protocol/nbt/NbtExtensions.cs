using System.Text;
using DotNetty.Buffers;

namespace Codexus.HeypixelExtension.protocol.nbt;

public static class NbtExtensions
{
    public static Nbt ReadNbt(this IByteBuffer buffer)
    {
        Nbt nbt = new();

        var type = buffer.ReadByte();

        switch (type)
        {
            case 8:
                nbt.Data.AddLast(new NbtField("", type, buffer.ReadNbtUtf()));
                nbt.IsText = true;
                return nbt;
            case 10:
                return buffer.ReadNbtBody(nbt);
            default:
                throw new InvalidDataException("Failed to decode NBT: Does not start with TAG_Compound or TAG_String");
        }
    }
    
    public static Nbt ReadNbtBody(this IByteBuffer buffer, Nbt nbt, uint depth = 0)
    {
        while (true)
        {
            var nextType = buffer.ReadByte();
            if (nextType == 0) return nbt;
            var nextName = buffer.ReadNbtUtf();
          
            nbt.Data.AddLast(new NbtField(nextName, nextType, buffer.ReadNbtField(nextType, depth)));
        }
    }

    public static object ReadNbtField(this IByteBuffer buffer, int next, uint depth)
    {
        switch (next)
        {
            case 1: // TAG_Byte
                return buffer.ReadByte();
            case 2: // TAG_Short
                return buffer.ReadShort();
            case 3: // TAG_Int
                return buffer.ReadInt();
            case 4: // TAG_Long
                return buffer.ReadLong();
            case 5: // TAG_Float
                return buffer.ReadFloat();
            case 6: // TAG_Double
                return buffer.ReadDouble();
            case 7: // TAG_Byte_Array
                return buffer.ReadBytes(buffer.ReadInt());
            case 8: // TAG_String
                return buffer.ReadNbtUtf();
            case 9: // TAG_List
                var type = buffer.ReadByte();
                var length1 = buffer.ReadInt();
                var items1 = new NbtField[length1];
                for (var i = 0; i < length1; i++)
                    items1[i] = new NbtField(i.ToString(), type, buffer.ReadNbtField(type, depth));
                return items1;
            case 10: // TAG_Compound
                return buffer.ReadNbtBody(new Nbt(), depth + 1);
            case 11: // TAG_Int_Array
                var length2 = buffer.ReadInt();
                var items2 = new int[length2];
                for (var i = 0; i < length2; i++)
                    items2[i] = (int) buffer.ReadNbtField(3, depth);
                return items2;
            case 12: // TAG_Long_Array
                var length3 = buffer.ReadInt();
                var items3 = new long[length3];
                for (var i = 0; i < length3; i++)
                    items3[i] = (long) buffer.ReadNbtField(4, depth);
                return items3;
            default:
                throw new InvalidDataException("Failed to decode NBT: Unknown field type " + next);
        }
    }
    
    public static string ReadNbtUtf(this IByteBuffer buffer)
    {
        int length = buffer.ReadUnsignedShort();
        if (length > buffer.ReadableBytes) length = buffer.ReadableBytes;
        var numArray = new byte[length];
        buffer.ReadBytes(numArray);
        return Encoding.UTF8.GetString(numArray);
    }

    public static void WriteNbt(this IByteBuffer buffer, Nbt nbt)
    {
        switch (nbt.IsText)
        {
            case true:
                buffer.WriteByte(8);
                buffer.WriteNbtUtf((string) nbt.Data.Single().Value);
                break;
            case false:
                buffer.WriteByte(10);
                buffer.WriteNbtBody(nbt);
                break;
        }
    }
    
    public static void WriteNbtBody(this IByteBuffer buffer, Nbt nbt)
    {
        foreach (var data in nbt.Data)
        {
            buffer.WriteByte(data.Type);
            buffer.WriteNbtUtf(data.Name);
            buffer.WriteNbtField(data);
        }
        buffer.WriteByte(0);
    }
    
    public static void WriteNbtField(this IByteBuffer buffer, NbtField data)
    {
        switch (data.Type)
        {
            case 1: // TAG_Byte
                buffer.WriteByte((byte) data.Value);
                break;
            case 2: // TAG_Short
                buffer.WriteShort((short) data.Value);
                break;
            case 3: // TAG_Int
                buffer.WriteInt((int) data.Value);
                break;
            case 4: // TAG_Long
                buffer.WriteLong((long) data.Value);
                break;
            case 5: // TAG_Float
                buffer.WriteFloat((float) data.Value);
                break;
            case 6: // TAG_Double
                buffer.WriteDouble((double) data.Value);
                break;
            case 7: // TAG_Byte_Array
                var bytes = (IByteBuffer) data.Value;
                buffer.WriteInt(bytes.ReadableBytes);
                buffer.WriteBytes(bytes);
                break;
            case 8: // TAG_String
                buffer.WriteNbtUtf((string) data.Value);
                break;
            case 9: // TAG_List
                var array1 = (NbtField[]) data.Value;
                buffer.WriteInt(array1.Length);
                foreach (var t in array1)
                    buffer.WriteNbtField(t);
                break;
            case 10: // TAG_Compound
                buffer.WriteNbtBody((Nbt) data.Value);
                break;
            case 11: // TAG_Int_Array
                var array2 = (int[]) data.Value;
                buffer.WriteInt(array2.Length);
                foreach (var v in array2)
                    buffer.WriteInt(v);
                break;
            case 12: // TAG_Long_Array
                var array3 = (long[]) data.Value;
                buffer.WriteInt(array3.Length);
                foreach (var v in array3)
                    buffer.WriteLong(v);
                break;
            default:
                throw new InvalidDataException("Failed to encode NBT: Unknown field type");
        }
    }
    
    public static void WriteNbtUtf(this IByteBuffer buffer, string content) 
    {
        var bytes = Encoding.UTF8.GetBytes(content);
        buffer.WriteUnsignedShort((ushort) bytes.Length);
        buffer.WriteBytes(bytes);
    }
}
