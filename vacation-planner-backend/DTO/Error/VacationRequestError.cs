namespace DTO.Error;

public static class VacationRequestErrors
{
    public static class Codes
    {
        public const string DuplicateEntry = "DUPLICATE_ENTRY";
    }

    public static class Messages
    {
        public const string DuplicateEntry = "A vacation request with the same details already exists.";
    }
}