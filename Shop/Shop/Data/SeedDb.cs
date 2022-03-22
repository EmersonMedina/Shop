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

        public SeedDb( DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync ()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCategoriesAsync();
            await CheckCountriesAsync();
            await CheckRolesAsync();
            await CheckUserAsync("1010", "Juan", "Zuluaga", "zulu@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", UserType.Admin);
        }

        private async Task<User> CheckUserAsync(
                                                string document,
                                                string firstName,
                                                string lastName,
                                                string email,
                                                string phone,
                                                string address,
                                                UserType userType)
        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
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
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
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
