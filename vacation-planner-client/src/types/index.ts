export const EVacationStatus = {
  Pending: 0,
  Approved: 1,
  Rejected: 2,
} as const

export type EVacationStatus =
  (typeof EVacationStatus)[keyof typeof EVacationStatus]

export type DateOnly = string

export interface VacationRequestDto {
  id?: string
  employeeId: string
  startDate: DateOnly
  endDate: DateOnly
  comment?: string
  status: EVacationStatus
  durationDays?: number
  isOverTime?: boolean
}

export type ApiResponse<T> =
  | { success: true; data: T }
  | { success: false; error: ApiErrorResponse }

export type ApiErrorResponse = {
  code: string
  message: string
}