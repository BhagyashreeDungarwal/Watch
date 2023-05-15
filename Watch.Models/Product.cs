using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Watch.Models
{
    public class Product
    {
        public string ImageUrl;

        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Descrptioin { get; set; }
        [Required]
        public string ISBN { get; set; }

        [Required]
        public string Brand { get; set; }
        [Required]

        [Display(Name = "List Price")]
        [Range(1, 10000)]
        public double ListPrice { get; set; }
        [Required]

        [Display(Name = "Price between 1 to 50")]
        [Range(1, 10000)]
        public double Price { get; set; }
        [Required]

        [Display(Name = "Price between 51 to 100")]
        [Range(1, 10000)]
        public double Price50 { get; set; }

        [Required]

        [Display(Name = "Price for more than 100")]
        [Range(1, 10000)]
        public double Price100 { get; set; }

        [Required]
        [ValidateNever]
        public string ImageURL { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }

        [Required]
        [Display(Name = "Cover Type")]
        public int CoverTypeId { get; set; }
        [ValidateNever]
        public CoverType CoverType { get; set; }
    }
}
