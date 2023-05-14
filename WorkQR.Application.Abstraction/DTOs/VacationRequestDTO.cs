﻿using WorkQR.Dictionaries;

namespace WorkQR.Application
{
    public class VacationRequestDTO
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = "";
        public bool IsApproved { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public VacationType VacationType { get; set; }
    }
}
