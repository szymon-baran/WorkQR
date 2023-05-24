using System.ComponentModel;

namespace WorkQR.Domain.Dictionaries
{
    public enum VacationType
    {
        [Description("Urlop")]
        AnnualLeave,
        [Description("Chorobowy")]
        SickLeave,
        [Description("Macierzyński")]
        MaternityLeave,
        [Description("Niestandardowy")]
        Other,
    }

}
