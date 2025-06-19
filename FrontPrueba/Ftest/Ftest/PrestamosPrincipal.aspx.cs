using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WA_Prueba.EjemplarWS;
using WA_Prueba.MaterialWS;
using WA_Prueba.PersonaWS;
using WA_Prueba.PrestamoEjemplarWS;
using WA_Prueba.PrestamoWS;

namespace WA_Prueba
{
    public partial class PrestamosPrincipal : System.Web.UI.Page
    {
        private PrestamoWSClient prestamoWSClient;
        private PersonaWSClient personaWSClient;
        private PrestamoEjemplarWSClient prestamoEjemplarWSClient;
        private EjemplarWSClient ejemplarWSClient;
        private MaterialWSClient materialWSClient;

        protected void Page_Init(object sender, EventArgs e)
        {
            // Inicializa el cliente del WebService
            prestamoWSClient = new PrestamoWSClient();
            personaWSClient = new PersonaWSClient();
            prestamoEjemplarWSClient = new PrestamoEjemplarWSClient();
            ejemplarWSClient = new EjemplarWSClient();
            materialWSClient = new MaterialWSClient();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            CargarPrestamos();
            CargarPrestamosSolicitados();
            CargarPrestamosAtrasados();
            CargarPrestamosDevueltos();
            CargarPrestamosNoCulminados();

        }
        // Función para cargar los préstamos utilizando el WebService
        private void CargarPrestamos()
        {
            StringBuilder htmlTable = new StringBuilder();

            try
            {

                WA_Prueba.PrestamoWS.prestamosDTO[] prestamosArray = prestamoWSClient.listarPrestamos();
                List<WA_Prueba.PrestamoWS.prestamosDTO> prestamos = prestamosArray.ToList();
                // Verificar si se obtuvieron resultados
                if (prestamos != null && prestamos.Count > 0)
                {
                    // Llenamos la tabla HTML con los datos obtenidos
                    foreach (WA_Prueba.PrestamoWS.prestamosDTO prestamo in prestamos)
                    {
                        htmlTable.Append("<tr>");
                        htmlTable.Append("<td>" + prestamo.idPrestamo + "</td>");
                        htmlTable.Append("<td>" + prestamo.fechaSolicitud.ToString("yyyy-MM-dd") + "</td>");
                        htmlTable.Append("<td>" + (prestamo.fechaPrestamo != DateTime.MinValue ? prestamo.fechaPrestamo.ToString("yyyy-MM-dd") : "No disponible") + "</td>");
                        htmlTable.Append("<td>" + (prestamo.fechaDevolucion != DateTime.MinValue ? prestamo.fechaDevolucion.ToString("yyyy-MM-dd") : "No disponible") + "</td>");
                        PersonaWS.personasDTO personaAux = personaWSClient.obtenerPersona(prestamo.persona.idPersona);
                        String textoUsuario = personaAux.codigo + " - " + personaAux.nombre + " " + personaAux.paterno;
                        htmlTable.Append("<td>" + textoUsuario + "</td>"); // Suponiendo que 'PrestamoDTO' tiene una propiedad 'PERSONA_NOMBRE'
                        htmlTable.Append("</tr>");
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                htmlTable.Append("<tr><td colspan='5'>Error al obtener los datos: " + ex.Message + "</td></tr>");
            }

            // Asignamos la tabla generada al campo "prestamosTable" en la página .aspx
            prestamosTable = htmlTable.ToString();
        }

        private void CargarPrestamosSolicitados()
        {
            StringBuilder htmlTable = new StringBuilder();

            try
            {
                // Obtenemos los préstamos solicitados desde el WebService
                WA_Prueba.PrestamoWS.prestamosDTO[] prestamosArray = prestamoWSClient.listarPrestamosSolicitados();
                List<WA_Prueba.PrestamoWS.prestamosDTO> prestamos = new List<WA_Prueba.PrestamoWS.prestamosDTO>(prestamosArray);

                if (prestamos != null && prestamos.Count > 0)
                {
                    foreach (WA_Prueba.PrestamoWS.prestamosDTO prestamo in prestamos)
                    {
                        if ((DateTime.Now - prestamo.fechaSolicitud).Days > 2)
                        {
                            continue;
                        }
                        htmlTable.Append("<tr>");
                        htmlTable.Append("<td>" + prestamo.idPrestamo + "</td>");
                        htmlTable.Append("<td>" + prestamo.fechaSolicitud.ToString("yyyy-MM-dd") + "</td>");
                        htmlTable.Append("<td>" + (prestamo.fechaPrestamo != DateTime.MinValue ? prestamo.fechaPrestamo.ToString("yyyy-MM-dd") : "No disponible") + "</td>");
                        htmlTable.Append("<td>" + (prestamo.fechaDevolucion != DateTime.MinValue ? prestamo.fechaDevolucion.ToString("yyyy-MM-dd") : "No disponible") + "</td>");
                        PersonaWS.personasDTO personaAux = personaWSClient.obtenerPersona(prestamo.persona.idPersona);
                        String textoUsuario = personaAux.codigo + " - " + personaAux.nombre + " " + personaAux.paterno;
                        htmlTable.Append("<td>" + textoUsuario + "</td>");
                        htmlTable.Append("</tr>");
                    }
                }
            }
            catch (Exception ex)
            {
                htmlTable.Append("<tr><td colspan='5'>Error al obtener los datos: " + ex.Message + "</td></tr>");
            }

            prestamosTableSolicitados = htmlTable.ToString();
        }

        // Método para cargar los préstamos atrasados
        private void CargarPrestamosAtrasados()
        {
            StringBuilder htmlTable = new StringBuilder();

            try
            {
                WA_Prueba.PrestamoWS.prestamosDTO[] prestamosArray = prestamoWSClient.listarPrestamosAtrasados();
                List<WA_Prueba.PrestamoWS.prestamosDTO> prestamos = new List<WA_Prueba.PrestamoWS.prestamosDTO>(prestamosArray);

                if (prestamos != null && prestamos.Count > 0)
                {
                    foreach (WA_Prueba.PrestamoWS.prestamosDTO prestamo in prestamos)
                    {
                        htmlTable.Append("<tr>");
                        htmlTable.Append("<td>" + prestamo.idPrestamo + "</td>");
                        htmlTable.Append("<td>" + prestamo.fechaSolicitud.ToString("yyyy-MM-dd") + "</td>");
                        htmlTable.Append("<td>" + (prestamo.fechaPrestamo != DateTime.MinValue ? prestamo.fechaPrestamo.ToString("yyyy-MM-dd") : "No disponible") + "</td>");
                        htmlTable.Append("<td>" + (prestamo.fechaDevolucion != DateTime.MinValue ? prestamo.fechaDevolucion.ToString("yyyy-MM-dd") : "No disponible") + "</td>");
                        PersonaWS.personasDTO personaAux = personaWSClient.obtenerPersona(prestamo.persona.idPersona);
                        String textoUsuario = personaAux.codigo + " - " + personaAux.nombre + " " + personaAux.paterno;
                        htmlTable.Append("<td>" + textoUsuario + "</td>");
                        htmlTable.Append("</tr>");
                    }
                }
            }
            catch (Exception ex)
            {
                htmlTable.Append("<tr><td colspan='5'>Error al obtener los datos: " + ex.Message + "</td></tr>");
            }

            prestamosTableAtrasados = htmlTable.ToString();
        }

        private void CargarPrestamosDevueltos()
        {
            StringBuilder htmlTable = new StringBuilder();

            try
            {
                WA_Prueba.PrestamoWS.prestamosDTO[] prestamosArray = prestamoWSClient.listarPrestamosDevueltos();
                List<WA_Prueba.PrestamoWS.prestamosDTO> prestamos = new List<WA_Prueba.PrestamoWS.prestamosDTO>(prestamosArray);

                if (prestamos != null && prestamos.Count > 0)
                {
                    foreach (WA_Prueba.PrestamoWS.prestamosDTO prestamo in prestamos)
                    {
                        htmlTable.Append("<tr>");
                        htmlTable.Append("<td>" + prestamo.idPrestamo + "</td>");
                        htmlTable.Append("<td>" + prestamo.fechaSolicitud.ToString("yyyy-MM-dd") + "</td>");
                        htmlTable.Append("<td>" + (prestamo.fechaPrestamo != DateTime.MinValue ? prestamo.fechaPrestamo.ToString("yyyy-MM-dd") : "No disponible") + "</td>");
                        htmlTable.Append("<td>" + (prestamo.fechaDevolucion != DateTime.MinValue ? prestamo.fechaDevolucion.ToString("yyyy-MM-dd") : "No disponible") + "</td>");
                        PersonaWS.personasDTO personaAux = personaWSClient.obtenerPersona(prestamo.persona.idPersona);
                        String textoUsuario = personaAux.codigo + " - " + personaAux.nombre + " " + personaAux.paterno;
                        htmlTable.Append("<td>" + textoUsuario + "</td>");
                        htmlTable.Append("</tr>");
                    }
                }
            }
            catch (Exception ex)
            {
                htmlTable.Append("<tr><td colspan='5'>Error al obtener los datos: " + ex.Message + "</td></tr>");
            }

            prestamosTableDevueltos = htmlTable.ToString();
        }

        private void CargarPrestamosNoCulminados()
        {
            StringBuilder htmlTable = new StringBuilder();

            try
            {
                // Obtenemos los préstamos no culminados desde el WebService
                WA_Prueba.PrestamoWS.prestamosDTO[] prestamosArray = prestamoWSClient.listarPrestamosNoCulminados();
                List<WA_Prueba.PrestamoWS.prestamosDTO> prestamos = new List<WA_Prueba.PrestamoWS.prestamosDTO>(prestamosArray);

                if (prestamos != null && prestamos.Count > 0)
                {
                    foreach (WA_Prueba.PrestamoWS.prestamosDTO prestamo in prestamos)
                    {
                        if (prestamo.fechaDevolucion < DateTime.Now && prestamo.fechaDevolucion != DateTime.MinValue)
                        {
                            var ejemplaresPrestamo = prestamoEjemplarWSClient.listarTodosPrestamoEjemplar();
                            var ejemplaresFiltrados = ejemplaresPrestamo.Where(x => x.idPrestamo == prestamo.idPrestamo).ToList();

                            foreach (var ejemplar in ejemplaresFiltrados)
                            {
                                ejemplar.estado = estadoPrestamoEjemplar.ATRASADO;
                                prestamoEjemplarWSClient.modificarPrestamoEjemplar(ejemplar);
                            }
                        }

                        htmlTable.Append("<tr>");
                        htmlTable.Append("<td>" + prestamo.idPrestamo + "</td>");
                        htmlTable.Append("<td>" + prestamo.fechaSolicitud.ToString("yyyy-MM-dd") + "</td>");
                        htmlTable.Append("<td>" + (prestamo.fechaPrestamo != DateTime.MinValue ? prestamo.fechaPrestamo.ToString("yyyy-MM-dd") : "No disponible") + "</td>");
                        htmlTable.Append("<td>" + (prestamo.fechaDevolucion != DateTime.MinValue ? prestamo.fechaDevolucion.ToString("yyyy-MM-dd") : "No disponible") + "</td>");
                        PersonaWS.personasDTO personaAux = personaWSClient.obtenerPersona(prestamo.persona.idPersona);
                        String textoUsuario = personaAux.codigo + " - " + personaAux.nombre + " " + personaAux.paterno;
                        htmlTable.Append("<td>" + textoUsuario + "</td>");
                        htmlTable.Append("</tr>");
                    }
                }
            }
            catch (Exception ex)
            {
                htmlTable.Append("<tr><td colspan='5'>Error al obtener los datos: " + ex.Message + "</td></tr>");
            }

            prestamosTableNoCulminados = htmlTable.ToString();
        }

        // Métodos similares para los otros tres casos: Prestamos Devueltos y Prestamos No Culminados

        public string prestamosTableSolicitados { get; set; }
        public string prestamosTableAtrasados { get; set; }
        public string prestamosTableDevueltos { get; set; }
        public string prestamosTableNoCulminados { get; set; }

        // Variable para almacenar la tabla HTML generada
        public string prestamosTable { get; set; }
    }
}