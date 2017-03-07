using System;
using System.Text;

public static class Base64Utils
{

    private static string CharString = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";

    static public string ToBase64(string input)
    {
        byte[] data = Encoding.Default.GetBytes(input);
        StringBuilder result = new StringBuilder((data.Length * 4) / 3);

        int b;
        for (int i = 0; i < data.Length; i += 3)
        {

            b = (data[i] & 0xFC) >> 2;
            result.Append(CharString[b]);
            b = (data[i] & 3) << 4;

            if (i + 1 < data.Length)
            {
                b |= (data[i + 1] & 0xF0) >> 4;
                result.Append(CharString[b]);
                b = (data[i + 1] & 15) << 2;

                if (i + 2 < data.Length)
                {
                    b |= (data[i + 2] & 0xC0) >> 6;
                    result.Append(CharString[b]);
                    b = data[i + 2] & 0x3F;
                    result.Append(CharString[b]);
                }
                else
                {
                    result.Append(CharString[b]);
                    result.Append('=');
                }
            }
            else
            {
                result.Append(CharString[b]);
                result.Append("==");
            }
        }
        return result.ToString();
    }


    private static string FromBase64(string input)
    {
        if (input.Length % 4 != 0)
        {
            throw new ArgumentOutOfRangeException("Invalid Input!!");
        }
        byte[] decoded = new byte[((input.Length * 3) / 4) - (input.IndexOf('=') > 0 ? (input.Length - input.IndexOf('=')) : 0)];
        char[] inChars = input.ToCharArray();
        int j = 0;
        int[] b = new int[4];
        for (int i = 0; i < inChars.Length; i += 4)
        {
            b[0] = CharString.IndexOf(inChars[i]);
            b[1] = CharString.IndexOf(inChars[i + 1]);
            b[2] = CharString.IndexOf(inChars[i + 2]);
            b[3] = CharString.IndexOf(inChars[i + 3]);
            decoded[j++] = (byte)((b[0] << 2) | (b[1] >> 4));
            if (b[2] < 64)
            {
                decoded[j++] = (byte)((b[1] << 4) | (b[2] >> 2));
                if (b[3] < 64)
                {
                    decoded[j++] = (byte)((b[2] << 6) | b[3]);
                }
            }
        }

        return Encoding.Default.GetString(decoded);
    }

    static void Main(string[] args)
    {

        string data = "this is a string!!";
        Console.WriteLine(ToBase64(data));
        Console.WriteLine(FromBase64(ToBase64(data)));
        Console.ReadLine();
    }
}
