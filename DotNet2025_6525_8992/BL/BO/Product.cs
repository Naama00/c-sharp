using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.BO;

public class Product
    {
        public int Id { get; init; }
        public string Name { get; set; } = string.Empty;
        public Categories Category { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }

        public override string ToString() => base.ToString();
    }
