using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealership.Models
{
    public class Model
    {
        #region Fields

        private int _id;
        //TODO: Не должны иметь значение null?
        private string _name = null!;

        private int _brandId;
        private Brand _brand = null!;

        private ICollection<CarConfiguration> _carConfigurations = null!;

        #endregion

        #region Properties

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value ?? throw new ArgumentNullException(nameof(value), "Name cannot be null"); }
        }

        public int BrandId
        {
            get { return _brandId; }
            set { _brandId = value; }
        }

        public Brand Brand
        {
            get { return _brand; }
            set { _brand = value ?? throw new ArgumentNullException(nameof(value), "Brand cannot be null"); }
        }

        public ICollection<CarConfiguration> CarConfigurations
        {
            get { return _carConfigurations; }
            set { _carConfigurations = value ?? throw new ArgumentNullException(nameof(value), "CarConfigurations cannot be null"); }
        }

        #endregion

    }
}
