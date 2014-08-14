using System.Collections.Generic;

namespace Motorcycle.Config.Data
{
    struct ReturnData
    {
        private readonly Dictionary<string, string> _dataDictionary, _filesDictionary;

        internal ReturnData(Dictionary<string, string> data, Dictionary<string, string> files)
        {
            _dataDictionary = data;
            _filesDictionary = files;
        }

        internal Dictionary<string, string> GetData { get { return _dataDictionary; } }
        internal Dictionary<string, string> GetFiles { get { return _filesDictionary; } }
    }
}