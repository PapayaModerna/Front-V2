document.addEventListener('DOMContentLoaded', () => {
    if (document.querySelector('#rotating-text')) {
        new Typed('#rotating-text', {
            strings: ['aprender', 'explorar', 'crecer', 'leer', 'conectar'],
            typeSpeed: 300,
            backSpeed: 300,
            backDelay: 1000,
            loop: true,
            showCursor: true,
            cursorChar: ''
        });
    }
});