namespace gestionSite.Core.Models
{
    public class Terrain
    {
        public int Id {  get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Image {  get; set; }
        public TerrainStatus? _TerrainStatus { get; set; }
        public int IdAdmin { get; set; }
    }
}
