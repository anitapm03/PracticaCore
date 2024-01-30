using PracticaCore.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticaCore.Repository
{
    public class RepositoryClientes
    {
        SqlConnection cn;
        SqlCommand com;
        SqlDataReader reader;
        //List<Cliente> listaClientes;

        public RepositoryClientes()
        {
            string connectionString = "Data Source=LOCALHOST\\SQLEXPRESS;Initial Catalog=NETCORE;Persist Security Info=True;User ID=SA;Password=MCSD2023";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
            //this.listaClientes = new List<Cliente>();
        }

        public List<string> GetClientes()
        {
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_CLIENTES";

            this.cn.Open();
            this.reader = this.com.ExecuteReader();
            List<string> clientes = new List<string>();
            while (this.reader.Read())
            {
                clientes.Add(this.reader["EMPRESA"].ToString());
            }
            this.reader.Close();
            this.cn.Close();

            return clientes;
        }

        public Cliente GetCliente(string empresaCliente)
        {
            string sql = "SELECT * FROM clientes WHERE Empresa =  @empresa";
            SqlParameter empresaparam = new SqlParameter("@empresa", empresaCliente);
            this.com.Parameters.Add(empresaparam);

            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;

            this.cn.Open();
            this.reader = this.com.ExecuteReader();
            
            this.reader.Read();
            
                string codigo = this.reader["CodigoCliente"].ToString();
                string empresa = this.reader["Empresa"].ToString();
                string contacto = this.reader["Contacto"].ToString();
                string cargo = this.reader["Cargo"].ToString();
                string ciudad = this.reader["Ciudad"].ToString();
                string telf = this.reader["Telefono"].ToString();

                Cliente cliente = new Cliente();
                cliente.CodigoCliente = codigo;
                cliente.Empresa = empresa;
                cliente.Contacto = contacto;
                cliente.Cargo = cargo;
                cliente.Ciudad = ciudad;
                cliente.Telefono = telf;
            
            this.reader.Close();
            this.cn.Close();
            this.com.Parameters.Clear();

            return cliente;
        }

        public List<string> GetPedidosCliente(string codCliente)
        {
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_PEDIDOS_CLIENTE";

            SqlParameter parCod = new SqlParameter("@CODCLI", codCliente);
            this.com.Parameters.Add(parCod);

            this.cn.Open();
            this.reader = this.com.ExecuteReader();
            List<string> pedidos = new List<string>();
            while (this.reader.Read())
            {
                pedidos.Add(this.reader["CodigoPedido"].ToString());
            }
            this.reader.Close();
            this.cn.Close();
            this.com.Parameters.Clear();
            return pedidos;
        }

        public Pedido GetInfoPedido(string codPedido)
        {
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_PEDIDO";

            SqlParameter parCod = new SqlParameter("@CODPED", codPedido);
            this.com.Parameters.Add(parCod);

            this.cn.Open();
            this.reader = this.com.ExecuteReader();

            this.reader.Read();
            string codigo = this.reader["CodigoPedido"].ToString();
            string cliente = this.reader["CodigoCliente"].ToString();
            string fecha = this.reader["FechaEntrega"].ToString();
            string forma = this.reader["FormaEnvio"].ToString();
            int importe = int.Parse(this.reader["Importe"].ToString());

            Pedido pedido = new Pedido();
            pedido.CodigoPedido = codigo;
            pedido.CodigoCliente = cliente;
            pedido.FechaEntrega = fecha;
            pedido.FormaEnvio = forma;
            pedido.Importe = importe;

            this.reader.Close();
            this.cn.Close();
            this.com.Parameters.Clear();
            return pedido;
        }

        public int InsertarPedido
            (string empresa, string codigopedido, string fecha, string forma, int importe)
        {
            
            SqlParameter parEmpresa = new SqlParameter("@EMPRESA", empresa);
            this.com.Parameters.Add(parEmpresa);
            SqlParameter parCodigo = new SqlParameter("@CODPED", codigopedido);
            this.com.Parameters.Add(parCodigo);
            SqlParameter parFecha = new SqlParameter("@FECHA", fecha);
            this.com.Parameters.Add(parFecha);
            SqlParameter parForma = new SqlParameter("@FORMA", forma);
            this.com.Parameters.Add(parForma);
            SqlParameter parImporte = new SqlParameter("@IMPORTE", importe);
            this.com.Parameters.Add(parImporte);

            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_INSERTAR_PEDIDO";

            this.cn.Open();
            int insertado = this.com.ExecuteNonQuery();

            this.cn.Close();
            this.com.Parameters.Clear();
            return insertado;
        }
    }
}
