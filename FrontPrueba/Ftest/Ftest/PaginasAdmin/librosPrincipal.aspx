<%@ Page Title="" Language="C#" MasterPageFile="~/BiblioMaster.Master" AutoEventWireup="true" CodeBehind="librosPrincipal.aspx.cs" Inherits="WA_Prueba.librosPrincipal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" runat="server">
    MyHolyLib - Administrador
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
        <style>
        .main-container {
            display: flex;
            justify-content: space-between; /* Alinea los contenedores a los extremos de la pantalla */
            margin-top: 20px;
            width: 100%;
            height: 70vh;
        }

        .left-container {
            width: 50%;
            flex-shrink: 0;       /* No se reduce */
            padding-right: 10px;  /* Espacio interno para que no pegue al borde */
            box-sizing: border-box; /* Para que el padding no aumente el ancho */
        }

        #divEjemplares {
            width: 50%;
            flex-shrink: 0;
            overflow-y: auto;
            padding-left: 10px;    /* Separación interna */
            box-sizing: border-box;
            background: white;
            border-radius: 8px;
            
            border: 1px solid #ccc;
        }
        
        .table-container {
            width: 100%;
            margin-top: 20px;
            height: 60vh; /* Fija la altura de la tabla */
            overflow-y: auto; /* Hace que solo la tabla sea desplazable */
        }

        h2 {
            font-size: 36px;
            font-weight: bold;
            margin: 0;
            padding-bottom: 10px;
        }

        .table-container {
            width: 100%;
            margin-top: 20px;
        }

        .detallesContainer {
            width: 80%; /* Ajusta el tamaño de la sección de detalles */
            background-color: #ffffff;
            padding: 20px;
            border-radius: 8px;
            font-size: 18px;
            line-height: 1.8;
            display: none; /* Usamos flexbox para alinearlos */
            flex-direction: column; /* Alineamos los elementos en fila */
            justify-content: flex-start; /* Alinea el contenido en el inicio */
            align-items: flex-start; /* Alinea los items al inicio (parte superior) */
        }

        .detalle-texto {
            flex: 70%; /* El área de texto toma el 70% del espacio */
            margin-right: 20px; /* Añadimos un espacio entre los detalles y los botones */
        }

        .button-container {
            display: flex;
            flex-direction: row;  /* Los botones estarán en fila */
            justify-content: flex-start;
            align-items: center; /* Alinea los botones al centro */
            width: 100%; /* Ocupa todo el espacio disponible */
            gap: 10px; /* Añade espacio entre los botones */
        }

        .action-button {
            padding: 10px;
            margin: 10px 0;
            background-color: #2c6b3e;
            color: white;
            border: none;
            cursor: pointer;
            border-radius: 5px;
            font-size: 16px;
            text-align: center;
        }

        .action-button:hover {
            background-color: #1f4b28;
        }

        .detalle-texto p {
            margin: 15px 0;
            font-size: 18px;
            line-height: 2.0; /* Ajustamos el espaciado entre líneas */
        }

        .stats-container {
            width: 80%;
            background-color: #ffffff;
            padding: 20px;
            border-radius: 8px;
            font-size: 22px;
            line-height: 1.8;
            display: block;
            flex-direction: column;
            justify-content: center;
            align-items: center;
            text-align: center;
        }

        th {
            background-color: #2c6b3e; /* Verde */
            color: white; /* Contraste con el fondo verde */
            padding: 10px;
            text-align: center;
            border: none;
        }

        table {
            width: 100%; /* Hace que la tabla ocupe todo el ancho del contenedor */
            border-collapse: collapse;
            margin: 0 auto;
        }

        th:nth-child(1), th:nth-child(2) {
            background-color: #2c6b3e;
            color: white;
        }

        th, td {
            padding: 10px;
            border: 1px solid #ddd;
        }
        .modal {
            display: none; /* Ocultar el modal por defecto */
            position: fixed; /* Fijo en la pantalla */
            z-index: 1; /* Asegura que esté por encima de otros elementos */
            left: 0;
            top: 0;
            width: 100%;
            height: 100%;
            background-color: rgba(0,0,0,0.5); /* Fondo oscuro con transparencia */
            justify-content: center;
            align-items: center;
            text-align: center;
        }

        /* Estilo del contenido del modal */
        .modal-content {
            background-color: white;
            padding: 20px;
            border-radius: 8px;
            width: 300px;
            margin: auto;
        }
        .modal-content h3 {
            font-size: 16px;  /* Tamaño más pequeño para el título */
        }

        .modal-content button {
            font-size: 14px; /* Tamaño más pequeño para los botones */
        }

        /* Estilo para el botón de cerrar */
        .close {
            color: #aaa;
            font-size: 28px;
            font-weight: bold;
            position: absolute;
            top: 10px;
            right: 25px;
        }

        .close:hover,
        .close:focus {
            color: black;
            text-decoration: none;
            cursor: pointer;
        }

        .filas-desplegables .fila {
            background-color: #f2f2f2;
            padding: 10px 20px;
            margin-bottom: 10px;
            border-radius: 5px;
            cursor: default;
            display: flex;
            flex-wrap: wrap;  /* Para que se acomoden en varias líneas si es necesario */
            gap: 12px 25px;   /* Espacio vertical y horizontal entre spans */
            align-items: center;
        }

        .filas-desplegables .fila div {
            font-size: 14px;
        }

        .filas-desplegables .fila span strong {
            font-weight: 700;
            margin-right: 6px;
        }

        .filas-desplegables hr {
            border: 0;
            border-top: 1px solid #ccc;
            margin: 10px 0;
        }

        .filas-desplegables .fila.clickable {
            cursor: pointer; /* Cambia el cursor solo en filas clickeables */
        }

        .filas-desplegables .fila:hover {
            background-color: #ddd; /* Fondo más oscuro cuando se pasa el ratón */
        }

        .filas-desplegables .fila.no-clickable {
            cursor: not-allowed; /* No permite hacer clic en estas filas */
            background-color: #d4edda;
        }

        .info {
            margin-top: 10px;
            padding: 10px;
            background-color: #e0e0e0; /* Fondo más claro para el contenido desplegable */
            border-radius: 5px;
        }

        .filas-desplegables .fila span {
            font-size: 14px;
            max-width: 70%; /* Aumentamos el ancho máximo */
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
            margin-right: 25px;
        }

        .header-left {
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: 15px; /* espacio entre título y botón */
            margin-bottom: 10px;
        }

        .header-left h2 {
            margin: 0;
        }

    </style>

<div class="main-container">
        <!-- Contenedor de la tabla -->
        <div class="left-container">
            <asp:DropDownList ID="ddlSedes" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSedes_SelectedIndexChanged" CssClass="action-button" style="font-weight: bold; background-color: #f8f8f8; border-radius: 8px; padding: 10px; border: 1px solid #ccc; width: 100%; margin-bottom: 15px; color: #333;font-size: 25px;">
                <asp:ListItem Text="Seleccione una sede" Value="0" />
                <asp:ListItem Text="Todas las sedes" Value="0" />
            </asp:DropDownList>
            <asp:Button ID="btnAgregarMaterial" runat="server" Text="Agregar Nuevo Material" CssClass="action-button" OnClientClick="window.location.href='InsertarMaterial.aspx'; return false;" />
            <div class="table-container">
                <asp:GridView ID="dgvLibros" runat="server" AllowPaging="false" AutoGenerateColumns="false" CssClass="table table-hover table-responsive table-striped" OnRowCommand="dgvLibros_RowCommand">
                    <Columns>
                        <asp:BoundField HeaderText="ID" DataField="idMaterial" />
                        <asp:BoundField HeaderText="INVENTARIO" DataField="titulo" />
                        <asp:TemplateField HeaderText="" ItemStyle-Width="50px">
                            <ItemTemplate>
                                <asp:Button ID="btnVerDetalle" runat="server" Text="➡️" CommandName="VerDetalles" CommandArgument='<%# Eval("idMaterial") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>

        <!-- Sección de detalles del libro -->
        <div id="detallesContainer" class="detallesContainer" runat="server" style="display: none;">
            <div class="detalle-texto">
                <h3>Detalles del Libro</h3>
                <p><strong>Título:</strong> <span id="detalleTitulo" runat="server"></span></p>
                <p><strong>Año de publicación:</strong> <span id="detalleAnio" runat="server"></span></p>
                <p><strong>Categorías:</strong> <span id="detalleCategorias" runat="server"></span></p>
                <p><strong>Autor:</strong> <span id="detalleAutor" runat="server"></span></p>
                <p><strong>Editorial:</strong> <span id="detalleEditorial" runat="server"></span></p>
                <p><strong>Descripción:</strong> <span id="detalleDescripcion" runat="server"></span></p>
                <asp:HiddenField ID="hfMaterialId" runat="server" />
                <button runat="server" id="btnVolver" class="action-button" OnClick="VolverALaLista_Click">Volver a la lista</button>
            </div>

            <div class="button-container">
               <asp:Button ID="btnEditar" runat="server" Text="Editar Material" CssClass="action-button" 
                CommandName="Editar" CommandArgument='<%# Eval("idMaterial") %>' OnCommand="btnEditar_Command" />



                <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CssClass="action-button" OnClick="EliminarMaterial_Click" />
                <asp:Button ID="btnVerEjemplares" runat="server" Text="Ver Ejemplares" CssClass="action-button" OnClick="btnVerEjemplares_Click" />
                <asp:LinkButton ID="btnAgregarEjemplar" runat="server" CssClass="action-button"
                    OnClientClick="redirigirAgregarEjemplar(); return false;">Agregar Ejemplar</asp:LinkButton>

            </div>

            <div class="filas-desplegables">
                <div class="fila no-clickable">
                    <span>Stock total de sede:</span> 
                    <asp:Label ID="stockTotalSede" runat="server" Text="0"></asp:Label>
                </div>
                <div class="fila clickable" onclick="toggleInfo('info2')">
                    <span>Lista de Ejemplares</span>
                    <div id="info2" class="info" style="display: none;">
                        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                    </div>
                </div>
                <div class="fila no-clickable">
                    <span>Ejemplares libres:</span> 
                    <asp:Label ID="ejemplarLibres" runat="server" Text="0"></asp:Label>
                </div>
                <div class="fila no-clickable">
                    <span>Ejemplares en préstamo:</span> 
                    <asp:Label ID="ejemplarPrestamo" runat="server" Text="0"></asp:Label>
                </div>
                <div class="fila clickable" onclick="toggleInfo('info5')">
                    <span>Lista de solicitudes de préstamo:</span>
                    <div id="info5" class="info" style="display: none;">
                        <p>No se encontraron solicitudes pendientes por el momento.</p>
                    </div>
                </div>
            </div>
        </div>

        <!-- Sección de estadísticas -->
        <div id="statsContainer" class="stats-container" runat="server" style="display: block;">
            <p><strong>CANTIDAD TOTAL DE OBRAS PRESENTES</strong></p>
            <asp:Label ID="cantidadTotalObrasLabel" runat="server" Text="0"></asp:Label>
            <p><strong>CANTIDAD TOTAL DE EJEMPLARES</strong></p>
            <asp:Label ID="cantidadTotalEjemplares" runat="server" Text="0"></asp:Label>
            <p><strong>USUARIOS EN LÍNEA</strong></p>
            <p>123</p>
        </div>
        <div id="divEjemplares" runat="server" class="filas-desplegables" style="display:none;">
            <asp:Literal ID="litEjemplares" runat="server"></asp:Literal>
        </div>
    </div>

    <div id="modalConfirmacion" class="modal">
        <div class="modal-content">
            <span class="close" onclick="cerrarModal()">&times;</span>
            <h3>¿Estás seguro de que deseas eliminar este material?</h3>
            <button onclick="eliminarMaterial()">Sí, eliminar</button>
            <button onclick="cerrarModal()">Cancelar</button>
        </div>
    </div>

    <script>
        function toggleInfo(id) {
            var info = document.getElementById(id);
            if (info.style.display === "none") {
                info.style.display = "block";
            } else {
                info.style.display = "none";
            }
        }

        function mostrarModal() {
            document.getElementById('modalConfirmacion').style.display = 'flex';
        }

        function cerrarModal() {
            document.getElementById('modalConfirmacion').style.display = 'none';
        }

        function eliminarMaterial() {
            alert("Material eliminado.");
            cerrarModal();
        }
        function RedirectToModifyPage(button) {
            var idMaterial = button.getAttribute('commandargument');
            window.location.href = 'ModificarMaterial.aspx?idMaterial=' + idMaterial;
            return false;
        }
        function redirigirAgregarEjemplar() {
            var idMaterial = document.getElementById('<%= hfMaterialId.ClientID %>').value;
            if (idMaterial && idMaterial !== "0" && idMaterial !== "") {
                window.location.href = 'InsertarEjemplar.aspx?idMaterial=' + idMaterial;
            } else {
                alert('No se encontró el ID del material.');
            }
        }

    </script>
</asp:Content>
