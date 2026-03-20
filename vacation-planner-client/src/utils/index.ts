export function formatDate(date: string) {
  const [y, m, d] = date.split("-")
  return `${d}.${m}.${y}`
}

export function isOvertime(startDate: string, endDate: string) {
  const start = new Date(startDate)
  const end = new Date(endDate)
  const defaultVacationDuration = import.meta.env.VITE_DEFAULT_VACATION_DURATION_DAYS || 28

  const diffInMs = end.getTime() - start.getTime()
  const diffInDays = diffInMs / (1000 * 60 * 60 * 24)

  return diffInDays > defaultVacationDuration
}

export function isDatesOverLapping(startDate: string, endDate: string) {
  const start = new Date(startDate)
  const end = new Date(endDate)

  return start > end
}

export function calculateDurationDays(startDate: string, endDate: string) {
   return startDate && endDate ? Math.max(0, Math.ceil((new Date(endDate).getTime() 
   - new Date(startDate).getTime()) / (1000 * 60 * 60 * 24))) : 0
}
