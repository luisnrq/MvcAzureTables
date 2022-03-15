using Microsoft.AspNetCore.Mvc;
using MvcAzureTables.Models;
using MvcAzureTables.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcAzureTables.Controllers
{
    public class AzureTablesController : Controller
    {
        private ServiceTableStorage service;

        public AzureTablesController(ServiceTableStorage service)
        {
            this.service = service;
        }

        public async Task<IActionResult> Index()
        {
            List<Cliente> clientes = await this.service.GetClientesAsync();
            return View(clientes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            await this.service.CreateClienteAsync(cliente.IdCliente, cliente.Empresa, cliente.Nombre, cliente.Edad, cliente.Salario);
            return RedirectToAction("Index");
        }
    }
}
