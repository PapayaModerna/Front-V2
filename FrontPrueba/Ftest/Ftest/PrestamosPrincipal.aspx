<%@ Page Title="" Language="C#" MasterPageFile="~/BiblioMaster.Master" AutoEventWireup="true" CodeBehind="PrestamosPrincipal.aspx.cs" Inherits="WA_Prueba.PrestamosPrincipal" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
    <style>
        table {
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
        }

        th, td {
            border: 1px solid #ddd;
            padding: 8px;
            text-align: left;
        }

        th {
            background-color: #4CAF50;
            color: white;
            font-weight: bold;
        }

        tr:nth-child(odd) {
            background-color: #f2f2f2;
        }

        tr:hover {
            background-color: #ddd;
        }

        table.empty-table td {
            text-align: center;
            font-style: italic;
            color: #999;
        }

        .button-container {
            position: fixed;
            top: 50px;
            z-index: 9999;
            right: 40px;
            display:flex;
            gap:10px;
        }
        .btn {
            opacity: 0.6;  
            transition: opacity 0.3s ease;
        }

        .btn:hover {
            opacity: 1;  
        }
        .button-container a {
            text-decoration: none;
        }
    </style>

    <div class="button-container">
        <asp:Button ID="btnDevolucion" runat="server" CssClass="btn btn-primary" Text="Devolución Préstamo" OnClientClick="window.location='DevolucionPrestamo.aspx'; return false;" />
        <asp:Button ID="btnRecojo" runat="server" CssClass="btn btn-primary" Text="Recojo Préstamo" OnClientClick="window.location='RecojoPrestamo.aspx'; return false;" />
    </div>

    <!-- Tabla para Prestamos Totales -->
    <h3>Prestamos Totales</h3>
    <table border="1" class="prestamos-table">
        <thead>
            <tr>
                <th>ID Préstamo</th>
                <th>Fecha Solicitada</th>
                <th>Fecha Préstamo</th>
                <th>Fecha Devolución</th>
                <th>Usuario</th>
            </tr>
        </thead>
        <tbody>
            <%= prestamosTable %>
        </tbody>
    </table>

    <!-- Tabla para Prestamos Solicitados -->
    <h3>Prestamos Solicitados</h3>
    <table border="1" class="prestamos-table">
        <thead>
            <tr>
                <th>ID Préstamo</th>
                <th>Fecha Solicitada</th>
                <th>Fecha Préstamo</th>
                <th>Fecha Devolución</th>
                <th>Usuario</th>
            </tr>
        </thead>
        <tbody>
            <%= prestamosTableSolicitados %>
        </tbody>
    </table>

    <!-- Tabla para Prestamos Atrasados -->
    <h3>Prestamos Atrasados</h3>
    <table border="1" class="prestamos-table">
        <thead>
            <tr>
                <th>ID Préstamo</th>
                <th>Fecha Solicitada</th>
                <th>Fecha Préstamo</th>
                <th>Fecha Devolución</th>
                <th>Usuario</th>
            </tr>
        </thead>
        <tbody>
            <%= prestamosTableAtrasados %>
        </tbody>
    </table>

    <!-- Tabla para Prestamos Devueltos -->
    <h3>Prestamos Devueltos</h3>
    <table border="1" class="prestamos-table">
        <thead>
            <tr>
                <th>ID Préstamo</th>
                <th>Fecha Solicitada</th>
                <th>Fecha Préstamo</th>
                <th>Fecha Devolución</th>
                <th>Usuario</th>
            </tr>
        </thead>
        <tbody>
            <%= prestamosTableDevueltos %>
        </tbody>
    </table>

    <!-- Tabla para Prestamos No Culminados -->
    <h3>Prestamos No Culminados</h3>
    <table border="1" class="prestamos-table">
        <thead>
            <tr>
                <th>ID Préstamo</th>
                <th>Fecha Solicitada</th>
                <th>Fecha Préstamo</th>
                <th>Fecha Devolución</th>   
                <th>Usuario</th>
            </tr>
        </thead>
        <tbody>
            <%= prestamosTableNoCulminados %>
        </tbody>
    </table>
</asp:Content>

