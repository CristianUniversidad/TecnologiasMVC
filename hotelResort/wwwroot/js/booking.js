const daysContainer = document.getElementById("days");
const monthYear = document.getElementById("month-year");
const prevBtn = document.getElementById("prev");
const nextBtn = document.getElementById("next");

let currentDate = new Date();
let checkInDate = null;
let checkOutDate = null;

function renderCalendar(date) {
    const year = date.getFullYear();
    const month = date.getMonth();

    const months = [
        "January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"
    ];

    monthYear.textContent = `${months[month]} ${year}`;

    const firstDay = new Date(year, month, 1).getDay();
    const lastDate = new Date(year, month + 1, 0).getDate();

    daysContainer.innerHTML = "";

    // Rellenar días vacíos antes del primer día
    for (let i = 0; i < firstDay; i++) {
        const empty = document.createElement("span");
        daysContainer.appendChild(empty);
    }

    // Rellenar días del mes
    for (let i = 1; i <= lastDate; i++) {
        const day = document.createElement("span");
        day.textContent = i;

        const today = new Date();
        const current = new Date(year, month, i);

        // Día actual
        if (
            i === today.getDate() &&
            month === today.getMonth() &&
            year === today.getFullYear()
        ) {
            day.classList.add("today");
        }

        // Marcar selección
        if (checkInDate && checkOutDate && current >= checkInDate && current <= checkOutDate) {
            day.classList.add("selected-range");
        } else if (checkInDate && sameDate(current, checkInDate)) {
            day.classList.add("check-in");
        } else if (checkOutDate && sameDate(current, checkOutDate)) {
            day.classList.add("check-out");
        }

        day.addEventListener("click", () => handleDateClick(current));
        daysContainer.appendChild(day);
    }
}

// --- Función auxiliar para comparar fechas sin hora ---
function sameDate(d1, d2) {
    return (
        d1.getFullYear() === d2.getFullYear() &&
        d1.getMonth() === d2.getMonth() &&
        d1.getDate() === d2.getDate()
    );
}

// --- Lógica de selección ---
function handleDateClick(date) {
    if (!checkInDate || (checkInDate && checkOutDate)) {
        // Reiniciar selección
        checkInDate = date;
        checkOutDate = null;
    } else if (date > checkInDate) {
        checkOutDate = date;
    } else {
        // Si selecciona una fecha anterior, reinicia el check-in
        checkInDate = date;
        checkOutDate = null;
    }

    renderCalendar(currentDate);
    showSelectedDates();
}

// --- Mostrar fechas seleccionadas en el formulario ---
function showSelectedDates() {
    const entradaInput = document.getElementById("entrada");
    const salidaInput = document.getElementById("salida");

    entradaInput.value = checkInDate
        ? checkInDate.toLocaleDateString()
        : "";

    salidaInput.value = checkOutDate
        ? checkOutDate.toLocaleDateString()
        : "";
}

// Botones de navegación
prevBtn.addEventListener("click", () => {
    currentDate.setMonth(currentDate.getMonth() - 1);
    renderCalendar(currentDate);
});

nextBtn.addEventListener("click", () => {
    currentDate.setMonth(currentDate.getMonth() + 1);
    renderCalendar(currentDate);
});

// === Enviar datos a resumen.html ===
const reservarBtn = document.querySelector(".reservar-btn");

reservarBtn.addEventListener("click", () => {
    // Capturar los valores del formulario
    const nombre = document.querySelector('input[placeholder="Name"]').value;
    const email = document.querySelector('input[placeholder="Email"]').value;
    const telefono = document.querySelector('input[placeholder="Phone"]').value;
    const habitacion = document.querySelectorAll("select")[0].value;
    const huespedes = document.querySelectorAll("select")[1].value;
    const fechaEntrada = document.getElementById("entrada").value;
    const fechaSalida = document.getElementById("salida").value;

    // Validar que los campos estén llenos
    if (!nombre || !email || !telefono || habitacion === "Seleccionar" || huespedes === "Seleccionar" || !fechaEntrada || !fechaSalida) {
        alert("Por favor, completa todos los campos antes de continuar.");
        return;
    }

    // Guardar los datos en localStorage
    const reservaData = {
        nombre,
        email,
        telefono,
        habitacion,
        huespedes,
        fechaEntrada,
        fechaSalida
    };

    localStorage.setItem("reservaData", JSON.stringify(reservaData));

    // Redirigir a resumen.html
    window.location.href = "/Home/resumen";
});



// Inicializar calendario
renderCalendar(currentDate);
