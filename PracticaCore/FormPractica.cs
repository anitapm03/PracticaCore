using PracticaCore.Models;
using PracticaCore.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
#region procedimientos
/*
 para seleccionar los clientes
CREATE PROCEDURE SP_CLIENTES 
AS
	SELECT * FROM clientes
GO

para seleccionar los pedidos de un cliente
 CREATE PROCEDURE SP_PEDIDOS_CLIENTE
(@CODCLI NVARCHAR(10))
AS
	SELECT * FROM pedidos WHERE CodigoCliente = @CODCLI
GO
 
 para seleccionar un pedido por su codigo
CREATE PROCEDURE SP_PEDIDO
(@CODPED NVARCHAR(50))
AS
	SELECT * FROM pedidos WHERE CodigoPedido = @CODPED
GO

insertar un pedido
CREATE PROCEDURE SP_INSERTAR_PEDIDO
(@EMPRESA NVARCHAR(30),
@CODPED NVARCHAR(30),
@FECHA NVARCHAR(30),
@FORMA NVARCHAR(30),
@IMPORTE INT
)
AS
	DECLARE @CODLI NVARCHAR(30)
	SELECT @CODLI = CodigoCliente FROM clientes
	WHERE Empresa = @EMPRESA

	INSERT INTO pedidos VALUES (@CODPED, @CODLI,
	@FECHA, @FORMA, @IMPORTE)
	PRINT 'INSERTADO CORRECTAMENTE'
GO
 */
#endregion
namespace PracticaCore
{
    public partial class FormPractica : Form
    {
        RepositoryClientes repo;
        public FormPractica()
        {
            InitializeComponent();

            this.repo = new RepositoryClientes();
            this.loadClientes();
        }

        private void loadClientes()
        {
            List<string> clientes = this.repo.GetClientes();
            foreach (string cliente in clientes)
            {
                this.cmbclientes.Items.Add(cliente);
            }
        }

        private void cmbclientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            string empresaCliente = this.cmbclientes.SelectedItem.ToString();

            Cliente cliente = this.repo.GetCliente(empresaCliente);

            this.txtempresa.Text = cliente.Empresa;
            this.txtcontacto.Text = cliente.Contacto;
            this.txtcargo.Text = cliente.Cargo;
            this.txtciudad.Text = cliente.Ciudad;
            this.txttelefono.Text = cliente.Telefono;
            string codigoCliente = cliente.CodigoCliente;

            this.loadPedidosCliente(codigoCliente);
        }

        private void loadPedidosCliente(string codigoCliente)
        {
            List<string> pedidos = this.repo.GetPedidosCliente(codigoCliente);
            this.lstpedidos.Items.Clear();
            foreach (string pedido in pedidos)
            {
                this.lstpedidos.Items.Add(pedido);
            }
        }

        private void lstpedidos_SelectedIndexChanged(object sender, EventArgs e)
        {
            string codigoPedido = this.lstpedidos.SelectedItem.ToString();

            Pedido pedido = this.repo.GetInfoPedido(codigoPedido);

            this.txtcodigopedido.Text = pedido.CodigoPedido;
            this.txtfechaentrega.Text = pedido.FechaEntrega;
            this.txtformaenvio.Text = pedido.FormaEnvio;
            this.txtimporte.Text = pedido.Importe.ToString();

        }

        private void btnnuevopedido_Click(object sender, EventArgs e)
        {
            string empresaCliente = this.cmbclientes.SelectedItem.ToString();
            string codigoPedido = this.txtcodigopedido.Text;
            string fecha = this.txtfechaentrega.Text;
            string forma = this.txtformaenvio.Text;
            int importe = int.Parse(this.txtimporte.Text);

            int insertado = this.repo.InsertarPedido(empresaCliente, codigoPedido,
                fecha, forma, importe);

            MessageBox.Show("Insertados: " + insertado);
            
            this.cmbclientes_SelectedIndexChanged(sender, e);
            this.txtcodigopedido.Clear();
            this.txtfechaentrega.Clear();
            this.txtformaenvio.Clear();
            this.txtimporte.Clear();
        }

        private void btneliminarpedido_Click(object sender, EventArgs e)
        {
            string codigoPedido = this.lstpedidos.SelectedItem.ToString();

            int eliminado = this.repo.EliminarPedido(codigoPedido);

            MessageBox.Show("Eliminados: " + eliminado);
            this.cmbclientes_SelectedIndexChanged(sender, e);
        }
    }
}
