using System.ComponentModel;

namespace WorkQR.Dictionaries
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
