using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surprise
{
    /// <summary>
    /// A Firebase model for saving a Description and the current Date
    /// </summary>
    public class ItemsModel
    {
        public string Name { get; set; }
        public string Number { get; set; }
        [JsonIgnore]
        public string Key { get; set; }
    }
}