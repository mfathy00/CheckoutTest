using System;
namespace ShoppingListApi
{
    public class ShoppingListItem
    {
        public int Id { get; set; }
        public int ItemID { get; set; }

        public string ItemName { get; set; }
        public double Price { get; set; }

    }
}
