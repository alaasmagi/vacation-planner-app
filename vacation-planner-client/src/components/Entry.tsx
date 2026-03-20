import { useNavigate } from "react-router-dom"
import { statusMeta } from "../models"
import { formatDate } from "../utils"
import type { VacationRequestDto } from "../types";

export default function Entry({request, index} : {request: VacationRequestDto, index: number}) {
  const status = statusMeta[request.status]
  const navigate = useNavigate();

  return (
    <div className="card border-secondary-subtle shadow-sm rounded-4">
        <div className="card-body p-3 p-md-4 d-flex justify-content-between align-items-start gap-3">
        <div className="text-start">
            <div className="d-flex flex-wrap align-items-center gap-2 mb-2">
            <span className="fw-semibold">Puhkusetaotlus #{index + 1}</span>
            <span className="text-secondary">
                {formatDate(request.startDate)} - {formatDate(request.endDate)}
            </span>
            </div>
            <p className="text-secondary mb-2">{request.comment || "—"}</p>
            <div className="d-flex align-items-center gap-2">
            <span className="badge text-bg-light border">
                {request.durationDays} päeva
            </span>
            <span className={`badge rounded-pill ${status.className}`}>
                {status.label}
            </span>
            </div>
        </div>
        <button className="btn btn-sm btn-outline-secondary" onClick={() => navigate(`/details/${request.id}`)}>
            Vaata
        </button>
        </div>
    </div>
  )
}
