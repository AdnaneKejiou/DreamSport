﻿namespace gestionEmployer.Core.Models
{
    public class Admin : Personne
    {
        public int Id { get; set; }
        public string Login {  get; set; }
        public string? Email { get; set; }
    }
}
