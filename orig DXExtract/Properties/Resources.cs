// Decompiled with JetBrains decompiler
// Type: DXExtract.Properties.Resources
// Assembly: DXExtract, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 593F5E8D-8016-45A8-94A1-EA2F500EB23F
// Assembly location: U:\Unpackers\DXExtract\DXExtract.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace DXExtract.Properties
{
  [DebuggerNonUserCode]
  [CompilerGenerated]
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (object.ReferenceEquals((object) DXExtract.Properties.Resources.resourceMan, (object) null))
          DXExtract.Properties.Resources.resourceMan = new ResourceManager("DXExtract.Properties.Resources", typeof (DXExtract.Properties.Resources).Assembly);
        return DXExtract.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get
      {
        return DXExtract.Properties.Resources.resourceCulture;
      }
      set
      {
        DXExtract.Properties.Resources.resourceCulture = value;
      }
    }

    internal Resources()
    {
    }
  }
}
