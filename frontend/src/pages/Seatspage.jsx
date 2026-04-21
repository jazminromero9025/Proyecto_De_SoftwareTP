import { useEffect, useState } from "react";

const API = "https://localhost:7016";
const USER_ID = 1; // usuario de prueba

export default function SeatsPage({ event, onBack }) {
    const [seats, setSeats] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [reserving, setReserving] = useState(null);
    const [message, setMessage] = useState(null);

    const fetchSeats = () => {
        setLoading(true);
        fetch(`${API}/api/v1/sectors/${event.sectorId}/seats`)
            .then((r) => r.json())
            .then((data) => { setSeats(data); setLoading(false); })
            .catch(() => { setError("No se pudo cargar el mapa de asientos."); setLoading(false); });
    };

    useEffect(() => { fetchSeats(); }, [event.id]);

    const handleReserve = async (seat) => {
        if (seat.status !== "Available") return;
        setReserving(seat.id);
        setMessage(null);

        try {
            const res = await fetch(`${API}/api/v1/reservations`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ seatId: seat.id, userId: USER_ID }),
            });

            if (res.ok) {
                setMessage({ type: "success", text: `✅ Butaca ${seat.seatNumber} reservada exitosamente. Tenés 5 minutos para completar el pago.` });
                fetchSeats(); // refresca el mapa
            } else if (res.status === 409) {
                setMessage({ type: "error", text: "⚠️ Esa butaca ya fue reservada por otro usuario." });
                fetchSeats();
            } else {
                setMessage({ type: "error", text: "❌ No se pudo completar la reserva." });
            }
        } catch {
            setMessage({ type: "error", text: "❌ Error de conexión con el servidor." });
        } finally {
            setReserving(null);
        }
    };

    // Agrupar por sector
    const bySector = seats.reduce((acc, seat) => {
        const key = seat.sectorName || "Sin sector";
        if (!acc[key]) acc[key] = [];
        acc[key].push(seat);
        return acc;
    }, {});

    const available = seats.filter((s) => s.status === "Available").length;
    const total = seats.length;

    return (
        <div className="page">
            <header className="header">
                <div className="header-inner">
                    <button className="back-btn" onClick={onBack}>← Volver</button>
                    <div>
                        <h1 className="title">{event.name}</h1>
                        <p className="event-venue">📍 {event.venue} · {new Date(event.eventDate).toLocaleDateString("es-AR", { dateStyle: "long" })}</p>
                    </div>
                </div>
            </header>

            <main className="main">
                {message && (
                    <div className={`toast ${message.type}`}>{message.text}</div>
                )}

                <div className="legend">
                    <span className="legend-item"><span className="dot available" />Disponible ({available})</span>
                    <span className="legend-item"><span className="dot reserved" />Reservada</span>
                    <span className="legend-item"><span className="dot sold" />Vendida</span>
                    <span className="legend-count">{available}/{total} disponibles</span>
                </div>

                {loading && <p className="status">Cargando mapa de asientos...</p>}
                {error && <p className="status error">{error}</p>}

                {Object.entries(bySector).map(([sectorName, sectorSeats]) => (
                    <div key={sectorName} className="sector">
                        <h3 className="sector-title">{sectorName}</h3>
                        <div className="seats-grid">
                            {sectorSeats
                                .sort((a, b) => a.seatNumber - b.seatNumber)
                                .map((seat) => (
                                    <button
                                        key={seat.id}
                                        className={`seat ${seat.status.toLowerCase()} ${reserving === seat.id ? "loading" : ""}`}
                                        onClick={() => handleReserve(seat)}
                                        disabled={seat.status !== "Available" || reserving !== null}
                                        title={`Butaca ${seat.seatNumber} - ${seat.status}`}
                                    >
                                        {reserving === seat.id ? "..." : seat.seatNumber}
                                    </button>
                                ))}
                        </div>
                    </div>
                ))}
            </main>
        </div>
    );
}