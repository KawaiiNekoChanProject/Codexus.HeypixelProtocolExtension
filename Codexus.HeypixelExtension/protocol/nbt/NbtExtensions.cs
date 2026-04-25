using System.Text;
using Codexus.HeypixelExtension.protocol.nbt.field;
using DotNetty.Buffers;
using Serilog;

namespace Codexus.HeypixelExtension.protocol.nbt;

public static class NbtExtensions
{
    public static NbtAny ReadNbt(this IByteBuffer buffer)
    {
        var type = buffer.ReadByte();

        switch (type)
        {
            case 8:
                return new NbtElement(8, buffer.ReadNbtUtf());
            case 10:
                NbtObject nbt = new();
                return buffer.ReadNbtBody(nbt);
            default:
                throw new InvalidDataException("Failed to decode NBT: Does not start with NbtElement/String or NbtObject");
        }
    }
    
    public static NbtObject ReadNbtBody(this IByteBuffer buffer, NbtObject nbt, uint depth = 0)
    {
        while (true)
        {
            var nextType = buffer.ReadByte();
            if (nextType == 0) return nbt;
            var nextName = buffer.ReadNbtUtf();

            nbt.Data[nextName] = buffer.ReadNbtField(nextType, depth);
        }
    }

    public static NbtAny ReadNbtField(this IByteBuffer buffer, int next, uint depth)
    {
        switch (next)
        {
            case 1: // TAG_Byte
                return new NbtElement(1, buffer.ReadByte());
            case 2: // TAG_Short
                return new NbtElement(2, buffer.ReadShort());
            case 3: // TAG_Int
                return new NbtElement(3, buffer.ReadInt());
            case 4: // TAG_Long
                return new NbtElement(4, buffer.ReadLong());
            case 5: // TAG_Float
                return new NbtElement(5, buffer.ReadFloat());
            case 6: // TAG_Double
                return new NbtElement(6, buffer.ReadDouble());
            case 7: // TAG_Byte_Array
                return new NbtElement(7, buffer.ReadBytes(buffer.ReadInt()));
            case 8: // TAG_String
                return new NbtElement(8, buffer.ReadNbtUtf());
            case 9: // TAG_List
                var listMemberType = buffer.ReadByte();
                var listLength = buffer.ReadInt();
                var list = new NbtList(listMemberType, listLength);
                for (var i = 0; i < listLength; i++)
                {
                    list.Members[i] = buffer.ReadNbtField(listMemberType, depth);
                }
                return list;
            case 10: // TAG_Compound
                return buffer.ReadNbtBody(new NbtObject(), depth + 1);
            case 11: // TAG_Int_Array
                var length2 = buffer.ReadInt();
                var arrayInt = new NbtIntArray(length2);
                for (var i = 0; i < length2; i++)
                    arrayInt.Members[i] = buffer.ReadInt();
                return arrayInt;
            case 12: // TAG_Long_Array
                var length3 = buffer.ReadInt();
                var arrayLong = new NbtLongArray(length3);
                for (var i = 0; i < length3; i++)
                    arrayLong.Members[i] = buffer.ReadLong();
                return arrayLong;
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

    public static void WriteNbt(this IByteBuffer buffer, NbtAny nbt)
    {
        switch (nbt)
        {
            case NbtElement nbtElement:
                if (nbtElement.Type != 8) throw new ArgumentException("Failed to encode NBT: Only allow NbtElement/String or NbtObject as root");
                
                buffer.WriteByte(nbtElement.Type);
                buffer.WriteNbtField(nbtElement);
                break;
            case NbtObject nbtObject:
                buffer.WriteByte(10);
                buffer.WriteNbtBody(nbtObject);
                break;
            default:
                throw new ArgumentException("Failed to encode NBT: Only allow NbtElement/String or NbtObject as root");
        }
    }
    
    public static void WriteNbtBody(this IByteBuffer buffer, NbtObject nbt)
    {
        foreach (var data in nbt.Data)
        {
            buffer.WriteByte(data.Value.Type);
            buffer.WriteNbtUtf(data.Key);
            buffer.WriteNbtField(data.Value);
        }
        buffer.WriteByte(0);
    }
    
    public static void WriteNbtField(this IByteBuffer buffer, NbtAny data)
    {
        switch (data.Type)
        {
            case 1: // TAG_Byte
                buffer.WriteByte((byte) ((NbtElement) data).Value);
                break;
            case 2: // TAG_Short
                buffer.WriteShort((short) ((NbtElement) data).Value);
                break;
            case 3: // TAG_Int
                buffer.WriteInt((int) ((NbtElement) data).Value);
                break;
            case 4: // TAG_Long
                buffer.WriteLong((long) ((NbtElement) data).Value);
                break;
            case 5: // TAG_Float
                buffer.WriteFloat((float) ((NbtElement) data).Value);
                break;
            case 6: // TAG_Double
                buffer.WriteDouble((double) ((NbtElement) data).Value);
                break;
            case 7: // TAG_Byte_Array
                var bytes = (IByteBuffer) ((NbtElement) data).Value;
                buffer.WriteInt(bytes.ReadableBytes);
                buffer.WriteBytes(bytes);
                break;
            case 8: // TAG_String
                buffer.WriteNbtUtf((string) ((NbtElement) data).Value);
                break;
            case 9: // TAG_List
                var list = (NbtList) data;
                buffer.WriteByte(list.MemberType);
                buffer.WriteInt(list.Members.Length);
                foreach (var member in list.Members)
                    buffer.WriteNbtField(member);
                break;
            case 10: // TAG_Compound
                buffer.WriteNbtBody((NbtObject) data);
                break;
            case 11: // TAG_Int_Array
                var arrayInt = (NbtIntArray) data;
                buffer.WriteInt(arrayInt.Members.Length);
                foreach (var v in arrayInt.Members)
                    buffer.WriteInt(v);
                break;
            case 12: // TAG_Long_Array
                var arrayLong = (NbtLongArray) data;
                buffer.WriteInt(arrayLong.Members.Length);
                foreach (var v in arrayLong.Members)
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
