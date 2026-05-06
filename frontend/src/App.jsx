import { useState } from "react";
import LoginPage from "./pages/LoginPage";
import EventsPage from "./pages/EventsPage";
import SeatsPage from "./pages/SeatsPage";
import CheckoutPage from "./pages/CheckoutPage";

export default function App() {
    const [user, setUser] = useState(null);
    const [selectedEvent, setSelectedEvent] = useState(null);
    const [checkout, setCheckout] = useState(null);

    if (!user) return <LoginPage onLogin={setUser} />;

    if (checkout) {
        return (
            <CheckoutPage
                reservations={checkout}
                onBack={() => setCheckout(null)}
                onSuccess={() => { setCheckout(null); setSelectedEvent(null); }}
            />
        );
    }

    return (
        <div className="app">
            {!selectedEvent ? (
                <EventsPage
                    onSelectEvent={setSelectedEvent}
                    user={user}
                    onLogout={() => setUser(null)}
                />
            ) : (
                <SeatsPage
                    event={selectedEvent}
                    user={user}
                    onBack={() => setSelectedEvent(null)}
                    onCheckout={(reservations) => setCheckout(reservations)}
                />
            )}
        </div>
    );
}
