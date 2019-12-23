using System.Collections.Generic;
using System.Linq;

namespace Models
{
    public class ItemPrototype
    {
        private static readonly Dictionary<string, ItemInfo> nameToItemInfo=new Dictionary<string, ItemInfo>();

        public static void AddToProtoList(string key, ItemInfo itemInfo)
        {
            nameToItemInfo.Add(key,itemInfo);
        }

        public static ItemInfo GetItemInfo(string key)
        {
            return nameToItemInfo.ContainsKey(key) ? nameToItemInfo[key] : null;
        }

        public static void InitializeObjects()
        {
            for (int i = 0; i < 5; i++)
                AddToProtoList("Computer" + i, new ItemInfo());
    
            Item Item;
            for (int i = 0; i < 5; i++)
            {
                Item = new Computer()
                {
                    Name = "Computer" + i,
                    mps = i + 1,
                    requiredLevel = (2 * i + 1),
                    requiredMoneyForUpgrade = 30
                };

                GetItemInfo("Computer" + i).item = Item;
            }
        }

        public static ItemInfo[] GetItemInfos()
        {
            return nameToItemInfo.Values.ToArray();
        }
    }
}