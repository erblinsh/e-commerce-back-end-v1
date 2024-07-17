﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public string CartIdentifier { get; set; } // for guest users, empty for registered ones

        [ForeignKey("UserId")]
        public int? UserId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public List<CartItem> CartItems { get; set; }
        [NotMapped]
        public int TotalQuantity => CartItems.Sum(item => item.Quantity);

        [NotMapped]
        public decimal TotalPrice => CartItems.Sum(item => item.Product.Price * item.Quantity);
    }
}
