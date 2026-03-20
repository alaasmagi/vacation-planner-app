import { useNavigate, useParams } from "react-router-dom"
import { statusMeta } from "../models"
import { useEffect, useState } from "react"
import { deleteVacationRequest, fetchVacationRequestById } from "../api"
import { type VacationRequestDto } from "../types"
import { Loading } from "../components/Loading"
import { getErrorMessageEt } from "../utils/ui"

function formatDate(date: string) {
  const [y, m, d] = date.split("-")
  return `${d}.${m}.${y}`
}

export default function Details() {
  const navigate = useNavigate()
  const { id } = useParams()
  const [vacationRequest, setVacationRequest] = useState<VacationRequestDto | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

    useEffect(() => {
      const loadVacationRequest = async () => {
        setLoading(true);
        const response = await fetchVacationRequestById(id!);
          if (!response.success) {
              setError(getErrorMessageEt(response.error.code));
          } else {
              setVacationRequest(response.data);
          }
          setLoading(false);
        }

      loadVacationRequest();
    }, [id]);

  const handleDelete = async () => {
    if (!vacationRequest) return

    const confirmed = window.confirm("Kas oled kindel, et soovid taotluse tühistada?")
    if (!confirmed) return

    setError(null)
    const response = await deleteVacationRequest(vacationRequest.id!)
    if (!response.success) {
      setError(getErrorMessageEt(response.error.code))
      return
    }

    navigate("/")
  }

  if (loading) {
    return (
      <div className="d-flex justify-content-center align-items-center min-vh-100">
        <div className="text-center">
          <div className="spinner-border text-secondary" role="status" />
          <div className="text-secondary mt-2">Laadimine...</div>
        </div>
      </div>
    )
  }

  if (!vacationRequest) {
    return (
      <div className="container py-5">
        <div className="alert alert-warning">Taotlust ei leitud</div>
        <button className="btn btn-outline-secondary btn-sm" onClick={() => navigate(-1)}>
          Tagasi
        </button>
      </div>
    )
  }

  const status = statusMeta[vacationRequest.status]
    
  if (loading) {
    return Loading();
  }

  return (
    <div className="container py-5">
      <div className="card border-secondary-subtle shadow-sm rounded-4">
        <div className="card-body p-4 p-md-5">
          <div className="d-flex justify-content-between align-items-start gap-3 mb-4">
            <div className="text-start">
              <div className="d-flex flex-wrap align-items-center gap-2 mb-2">
                <h1 className="h3 mb-0 fw-semibold">Taotluse sisu</h1>
                <span className={`badge rounded-pill ${status.className}`}>
                  {status.label}
                </span>
              </div>
              <p className="text-secondary mb-0">
                Puhkuse taotluse üksikasjad
              </p>
            </div>
            <button
              className="btn btn-outline-secondary btn-sm"
              onClick={() => navigate(-1)}
            >
              Tagasi
            </button>
          </div>
          {error && <div className="alert alert-danger">{error}</div>}
          <div className="row g-3 mb-4">
            <div className="col-12 col-md-3">
              <div className="bg-body-tertiary border border-secondary-subtle rounded-4 p-3 h-100">
                <div className="text-secondary small mb-1">Algus</div>
                <div className="fw-semibold">{formatDate(vacationRequest.startDate)}</div>
              </div>
            </div>
            <div className="col-12 col-md-3">
              <div className="bg-body-tertiary border border-secondary-subtle rounded-4 p-3 h-100">
                <div className="text-secondary small mb-1">Lõpp</div>
                <div className="fw-semibold">{formatDate(vacationRequest.endDate)}</div>
              </div>
            </div>
            <div className="col-12 col-md-3">
              <div className="bg-body-tertiary border border-secondary-subtle rounded-4 p-3 h-100">
                <div className="text-secondary small mb-1">Kestus</div>
                <div className="fw-semibold">
                  {vacationRequest.durationDays} {vacationRequest.durationDays === 1 ? "päev" : "päeva"}
                </div>
              </div>
            </div>
            <div className="col-12 col-md-3">
              <div className="bg-body-tertiary border border-secondary-subtle rounded-4 p-3 h-100">
                <div className="text-secondary small mb-1">Tüüp</div>
                <div className="fw-semibold">
                  {vacationRequest.isOverTime ? "Ületunnid" : "Puhkus"}
                </div>
              </div>
            </div>
          </div>
          <div className="bg-body-tertiary border border-secondary-subtle rounded-4 p-3 p-md-4 mb-4">
            <div className="text-secondary small mb-2">Kommentaar</div>
            <div className="fs-6">{vacationRequest.comment || "—"}</div>
          </div>
          <div className="d-flex flex-wrap gap-2">
            <button className="btn btn-danger" onClick={handleDelete}>
              Tühista taotlus
            </button>
            <button className="btn btn-outline-secondary" onClick={() => navigate(`/edit/${vacationRequest.id}`)}>
              Muuda
            </button>
          </div>
        </div>
      </div>
    </div>
  )
}
