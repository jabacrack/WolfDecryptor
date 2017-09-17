using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using WolfDecryptor.Annotations;

namespace WolfDecryptor
{
    class MainWindowVM : INotifyPropertyChanged
    {
        private const string KeysFilePath = @"keys.ini";
        private const string KeySeparator = " ";
        private string userKey;
        private string filePath;
        private string keyMessage;

        public MainWindowVM()
        {
        }

        public string FilePath
        {
            get { return filePath; }
            set
            {
                if (value == filePath) return;
                filePath = value;
                OnPropertyChanged(nameof(FilePath));
            }
        }
        
        public string UserKey
        {
            get { return userKey; }
            set
            {
                if (value == userKey) return;
                userKey = value;
                OnPropertyChanged(nameof(UserKey));
            }
        }

        public string KeyMessage
        {
            get { return keyMessage; }
            set
            {
                if (value == keyMessage) return;
                keyMessage = value;
                OnPropertyChanged(nameof(KeyMessage));
            }
        }

        private string[] ReadKeys()
        {
            var fullPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                KeysFilePath);
            var separator = KeySeparator.ToCharArray();
            return File.ReadAllLines(fullPath)
                .Select(line => line.Split(separator, StringSplitOptions.RemoveEmptyEntries))
                .Where(x => x.Length > 1)
                .Select(x => x[1])
                .ToArray();

        }

        private bool CheckKeys()
        {
            if
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
