import { useState } from "react";
import LoginPage from "./pages/LoginPage";
import EventsPage from "./pages/EventsPage";
import SeatsPage from "./pages/SeatsPage";
import CheckoutPage from "./pages/CheckoutPage";
import AdminCreateEventPage from "./pages/AdminCreateEventPage";

export default function App() {
    const [user, setUser] = useState(null);
    const [selectedEvent, setSelectedEvent] = useState(null);
    const [checkout, setCheckout] = useState(null);
    const [seatsKey, setSeatsKey] = useState(0);
    const [creatingEvent, setCreatingEvent] = useState(false);

    if (!user) return <LoginPage onLogin={setUser} />;

    if (creatingEvent) {
        return <AdminCreateEventPage onBack={() => setCreatingEvent(false)} />;
    }

    if (checkout) {
        return (
            <CheckoutPage
                reservations={checkout}
                onBack={() => { setCheckout(null); setSeatsKey(k => k + 1); }}
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
                    onCreateEvent={() => setCreatingEvent(true)}
                />
            ) : (
                <SeatsPage
                    key={seatsKey}
                    event={selectedEvent}
                    user={user}
                    onBack={() => setSelectedEvent(null)}
                    onCheckout={(reservations) => setCheckout(reservations)}
                />
            )}
        </div>
    );
}
