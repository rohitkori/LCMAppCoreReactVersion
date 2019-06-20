using LCMAppCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCMAppCore.ViewModels
{
    public class LCMViewModel
    {
        public History History { get; set; }
        public List<AlgorithmType> AlgorithmTypes { get; set; }
    }
}
