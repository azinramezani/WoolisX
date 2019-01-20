using System;
using System.Collections.Generic;

namespace WollisXExercise.Models
{
    public class TrolleyCalculatorRequest
    {
        public IEnumerable<Product> Products { get; set; }

        public IEnumerable<SpecialClass> Specials { get; set; }

        public IEnumerable<QuantityClass> Quantities { get; set; }
    }

    public class QuantityClass
    {
        public string Name { get; set; }

        public int Quantity { get; set; }
    }

    public class SpecialClass
    {
        public IEnumerable<QuantityClass> Quantities { get; set; }

        public decimal Total { get; set; }
    }
}
