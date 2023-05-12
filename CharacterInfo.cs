using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratory_work_No._5
{
    public class CharacterInfo
    {
        public string Name { get; set; }
        public Characteristics Characteristics { get; set; }
        public List<ActionInfo> AvailableActions { get; set; }
        public List<ItemInfo> Items { get; set; }
        public List<ResourceInfo> Resources { get; set; }
    }

    public class Characteristics
    {
        public int MaxHealth { get; set; }
        public int MaxMana { get; set; }
        public int MaxSuitSize { get; set; }
        public int Wisdom { get; set; }
        public int Power { get; set; }
    }

    public class ActionInfo
    {
        public string Name { get; set; }
    }

    public class ItemInfo
    {
        public string Name { get; set; }
        public int Amount { get; set; }
        public int Power { get; set; }
    }

    public class ResourceInfo
    {
        public string Name { get; set; }
        public int Amount { get; set; }

    }
}
