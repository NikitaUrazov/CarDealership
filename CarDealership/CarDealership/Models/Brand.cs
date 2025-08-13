using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealership.Models
{
    public class Brand
    {
        #region Fields

        private int _id;
        //TODO: Не должны иметь значение null?
        //TODO: Оставить только автосвойства?
        //TODO: dbo перед названием таблиц в БД?
        private string _name = null!;
        private ICollection<Model> _models = null!;

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

        public ICollection<Model> Models
        {
            get { return _models; }
            set { _models = value ?? throw new ArgumentNullException(nameof(value), "Models cannot be null"); }
        }

        #endregion

        #region Ctr
        //TODO: Установка значений (=null!) после пустого конструктора.
        #endregion
    }
}
