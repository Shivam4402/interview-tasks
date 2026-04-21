using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JS_CRUD_SinglePage.Models;

public class Student
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string Gender { get; set; }

    public DateTime Dob { get; set; }

    public int StateId { get; set; }

    public int CityId { get; set; }

    public string Technologies { get; set; }

    public string ProfileImage { get; set; }
}

public class StudentDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(100)]
    public string Name { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email")]
    public string Email { get; set; }

    [Required]
    public string Gender { get; set; }

    [Required(ErrorMessage = "DOB is required")]
    public DateTime Dob { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Select State")]
    public int StateId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Select City")]
    public int CityId { get; set; }

    [Required(ErrorMessage = "Select at least one technology")]
    public string Technologies { get; set; }

    // ✅ IMPORTANT FOR FILE UPLOAD
    public IFormFile ProfileImage { get; set; }
}

public class StudentViewDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Gender { get; set; }
    public DateTime Dob { get; set; }
    public string Email { get; set; }
    public string Technologies { get; set; }
    public string ProfileImage { get; set; }

    public string StateName { get; set; }
    public string CityName { get; set; }
}