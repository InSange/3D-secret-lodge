using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

public class FileIO
{
    public static void save(string path, byte[] bytes)
    {
        FileStream fs = File.Open(path, FileMode.OpenOrCreate);
        fs.Write(bytes, 0, bytes.Length);
        fs.Close();
    }

    public static byte[] load(string path)
    {
        if (File.Exists(path) == false)
            return null;

        FileStream fs = File.Open(path, FileMode.Open);
        int len = (int)fs.Length;
        byte[] bytes = new byte[len];
        fs.Read(bytes, 0, len);
        fs.Close();

        return bytes;
    }

    public static byte[] struct2bytes(object obj)
    {
        int len = Marshal.SizeOf(obj);
        byte[] bytes = new byte[len];

        IntPtr ptr = Marshal.AllocHGlobal(len);
        Marshal.StructureToPtr(obj, ptr, false);
        Marshal.Copy(ptr, bytes, 0, len);
        Marshal.FreeHGlobal(ptr);

        return bytes;
    }

    public static T bytes2struct<T>(byte[] bytes) where T : struct
    {
        int len = Marshal.SizeOf(typeof(T));
        if (len > bytes.Length)
            throw new Exception();

        IntPtr ptr = Marshal.AllocHGlobal(len);
        Marshal.Copy(bytes, 0, ptr, len);
        T obj = (T)Marshal.PtrToStructure(ptr, typeof(T));
        Marshal.FreeHGlobal(ptr);

        return obj;
    }
}
