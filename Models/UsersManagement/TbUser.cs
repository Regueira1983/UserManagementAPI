using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models.UsersManagement;

public partial class TbUser
{
    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Email { get; set; }

    public DateOnly DateOfBirth { get; set; }

    public DateOnly? RetirementDate { get; set; }

    public TbUser() { }

    public TbUser(string FirstName, string LastName, string? Email, DateOnly DateOfBirth)
    {
        this.FirstName = FirstName;
        this.LastName = LastName;
        this.Email = Email;
        this.DateOfBirth = DateOfBirth;
    }

    public TbUser(int UserId,string FirstName, string LastName, string? Email, DateOnly DateOfBirth)
    {
        this.UserId = UserId;
        this.FirstName = FirstName;
        this.LastName = LastName;
        this.Email = Email;
        this.DateOfBirth = DateOfBirth;
    }
}
