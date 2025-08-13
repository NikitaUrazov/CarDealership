using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace CarDealership.Models
{
    public class CarConfiguration
    {
        #region Fields

        private int _id;

        private int _modelId;
        private Model _model = null!;

        private string _color = null!;
        private string _engine = null!;

        /// <summary>
        /// Комплектация
        /// </summary>
        private string _package = null!;
        private decimal _price;

        private ICollection<Order> _orders = null!;

        #endregion

        #region Properties

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public int ModelId
        {
            get { return _modelId; }
            set { _modelId = value; }
        }

        public Model Model
        {
            get { return _model; }
            set { _model = value ?? throw new ArgumentNullException(nameof(value), "Model cannot be null"); }
        }

        public string Color
        {
            get { return _color; }
            set { _color = value ?? throw new ArgumentNullException(nameof(value), "Color cannot be null"); }
        }

        public string Engine
        {
            get { return _engine; }
            set { _engine = value ?? throw new ArgumentNullException(nameof(value), "Engine cannot be null"); }
        }

        public string Package
        {
            get { return _package; }
            set { _package = value ?? throw new ArgumentNullException(nameof(value), "Package cannot be null"); }
        }

        public decimal Price
        {
            get { return _price; }
            set { _price = value; }
        }

        public ICollection<Order> Orders
        {
            get { return _orders; }
            set { _orders = value ?? throw new ArgumentNullException(nameof(value), "Orders cannot be null"); }
        }

        #endregion
    }
}
