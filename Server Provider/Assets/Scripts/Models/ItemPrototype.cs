using System.Collections.Generic;
using System.Linq;

namespace Models
{
    public class ItemPrototype
    {
        private static Dictionary<string, ItemInfo> nameToItemInfo = new Dictionary<string, ItemInfo>();

        public static void AddToProtoList(string key, ItemInfo itemInfo)
        {
            nameToItemInfo.Add(key, itemInfo);
        }

        public static ItemInfo GetItemInfo(string key)
        {
            return nameToItemInfo.ContainsKey(key) ? nameToItemInfo[key] : null;
        }

        public static void InitializeObjects()
        {
            for (int i = 0; i < 7; i++)
                AddToProtoList("Animal" + i, new ItemInfo());

            Item Item;
            for (int i = 0; i < 7; i++)
            {
                Item = new Cat()
                {
                    Name = "Animal" + i,
                    mps = i * 3 + 2,
                    requiredLevel = (2 * i + 1),
                    requiredMoneyForUpgrade = 30 * (i + 1),
                    requiredMoneyForPlant = i * 1000
                };

                GetItemInfo("Animal" + i).item = Item;
            }
        }

        public static ItemInfo[] GetItemInfos()
        {
            return nameToItemInfo.Values.ToArray();
        }
    }
}