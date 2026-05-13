import { useEffect, useState } from "react";
import CountdownTimer from "../components/CountdownTimer";

const API = "https://localhost:7016";

export default function SeatsPage({ event, user, onBack, onCheckout }) {
    const [sectors, setSectors] = useState([]);
    const [seatsBySector, setSeatsBySector] = useState({});
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [reserving, setReserving] = useState(null);
    const [message, setMessage] = useState(null);
    const [pendingReservations, setPendingReservations] = useState([]);

    const fetchSeats = async (sectorList) => {
        const result = {};
        for (const sector of sectorList) {
            const res = await fetch(`${API}/api/v1/sectors/${sector.id}/seats`);
            const data = await res.json();
            result[sector.name] = data;
        }
        setSeatsBySector(result);
    };

    useEffect(() => {
        fetch(`${API}/api/v1/events/${event.id}/sectors`)
            .then((r) => r.json())
            .then(async (data) => {
                setSectors(data);
                await fetchSeats(data);
                setLoading(false);
            })
            .catch(() => {
                setError("No se pudo cargar el mapa de asientos.");
                setLoading(false);
            });
    }, [event.id]);

    const handleReserve = async (seat, sectorName, price) => {
        if (seat.status !== "Available") return;
        setReserving(seat.id);
        setMessage(null);

        try {
            const res = await fetch(`${API}/api/v1/reservations`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ seatId: seat.id, userId: user.id }),
            });

            if (res.ok) {
                const data = await res.json();
                setPendingReservations((prev) => [
                    ...prev,
                    { reservationId: data.id, seatNumber: seat.number, sectorName, price, expiresAt: data.expiresAt },
                ]);
                setMessage({ type: "success", text: `✅ Butaca ${seat.number} agregada al carrito.` });
                await fetchSeats(sectors);
            } else if (res.status === 409) {
                setMessage({ type: "error", text: "⚠️ Esa butaca ya fue reservada por otro usuario." });
                await fetchSeats(sectors);
            } else {
                setMessage({ type: "error", text: "❌ No se pudo completar la reserva." });
            }
        } catch {
            setMessage({ type: "error", text: "❌ Error de conexión con el servidor." });
        } finally {
            setReserving(null);
        }
    };

    const cartTotal = pendingReservations.reduce((sum, r) => sum + r.price, 0);

    const totalSeats = Object.values(seatsBySector).flat();
    const available = totalSeats.filter((s) => s.status === "Available").length;
    const total = totalSeats.length;

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

                {Object.entries(seatsBySector).map(([sectorName, seats]) => {
                    const sector = sectors.find((s) => s.name === sectorName);
                    const price = sector?.price ?? 0;
                    return (
                        <div key={sectorName} className="sector">
                            <h3 className="sector-title">
                                {sectorName}
                                <span className="sector-price">${price.toFixed(2)} por entrada</span>
                            </h3>
                            <div className="seats-grid">
                                {seats
                                    .sort((a, b) => a.Number - b.Number)
                                    .map((seat) => (
                                        <button
                                            key={seat.id}
                                            className={`seat ${seat.status.toLowerCase()} ${reserving === seat.id ? "loading" : ""}`}
                                            onClick={() => handleReserve(seat, sectorName, price)}
                                            disabled={seat.status !== "Available" || reserving !== null}
                                            title={`Butaca ${seat.number} - ${seat.status}`}
                                        >
                                            {reserving === seat.id ? "..." : seat.number}
                                        </button>
                                    ))}
                            </div>
                        </div>
                    );
                })}
            </main>

            {pendingReservations.length > 0 && (
                <div className="cart-bar">
                    <div className="cart-info">
                        <span className="cart-count">{pendingReservations.length} {pendingReservations.length === 1 ? "butaca" : "butacas"} seleccionadas</span>
                        <span className="cart-seats">
                            {pendingReservations.map((r) => r.seatNumber).join(", ")}
                        </span>
                    </div>
                    <CountdownTimer expiresAt={pendingReservations[0].expiresAt} />
                    <div className="cart-right">
                        <span className="cart-total">${cartTotal.toFixed(2)}</span>
                        <button className="cart-btn" onClick={() => onCheckout(pendingReservations)}>
                            Ir al pago →
                        </button>
                    </div>
                </div>
            )}
        </div>
    );
}
