import { useEffect, useState } from "react"
import Entry from "../components/Entry"
import { fetchVacationRequests } from "../api";
import { useNavigate } from "react-router-dom";
import type { VacationRequestDto } from "../types";
import { Loading } from "../components/Loading";

export default function Home() {
    const navigate = useNavigate();
    const [loading, setLoading] = useState(true)
    const [vacationRequests, setVacationRequests] = useState<VacationRequestDto[]>([]);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const loadVacationRequests = async () => {
            setLoading(true);
            const response = await fetchVacationRequests();
            if (!response.success) {
                setError(response.error.message);
            } else {
                setVacationRequests(response.data);
            }
            setLoading(false);
        };

        loadVacationRequests();
    },[]);

    if (loading) {
       return Loading();
    }
        
    return (
        <div className="container py-5">
            <div className="card border-secondary-subtle shadow-sm rounded-4">
                <div className="card-body p-4 p-md-5">
                    <div className="d-flex flex-wrap justify-content-between align-items-center gap-3 mb-4">
                        <div className="text-start">
                            <h1 className="h3 mb-1 fw-semibold">Sinu taotlused</h1>
                            <p className="text-secondary mb-0">Kõik sinu esitatud puhkusesoovid</p>
                        </div>
                        <button className="btn btn-primary" onClick={() => navigate("/edit/new")}>
                            Lisa uus
                        </button>
                    </div>
                    <div className="d-flex flex-column gap-3 w-100">
                        {error && <div className="alert alert-danger">{error}</div>}
                        {vacationRequests.map((request, index) => (
                            <Entry key={request.id} request={request} index={index} />
                        ))}
                        {vacationRequests.length === 0 && (
                            <p className="text-secondary">Taotlusi pole veel</p>
                        )}
                    </div>
                </div>
            </div>
        </div>
    )
}
