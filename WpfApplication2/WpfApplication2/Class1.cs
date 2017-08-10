using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WpfApplication2
{
    public class Persona : IDataErrorInfo
    {

        bool invalid_email = false;

        public string DNI { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public string Telefono { get; set; }

        public string FechaNacimiento { get; set; }

        public string Email { get; set; }

        public string Genero { get; set; }

        public string Direccion { get; set; }


        public string Error
        {
            get { return String.Empty; }
        }

        public string this[string columnName]
        {
            get
            {
                string result = null;

                if (columnName == "DNI")
                {
                    
                    if (string.IsNullOrEmpty(DNI))
                    {
                        result = "Por favor ingrese un DNI valido";
                    }
                    else
                    {
                        Regex regex = new Regex(@"^[0-9.]+$");
                        if (regex.IsMatch(DNI) == false)
                        {
                            result = "Utilice solo numeros y '.' ";
                        }
                    }
                }

                if (columnName == "Nombre")
                {
                    if (string.IsNullOrEmpty(Nombre))
                        result = "Por favor ingrese un Nombre";
                    else
                    {
                        Regex regex = new Regex(@"^[a-zA-Z ]*$");
                        if (regex.IsMatch(Nombre) == false)
                        {
                            result = "Nombre solo puede contenet letras";
                        }
                    }
                }

                if (columnName == "Apellido")
                {
                    if (string.IsNullOrEmpty(Apellido))
                        result = "Por favor ingrese el Apellido";
                    else
                    {
                        Regex regex = new Regex(@"^[a-zA-Z ]*$");
                        if (regex.IsMatch(Apellido) == false)
                        {
                            result = "Apellido solo puede contener letras";
                        }
                    }
                }

                if (columnName == "Telefono")
                {

                    if (string.IsNullOrEmpty(Telefono))
                    {
                        result = "Por favor ingrese un telefono valido";
                    }
                    else
                    {
                        Regex regex = new Regex(@"^[0-9]+$");
                        if (regex.IsMatch(Telefono) == false)
                        {
                            result = "Utilice solo numeros ";
                        }
                    }
                }

                if (columnName == "Email")
                {
                    if (string.IsNullOrEmpty(Email))
                        result = "Por favor ingrese el Email";
                    else
                    {
                        
                        if (checkIfValidEmail(Email) == false)
                            result = "Por favor ingrese un Email valido";
                    }
                }

                if (columnName == "FechaNacimiento")
                {
                    if (string.IsNullOrEmpty(FechaNacimiento))
                        result = "Por favor ingrese la Fecha de Nacimiento";
                }

                if (columnName == "Direccion")
                {
                    if (string.IsNullOrEmpty(Direccion))
                        result = "Por favor ingrese una Direccion";
                    else
                    {
                        Regex regex = new Regex(@"^[a-zA-Z][\w ]*$");
                        if (regex.IsMatch(Direccion) == false)
                        {
                            result = "Debe ingresar solo letras y nuneros";
                        }
                    }
                }

                return result;
            }
        }

        //para email verification
        private string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                invalid_email = true;
            }
            return match.Groups[1].Value + domainName;
        }

        public bool checkIfValidEmail(string checkingEmail)
        {
            invalid_email = false;

            if (String.IsNullOrEmpty(checkingEmail))
                return false;

            // Use IdnMapping class to convert Unicode domain names.
            try
            {
                checkingEmail = Regex.Replace(checkingEmail, @"(@)(.+)$", this.DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(100));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

            if (invalid_email)
                return false;

            // Return true if valid e-mail format.
            try
            {
                return Regex.IsMatch(checkingEmail,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(150));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        
    }
}
