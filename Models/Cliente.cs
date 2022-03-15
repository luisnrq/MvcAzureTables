using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcAzureTables.Models
{
    public class Cliente: TableEntity
    {
        //Toda table entity debe contener un rowkey
        //Necesitamos almacenar el rowkey cuando cambiemos la propiedad idcliente, necesitamos una propiedad extendida
        private string _IdCliente;
        public string IdCliente { 
            get { return this._IdCliente; }
            set {
                this._IdCliente = value;
                this.RowKey = value;
            } 
        }

        //Opcional, podria contener una categoria/grupo llamada partition key
        private string _Empresa;
        public string Empresa {
            get { return this._Empresa; }
            set {
                this._Empresa = value;
                this.PartitionKey = value;
            }
        }

        public string Nombre { get; set; }
        public int Edad { get; set; }
        public int Salario { get; set; }
    }
}
