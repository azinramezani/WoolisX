using System;
using System.Collections.Generic;

namespace WollisXExercise.Models
{
    public class CustomerProduct
    {
        public int CustomerId { get; set; }

        public IEnumerable<Product> Products { get; set; }
    }
}
