namespace gestionSite.API.DTOs.TerrainDtos
{
    public class AddTerrainDto
    {


        
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public int TerrainStatusId { get; set; }
        public int AdminId { get; set; }


    }
}

