using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeRegistration.Enums
{
    public enum Position
    {
        Boss = 0,
        Accountant = 1,
        Marketing = 2,
        ProjectManager = 3,
        Programmer = 4,
        Designer = 5,
        Tester = 6,
        Other = 7
    }

    public enum Status
    {
        New = 0,
        Approved = 1,
        InDevelopment = 2,
        Implemented = 3,
        InTest = 4,
        Solved = 5
    }

    public enum AbsenceReason
    {
        None = 0,
        Sickness = 1,
        DoctorTime = 2,
        VacationPayed = 3,
        VacationNotPayed = 4,
        Other = 5
    }
}