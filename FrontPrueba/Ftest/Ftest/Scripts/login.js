document.addEventListener('DOMContentLoaded', function () {
    const resetBtn = document.querySelector('#reset-password-btn');
    const backBtn = document.querySelector('.back-to-login');

    const pnlLogin = document.getElementById('<%= pnlLogin.ClientID %>');
    const pnlReset = document.getElementById('<%= pnlReset.ClientID %>');

    resetBtn.addEventListener('click', (e) => {
        e.preventDefault();
        pnlLogin.classList.remove('visible-panel');
        pnlLogin.classList.add('hidden-panel');
        pnlReset.classList.remove('hidden-panel');
        pnlReset.classList.add('visible-panel');
    });

    backBtn.addEventListener('click', (e) => {
        e.preventDefault();
        limpiarMensajes();
        pnlReset.classList.remove('visible-panel');
        pnlReset.classList.add('hidden-panel');
        pnlLogin.classList.remove('hidden-panel');
        pnlLogin.classList.add('visible-panel');
    });


});

function limpiarMensaje() {
    const mensaje = document.getElementById('<%= lblMensaje.ClientID %>');
    if (mensaje) {
        mensaje.classList.add('d-none');
        mensaje.innerText = '';
    }
}
function limpiarMensajes() {
    const lblExito = document.getElementById('<%= lblConfirmacion.ClientID %>');
    const lblError = document.getElementById('<%= lblMensaje.ClientID %>');

    if (lblExito) {
        lblExito.classList.add('d-none');
        lblExito.innerText = '';
    }
    if (lblError) {
        lblError.classList.add('d-none');
        lblError.innerText = '';
    }
}

$(document).ready(function () {
    // Cambio entre paneles
    $('#reset-password-btn').on('click', function (e) {
        e.preventDefault();
        $('.login-panel').addClass('d-none');
        $('.reset-panel').removeClass('d-none');
        limpiarMensaje();
    });

    $('.back-to-login').on('click', function (e) {
        e.preventDefault();
        $('.reset-panel').addClass('d-none');
        $('.login-panel').removeClass('d-none');
        limpiarMensaje();
    });

    // Mostrar/Ocultar contraseña
    $('.toggle-password').on('click', function () {
        const $input = $(this).siblings('input');
        const type = $input.attr('type') === 'password' ? 'text' : 'password';
        $input.attr('type', type);
    });


    function validateForm(selector) {
        let isValid = true;
        $(`${selector} input`).each(function () {
            const $input = $(this);
            clearTooltip($input);

            const value = $input.val().trim();
            const type = $input.attr('type');

            // Validación de campo vacío
            if (!value) {
                showTooltip($input, 'Este campo es obligatorio');
                isValid = false;
            }
            // Validación de formato de correo (si aplica)
            else if (type === 'email' && !/^\S+@\S+\.\S+$/.test(value)) {
                showTooltip($input, 'Ingresa un correo válido');
                isValid = false;
            }
        });
        return isValid;
    }

    $('.reset-panel .btn').on('click', function (e) {
        if (!validateForm('.reset-panel')) {
            e.preventDefault();
        }
    });


    $('.login-panel .btn').on('click', function (e) {
        if (!validateForm('.login-panel')) {
            e.preventDefault();
        }
    });

    $('.reset-panel .btn').on('click', function (e) {
        if (!validateForm('.reset-panel')) {
            e.preventDefault();
        }
    });
});
$('.login-panel .btn').on('click', function (e) {
    if (!validateForm('.login-panel')) {
        e.preventDefault();
    }
});
$('.login-panel input').on('input', function () {
    limpiarMensaje();
});
