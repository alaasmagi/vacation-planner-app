import axios from "axios"
import type { ApiErrorResponse, ApiResponse, VacationRequestDto } from "../types"

const BASE_URL = import.meta.env.VITE_API_URL || ""
const api = axios.create({ baseURL: BASE_URL })

async function request<T>(path: string, method: string, data?: unknown): Promise<ApiResponse<T>> {
  const headers: Record<string, string> = {}
  if (data) {
    headers["Content-Type"] = "application/json"
  }

  try {
    const response = await api.request<T>({url: path, method, headers, data})
    return { success: true, data: response.data as T }
  } catch (err: any) {
    const response = err && err.response ? err.response : null
    const body = response ? response.data : null
    const status = response ? response.status : null

    if (body && typeof body.code === "string" && typeof body.message === "string") {
      return { success: false, error: body as ApiErrorResponse }
    }

    const error: ApiErrorResponse = {
      code: status,
      message: status,
    }
    return { success: false, error }
  }
}

export async function fetchVacationRequests(): Promise<ApiResponse<VacationRequestDto[]>> {
  return request<VacationRequestDto[]>("/VacationRequest", "GET")
}

export async function fetchVacationRequestById(id: string): Promise<ApiResponse<VacationRequestDto>> {
  return request<VacationRequestDto>(`/VacationRequest/${id}`, "GET")
}

export async function createVacationRequest(data: VacationRequestDto): Promise<ApiResponse<VacationRequestDto>> {
  return request<VacationRequestDto>("/VacationRequest", "POST", data)
}

export async function updateVacationRequest(id: string, data: VacationRequestDto): Promise<ApiResponse<void>> {
  return request<void>(`/VacationRequest/${id}`, "PUT", data)
}

export async function deleteVacationRequest(id: string): Promise<ApiResponse<void>> {
  return request<void>(`/VacationRequest/${id}`, "DELETE")
}
