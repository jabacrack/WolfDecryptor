// Decompiled with JetBrains decompiler
// Type: DXExtract.Properties.Settings
// Assembly: DXExtract, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 593F5E8D-8016-45A8-94A1-EA2F500EB23F
// Assembly location: U:\Unpackers\DXExtract\DXExtract.exe

using System.CodeDom.Compiler;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace DXExtract.Properties
{
  [CompilerGenerated]
  [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
  internal sealed class Settings : ApplicationSettingsBase
  {
    private static Settings defaultInstance = (Settings) SettingsBase.Synchronized((SettingsBase) new Settings());

    public static Settings Default
    {
      get
      {
        return Settings.defaultInstance;
      }
    }
  }
}
