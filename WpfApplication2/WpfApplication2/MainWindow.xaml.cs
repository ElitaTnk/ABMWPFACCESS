using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data.OleDb;
using System.Configuration;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int _errors = 0;
        private Boolean addingNew;
        private Boolean editDone;
        private Persona identidad = new Persona();

        public MainWindow()
        {
            InitializeComponent();
            loadGrid();
            disableForm();
            GridFields.DataContext = identidad;

            addingNew = false;
            editDone = false;
            Button_save.Visibility = Visibility.Hidden;

        }

        private void disableForm()
        {
            if (formABM.Visibility != Visibility.Hidden)
            {
                formABM.Visibility = Visibility.Hidden;
            }
        }

        
        private void loadGrid()
        {
            OleDbConnection conexion = new OleDbConnection();
            conexion.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();

            OleDbCommand cmd = new OleDbCommand();

            if (conexion.State != ConnectionState.Open)
            {
                conexion.Open();
            }

            cmd.Connection = conexion;

            cmd.CommandText = "select * from [personas]";
            

            OleDbDataAdapter dataAdapterGridABM = new OleDbDataAdapter(cmd);
            DataTable datatableGridABM = new DataTable();
            dataAdapterGridABM.Fill(datatableGridABM);

            dataGridABM.ItemsSource = datatableGridABM.AsDataView();
            
        }

        private void Button_add_Click(object sender, RoutedEventArgs e)
        {
            
            formABM.Visibility = Visibility.Visible;
            Button_save.Visibility = Visibility.Visible;
            Button_edit.Visibility = Visibility.Hidden;
            Button_remove.Visibility = Visibility.Hidden;

            addingNew = true;
            
        }

        private void resetAll()
        {
            PersonaDNI.Text = "";
            PersonaNombre.Text = "";
            PersonaApellido.Text = "";
            PersonaGenero.SelectedIndex = 0;
            PersonaTelefono.Text = "";
            PersonaDireccion.Text = "";
            PersonaFechaNac.Text = "";
            PersonaEmail.Text = "";
        }

        private void Button_close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Button_cancel_Click(object sender, RoutedEventArgs e)
        {
            resetAll();
            disableForm();
            loadGrid();
            if (Button_save.Visibility == Visibility.Visible) {
                Button_save.Visibility = Visibility.Hidden;
                Button_add.Visibility = Visibility.Visible;
                Button_remove.Visibility = Visibility.Visible;
                Button_edit.Visibility = Visibility.Visible;
            }

            addingNew = false;
            editDone = false;
            MessageBox.Show("Accion cancelada");
        }

        private void Button_save_Click(object sender, RoutedEventArgs e)
        {
            
            if (addingNew)
            {
                OleDbConnection conexion = new OleDbConnection();
                conexion.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();

                OleDbCommand cmd = new OleDbCommand();

                if (conexion.State != ConnectionState.Open)
                {
                    conexion.Open();
                }
                cmd.Connection = conexion;

                if (_errors <= 0)
                {
                    cmd.CommandText = "insert into [personas](DNI,Nombre,Apellido,Telefono,FechaNacimiento,Direccion,Genero,Email) Values ('" + PersonaDNI.Text + "','" + PersonaNombre.Text + "','" + PersonaApellido.Text + "','" + PersonaTelefono.Text + "','" + PersonaFechaNac.Text + "','" + PersonaDireccion.Text + "','" + PersonaGenero.Text + "','" + PersonaEmail.Text  + "')";
                    int success = cmd.ExecuteNonQuery();

                    if (success > 0)
                    {
                        MessageBox.Show("Registro salvado");
                    }

                    loadGrid();

                    resetAll();

                    addingNew = false;

                    disableForm();
                    Button_save.Visibility = Visibility.Hidden;
                    Button_remove.Visibility = Visibility.Visible;
                    Button_edit.Visibility = Visibility.Visible;

                }
                else
                {
                    MessageBox.Show("Ingrese los datos obligatorios de la persona");
                }
            }

            if(editDone)
            {
                OleDbConnection conexion = new OleDbConnection();
                conexion.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();

                OleDbCommand cmd = new OleDbCommand();

                if (conexion.State != ConnectionState.Open)
                {
                    conexion.Open();
                }
                cmd.Connection = conexion;

                DataRowView row = (DataRowView)dataGridABM.SelectedItems[0];

                if (_errors <= 0)
                {

                    cmd.CommandText = "update [personas] set DNI='" + PersonaDNI.Text + "',Nombre='" + PersonaNombre.Text + "',Apellido='" + PersonaApellido.Text + "',Telefono='" + PersonaTelefono.Text + "',FechaNacimiento='" + PersonaFechaNac.Text + "',Direccion='" + PersonaDireccion.Text + "',Genero='" + PersonaGenero.Text + "',Email='" + PersonaEmail.Text + "' where DNI='" + PersonaDNI.Text +"'" ;
                    int success = cmd.ExecuteNonQuery();

                    if (success > 0)
                    {
                        MessageBox.Show("Registro salvado");
                    }
                    editDone = false;

                    resetAll();

                    disableForm();
                    Button_save.Visibility = Visibility.Hidden;

                    loadGrid();

                    Button_add.Visibility = Visibility.Visible;
                    Button_remove.Visibility = Visibility.Visible;
                            
                }
                else
                {
                    MessageBox.Show("Ingrese los datos obligatorios de la persona");
                }

            }

        }

        private void Button_remove_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridABM.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)dataGridABM.SelectedItems[0];
                
                OleDbConnection conexion = new OleDbConnection();
                conexion.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();

                OleDbCommand cmd = new OleDbCommand();

                if (conexion.State != ConnectionState.Open)
                {
                    conexion.Open();
                }

                cmd.Connection = conexion;
                cmd.CommandText = "delete from [personas] where DNI='" + row["DNI"].ToString() +"'";
                int success = cmd.ExecuteNonQuery();

                if (success > 0)
                {
                    MessageBox.Show("Registro borrado");
                }
                resetAll();
                loadGrid();

                
            }
            else
            {
                MessageBox.Show("Seleccione una persona de la grilla y luego apriete Borrar Registro nuevamente");
            }
        }

        private void Button_edit_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridABM.SelectedItems.Count > 0)
            {
                editDone = true;
                Button_save.Visibility = Visibility.Visible;
                formABM.Visibility = Visibility.Visible;

                Button_add.Visibility = Visibility.Hidden;
                Button_remove.Visibility = Visibility.Hidden;

                DataRowView row = (DataRowView)dataGridABM.SelectedItems[0];
                PersonaDNI.Text = row["DNI"].ToString();
                PersonaNombre.Text = row["Nombre"].ToString();
                PersonaApellido.Text = row["Apellido"].ToString();
                PersonaFechaNac.Text = row["FechaNacimiento"].ToString();
                PersonaGenero.Text = row["Genero"].ToString();
                PersonaTelefono.Text = row["Telefono"].ToString();
                PersonaEmail.Text = row["Email"].ToString();
                PersonaDireccion.Text = row["Direccion"].ToString();
                
            }
            else
            {
                MessageBox.Show("Seleccione una persona de la grilla y luego apriete Editar Registro nuevamente");
            }
        }

        private void validationCheck(object sender, ValidationErrorEventArgs e)
        {

            if (e.Action == ValidationErrorEventAction.Added)
                _errors++;
            else
                _errors--;
        }

        private void saveExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Persona indentiad = GridFields.DataContext as Persona;
            identidad = new Persona();
            GridFields.DataContext = identidad;
            e.Handled = true;
        }

        private void saveCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _errors == 0;
            e.Handled = true;
        }
    }
}
