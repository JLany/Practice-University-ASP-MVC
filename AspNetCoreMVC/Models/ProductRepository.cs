namespace AspNetCoreMVC.Models
{
    public class ProductRepository
    {
        private List<Product> _products;

        public ProductRepository()
        {
            _products = new()
            {
                new Product
                {
                    Id = 1,
                    Name = "Cannon Camera",
                    Description = "Professional Camera to save the best moments of life.",
                    Price = 19.99m,
                    ImageUri = "/images/camera.jpg"
                },
                new Product
                {
                    Id = 2,
                    Name = "Samsung Headphones",
                    Description = "Keep the world around you quite, and listen.",
                    Price = 29.99m,
                    ImageUri = "/images/headphones.jpg"
                },
                new Product
                {
                    Id = 3,
                    Name = "Apple Watch",
                    Description = "Another useless apple product.",
                    Price = 99.99m,
                    ImageUri = "/images/watchjpg.jpg"
                }
            };
        }

        public IEnumerable<Product> GetAll()
        {
            return _products;
        }

        public Product? Get(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }
    }
}
