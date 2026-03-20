import { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { createVacationRequest, fetchVacationRequestById, updateVacationRequest } from "../api";
import { VacationRequestForm } from "../components/VacationRequestForm";
import type { VacationRequestDto } from "../types";
import { Loading } from "../components/Loading";
import { getErrorMessageEt } from "../utils/ui";

export default function Edit() {
  const { id } = useParams();
  const navigate = useNavigate();
  const isEdit = !!id;

  const [initial, setInitial] = useState<VacationRequestDto | null>(null);
  const [loading, setLoading] = useState(isEdit);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (!isEdit) return;

    const load = async () => {
      try {
        const response = await fetchVacationRequestById(id!);
        if (!response.success) {
          setError(getErrorMessageEt(response.error.code));
        } else {
          setInitial(response.data);
        }
        setLoading(false);
      } catch (e: any) {
        setError(e.message);
        setLoading(false);
      }
    };

    load();
  }, [id, isEdit]);

  const handleSubmit = async (data: any): Promise<void> => {
    const response = isEdit ? await updateVacationRequest(id, data) : await createVacationRequest(data);

    if (!response.success) {
      setError(getErrorMessageEt(response.error.code));
      return;
    }

    navigate("/");
  };

  if (loading) {
    return Loading();
  }
  
  return (
    <div className="container py-5">
      <div className="d-flex flex-wrap justify-content-between align-items-center gap-3 mb-3">
        <div className="text-start">
          <h1 className="h3 mb-1 fw-semibold">
            {isEdit ? "Muuda puhkusesoovi" : "Uus puhkusesoov"}
          </h1>
          <p className="text-secondary mb-0">
            {isEdit ? "Täienda olemasolevat taotlust." : "Sisesta uus puhkusesoov."}
          </p>
        </div>
        <button type="button" className="btn btn-outline-secondary btn-sm" onClick={() => navigate(-1)}>
          Tagasi
        </button>
      </div>

      {error && <div className="alert alert-danger">{error}</div>}
      <VacationRequestForm initial={initial} onSubmit={handleSubmit} />
    </div>
  );
}
