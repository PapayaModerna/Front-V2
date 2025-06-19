function openModal(titulo, codigo, edicion, anio, portada, nivel, editorial) {
    // Setear datos básicos en el modal
    document.getElementById("modal-title").textContent = titulo;
    document.getElementById("modal-codigo").textContent = codigo;
    document.getElementById("modal-edicion").textContent = edicion;
    document.getElementById("modal-anio").textContent = anio;
    document.getElementById("modal-portada").src = portada || "Images/recurso.png";
    document.getElementById("modal-nivel").textContent = nivel;
    document.getElementById("modal-editorial").textContent = editorial;

    // Guardar material en sesiónStorage por si lo necesitas luego
    sessionStorage.setItem("idMaterial", codigo);

    // Limpiar tabla de ejemplares
    const tbody = document.getElementById("tbodyEjemplares");
    tbody.innerHTML = '<tr><td colspan="5">Cargando ejemplares...</td></tr>';

    // Obtener ejemplares desde WebMethod enriquecido
    $.ajax({
        type: "POST",
        url: "Recursos.aspx/ObtenerEjemplaresPorMaterial",
        data: JSON.stringify({ idMaterial: parseInt(codigo) }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            const ejemplares = response.d;
            tbody.innerHTML = '';

            if (!ejemplares || ejemplares.length === 0) {
                tbody.innerHTML = '<tr><td colspan="5">No hay ejemplares disponibles.</td></tr>';
                return;
            }

            ejemplares.forEach(e => {
                const idSede = e.sede?.idSede || "";
                const nombreSede = e.sede?.nombre || ("Sede " + idSede);
                const tipoTexto = e.tipo === 0 ? "Físico" : "Digital";
                const ubicacionTexto = e.ubicacion || "";

                const checkboxHTML = e.tipo === 0
                    ? `<input type="checkbox" class="chkEjemplar" data-id="${e.idEjemplar}" data-idsede="${idSede}">`
                    : "";

                const fila = `
                    <tr>
                        <td>${checkboxHTML}</td>
                        <td>${e.idEjemplar}</td>
                        <td>${nombreSede}</td>
                        <td>${tipoTexto}</td>
                        <td>${ubicacionTexto}</td>
                    </tr>
                `;

                tbody.insertAdjacentHTML("beforeend", fila);
            });

            // Asignar eventos a los checkbox (uno activo a la vez)
            tbody.querySelectorAll(".chkEjemplar").forEach(chk => {
                chk.addEventListener("change", function (e) {
                    const btnSolicitar = document.getElementById("btnSolicitarPrestamo");
                    if (e.target.checked) {
                        const idSede = e.target.getAttribute("data-idsede") || "";

                        // Asignar al botón los datos necesarios sin usar hidden inputs
                        if (btnSolicitar) {
                            const commandArg = codigo + "|" + idSede;
                            btnSolicitar.setAttribute("data-idmaterial", codigo);
                            btnSolicitar.setAttribute("data-idsede", idSede);
                            btnSolicitar.setAttribute("data-command", commandArg);
                            btnSolicitar.setAttribute("CommandArgument", commandArg);
                        }

                        // Desmarcar los demás checkbox
                        tbody.querySelectorAll(".chkEjemplar").forEach(other => {
                            if (other !== e.target) other.checked = false;
                        });
                    } else {
                        // Si se desmarca, se limpia la asignación
                        if (btnSolicitar) {
                            btnSolicitar.removeAttribute("data-idsede");
                            btnSolicitar.removeAttribute("CommandArgument");
                        }
                    }
                });
            });
        },
        error: function () {
            tbody.innerHTML = '<tr><td colspan="5">Error cargando ejemplares.</td></tr>';
        }
    });

    // Mostrar el modal
    const myModal = new bootstrap.Modal(document.getElementById('modal'));
    myModal.show();
}
function closeModal() {
    const scrollY = document.body.style.getPropertyValue('--scroll-y');
    document.getElementById("modal").style.display = "none";
    document.body.style.position = '';
    document.body.style.top = '';
    window.scrollTo(0, parseInt(scrollY || '0') * -1);
}

// Toggle filter sections con jQuery
function initToggleButtons() {
    const toggleButtons = $('.toggle-btn');

    toggleButtons.off('click').on('click', function () {
        const btn = $(this);
        const content = btn.next('.filter-options');
        const isExpanded = btn.toggleClass('expanded').hasClass('expanded');

        btn.attr('aria-expanded', isExpanded);
        content.attr('aria-hidden', !isExpanded);

        if (isExpanded) {
            // Para abrir: max-height al scrollHeight
            content.css('max-height', content.prop('scrollHeight') + "px");
        } else {
            // Para cerrar: primero max-height al scrollHeight para iniciar la transición
            content.css('max-height', content.prop('scrollHeight') - "px");

        }
    });
}


// Ejecutar al cargar la página y también tras postbacks parciales en ASP.NET
$(document).ready(function () {
    initToggleButtons();
});

if (typeof Sys !== "undefined" && Sys.Application) {
    Sys.Application.add_load(initToggleButtons);
}
function toggleFiltros() {
    const panel = document.getElementById("panelFiltros");
    panel.classList.toggle("oculto");
}

document.addEventListener("DOMContentLoaded", () => {
    const checkboxes = document.querySelectorAll('.chk-group');
    console.log("Checkboxes encontrados:", checkboxes.length);
});
document.addEventListener("DOMContentLoaded", function () {
    function aplicarComportamientoExclusivo() {
        const checkboxes = document.querySelectorAll('.chk-group');
        checkboxes.forEach(chk => {
            chk.addEventListener('click', function () {
                if (this.checked) {
                    checkboxes.forEach(other => {
                        if (other !== this) {
                            other.checked = false;
                        }
                    });
                }
            });
        });
    }

    aplicarComportamientoExclusivo();

    // Si usas UpdatePanel, vuelve a aplicar después de cada postback parcial
    if (typeof Sys !== "undefined" && Sys.WebForms && Sys.WebForms.PageRequestManager) {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            aplicarComportamientoExclusivo();
        });
    }
}); 

function closeModal() {
    const scrollY = document.body.style.getPropertyValue('--scroll-y');
    document.getElementById("modal").style.display = "none";
    document.body.style.position = '';
    document.body.style.top = '';
    window.scrollTo(0, parseInt(scrollY || '0') * -1);
}

// Toggle filter sections con jQuery
function initToggleButtons() {
    const toggleButtons = $('.toggle-btn');

    toggleButtons.off('click').on('click', function () {
        const btn = $(this);
        const content = btn.next('.filter-options');
        const isExpanded = btn.toggleClass('expanded').hasClass('expanded');

        btn.attr('aria-expanded', isExpanded);
        content.attr('aria-hidden', !isExpanded);

        if (isExpanded) {
            // Para abrir: max-height al scrollHeight
            content.css('max-height', content.prop('scrollHeight') + "px");
        } else {
            // Para cerrar: primero max-height al scrollHeight para iniciar la transición
            content.css('max-height', content.prop('scrollHeight') - "px");

        }
    });
}


// Ejecutar al cargar la página y también tras postbacks parciales en ASP.NET
$(document).ready(function () {
    initToggleButtons();
});

if (typeof Sys !== "undefined" && Sys.Application) {
    Sys.Application.add_load(initToggleButtons);
}
function toggleFiltros() {
    const panel = document.getElementById("panelFiltros");
    panel.classList.toggle("oculto");
}

document.addEventListener("DOMContentLoaded", () => {
    const checkboxes = document.querySelectorAll('.chk-group');
    console.log("Checkboxes encontrados:", checkboxes.length);
});
document.addEventListener("DOMContentLoaded", function () {
    function aplicarComportamientoExclusivo() {
        const checkboxes = document.querySelectorAll('.chk-group');
        checkboxes.forEach(chk => {
            chk.addEventListener('click', function () {
                if (this.checked) {
                    checkboxes.forEach(other => {
                        if (other !== this) {
                            other.checked = false;
                        }
                    });
                }
            });
        });
    }

    aplicarComportamientoExclusivo();

    // Si usas UpdatePanel, vuelve a aplicar después de cada postback parcial
    if (typeof Sys !== "undefined" && Sys.WebForms && Sys.WebForms.PageRequestManager) {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            aplicarComportamientoExclusivo();
        });
    }
});



function showToast() {
    var toast = document.getElementById("toast");
    toast.style.visibility = "visible";
    setTimeout(function () {
        toast.style.visibility = "hidden";
    }, 3000); // duración 3 segundos
}

function debugLog(location, ...values) {
    console.log("DEBUG - " + location + ":", ...values);
}