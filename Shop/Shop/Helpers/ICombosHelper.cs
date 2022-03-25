using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Helpers
{
    public interface ICombosHelper
    {
        Task<IEnumerable<SelectListItem>> GetComboCategoriesAsync();

        Task<IEnumerable<SelectListItem>> GetComboCountriesAsync();

        Task<IEnumerable<SelectListItem>> GetComboStatesAsync(int countryId);

        Task<IEnumerable<SelectListItem>> GetComboCitiesAsync(int stateId);

    }
}
