using System.Text;

namespace OpenTTD.Networking.AdminPort;

public class Packet
{
    public ushort Size { get; private set; } = 2;
    public ushort Position { get; private set; }

    public byte[] Buffer { get; } = new byte[1460];

    public Packet()
    {
    }

    public Packet(byte[] buffer)
    {
        Buffer = buffer;
        Size = ReadU16();
    }

    public void PrepareToSend()
    {
        var bytes = BitConverter.GetBytes(Size);
        Buffer[0] = bytes[0];
        Buffer[1] = bytes[1];
    }

    public void SendByte(byte value)
    {
        Buffer[Size] = value;
        Size += 1;
    }

    public void SendU16(ushort value)
    {
        var bytes = BitConverter.GetBytes(value);
        Buffer[Size] = bytes[0];
        Buffer[Size + 1] = bytes[1];

        Size += 2;
    }

    public void SendU32(uint value)
    {
        var bytes = BitConverter.GetBytes(value);
        Buffer[Size] = bytes[0];
        Buffer[Size + 1] = bytes[1];
        Buffer[Size + 2] = bytes[2];
        Buffer[Size + 3] = bytes[3];

        Size += 4;
    }

    public void SendU64(long value)
    {
        var bytes = BitConverter.GetBytes(value);
        Buffer[Size] = bytes[0];
        Buffer[Size + 1] = bytes[1];
        Buffer[Size + 2] = bytes[2];
        Buffer[Size + 3] = bytes[3];
        Buffer[Size + 4] = bytes[4];
        Buffer[Size + 5] = bytes[5];
        Buffer[Size + 6] = bytes[6];
        Buffer[Size + 7] = bytes[7];

        Size += 8;
    }


    public void SendString(string str)
    {
        var bytes = Encoding.Default.GetBytes(str);
        foreach (var b in bytes)
            SendByte(b);

        SendByte(0);
    }

    public void SendString(string str, int size)
    {
        var bytes = Encoding.Default.GetBytes(str);

        for (var i = 0; i < size; ++i)
        {
            if (i < bytes.Length)
            {
                SendByte(bytes[i]);
            }
            else
            {
                SendByte(0);
                break;
            }
        }
    }

    public byte ReadByte() => Buffer[Position++];

    public bool ReadBool() => ReadByte() != 0;

    public ushort ReadU16() => BitConverter.ToUInt16(Buffer, (Position += 2) - 2);

    public uint ReadU32() => BitConverter.ToUInt32(Buffer, (Position += 4) - 4);

    public ulong ReadU64() => BitConverter.ToUInt64(Buffer, (Position += 8) - 8);

    public long ReadI64() => BitConverter.ToInt64(Buffer, (Position += 8) - 8);

    public string ReadString()
    {
        var bytes = new List<byte>();

        while (Buffer[Position] != 0)
        {
            bytes.Add(Buffer[Position++]);
        }

        Position++;

        return Encoding.Default.GetString(bytes.ToArray());
    }
}