using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonTasker.Models
{
    public class ShopModel
    {
        public List<ItemModel> BuyWeapons = new List<ItemModel>(); // Store weapon item details
        public List<ItemModel> BuyItem = new List<ItemModel>(); // Store weapon item details
        public List<ItemModel> BuyCharacter = new List<ItemModel>();
        public InventoryItemsModel Inv;
        public ShopModel(InventoryItemsModel Inv)
        {
            this.Inv = Inv;
            Rebuild();
        }

        public void Rebuild()
        {
            CreateWeaponPool();
            CreateItemPool();
            CreateCharacterPool();
        }

        private void CreateWeaponPool()
        {
            BuyWeapons.Clear();
            Random rnd = new Random();
            string[] list = { "SteelSword", "SteelAxe", "DiamondBow" };
            int weapon1 = rnd.Next(0, 3);
            int weapon2 = rnd.Next(0, 3);
            while (weapon2 == weapon1)
                weapon2 = rnd.Next(0, 3);

            BuyWeapons.Add(new ItemModel(list[weapon1]));
            BuyWeapons.Add(new ItemModel(list[weapon2]));
        }

        private void CreateItemPool()
        {
            BuyItem.Clear();
            BuyItem.Add(new ItemModel("HealthPotion"));
            BuyItem.Add(new ItemModel("MagicPotion"));
        }

        private void CreateCharacterPool()
        {
            BuyCharacter.Clear();
            Random rnd = new Random();
            string[] list = { "( ͡° ͜ʖ ͡°)", "(¬‿¬)", "(ノಠ益ಠ)ノ", "(；一_一)" };
            int Character1 = rnd.Next(0, list.Length);
            int Character2 = rnd.Next(0, list.Length);
            while (Character1 == Character2)
                Character2 = rnd.Next(0, list.Length);
            BuyCharacter.Add(new ItemModel(list[Character1]));
            BuyCharacter.Add(new ItemModel(list[Character2]));
        }

        public int CheckItem(string item)
        {
            if(WeaponInfoModel.ObtainWeaponInfo(item,true)!= 0)
            {
                return 0; 
            }
            else if(ItemInfoModel.ObtainItemInfo(item,true)!=0)
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }

    }
}
