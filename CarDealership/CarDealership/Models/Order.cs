using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealership.Models
{
    public class Order
    {
        #region Fields

        private int _id;

        private int _carConfigurationId;
        private CarConfiguration _carConfiguration = null!;

        private int _quantity;
        private DateTime _orderDate;

        #endregion

        #region Properties

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public int CarConfigurationId
        {
            get { return _carConfigurationId; }
            set { _carConfigurationId = value; }
        }

        public CarConfiguration CarConfiguration
        {
            get { return _carConfiguration; }
            set { _carConfiguration = value ?? throw new ArgumentNullException(nameof(value), "CarConfiguration cannot be null"); }
        }

        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        public DateTime OrderDate
        {
            get { return _orderDate; }
            set { _orderDate = value; }
        }

        #endregion
    }
}
