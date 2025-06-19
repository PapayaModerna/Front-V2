function openPrestamoModal(titulo, idEjemplar, codMaterial, fSolicitud, fPrestamo, fDevolucion) {
    document.getElementById("modal-title").textContent = titulo;
    document.getElementById("modal-ejemplar").textContent = idEjemplar;
    document.getElementById("modal-material-codigo").textContent = codMaterial;
    document.getElementById("modal-material-titulo").textContent = titulo;
    document.getElementById("modal-fecha-solicitud").textContent = fSolicitud;
    document.getElementById("modal-fecha-prestamo").textContent = fPrestamo;
    document.getElementById("modal-fecha-devolucion").textContent = fDevolucion;
    document.getElementById("modal").style.display = "block";
}

function closeModal() {
    document.getElementById("modal").style.display = "none";
}