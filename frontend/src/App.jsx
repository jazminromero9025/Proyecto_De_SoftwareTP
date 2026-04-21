import { useState } from "react";
import EventsPage from "./pages/EventsPage";
import SeatsPage from "./pages/SeatsPage";

export default function App() {
    const [selectedEvent, setSelectedEvent] = useState(null);

    return (
        <div className="app">
            {!selectedEvent ? (
                <EventsPage onSelectEvent={setSelectedEvent} />
            ) : (
                <SeatsPage event={selectedEvent} onBack={() => setSelectedEvent(null)} />
            )}
        </div>
    );
}