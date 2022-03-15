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

        public async Task<IActionResult> Edit(string rowkey, string partitionkey)
        {
            Cliente cliente =
            await this.service.FindClienteAsync(rowkey, partitionkey);
            return View(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Cliente cliente)
        {
            await this.service.UpdateClienteAsync(cliente.RowKey
            , cliente.PartitionKey, cliente.Nombre, cliente.Edad, cliente.Salario);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string rowkey, string partitionkey)
        {
            await this.service.DeleteClienteAsync(rowkey, partitionkey);
            return RedirectToAction("Index");
        }

        public IActionResult ClientesEmpresa()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ClientesEmpresa(string empresa)
        {
            List<Cliente> clientes =
            await this.service.GetClientesEmpresaAsync(empresa);
            return View(clientes);
        }

        public async Task<IActionResult> Details(string rowkey, string partitionkey)
        {
            Cliente cliente =
            await this.service.FindClienteAsync(rowkey, partitionkey);
            return View(cliente);
        }
    }
}
