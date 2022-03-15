using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.Cosmos.Tables;
using MvcAzureTables.Models;

namespace MvcAzureTables.Services
{
    public class ServiceTableStorage
    {
        private CloudTable tablaClientes;

        public ServiceTableStorage(string azureKeys)
        {
            //Para acceder a las tablas y los servicios necesitamos una clase de azurestorage
            CloudStorageAccount account = CloudStorageAccount.Parse(azureKeys);

            CloudTableClient client = account.CreateCloudTableClient();

            //El nombre de la tabla se pone en minusculas
            this.tablaClientes = client.GetTableReference("clientes");

            //Sin usar asincrono: this.tablaClientes.CreateIfNotExists();

            Task.Run(async () => 
            {
                await this.tablaClientes.CreateIfNotExistsAsync();
            });
        }

        public async Task CreateClienteAsync(string id, string empresa, string nombre, int edad, int salario)
        {
            Cliente cliente = new Cliente 
            {
                IdCliente = id,
                Empresa = empresa,
                Nombre = nombre,
                Edad = edad,
                Salario = salario
            };

            //Las consultas de accion se realizan mediante objetos de tipo TableOperation
            //Posteriormente, se ejecutan esas consultas sobre la propia table
            TableOperation insert = TableOperation.Insert(cliente);
            await this.tablaClientes.ExecuteAsync(insert);
        }

        public async Task<List<Cliente>> GetClientesAsync()
        {
            //Para realizar busquedas sobre una tabla entity debemos utilizar la clase TableQuery<T>
            TableQuery<Cliente> query = new TableQuery<Cliente>();
            //Las consultas de seleccion se realizan mediante segmentos, que internamente va construyendo la consulta de objetos json a clases
            TableQuerySegment<Cliente> segment = await this.tablaClientes.ExecuteQuerySegmentedAsync(query, null);
            List<Cliente> clientes = segment.Results;
            return clientes;
        }

        public async Task<List<Cliente>> GetClientesEmpresaAsync(string empresa)
        {
            TableQuery<Cliente> query = new TableQuery<Cliente>();
            TableQuerySegment<Cliente> segment = await this.tablaClientes.ExecuteQuerySegmentedAsync(query, null);
            //Filtro re serealiza sobre el propio segment 
            List<Cliente> clientes = segment.Where(x => x.Empresa == empresa).ToList();
            return clientes;
        }

        //Para buscar objetos unicos dentro de TableEntity podemos hacerlo como el metodo anterior o podemos hacerlo por su rowkey
        //No existe un metodo de busqueda solamente por rowkey nos pide también partitionkey
        public async Task<Cliente> FindClienteAsync(string rowkey, string partitionkey)
        {
            TableOperation select = TableOperation.Retrieve<Cliente>(partitionkey, rowkey);
            TableResult result = await this.tablaClientes.ExecuteAsync(select);
            Cliente cliente = result.Result as Cliente;
            return cliente;
        }

        public async Task DeleteClienteAsync(string rowkey, string partitionkey)
        {
            Cliente cliente = await this.FindClienteAsync(rowkey, partitionkey);
            TableOperation delete = TableOperation.Delete(cliente);
            await this.tablaClientes.ExecuteAsync(delete);
        }

        public async Task UpdateClienteAsync(string rowkey, string partitionkey, string nombre, int edad, int salario)
        {
            Cliente cliente = await this.FindClienteAsync(rowkey, partitionkey);
            cliente.Nombre = nombre;
            cliente.Edad = edad;
            cliente.Salario = salario;
            TableOperation update = TableOperation.Replace(cliente);
            await this.tablaClientes.ExecuteAsync(update);
        }
    }
}
