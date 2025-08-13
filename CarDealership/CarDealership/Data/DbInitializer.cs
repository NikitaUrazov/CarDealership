using Bogus;
using CarDealership.Models;

namespace CarDealership.Data
{
    public static class DbInitializer
    {

        #region Fields

        private static string[] _colors = new string[]
        {
            "Red", "Blue", "Black", "White"
        };

        private static Faker _faker = new Faker();

        #endregion

        #region Methods

        private static List<Brand> GetBrands(int count)
        {
            List<Brand> brands = new List<Brand>();
            HashSet<string> uniqueBrands = new HashSet<string>();

            while (uniqueBrands.Count < count)
            {
                string brandName = _faker.Vehicle.Manufacturer();
                if (uniqueBrands.Add(brandName))
                {
                    Brand brand = new Brand() { Name = brandName };
                    brands.Add(brand);
                }
            }

            return brands;
        }

        private static List<Model> GetModels(List<Brand> brands, int countPerBrand)
        {
            // Пары Марка-Модель уникальны. В разных марках могут быть одинаковые модели.
            // Например, "Toyota Camry" и "Honda Camry" - это разные модели, но с одинаковым названием.
            List<Model> models = new List<Model>();
            HashSet<string> uniqueModels = new HashSet<string>();
            foreach (Brand brand in brands)
            {
                int count = 0;
                while (count < countPerBrand)
                {
                    string modelName = _faker.Vehicle.Model();
                    if (uniqueModels.Add($"{brand.Name}|{modelName}"))
                    {
                        count++;
                        Model model = new Model() 
                        { 
                            Name = modelName, 
                            Brand = brand 
                        };

                        models.Add(model);
                    }
                }
            }

            return models;
        }

        private static List<CarConfiguration> GetCarConfigurations(List<Model> models, int minCount, int maxCount)
        {
            const int MIN_PRICE = 1_000_000;
            const int MAX_PRICE = 10_000_000;

            HashSet<string> uniqueCarConfigurations = new HashSet<string>();
            List<CarConfiguration> carConfigurations = new List<CarConfiguration>();

            foreach (Model model in models)
            {
                int count = _faker.Random.Int(minCount, maxCount);
                while (count > 0)
                {
                    string package = _faker.Vehicle.Type();
                    string engine = _faker.Vehicle.Fuel();
                    string color = _faker.PickRandom(_colors);

                    decimal price = _faker.Random.Decimal(MIN_PRICE, MAX_PRICE);
                    decimal roundedPrice = price - (price % 1000);

                    string carConfigString = $"{model.Brand.Name}|{model.Name}|{package}|{engine}|{color}";
                    if (uniqueCarConfigurations.Add(carConfigString))
                    {
                        count--;
                        CarConfiguration carConfiguration = new CarConfiguration()
                        {
                            Model = model,
                            Color = color,
                            Engine = engine,
                            Package = package,
                            Price = roundedPrice
                        };
                        carConfigurations.Add(carConfiguration);
                    }
                }
            }

            return carConfigurations;
        }

        private static List<Order> GetOrders(List<CarConfiguration> carConfigurations)
        {
            // Генерируем случайное кол-во заказов (>= 1000)
            int orderCount = _faker.Random.Int(1_000, 2_000);

            List<Order> orders = new List<Order>();
            for (int i = 0; i < orderCount; i++)
            {
                CarConfiguration carConfiguration = _faker.PickRandom(carConfigurations);
                DateTime orderDate = _faker.Date.Between(DateTime.Now.AddYears(-5), DateTime.Now);

                // Сделаем 10% шанс на оптовый заказ
                int quantity = _faker.Random.Double(0, 1) < 0.1 ? _faker.Random.Int(2, 10) : 1;

                Order order = new Order()
                {
                    CarConfiguration = carConfiguration,
                    OrderDate = orderDate,
                    Quantity = quantity
                };
                orders.Add(order);
            }

            return orders;
        }

        public static void Seed(ApplicationDbContext context)
        {
            const int BRANDS_COUNT = 6;
            const int MODELS_PER_BRAND = 5;
            const int MIN_CONFIGURATIONS_PER_MODEL = 1;
            const int MAX_CONFIGURATIONS_PER_MODEL = 3;

            List<Brand> brands = GetBrands(BRANDS_COUNT);
            List<Model> models = GetModels(brands, MODELS_PER_BRAND);
            List<CarConfiguration> carConfigurations = GetCarConfigurations(models, MIN_CONFIGURATIONS_PER_MODEL, MAX_CONFIGURATIONS_PER_MODEL);
            List<Order> orders = GetOrders(carConfigurations);


            context.Brands.AddRange(brands);
            context.Models.AddRange(models);
            context.CarConfigurations.AddRange(carConfigurations);
            context.Orders.AddRange(orders);
            context.SaveChanges();
        }

        #endregion
    }
}
