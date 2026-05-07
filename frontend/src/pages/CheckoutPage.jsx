import { useState } from "react";
import CountdownTimer from "../components/CountdownTimer";

const API = "https://localhost:7016";

export default function CheckoutPage({ reservations, onBack, onSuccess }) {
    const [paying, setPaying] = useState(false);
    const [error, setError] = useState(null);
    const [success, setSuccess] = useState(false);

    const total = reservations.reduce((sum, r) => sum + r.price, 0);
    const earliestExpiry = reservations.reduce(
        (min, r) => new Date(r.expiresAt) < new Date(min) ? r.expiresAt : min,
        reservations[0]?.expiresAt
    );

    const handleConfirm = async () => {
        setPaying(true);
        setError(null);

        try {
            const res = await fetch(`${API}/api/v1/reservations/confirm-payment-bulk`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ reservationIds: reservations.map((r) => r.reservationId) }),
            });

            if (res.ok) {
                setSuccess(true);
            } else {
                const body = await res.json();
                setError(body.message || "No se pudo procesar el pago.");
            }
        } catch {
            setError("Error de conexión con el servidor.");
        } finally {
            setPaying(false);
        }
    };

    if (success) {
        return (
            <div className="page">
                <header className="header">
                    <div className="header-inner">
                        <span className="logo">🎟 TicketApp</span>
                    </div>
                </header>
                <main className="main checkout-main">
                    <div className="success-box">
                        <div className="success-icon">🎉</div>
                        <h2 className="success-title">¡Compra realizada con éxito!</h2>
                        <p className="success-sub">
                            Compraste {reservations.length} {reservations.length === 1 ? "entrada" : "entradas"} por un total de <strong>${total.toFixed(2)}</strong>.
                        </p>
                        <button className="pay-btn" onClick={onSuccess}>Volver a eventos</button>
                    </div>
                </main>
            </div>
        );
    }

    return (
        <div className="page">
            <header className="header">
                <div className="header-inner">
                    <button className="back-btn" onClick={onBack}>← Volver</button>
                    <span className="logo">🎟 TicketApp</span>
                    <h1 className="title" style={{ marginLeft: "1rem" }}>Confirmación de compra</h1>
                </div>
            </header>

            <main className="main checkout-main">
                <div className="checkout-card">
                    <h2 className="checkout-heading">Resumen de tu compra</h2>

                    <table className="checkout-table">
                        <thead>
                            <tr>
                                <th>Sector</th>
                                <th>Butaca</th>
                                <th>Precio</th>
                            </tr>
                        </thead>
                        <tbody>
                            {reservations.map((r) => (
                                <tr key={r.reservationId}>
                                    <td>{r.sectorName}</td>
                                    <td>{r.seatNumber}</td>
                                    <td>${r.price.toFixed(2)}</td>
                                </tr>
                            ))}
                        </tbody>
                        <tfoot>
                            <tr className="checkout-total-row">
                                <td colSpan={2}>Total</td>
                                <td>${total.toFixed(2)}</td>
                            </tr>
                        </tfoot>
                    </table>

                    <CountdownTimer expiresAt={earliestExpiry} />

                    {error && <div className="toast error" style={{ marginTop: "1rem" }}>{error}</div>}

                    <button
                        className="pay-btn checkout-pay-btn"
                        onClick={handleConfirm}
                        disabled={paying}
                    >
                        {paying ? "Procesando pago..." : "Confirmar Pago"}
                    </button>
                </div>
            </main>
        </div>
    );
}
