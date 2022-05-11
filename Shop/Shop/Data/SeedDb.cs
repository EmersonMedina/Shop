using Microsoft.EntityFrameworkCore;
using Shop.Data.Entities;
using Shop.Enums;
using Shop.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Data
{
    public class SeedDb
    {
        private DataContext _context;
        private readonly  IUserHelper _userHelper;
        private readonly IBlopHelper _blopHelper;

        public SeedDb( DataContext context, IUserHelper userHelper, IBlopHelper blopHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _blopHelper = blopHelper;
        }

        public async Task SeedAsync ()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCategoriesAsync();
            await CheckCountriesAsync();
            await CheckRolesAsync();
            await CheckUserAsync("1010", "Juan", "Zuluaga", "zulu@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", "no_image.png" ,UserType.Admin);
            await CheckUserAsync("1011", "Emerson", "Medina", "emermedina@gmail.com", "322 311 4620", "Calle Luna Calle Sol", "no_image.png", UserType.Admin);
            await CheckUserAsync("1012", "Emer", "Medina", "emersonmedina@gmail.com", "322 311 4620", "Calle Luna Calle Sol", "no_image.png", UserType.User);
            await CheckUserAsync("1013", "Jack", "Sparrow", "jack@gmail.com", "322 311 4620", "Calle Luna Calle Sol", "jack-sparrow.webp", UserType.User);
            await CheckUserAsync("1014", "Jennifer", "Lawrence", "jennifer@gmail.com", "322 311 4620", "Calle Luna Calle Sol", "jennifer-lawrence.png", UserType.User);
            await CheckUserAsync("1015", "Saul", "Goodman", "saul@gmail.com", "322 311 4620", "Calle Luna Calle Sol", "saul-goodman.webp", UserType.User);
            await CheckUserAsync("1016", "Tyrion", "Lannister", "tyrion@gmail.com", "322 311 4620", "Calle Luna Calle Sol", "tyrion-lannister.png", UserType.User);
            await CheckUserAsync("1017", "Walter", "White", "walter@gmail.com", "322 311 4620", "Calle Luna Calle Sol", "walter-white.png", UserType.User);

            await CheckProductsAsync();
        }

        private async Task CheckProductsAsync()
        {
            if (!_context.Products.Any())
            {
                await AddProductAsync("Apple TV", 10000M, 12F, new List<string>() { "Apple", "Tecnología" }, new List<string>() { "apple-tv.png" });
                await AddProductAsync("Bicicleta Deporte Extremo", 250000M, 12F, new List<string>() { "Deportes" }, new List<string>() { "bicicleta-deportiva.png" });
                await AddProductAsync("Burros Timberland", 2000M, 12F, new List<string>() {"Calzado" }, new List<string>() { "burros-timberland.png"});
                await AddProductAsync("Camisa Under Armour", 1000M, 12F, new List<string>() { "Ropa" }, new List<string>() { "camisa-under-armour.webp" });
                await AddProductAsync("Comida de perro", 1500M, 6F, new List<string>() { "Mascotas" }, new List<string>() { "comida-perro.png" });
                await AddProductAsync("Samsung Galaxy S10", 6800M, 24F, new List<string>() { "Tecnología" }, new List<string>() { "galaxys10.webp", "galaxys10-2.png" });
                await AddProductAsync("Gelatina Cabello Hombre", 100M, 12F, new List<string>() { "Belleza" }, new List<string>() { "gelatina-cabello.png"});
                await AddProductAsync("Juguete Perro", 300M, 6F, new List<string>() { "Mascotas"}, new List<string>() { "juguete-perro.png" });
                await AddProductAsync("Mac Pro", 20000M, 6F, new List<string>() { "Tecnología", "Apple" }, new List<string>() { "mac-pro.png" });
                await AddProductAsync("Micrófono", 8000M, 6F, new List<string>() { "Tecnología"}, new List<string>() { "microfono.png" });
                await AddProductAsync("Monitor", 1700M, 12F, new List<string>() { "Tecnología" }, new List<string>() { "monitor.png" });
                await AddProductAsync("Mouse", 900M, 100F, new List<string>() { "Tecnología" }, new List<string>() { "mouse.png" });
                await AddProductAsync("Mouse pad", 250M, 12F, new List<string>() { "Tecnología"}, new List<string>() { "mouse-pad-1.png", "mouse-pad-2.png" });
                await AddProductAsync("Pantalón Caballero", 800M, 12F, new List<string>() {"Ropa" }, new List<string>() { "pantalon.png" });
                await AddProductAsync("Pelota Fútbol", 1800M, 12F, new List<string>() { "Deportes" }, new List<string>() { "pelota-futbol.png" });
                await AddProductAsync("Rasuradora Caballero", 1000M, 12F, new List<string>() {"Belleza" }, new List<string>() { "rasuradora.png" });
                await AddProductAsync("Sandalias Caballero", 400M, 12F, new List<string>() { "Ropa" }, new List<string>() { "sandalias-caballero.webp" });
                await AddProductAsync("Vitaminas Niños", 600M, 12F, new List<string>() { "Nutrición" }, new List<string>() { "vitaminas-niños.png" });
                await _context.SaveChangesAsync();
            };
        }

        private async Task AddProductAsync(string name, decimal price, float stock, List<string> categories, List<string> images)
        {
            Product prodcut = new()
            {
                Description = name,
                Name = name,
                Price = price,
                Stock = stock,
                ProductCategories = new List<ProductCategory>(),
                ProductImages = new List<ProductImage>()
            };

            foreach (string? category in categories)
            {
                prodcut.ProductCategories.Add(new ProductCategory { Category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == category) });
            }


            foreach (string? image in images)
            {
                Guid imageId = await _blopHelper.UploadBlobAsync($"{Environment.CurrentDirectory}\\wwwroot\\images\\products\\{image}", "products");
                prodcut.ProductImages.Add(new ProductImage { ImageId = imageId });
            }

            _context.Products.Add(prodcut);
        }

        private async Task<User> CheckUserAsync(
                                                string document,
                                                string firstName,
                                                string lastName,
                                                string email,
                                                string phone,
                                                string address,
                                                string image,
                                                UserType userType)
        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                Guid imageId = await _blopHelper.UploadBlobAsync($"{Environment.CurrentDirectory}\\wwwroot\\images\\users\\{image}", "users");
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
                    City = _context.Cities.FirstOrDefault(),
                    UserType = userType,
                    ImageId = imageId
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());

                string token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);

            }

            return user;
        }


        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }

        private async Task CheckCountriesAsync()
        {
            if (!_context.Countries.Any())
            {
                _context.Countries.Add(
                    new Country
                    {
                        Name = "Honduras",
                        States = new List<State>() {
                             new State {
                                    Name = "Francisco Morazán" ,
                                    Cities =  new List<City>()
                                    {
                                        new City
                                        {
                                            Name ="Tegucigalpa"
                                        }
                                    }
                             },
                             new State
                             {
                                    Name = "Cortes" ,
                                    Cities =  new List<City>()
                                    {
                                        new City
                                        {
                                            Name ="San Pedro Sula"
                                        }
                                    }
                             },
                             new State
                             {
                                    Name = "Comayagua" ,
                                    Cities =  new List<City>()
                                    {
                                        new City
                                        {
                                            Name ="Comayagua"
                                        }
                                    }
                             },
                             new State
                             {
                                    Name = "Atlántida" ,
                                    Cities =  new List<City>()
                                    {
                                        new City
                                        {
                                            Name ="La Ceiba"
                                        }
                                    }
                             },
                             new State
                             {
                                    Name = "Choluteca" ,
                                    Cities =  new List<City>()
                                    {
                                        new City
                                        {
                                            Name ="Choluteca"
                                        }
                                    }
                             },
                             new State
                             {
                                    Name = "El Paraíso" ,
                                    Cities =  new List<City>()
                                    {
                                        new City
                                        {
                                            Name ="Yuscarán"
                                        }
                                    }
                             },
                             new State
                             {
                                    Name = "Gracias a Dios" ,
                                    Cities =  new List<City>()
                                    {
                                        new City
                                        {
                                            Name ="Puerto Lempira"
                                        }
                                    }
                             },
                             new State
                             {
                                    Name = "Ocotepeque" ,
                                    Cities =  new List<City>()
                                    {
                                        new City
                                        {
                                            Name ="Ocotepeque"
                                        }
                                    }
                             }
                        }
                    });

                _context.Countries.Add(
                    new Country
                    {
                        Name = "Estados Unidos",
                        States = new List<State>() {
                             new State {
                                    Name = "Carolina del norte" ,
                                    Cities =  new List<City>()
                                    {
                                        new City
                                        {
                                            Name ="Raleigh"
                                        }
                                    }
                             },
                             new State
                             {
                                    Name = "Georgia" ,
                                    Cities =  new List<City>()
                                    {
                                        new City
                                        {
                                            Name ="Atlanta"
                                        }
                                    }
                             },
                             new State
                             {
                                    Name = "Arkansas" ,
                                    Cities =  new List<City>()
                                    {
                                        new City
                                        {
                                            Name ="Little Rock"
                                        }
                                    }
                             },
                             new State
                             {
                                    Name = "Louisiana" ,
                                    Cities =  new List<City>()
                                    {
                                        new City
                                        {
                                            Name ="Baton Rouge"
                                        }
                                    }
                             },
                             new State
                             {
                                    Name = "Texas" ,
                                    Cities =  new List<City>()
                                    {
                                        new City
                                        {
                                            Name ="Austin"
                                        }
                                    }
                             },
                             new State
                             {
                                    Name = "Arizona" ,
                                    Cities =  new List<City>()
                                    {
                                        new City
                                        {
                                            Name ="Phoenix"
                                        }
                                    }
                             },
                             new State
                             {
                                    Name = "Colorado" ,
                                    Cities =  new List<City>()
                                    {
                                        new City
                                        {
                                            Name ="Denver"
                                        }
                                    }
                             },
                             new State
                             {
                                    Name = "Kansas" ,
                                    Cities =  new List<City>()
                                    {
                                        new City
                                        {
                                            Name ="Topeka"
                                        }
                                    }
                             }
                        }
                    });

                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckCategoriesAsync()
        {
            if (!_context.Categories.Any())
            {
                _context.Categories.Add(new Category { Name = "Tecnología" });
                _context.Categories.Add(new Category { Name = "Ropa" });
                _context.Categories.Add(new Category { Name = "Calzado" });
                _context.Categories.Add(new Category { Name = "Belleza" });
                _context.Categories.Add(new Category { Name = "Deportes" });
                _context.Categories.Add(new Category { Name = "Apple" });
                _context.Categories.Add(new Category { Name = "Mascotas" });
                _context.Categories.Add(new Category { Name = "Nutrición" });
                await _context.SaveChangesAsync();
            }
        }
    }
}
