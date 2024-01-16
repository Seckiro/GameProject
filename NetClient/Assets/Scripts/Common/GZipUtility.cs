using System;
using System.IO;
using System.IO.Compression;
using System.Text;

public static class GZipUtility
{
    public static string CompressStringToString(string data)
    {
        return Convert.ToBase64String(CompressByBytes(Encoding.UTF8.GetBytes(data)));
    }

    public static string DecompressStringToString(string data)
    {
        return Encoding.UTF8.GetString(DecompressByBytes(Convert.FromBase64String(data)));
    }

    public static byte[] StringToByte(string data, bool isCompress)
    {
        return isCompress ? CompressByBytes(Encoding.UTF8.GetBytes(data)) : Encoding.UTF8.GetBytes(data);
    }

    public static string ByteToString(byte[] data, bool isDecompress)
    {
        return isDecompress ? Encoding.UTF8.GetString(DecompressByBytes(data)) : Encoding.UTF8.GetString(data);
    }

    public static byte[] CompressByBytes(byte[] data)
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            using (GZipStream zip = new GZipStream(memoryStream, CompressionMode.Compress, true))
            {
                zip.Write(data, 0, data.Length);
            }
            return memoryStream.ToArray();
        }
    }

    public static byte[] DecompressByBytes(byte[] data)
    {
        using (MemoryStream memoryStream = new MemoryStream(data))
        {
            using (GZipStream zip = new GZipStream(memoryStream, CompressionMode.Decompress))
            {
                using (MemoryStream msreader = new MemoryStream())
                {
                    byte[] buffer = new byte[0x1000];
                    int reader;
                    while ((reader = zip.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        msreader.Write(buffer, 0, reader);
                    }
                    return msreader.ToArray();
                }
            }
        }
    }
}


