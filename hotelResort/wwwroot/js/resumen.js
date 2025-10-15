document.addEventListener("DOMContentLoaded", () => {
    const data = JSON.parse(localStorage.getItem("reservaData"));

    if (data) {
        document.getElementById("nombre").value = data.nombre;
        document.getElementById("telefono").value = data.telefono;
        document.getElementById("email").value = data.email;
        document.getElementById("habitaciones").value = data.habitacion;
        document.getElementById("cantidad").value = data.huespedes;
        document.getElementById("fecha-ingreso").value = formatearFechaParaInput(data.fechaEntrada);
        document.getElementById("fecha-salida").value = formatearFechaParaInput(data.fechaSalida);
    }

    // Función para convertir dd/mm/yyyy a formato yyyy-mm-dd
    function formatearFechaParaInput(fecha) {
        const partes = fecha.split("/");
        if (partes.length === 3) {
            const [dia, mes, año] = partes;
            return `${año}-${mes.padStart(2, "0")}-${dia.padStart(2, "0")}`;
        }
        return "";
    }

    // Confirmar reserva
    document.getElementById("confirmar").addEventListener("click", () => {
        alert("✅ ¡Reserva confirmada! Gracias por elegirnos.");
        localStorage.removeItem("reservaData");
    });
});
