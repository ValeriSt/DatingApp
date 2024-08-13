using System;

namespace API.Extensions;

public static class DateTimeExtensions
{
    public static int CalculateAge(this DateOnly dob)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var age = today.Year - dob.Year;
        //for birthdays that have not occurred yet, not working for leap years
        if (dob > today.AddYears(-age)) age--;
        return age;
    }
}
