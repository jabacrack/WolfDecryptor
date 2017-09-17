// Decompiled with JetBrains decompiler
// Type: DXExtract.DXParser
// Assembly: DXExtract, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 593F5E8D-8016-45A8-94A1-EA2F500EB23F
// Assembly location: U:\Unpackers\DXExtract\DXExtract.exe

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace DXExtract
{
  internal class DXParser
  {
    private List<Entry> entries = new List<Entry>();
    private List<DXDir> dirs = new List<DXDir>();
    private const string keyPath = "keys.ini";
    private string archivePath;
    private BinaryReader inFile;
    protected BinaryWriter outFile;
    private byte[] data;
    private byte[] key;
    private long tableOfs;
    private long entryOfs;
    private long dirOfs;
    private int headerSize;
    private string rootName;

    public DXParser(string path)
    {
      this.archivePath = path;
      this.rootName = Path.GetFileNameWithoutExtension(path);
      this.inFile = new BinaryReader((Stream) File.OpenRead(path));
    }

    private void createFile(string path)
    {
      path = Path.Combine(this.rootName, path);
      string directoryName = Path.GetDirectoryName(this.archivePath);
      string path1 = Path.Combine(directoryName, Path.GetDirectoryName(path));
      string path2 = Path.Combine(directoryName, path);
      Directory.CreateDirectory(path1);
      this.outFile = new BinaryWriter((Stream) File.OpenWrite(path2));
    }

    private byte[] readData(long offset, int size, int zsize)
    {
      this.inFile.BaseStream.Seek(offset, SeekOrigin.Begin);
      if (zsize != -1)
        return this.decompress(this.inFile.ReadBytes(size), zsize, size);
      return this.inFile.ReadBytes(size);
    }

    private void writeFile(Entry e)
    {
      if (e.type != 32)
        return;
      this.createFile(e.name);
      this.data = this.readData(e.offset, e.size, e.zsize);
      this.outFile.Write(this.data);
      this.outFile.Close();
      Console.WriteLine("{0} wrote out successfully", (object) e.name);
    }

    private void writeEntries()
    {
      foreach (Entry entry in this.entries)
        this.writeFile(entry);
    }

    private bool validArchive(byte[] header)
    {
      if ((int) header[0] == 68)
        return (int) header[1] == 88;
      return false;
    }

    public byte[] makeKey(string str)
    {
      int length = str.Length;
      byte[] numArray = new byte[length / 2];
      int startIndex = 0;
      while (startIndex < length)
      {
        numArray[startIndex / 2] = Convert.ToByte(str.Substring(startIndex, 2), 16);
        startIndex += 2;
      }
      return numArray;
    }

    private bool testKey(byte[] header, byte[] key)
    {
      byte[] numArray = new byte[4]
      {
        (byte) ((uint) header[0] ^ (uint) key[0]),
        (byte) ((uint) header[1] ^ (uint) key[1]),
        (byte) 0,
        (byte) ((uint) header[3] ^ (uint) key[3])
      };
      if ((int) numArray[0] == 68 && (int) numArray[1] == 88)
        return (int) numArray[3] == 0;
      return false;
    }

    private bool checkKeyFile()
    {
      return File.Exists(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "keys.ini"));
    }

    public string checkForKey()
    {
      byte[] header = this.inFile.ReadBytes(4);
      if (this.validArchive(header) || !this.checkKeyFile())
        return "";
      foreach (string readAllLine in File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "keys.ini")))
      {
        char[] chArray = new char[1]{ ' ' };
        string str = readAllLine.Split(chArray)[0].Trim();
        byte[] key = this.makeKey(str);
        if (this.testKey(header, key))
        {
          this.key = key;
          return str;
        }
      }
      return "";
    }

    private byte[] decompress(byte[] src, int srclen, int destlen)
    {
      byte[] numArray1 = new byte[destlen];
      byte num1 = src[8];
      int index1 = 9;
      int num2 = 0;
      while (index1 < srclen && num2 < destlen)
      {
        if ((int) src[index1] == (int) num1)
        {
          int index2 = index1 + 1;
          if ((int) src[index2] == (int) num1)
          {
            byte[] numArray2 = numArray1;
            int index3 = num2++;
            byte[] numArray3 = src;
            int index4 = index2;
            int num3 = 1;
            index1 = index4 + num3;
            int num4 = (int) numArray3[index4];
            numArray2[index3] = (byte) num4;
          }
          else
          {
            if ((int) src[index2] >= (int) num1)
              --src[index2];
            int num3 = ((int) src[index2] >> 3) + 4;
            byte[] numArray2 = src;
            int index3 = index2;
            int num4 = 1;
            index1 = index3 + num4;
            byte num5 = (byte) ((uint) numArray2[index3] & 7U);
            byte num6 = (byte) ((uint) num5 >> 2);
            byte num7 = (byte) ((uint) num5 & 3U);
            if ((int) num6 != 0)
              num3 += (int) src[index1++] << 5;
            int num8;
            switch (num7)
            {
              case 0:
                num8 = (int) src[index1++] + 1;
                break;
              case 1:
                num8 = (int) src[index1] + ((int) src[index1 + 1] << 8) + 1;
                index1 += 2;
                break;
              case 2:
                num8 = (int) src[index1] + ((int) src[index1 + 1] << 8) + ((int) src[index1 + 2] << 16) + 1;
                index1 += 3;
                break;
              default:
                num8 = 0;
                break;
            }
            for (int index4 = 0; index4 < num3; ++index4)
              numArray1[num2 + index4] = numArray1[num2 - num8 + index4];
            num2 += num3;
          }
        }
        else
          numArray1[num2++] = src[index1++];
      }
      return numArray1;
    }

    private int decryptFile()
    {
      this.inFile.BaseStream.Seek(0L, SeekOrigin.Begin);
      int length = (int) this.inFile.BaseStream.Length;
      this.data = this.inFile.ReadBytes(length);
      if ((int) this.data[0] == 68 && (int) this.data[1] == 88)
        return 1;
      for (int index = 0; index < length; ++index)
        this.data[index] ^= this.key[index % 12];
      this.inFile = new BinaryReader((Stream) new MemoryStream(this.data));
      return 1;
    }

    private string buildPath(DXDir dir, string name)
    {
      if (dir.parentID == -1)
        return name;
      return this.buildPath(this.dirs[dir.parentID], Path.Combine(dir.name, name));
    }

    private void parseHeader()
    {
      this.inFile.BaseStream.Seek(0L, SeekOrigin.Begin);
      this.inFile.ReadBytes(2);
      int num = (int) this.inFile.ReadInt16();
      this.inFile.ReadInt32();
      this.headerSize = this.inFile.ReadInt32();
      this.tableOfs = (long) this.inFile.ReadInt32();
      this.entryOfs = (long) this.inFile.ReadInt32() + this.tableOfs;
      this.dirOfs = (long) this.inFile.ReadInt32() + this.tableOfs;
    }

    private void parseDirectories()
    {
      this.inFile.BaseStream.Seek(this.dirOfs, SeekOrigin.Begin);
      while (this.inFile.BaseStream.Position != this.inFile.BaseStream.Length)
      {
        DXDir dxDir = new DXDir();
        dxDir.entryOfs = (long) this.inFile.ReadInt32() + this.entryOfs;
        dxDir.parentID = this.inFile.ReadInt32();
        dxDir.entryCount = this.inFile.ReadInt32();
        this.inFile.ReadInt32();
        if (dxDir.parentID != -1)
          dxDir.parentID /= 16;
        this.dirs.Add(dxDir);
      }
      foreach (DXDir dir in this.dirs)
      {
        this.inFile.BaseStream.Seek(dir.entryOfs, SeekOrigin.Begin);
        long offset = (long) (int) ((long) this.inFile.ReadUInt32() + this.tableOfs);
        dir.name = this.getName(offset);
      }
      Console.WriteLine("{0} directories found", (object) this.dirs.Count);
    }

    private void parseEntries()
    {
      this.inFile.BaseStream.Seek(this.entryOfs + 44L, SeekOrigin.Begin);
      for (int index1 = 0; index1 < this.dirs.Count; ++index1)
      {
        for (int index2 = 0; index2 < this.dirs[index1].entryCount; ++index2)
        {
          Entry entry = new Entry();
          long offset = (long) this.inFile.ReadInt32() + this.tableOfs;
          entry.type = (int) this.inFile.ReadByte();
          this.inFile.ReadBytes(3);
          this.inFile.BaseStream.Seek(24L, SeekOrigin.Current);
          entry.offset = (long) (this.inFile.ReadInt32() + this.headerSize);
          entry.size = this.inFile.ReadInt32();
          entry.zsize = this.inFile.ReadInt32();
          long position = this.inFile.BaseStream.Position;
          string name = this.getName(offset);
          this.inFile.BaseStream.Seek(position, SeekOrigin.Begin);
          entry.name = this.buildPath(this.dirs[index1], name);
          if (entry.name != "")
            this.entries.Add(entry);
        }
      }
      Console.WriteLine("{0} files found", (object) this.entries.Count);
    }

    private string getName(long offset)
    {
      this.inFile.BaseStream.Seek(offset, SeekOrigin.Begin);
      int count = (int) this.inFile.ReadInt16() * 4;
      int num = (int) this.inFile.ReadInt16();
      this.inFile.ReadBytes(count);
      return Encoding.GetEncoding("shift-JIS").GetString(this.inFile.ReadBytes(count)).Replace("\0", string.Empty);
    }

    public List<Entry> parseFile()
    {
      this.decryptFile();
      this.parseHeader();
      this.parseDirectories();
      this.parseEntries();
      return this.entries;
    }

    public byte[] getFiledata(Entry e)
    {
      return this.readData(e.offset, e.size, e.zsize);
    }

    public void saveFile(string path)
    {
      this.inFile.BaseStream.Seek(0L, SeekOrigin.Begin);
      BinaryWriter binaryWriter = new BinaryWriter((Stream) File.OpenWrite(path));
      byte[] buffer = this.inFile.ReadBytes((int) this.inFile.BaseStream.Length);
      binaryWriter.Write(buffer);
      binaryWriter.Close();
    }

    public void exportArchive()
    {
      this.writeEntries();
    }

    public void exportFile(Entry e)
    {
      this.writeFile(e);
    }
  }
}
