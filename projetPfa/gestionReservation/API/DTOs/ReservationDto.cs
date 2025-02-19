namespace gestionReservation.API.DTOs
{
    public class ReservationDto
    {
        public DateTime DateRes { get; set; }
        public int IdUtilisateur { get; set; }
        public int IdTerrain { get; set; }
        public int IdEmploye { get; set; }
        public int IdAdmin { get; set; }
        public int IdStatus { get; set; }
    }
}
