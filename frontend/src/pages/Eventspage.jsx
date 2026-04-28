import { useEffect, useState } from "react";

const API = "https://localhost:7016";

export default function EventsPage({ onSelectEvent }) {
    const [events, setEvents] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        fetch(`${API}/api/v1/Events`)
            .then((r) => r.json())
            .then((data) => { setEvents(data); setLoading(false); })
            .catch(() => { setError("No se pudo conectar con el servidor."); setLoading(false); });
    }, []);

    return (
        <div className="page">
            <header className="header">
                <div className="header-inner">
                    <span className="logo">🎟 TicketApp</span>
                    <h1 className="title">Eventos disponibles</h1>
                </div>
            </header>

            <main className="main">
                {loading && <p className="status">Cargando eventos...</p>}
                {error && <p className="status error">{error}</p>}
                {!loading && !error && events.length === 0 && (
                    <p className="status">No hay eventos disponibles.</p>
                )}
                <div className="events-grid">
                    {events.map((event) => (
                        <div key={event.id} className="event-card" onClick={() => onSelectEvent(event)}>
                            <div className="event-badge">{new Date(event.eventDate).toLocaleDateString("es-AR", { day: "2-digit", month: "short" })}</div>
                            <div className="event-info">
                                <h2 className="event-name">{event.name}</h2>
                                <p className="event-venue">📍 {event.venue}</p>
                                <span className={`event-status ${event.status === "Active" ? "active" : "inactive"}`}>
                                    {event.status === "Active" ? "Disponible" : event.status}
                                </span>
                            </div>
                            <div className="event-arrow">→</div>
                        </div>
                    ))}
                </div>
            </main>
        </div>
    );
}