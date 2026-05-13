import { useEffect, useState } from "react";

export default function CountdownTimer({ expiresAt }) {
    const [remaining, setRemaining] = useState(() =>
        Math.max(0, new Date(expiresAt) - new Date())
    );

    useEffect(() => {
        const tick = () => setRemaining(Math.max(0, new Date(expiresAt) - new Date()));
        tick();
        const interval = setInterval(tick, 1000);
        return () => clearInterval(interval);
    }, [expiresAt]);

    const expired = remaining === 0;
    const minutes = Math.floor(remaining / 60000);
    const seconds = Math.floor((remaining % 60000) / 1000);
    const urgent = !expired && remaining < 60000;

    return (
        <div className="countdown">
            <span className="countdown-label">⏱ Tiempo restante</span>
            <span className={`countdown-time ${expired ? "expired" : urgent ? "urgent" : ""}`}>
                {`${String(minutes).padStart(2, "0")}:${String(seconds).padStart(2, "0")}`}
            </span>
            {expired && <p className="countdown-expired">Reserva vencida</p>}
        </div>
    );
}
