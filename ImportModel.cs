using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratory_work_No._5
{
    internal class ImportModel
    {
        public class Settings
        {
            public int? id_profile { get; set; }
            public int? id_setting { get; set; }
            public byte? value { get; set; }
        }

        public class Character
        {
            public int? id_profile { get; set; }
            public string name { get; set; }
            public int? id_character_characteristics { get; set; }
            public int? id_skill { get; set; }
        }

        public class Item
        {
            public int? id_character { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public int? amount { get; set; }
            public int? id_type { get; set; }
            public int? power { get; set; }
        }
    }
}
