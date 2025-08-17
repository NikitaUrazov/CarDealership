using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealership.Dtos
{
    public class CarConfigurationDto
    {
        public int Id { get; set; }
        public string BrandName { get; set; }
        public string ModelName { get; set; }
        public string Color { get; set; }
        public string Package { get; set; }
        public string Engine { get; set; }
        public decimal Price { get; set; }
    }
}
