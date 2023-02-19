using System.ComponentModel.DataAnnotations;

namespace WorkQR.Dictionaries
{
    public enum EventType
    {
        [Display(Name = "Rozpoczęcie pracy")]
        StartWork,
        [Display(Name = "Zakończenie pracy")]
        EndWork,
        [Display(Name = "Rozpoczęcie przerwy")]
        StartBreak,
        [Display(Name = "Zakończenie przerwy")]
        EndBreak,
    }

}
