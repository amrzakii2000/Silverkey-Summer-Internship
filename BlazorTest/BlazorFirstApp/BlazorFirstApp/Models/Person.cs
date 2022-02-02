using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorFirstApp.Models
{
    public class Person
    {
        [Required]
        public string Name { get; set; }

        [Range(18, 90)]
        public int Age { get; set; }

        public string Gender { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public Person()
        {

        }

        public Person(string name, int age, string gender, string? email)
        {
            Name = name;
            Age = age;
            Gender = gender;
            Email = email ?? "Hahahah I'm not giving you my email";
        }

        public override string ToString()
        {
            return $"I'm {Name}. I'm {Age} years old.\n Contact me at {Email}";
        }
    }
}
