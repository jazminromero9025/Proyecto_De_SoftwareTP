import { useState } from "react";

const API = "https://localhost:7016";

export default function AdminCreateEventPage({ onBack }) {
    // Paso 1: crear evento
    const [step, setStep] = useState("event"); // "event" | "sectors"
    const [createdEvent, setCreatedEvent] = useState(null);

    // Form evento
    const [eventName, setEventName] = useState("");
    const [eventDate, setEventDate] = useState("");
    const [eventVenue, setEventVenue] = useState("");
    const [savingEvent, setSavingEvent] = useState(false);
    const [eventError, setEventError] = useState(null);

    // Sectores
    const [sectors, setSectors] = useState([]);
    const [sectorName, setSectorName] = useState("");
    const [sectorPrice, setSectorPrice] = useState("");
    const [sectorCapacity, setSectorCapacity] = useState("");
    const [savingSector, setSavingSector] = useState(false);
    const [sectorError, setSectorError] = useState(null);

    const handleCreateEvent = async (e) => {
        e.preventDefault();
        setSavingEvent(true);
        setEventError(null);

        try {
            const res = await fetch(`${API}/api/v1/events`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ name: eventName, eventDate, venue: eventVenue }),
            });

            if (res.ok) {
                const data = await res.json();
                setCreatedEvent(data);
                setStep("sectors");
            } else {
                setEventError("No se pudo crear el evento.");
            }
        } catch {
            setEventError("Error de conexión con el servidor.");
        } finally {
            setSavingEvent(false);
        }
    };

    const handleAddSector = async (e) => {
        e.preventDefault();
        setSavingSector(true);
        setSectorError(null);

        try {
            const res = await fetch(`${API}/api/v1/events/${createdEvent.id}/sectors`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({
                    name: sectorName,
                    price: parseFloat(sectorPrice),
                    capacity: parseInt(sectorCapacity),
                }),
            });

            if (res.ok) {
                const data = await res.json();
                setSectors((prev) => [...prev, data]);
                setSectorName("");
                setSectorPrice("");
                setSectorCapacity("");
            } else {
                setSectorError("No se pudo agregar el sector.");
            }
        } catch {
            setSectorError("Error de conexión con el servidor.");
        } finally {
            setSavingSector(false);
        }
    };

    return (
        <div className="page">
            <header className="header">
                <div className="header-inner">
                    <button className="back-btn" onClick={onBack}>← Volver</button>
                    <span className="logo">🎟 TicketApp</span>
                    <h1 className="title" style={{ marginLeft: "1rem" }}>Crear evento</h1>
                </div>
            </header>

            <main className="main checkout-main">
                {step === "event" && (
                    <div className="checkout-card">
                        <h2 className="checkout-heading">Datos del evento</h2>

                        {eventError && <div className="toast error" style={{ marginBottom: "1rem" }}>{eventError}</div>}

                        <form className="admin-form" onSubmit={handleCreateEvent}>
                            <div className="form-group">
                                <label>Nombre del evento</label>
                                <input
                                    type="text"
                                    placeholder="Ej: Concierto de Rock"
                                    value={eventName}
                                    onChange={(e) => setEventName(e.target.value)}
                                    required
                                />
                            </div>
                            <div className="form-group">
                                <label>Fecha y hora</label>
                                <input
                                    type="datetime-local"
                                    value={eventDate}
                                    onChange={(e) => setEventDate(e.target.value)}
                                    required
                                />
                            </div>
                            <div className="form-group">
                                <label>Lugar</label>
                                <input
                                    type="text"
                                    placeholder="Ej: Estadio River Plate"
                                    value={eventVenue}
                                    onChange={(e) => setEventVenue(e.target.value)}
                                    required
                                />
                            </div>
                            <button type="submit" className="pay-btn checkout-pay-btn" disabled={savingEvent}>
                                {savingEvent ? "Creando..." : "Crear evento →"}
                            </button>
                        </form>
                    </div>
                )}

                {step === "sectors" && (
                    <div className="checkout-card" style={{ maxWidth: "700px" }}>
                        <div className="admin-event-banner">
                            <span>✅ Evento creado:</span>
                            <strong>{createdEvent.name}</strong>
                        </div>

                        <h2 className="checkout-heading" style={{ marginTop: "1.5rem" }}>Agregar sectores</h2>
                        <p style={{ color: "var(--text2)", fontSize: "0.875rem", marginBottom: "1.5rem" }}>
                            Cada sector genera butacas automáticamente según la capacidad indicada.
                        </p>

                        {sectorError && <div className="toast error" style={{ marginBottom: "1rem" }}>{sectorError}</div>}

                        <form className="admin-form admin-sector-form" onSubmit={handleAddSector}>
                            <div className="form-group">
                                <label>Nombre del sector</label>
                                <input
                                    type="text"
                                    placeholder="Ej: Campo, Platea, VIP"
                                    value={sectorName}
                                    onChange={(e) => setSectorName(e.target.value)}
                                    required
                                />
                            </div>
                            <div className="form-group">
                                <label>Precio</label>
                                <input
                                    type="number"
                                    placeholder="Ej: 5000"
                                    min="0"
                                    step="0.01"
                                    value={sectorPrice}
                                    onChange={(e) => setSectorPrice(e.target.value)}
                                    required
                                />
                            </div>
                            <div className="form-group">
                                <label>Capacidad (butacas)</label>
                                <input
                                    type="number"
                                    placeholder="Ej: 50"
                                    min="1"
                                    value={sectorCapacity}
                                    onChange={(e) => setSectorCapacity(e.target.value)}
                                    required
                                />
                            </div>
                            <button type="submit" className="cart-btn" disabled={savingSector}>
                                {savingSector ? "Agregando..." : "+ Agregar sector"}
                            </button>
                        </form>

                        {sectors.length > 0 && (
                            <div style={{ marginTop: "1.5rem" }}>
                                <h3 className="sector-title">Sectores agregados</h3>
                                <table className="checkout-table" style={{ marginTop: "0.75rem" }}>
                                    <thead>
                                        <tr>
                                            <th>Sector</th>
                                            <th>Precio</th>
                                            <th>Butacas</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {sectors.map((s) => (
                                            <tr key={s.id}>
                                                <td>{s.name}</td>
                                                <td>${parseFloat(s.price).toFixed(2)}</td>
                                                <td>{s.capacity ?? "—"}</td>
                                            </tr>
                                        ))}
                                    </tbody>
                                </table>
                            </div>
                        )}

                        <button
                            className="pay-btn checkout-pay-btn"
                            style={{ marginTop: "2rem" }}
                            onClick={onBack}
                            disabled={sectors.length === 0}
                        >
                            Finalizar
                        </button>
                        {sectors.length === 0 && (
                            <p style={{ color: "var(--text2)", fontSize: "0.8rem", textAlign: "center", marginTop: "0.5rem" }}>
                                Agregá al menos un sector para finalizar.
                            </p>
                        )}
                    </div>
                )}
            </main>
        </div>
    );
}
