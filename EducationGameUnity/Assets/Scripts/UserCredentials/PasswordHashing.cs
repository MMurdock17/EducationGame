using UnityEngine;
using System.Text;
using System.Security.Cryptography;

public static class PasswordHashing
{
    public static string Hash(string input)
    {
        SHA256 sha = SHA256.Create();
        byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));

        StringBuilder sb = new StringBuilder();
        foreach (byte b in bytes)
        {
            sb.Append(b.ToString("x2"));
        }
        return sb.ToString();
    }
}
