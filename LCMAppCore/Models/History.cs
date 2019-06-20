using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LCMAppCore.Models
{
    public class History
    {
        public int Id { get; set; }

        public AlgorithmType AlgorithmType { get; set; }
        [Display(Name = "Algorithm:")]
        [Required(ErrorMessage = "The Algorithm is required.")]
        public int AlgorithmTypeId { get; set; }

        [DisplayName("Inputs:")]
        [RegularExpression(@"^[0-9]+(,[0-9]+)*$", ErrorMessage = "Only comma separated numbers are allowed.")]
        [Required(ErrorMessage = "The Input(s) is/are required.")]
        [DataType(DataType.MultilineText)]
        public string Inputs { get; set; }

        [DisplayName("Time:")]
        public string TimeComplexity { get; set; }

        [DisplayName("Space:")]
        public string SpaceComplexity { get; set; }

        [DisplayName("Result:")]
        public string Result { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser ApplicationUser { get; set; }
        public string UserId { get; set; }
    }
}
