export const ERROR_CODE_MESSAGES_ET: Record<string, string> = {
  NOT_FOUND: "Andmeid ei leitud",
  UPDATING_FAILED: "Andmete uuendamine ebaõnnestus",
  REMOVING_FAILED: "Andmete eemaldamine ebaõnnestus",
  MAPPING_FAILED: "Andmete päring ebaõnnestus",
  DUPLICATE_ENTRY: "Valitud perioodiks on taotlus juba esitatud",
}

export function getErrorMessageEt(code?: string, fallback?: string) {
  if (code && ERROR_CODE_MESSAGES_ET[code]) {
    return ERROR_CODE_MESSAGES_ET[code]
  }
  return fallback || "Midagi läks valesti"
}
