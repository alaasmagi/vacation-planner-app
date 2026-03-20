import { useMemo, useState } from "react";
import { calculateDurationDays, isDatesOverLapping, isOvertime } from "../utils";

interface FormData {
  id?: string;
  startDate: string;
  endDate: string;
  comment?: string;
  employeeId: string;
}

interface Props {
  initial?: FormData | null;
  onSubmit: (data: FormData) => Promise<void>;
}

export function VacationRequestForm({ initial, onSubmit }: Props) {
  const EMPLOYEE_ID = import.meta.env.VITE_EMPLOYEE_ID ?? ""
  
  const [startDate, setStartDate] = useState(initial?.startDate ?? "");
  const [endDate, setEndDate] = useState(initial?.endDate ?? "");
  const [comment, setComment] = useState(initial?.comment ?? "");
  const [submitting, setSubmitting] = useState(false);
  
  const durationDays = calculateDurationDays(startDate, endDate);
  const minDate = useMemo(() => new Date().toISOString().slice(0, 10), []);

  const hasOverlap = useMemo(() => {
    if (!startDate || !endDate) return false;
    return isDatesOverLapping(startDate, endDate);
  }, [endDate, startDate]);

  const isOverTime = useMemo(() => {
    if (!startDate || !endDate) return false;
    return isOvertime(startDate, endDate);
  }, [endDate, startDate]);

  const isInvalidLength = useMemo(() => {
    if (!startDate || !endDate) return false;
    return durationDays <= 0;
  }, [endDate, startDate]);


  const handleSubmit = async (e: React.SubmitEvent) => {
    e.preventDefault();
    if (hasOverlap || isInvalidLength) {
      return;
    }
    setSubmitting(true);
    try {
      await onSubmit({ id: initial?.id, startDate, endDate, comment, employeeId: EMPLOYEE_ID });
    } finally {
      setSubmitting(false);
    }
  };

return (
    <div className="card border-secondary-subtle shadow-sm rounded-4">
      <div className="card-body p-4 p-md-5">
        <div className="mb-4">
          <h2 className="h4 mb-1 fw-semibold">Puhkusesoovi vorm</h2>
          <p className="text-secondary mb-0">
            Täida andmed ja salvesta muudatused.
          </p>
        </div>
        <form onSubmit={handleSubmit}>
          <div className="row g-4">
            <div className="col-12 col-md-6">
              <label className="form-label fw-medium">Algus</label>
              <input
                type="date"
                className="form-control"
                value={startDate}
                onChange={(e) => setStartDate(e.target.value)}
                min={minDate}
                required
              />
            </div>
            <div className="col-12 col-md-6">
              <label className="form-label fw-medium">Lõpp</label>
              <input
                type="date"
                className="form-control"
                value={endDate}
                onChange={(e) => setEndDate(e.target.value)}
                min={startDate + 1}
                required
              />
            </div>
            {(hasOverlap || isOverTime || isInvalidLength) && (
              <div className="col-12">
                {hasOverlap && (
                  <div className="alert alert-danger mb-2">
                    Alguskuupäev ei tohi olla hiljem kui lõppkuupäev
                  </div>
                )}
                {isOverTime && (
                  <div className="alert alert-info mb-2">
                    Puhkus ületab seadusandluses määratletud puhkuse kestuse limiidi, 
                    kuid taotluse esitamine on siiski võimalik
                  </div>
                )}
                {isInvalidLength && (
                  <div className="alert alert-danger mb-2">
                    Puhkuse kestus peab olema vähemalt 1 päev
                  </div>
                )}
              </div>
            )}
            <div className="col-12">
              <div className="bg-body-tertiary border border-secondary-subtle rounded-4 p-3 p-md-4">
                <div className="d-flex justify-content-between align-items-center flex-wrap gap-2">
                  <span className="text-secondary">Kestus</span>
                  <span className="fw-semibold fs-5">
                    {durationDays} {durationDays === 1 ? "päev" : "päeva"}
                  </span>
                </div>
              </div>
            </div>
            <div className="col-12">
              <label className="form-label fw-medium">Kommentaar</label>
              <textarea
                className="form-control"
                rows={4}
                value={comment}
                onChange={(e) => setComment(e.target.value)}
                placeholder="Kirjuta siia lisainfo või põhjendus..."
              />
            </div>
          </div>
          <div className="d-flex flex-column flex-sm-row gap-2 justify-content-end mt-4 pt-3 border-top">
            <button
              type="submit"
              className="btn btn-primary"
              disabled={submitting || hasOverlap || isInvalidLength}
            >
              {submitting ? "Salvestan..." : "Salvesta"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
