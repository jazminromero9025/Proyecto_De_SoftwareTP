import { useState } from "react";
import LoginPage from "./pages/LoginPage";
import EventsPage from "./pages/EventsPage";
import SeatsPage from "./pages/SeatsPage";

export default function App() {
    const [user, setUser] = useState(null);
    const [selectedEvent, setSelectedEvent] = useState(null);

    if (!user) return <LoginPage onLogin={setUser} />;

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
                    onLogout={() => setUser(null)}
                />
            )}
        </div>
    );
}