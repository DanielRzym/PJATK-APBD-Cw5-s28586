namespace PJATK_APBD_Cw5_s28586.DTOs
{
    public class BedAssignmentRequestDto
    {
        public DateTime From { get; set; }
        public DateTime? To { get; set; }
        public string BedType { get; set; } = string.Empty;
        public string Ward { get; set; } = string.Empty;
    }
}