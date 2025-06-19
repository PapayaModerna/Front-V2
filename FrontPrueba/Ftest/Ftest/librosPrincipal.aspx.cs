using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WA_Prueba.CreadorWS;
using WA_Prueba.EditorialWS;
using WA_Prueba.EjemplarWS;
using WA_Prueba.MaterialWS;
using WA_Prueba.SedeWS;
using WA_Prueba.TemaWS;

namespace WA_Prueba
{
    public partial class librosPrincipal : System.Web.UI.Page
    {
        private MaterialWSClient materialwsClient;
        private EjemplarWSClient ejemplarwsClient;
        private EditorialWSClient editorialwsClient;
        private SedeWSClient sedewsClient;
        protected void Page_Init(object sender, EventArgs e)
        {
            materialwsClient = new MaterialWSClient();
            ejemplarwsClient = new EjemplarWSClient();
            editorialwsClient = new EditorialWSClient();
            sedewsClient = new SedeWSClient();
        }

        private void LoadSedes()
        {
            var sedes = sedewsClient.listarSedes();


            ddlSedes.DataSource = sedes;
            ddlSedes.DataTextField = "nombre";
            ddlSedes.DataValueField = "idSede";
            ddlSedes.DataBind();
            ddlSedes.Items.Insert(0, new ListItem("Todas las sedes", "0"));

        }

        protected void ddlSedes_SelectedIndexChanged(object sender, EventArgs e)
        {
            int sedeId = Convert.ToInt32(ddlSedes.SelectedValue);
            int idMaterial = 0;

            if (!string.IsNullOrEmpty(hfMaterialId.Value))
            {
                idMaterial = Convert.ToInt32(hfMaterialId.Value);
            }
            if (detallesContainer.Style["display"] == "block")
            {
                UpdateStockAndEjemplares(sedeId, idMaterial);
            }

            if (sedeId > 0)
            {
                LoadMaterialesPorSede(sedeId);
            }
            else
            {
                LoadMateriales();
            }
        }

        private void UpdateStockAndEjemplares(int sedeId, int idMaterial)
        {

            try
            {
                string ejemplaresHTML = "";
                int stockFisico = 0, stockDigital = 0, ejemplaresLibres = 0, ejemplaresPrestamo = 0;

                // Si hay sede seleccionada
                if (sedeId > 0)
                {
                    var ejemplaresSede = ejemplarwsClient.listarEjemplares();
                    var ejemplaresMaterial = ejemplaresSede.Where(e => e.material.idMaterial == idMaterial && e.sede.idSede == sedeId).ToList();

                    stockFisico = ejemplaresMaterial.Count(e => e.tipo == WA_Prueba.EjemplarWS.tipoEjemplar.FISICO);
                    stockDigital = ejemplaresMaterial.Count(e => e.tipo == WA_Prueba.EjemplarWS.tipoEjemplar.DIGITAL);

                    ejemplaresLibres = ejemplarwsClient.contarEjemplaresFisicosDisponiblesPorMaterialYSede(idMaterial, sedeId);
                    ejemplaresPrestamo = stockFisico - ejemplaresLibres;
                    if (ejemplaresMaterial.Any())
                    {
                        ejemplaresHTML = "<ul>";
                        foreach (var ej in ejemplaresMaterial)
                        {
                            ejemplaresHTML +=
                                $"<li><strong>Código:</strong> {ej.idEjemplar.ToString("D3")} - <strong>Fecha de adquisición:</strong> {ej.fechaAdquisicion.ToString("dd/MM/yyyy")} - <strong>Tipo:</strong> {ej.tipo}</li>";
                        }
                        ejemplaresHTML += "</ul>";
                    }
                }
                else
                {
                    var ejemplaresSede = ejemplarwsClient.listarEjemplares();
                    var ejemplaresMaterial = ejemplaresSede.Where(e => e.material.idMaterial == idMaterial).ToList();

                    stockFisico = ejemplaresMaterial.Count(e => e.tipo == WA_Prueba.EjemplarWS.tipoEjemplar.FISICO);
                    stockDigital = ejemplaresMaterial.Count(e => e.tipo == WA_Prueba.EjemplarWS.tipoEjemplar.DIGITAL);

                    ejemplaresLibres = ejemplaresMaterial.Count(e => e.tipo == WA_Prueba.EjemplarWS.tipoEjemplar.FISICO && e.disponible == true);
                    ejemplaresPrestamo = ejemplaresMaterial.Count(e => e.tipo == WA_Prueba.EjemplarWS.tipoEjemplar.FISICO && e.disponible == false);

                    if (ejemplaresMaterial.Any())
                    {
                        ejemplaresHTML = "<ul>";
                        foreach (var ej in ejemplaresMaterial)
                        {
                            ejemplaresHTML +=
                                $"<li><strong>Código:</strong> {ej.idEjemplar.ToString("D3")} - <strong>Fecha de adquisición:</strong> {ej.fechaAdquisicion.ToString("dd/MM/yyyy")} - <strong>Tipo:</strong> {ej.tipo}</li>";
                        }
                        ejemplaresHTML += "</ul>";
                    }
                }

                stockTotalSede.Text = $"Stock total: {stockFisico} FISICOS - {stockDigital} DIGITALES";
                ejemplarLibres.Text = ejemplaresLibres.ToString("D3");
                ejemplarPrestamo.Text = ejemplaresPrestamo.ToString("D3");
                Literal1.Text = ejemplaresHTML;
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error al obtener los datos de ejemplares: " + ex.Message + "');</script>");
            }
        }

        protected void LoadMateriales()
        {
            try
            {
                WA_Prueba.MaterialWS.materialesDTO[] materialesArray = materialwsClient.listarTodos();
                List<WA_Prueba.MaterialWS.materialesDTO> materiales = materialesArray.ToList();
                dgvLibros.DataSource = materiales;
                dgvLibros.DataBind();

                cantidadTotalObrasLabel.Text = materiales.Count.ToString("D3");
                WA_Prueba.EjemplarWS.ejemplaresDTO[] ejemplaresArray = ejemplarwsClient.listarEjemplares();
                List<WA_Prueba.EjemplarWS.ejemplaresDTO> ejemplares = ejemplaresArray.ToList();
                cantidadTotalEjemplares.Text = ejemplares.Count.ToString("D3");
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error al cargar los materiales: " + ex.Message + "');</script>");
            }
        }

        // Cargar materiales por sede
        protected void LoadMaterialesPorSede(int sedeId)
        {
            try
            {
                dgvLibros.DataSource = null;

                WA_Prueba.MaterialWS.materialesDTO[] materialesArray = materialwsClient.listarMaterialesPorSede(sedeId);
                List<WA_Prueba.MaterialWS.materialesDTO> materiales = new List<WA_Prueba.MaterialWS.materialesDTO>();

                if (materialesArray != null && materialesArray.Length > 0)
                {
                    foreach (var materialId in materialesArray)
                    {
                        WA_Prueba.MaterialWS.materialesDTO material = materialwsClient.obtenerPorId(materialId.idMaterial);

                        if (material != null)
                        {
                            materiales.Add(material);
                        }
                        else
                        {
                            Response.Write("<script>alert('Material no encontrado para ID: " + materialId.idMaterial + "');</script>");
                        }
                    }

                    dgvLibros.DataSource = materiales;
                    dgvLibros.DataBind();
                    cantidadTotalObrasLabel.Text = materiales.Count.ToString("D3");
                    cantidadTotalEjemplares.Text = ejemplarwsClient.contarTotalPorSede(sedeId).ToString("D3");
                }
                else
                {
                    dgvLibros.DataSource = null;
                    dgvLibros.DataBind();
                    cantidadTotalObrasLabel.Text = "0";
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error al cargar los materiales por sede: " + ex.Message + "');</script>");
            }
        }

        protected void dgvLibros_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "VerDetalles")
            {
                try
                {
                    string idMaterial = e.CommandArgument.ToString();
                    if (string.IsNullOrEmpty(idMaterial))
                    {
                        Response.Write("<script>alert('ID del material no válido');</script>");
                        return;
                    }

                    hfMaterialId.Value = idMaterial;

                    int sedeId = Convert.ToInt32(ddlSedes.SelectedValue);

                    UpdateStockAndEjemplares(sedeId, Convert.ToInt32(idMaterial));

                    WA_Prueba.MaterialWS.materialesDTO material = materialwsClient.obtenerPorId(Convert.ToInt32(idMaterial));

                    if (material != null)
                    {
                        detalleTitulo.InnerText = material.titulo ?? "No disponible";
                        detalleAnio.InnerText = material.anioPublicacion.ToString() ?? "No disponible";
                        int editorialId = material.editorial.idEditorial;
                        WA_Prueba.EditorialWS.editorialesDTO editorial = editorialwsClient.obtenerEditorial(editorialId);
                        detalleEditorial.InnerText = editorial.nombre;
                        detalleDescripcion.InnerText = "No disponible aun";

                        WA_Prueba.MaterialWS.creadoresDTO[] creadoresArray = materialwsClient.listarCreadoresPorMaterial(Convert.ToInt32(idMaterial));
                        var creadorNombres = creadoresArray.Select(c => c.nombre).ToList();
                        detalleAutor.InnerText = string.Join(", ", creadorNombres);

                        WA_Prueba.MaterialWS.temasDTO[] temasArray = materialwsClient.listarTemasPorMaterial(Convert.ToInt32(idMaterial));
                        var temasDescripciones = temasArray.Select(c => c.descripcion).ToList();
                        detalleCategorias.InnerText = string.Join(", ", temasDescripciones);

                        detallesContainer.Style["display"] = "block";
                        statsContainer.Style["display"] = "none";
                        divEjemplares.Style["display"] = "none";
                    }
                    else
                    {
                        Response.Write("<script>alert('No se encontró el material');</script>");
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('Error al obtener los detalles: " + ex.Message + "');</script>");
                }
            }
        }
        protected void btnVerEjemplares_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hfMaterialId.Value))
            {
                int idMaterial = int.Parse(hfMaterialId.Value);
                int sedeId = Convert.ToInt32(ddlSedes.SelectedValue);

                // Verificar si sedeId es mayor a 0 y llamar a la función correcta
                if (sedeId > 0)
                {
                    try
                    {
                        // Llamar a listarFisicosDisponiblesPorMaterialYSede
                        var ejemplaresSede = ejemplarwsClient.listarFisicosDisponiblesPorMaterialYSede(idMaterial, sedeId);

                        if (ejemplaresSede != null && ejemplaresSede.Any())
                        {
                            string html = "";
                            foreach (var ej in ejemplaresSede)
                            {
                                html +=
                                    "<div class='fila no-clickable'>" +
                                        $"<span><strong>CÓDIGO:</strong> {ej.idEjemplar.ToString("D3")}</span>" +
                                        $"<span><strong>FECHA ADQUIRIDO:</strong> {ej.fechaAdquisicion.ToString("dd/MM/yyyy")}</span>" +
                                        $"<span><strong>DISPONIBILIDAD:</strong> {(ej.disponible ? "<strong>SI</strong>" : "<strong>NO</strong>")}</span>" +
                                        $"<span><strong>TIPO:</strong> {ej.tipo}</span>" +
                                        $"<span><strong>FORMATO DIGITAL:</strong> {(ej.formatoDigital != null ? ej.formatoDigital.ToString() : "-")}</span>" +
                                        $"<span><strong>UBICACIÓN:</strong> {ej.ubicacion}</span>" +
                                    "</div>";
                            }

                            litEjemplares.Text = html; // Mostrar los ejemplares
                        }
                        else
                        {
                            litEjemplares.Text = "No hay ejemplares disponibles para este material en esta sede.";
                        }
                    }
                    catch (Exception ex)
                    {
                        Response.Write("<script>alert('Error al obtener los ejemplares de la sede: " + ex.Message + "');</script>");
                    }
                }
                else // Si no se selecciona una sede específica, mostrar todos los ejemplares
                {
                    var ejemplares = materialwsClient.listarEjemplaresMaterial(idMaterial);
                    if (ejemplares != null && ejemplares.Any())
                    {
                        string html = "";
                        foreach (var ej in ejemplares)
                        {
                            html +=
                                "<div class='fila no-clickable'>" +
                                    $"<span><strong>CÓDIGO:</strong> {ej.idEjemplar.ToString("D3")}</span>" +
                                    $"<span><strong>FECHA ADQUIRIDO:</strong> {ej.fechaAdquisicion.ToString("dd/MM/yyyy")}</span>" +
                                    $"<span><strong>DISPONIBILIDAD:</strong> {(ej.disponible ? "<strong>SI</strong>" : "<strong>NO</strong>")}</span>" +
                                    $"<span><strong>TIPO:</strong> {ej.tipo}</span>" +
                                    $"<span><strong>FORMATO DIGITAL:</strong> {(ej.formatoDigital != null ? ej.formatoDigital.ToString() : "-")}</span>" +
                                    $"<span><strong>UBICACIÓN:</strong> {ej.ubicacion}</span>" +
                                "</div>";
                        }

                        litEjemplares.Text = html;
                    }
                    else
                    {
                        litEjemplares.Text = "No hay ejemplares para este material.";
                    }
                }

                // Ocultar detallesContainer y mostrar divEjemplares
                detallesContainer.Style["display"] = "none";
                divEjemplares.Style["display"] = "block";
            }
            else
            {
                litEjemplares.Text = "Por favor, selecciona un material primero.";
                detallesContainer.Style["display"] = "none";
                divEjemplares.Style["display"] = "block";
            }
        }
        protected void EliminarMaterial_Click(object sender, EventArgs e)
        {
            try
            {
                int idMaterial = Convert.ToInt32(hfMaterialId.Value);
                int result = materialwsClient.eliminarMaterial(idMaterial);
                if (result > 0)
                {
                    LoadMateriales();

                    // Ocultar la sección de detalles y mostrar nuevamente la lista y estadísticas
                    detallesContainer.Style["display"] = "none";
                    statsContainer.Style["display"] = "block";
                    dgvLibros.Style["display"] = "block";  // Mostrar la lista de libros

                    // Mostrar mensaje de éxito
                    Response.Write("<script>alert('Material eliminado exitosamente');</script>");
                }
                else
                {
                    Response.Write("<script>alert('No se pudo eliminar el material, probablemente hayan ejemplares en el sistema');</script>");
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error al eliminar el material: " + ex.Message + "');</script>");
            }
        }
        protected void btnEditar_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                int idMaterial = Convert.ToInt32(hfMaterialId.Value);
                Response.Redirect($"ModificarMaterial.aspx?idMaterial={idMaterial}");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadMateriales();
                LoadSedes();

            }
        }
    }
}