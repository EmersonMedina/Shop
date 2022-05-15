using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Data.Entities;
using Shop.Enums;
using Shop.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrdersController : Controller
    {
        private readonly DataContext _context;
        private readonly IOrdersHelper _ordersHelper;
        private readonly INotyfService _notifyService;

        public OrdersController(DataContext context, IOrdersHelper ordersHelper, INotyfService notifyService)
        {
            _context = context;
            _ordersHelper = ordersHelper;
            _notifyService = notifyService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sales
                .Include(s => s.User)
                .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Product)
                .ToListAsync());
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Sale sale = await _context.Sales
                .Include(s => s.User)
                .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Product)
                .ThenInclude(p => p.ProductImages)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (sale == null)
            {
                return NotFound();
            }

            return View(sale);
        }
        public async Task<IActionResult> Dispatch(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Sale sale = await _context.Sales.FindAsync(id);
            if (sale == null)
            {
                return NotFound();
            }

            if (sale.OrderStatus != OrderStatus.Nuevo)
            {
                _notifyService.Error("Solo se pueden despachar pedidos que estén es estado NUEVO."); 
            }
            else
            {
                sale.OrderStatus = OrderStatus.Despachado;
                _context.Sales.Update(sale);
                await _context.SaveChangesAsync();
                _notifyService.Success("El estado del pedido ha sido cambiado a DESPACHADO.");
            }
             
            return RedirectToAction(nameof(Details), new { Id = sale.Id });
        }
        public async Task<IActionResult> Send(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Sale sale = await _context.Sales.FindAsync(id);
            if (sale == null)
            {
                return NotFound();
            }

            if (sale.OrderStatus != OrderStatus.Despachado)
            {
                _notifyService.Warning("Solo se pueden enviar pedidos que estén en estado DESPACHADO.");
            }
            else
            {
                sale.OrderStatus = OrderStatus.Enviado;
                _context.Sales.Update(sale);
                await _context.SaveChangesAsync();
                _notifyService.Success("El estado del pedido ha sido cambiado a ENVIADO.");
            }

            return RedirectToAction(nameof(Details), new { Id = sale.Id });
        }
        public async Task<IActionResult> Confirm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Sale sale = await _context.Sales.FindAsync(id);
            if (sale == null)
            {
                return NotFound();
            }

            if (sale.OrderStatus != OrderStatus.Enviado)
            {
                _notifyService.Warning("Solo se pueden confirmar pedidos que estén en estado ENVIADO.");
            }
            else
            {
                sale.OrderStatus = OrderStatus.Confirmado;
                _context.Sales.Update(sale);
                await _context.SaveChangesAsync();
                _notifyService.Success("El estado del pedido ha sido cambiado a CONFIRMADO.");
            }

            return RedirectToAction(nameof(Details), new { Id = sale.Id });
        }

        public async Task<IActionResult> Cancel(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Sale sale = await _context.Sales.FindAsync(id);
            if (sale == null)
            {
                return NotFound();
            }

            if (sale.OrderStatus == OrderStatus.Cancelado)
            {
                _notifyService.Error("No se puede cancelar un pedido que esté en estado CANCELADO.");
            }
            else
            {
                await _ordersHelper.CancelOrderAsync(sale.Id);
                _notifyService.Success("El estado del pedido ha sido cambiado a CANCELADO.");
            }

            return RedirectToAction(nameof(Details), new { Id = sale.Id });
        }

    }


}
