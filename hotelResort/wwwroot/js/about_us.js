

// 1. Efecto hover en imágenes
document.addEventListener('DOMContentLoaded', function() {
    const imagenes = document.querySelectorAll('img');
    imagenes.forEach(img => {
        img.addEventListener('mouseenter', function() {
            this.style.transform = 'scale(1.05)';
            this.style.transition = 'transform 0.3s ease';
        });
        img.addEventListener('mouseleave', function() {
            this.style.transform = 'scale(1)';
        });
    });
});

// 2. Efecto de escritura en el título principal
function efectoEscrituraTitulo() {
    const titulo = document.querySelector('.title-sobrenosotros');
    if (titulo) {
        const textoOriginal = titulo.textContent;
        titulo.textContent = '';
        let i = 0;
        function escribir() {
            if (i < textoOriginal.length) {
                titulo.textContent += textoOriginal.charAt(i);
                i++;
                setTimeout(escribir, 100);
            }
        }
        escribir();
    }
}

// 3. Efecto de escritura en Misión, Visión y Valores
function efectoEscrituraMisionVision() {
    const titulos = document.querySelectorAll('.title-mision, .title-vision, .title-valores');
    titulos.forEach((titulo, index) => {
        const textoOriginal = titulo.textContent;
        titulo.textContent = '';
        setTimeout(() => {
            let i = 0;
            function escribir() {
                if (i < textoOriginal.length) {
                    titulo.textContent += textoOriginal.charAt(i);
                    i++;
                    setTimeout(escribir, 100);
                }
            }
            escribir();
        }, index * 800 + 2000);
    });
}


// 4. Animación de aparición para secciones
function animarSecciones() {
    const secciones = document.querySelectorAll('section');
    secciones.forEach((seccion, index) => {
        seccion.style.opacity = '0';
        seccion.style.transform = 'translateY(30px)';
        seccion.style.transition = 'opacity 0.6s ease, transform 0.6s ease';
        setTimeout(() => {
            seccion.style.opacity = '1';
            seccion.style.transform = 'translateY(0)';
        }, index * 200);
    });
}

// 5. Efecto suave en banderas (sin rotación)
function efectoBanderas() {
    const banderas = document.querySelectorAll('.country-flag');
    banderas.forEach(bandera => {
        bandera.addEventListener('mouseenter', function() {
            this.style.transform = 'scale(1.1)';
            this.style.transition = 'transform 0.3s ease';
        });
        bandera.addEventListener('mouseleave', function() {
            this.style.transform = 'scale(1)';
        });
    });
}


// 6. Botón scroll hacia arriba
function botonScrollArriba() {
    const boton = document.createElement('button');
    boton.innerHTML = '↑';
    boton.style.cssText = `
        position: fixed;
        bottom: 20px;
        right: 20px;
        width: 50px;
        height: 50px;
        border-radius: 50%;
        background: #007bff;
        color: white;
        border: none;
        font-size: 20px;
        cursor: pointer;
        display: none;
        z-index: 1000;
        transition: all 0.3s ease;
    `;
    
    document.body.appendChild(boton);
    boton.addEventListener('click', () => window.scrollTo({ top: 0, behavior: 'smooth' }));
    
    window.addEventListener('scroll', () => {
        boton.style.display = window.pageYOffset > 300 ? 'block' : 'none';
    });
}

// 7. Efecto de brillo en botones
function brillarBotones() {
    const botones = document.querySelectorAll('.btn-pulse, .social-icon');
    botones.forEach(boton => {
        boton.addEventListener('mouseenter', function() {
            this.style.boxShadow = '0 0 20px rgba(0, 123, 255, 0.5)';
            this.style.transition = 'box-shadow 0.3s ease';
        });
        boton.addEventListener('mouseleave', function() {
            this.style.boxShadow = 'none';
        });
    });
}

// 8. Logo flotante
function efectoLogo() {
    const logo = document.querySelector('.logo img');
    if (logo) {
        setInterval(() => {
            logo.style.filter = 'brightness(1.2)';
            setTimeout(() => logo.style.filter = 'brightness(1)', 200);
        }, 3000);
    }
}

// Función principal
function inicializar() {
    animarSecciones();
    botonScrollArriba();
    brillarBotones();
    efectoBanderas();
    efectoLogo();
    
    setTimeout(efectoEscrituraTitulo, 1000);
    setTimeout(efectoEscrituraMisionVision, 2000);
}

// Inicializar
document.addEventListener('DOMContentLoaded', inicializar);

// Estilos CSS
const estilos = document.createElement('style');
estilos.textContent = `
    .logo img {
        transition: filter 0.3s ease;
    }
    .country-flag {
        transition: transform 0.3s ease;
    }
`;
document.head.appendChild(estilos);