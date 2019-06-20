using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LCMAppCore.Data;
using LCMAppCore.Models;
using LCMAppCore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace LCMAppCore.Controllers
{
    [Authorize]
    public class LCMController : Controller
    {
        private ApplicationDbContext _context;

        public LCMController(ApplicationDbContext context)
        {
            _context = context;
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public IActionResult Index()
        {
            var vm = new LCMViewModel()
            {
                History = new History(),
                AlgorithmTypes = _context.AlgorithmTypes.ToList()
            };

            return View("Index", vm);
        }

        public IActionResult Calculate()
        {
            var vm = new LCMViewModel()
            {
                History = new History(),
                AlgorithmTypes = _context.AlgorithmTypes.ToList()
            };

            return View("Index", vm);
        }

        [Route("lcm/calculate")]
        [HttpPost]
        public IActionResult Calculate(History obj)
        {
            if (!ModelState.IsValid)
            {
                return Json(new History());
            }


            int[] arr = Array.ConvertAll(obj.Inputs.Split(','), a => Convert.ToInt32(a));

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            long beforeAlloc = GC.GetTotalMemory(false);

            long result = 1;

            if (obj.AlgorithmTypeId == 1)
            {
                result = lcmUsingBestTimeComplexity(arr, arr.Length);
            }
            else if (obj.AlgorithmTypeId == 2)
            {
                result = lcmUsingBestSpaceComplexity(arr);
            }
            else
            {
                result = lcmUsingOptimalComplexity(arr);
            }


            long afterAlloc = GC.GetTotalMemory(false);


            watch.Stop();

            var history = new History()
            {
                Inputs = obj.Inputs,
                AlgorithmTypeId = obj.AlgorithmTypeId,

                //need to calculate
                TimeComplexity = Convert.ToString(watch.ElapsedMilliseconds) + " ms",
                SpaceComplexity = Convert.ToString(afterAlloc - beforeAlloc) + " B",

                Result = Convert.ToString(result),
                UserId = userId

            };



            _context.Histories.Add(history);



            var vm1 = new LCMViewModel()
            {
                AlgorithmTypes = _context.AlgorithmTypes.ToList(),
                History = history
            };
            _context.SaveChanges();



            return Json(history);
            //return View("Index", vm1);
        }

        public JsonResult GetHistories()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var histories = _context.Histories.Include(m => m.AlgorithmType).Where(m => m.UserId == userId);

            return Json(histories.ToList());
        }

        public static long lcmUsingBestSpaceComplexity(int[] arr, int start_index = 0)
        {
            long lcm_of_array_elements = 1;
            int divisor = 2;

            while (true)
            {

                int counter = 0;
                bool divisible = false;
                for (int i = start_index; i < arr.Length; i++)
                {
                    if (arr[i] == 0)
                    {
                        return 0;
                    }
                    else if (arr[i] < 0)
                    {
                        arr[i] = arr[i] * (-1);
                    }
                    if (arr[i] == 1)
                    {
                        counter++;
                    }

                    if (arr[i] % divisor == 0)
                    {
                        divisible = true;
                        arr[i] = arr[i] / divisor;
                    }
                }

                if (divisible)
                {
                    lcm_of_array_elements = lcm_of_array_elements * divisor;
                }
                else
                {
                    divisor++;
                }

                if (counter == arr.Length - start_index)
                {
                    return lcm_of_array_elements;
                }
            }
        }

        public static long lcmUsingBestTimeComplexity(int[] arr, int n)
        {
            // Find the maximum value in arr[]  
            int max_num = 0;
            for (int i = 0; i < n; i++)
            {
                if (max_num < arr[i])
                {
                    max_num = arr[i];
                }
            }

            // Initialize result  
            long res = 1;

            // Find all factors that are present  
            // in two or more array elements.  
            int x = 2; // Current factor.  
            while (x <= max_num)
            {
                // To store indexes of all array  
                // elements that are divisible by x.  
                ArrayList indexes = new ArrayList();
                for (int j = 0; j < n; j++)
                {
                    if (arr[j] % x == 0)
                    {
                        indexes.Add(j);
                    }
                }

                // If there are 2 or more array elements  
                // that are divisible by x.  
                if (indexes.Count >= 2)
                {
                    // Reduce all array elements divisible  
                    // by x.  
                    for (int j = 0; j < indexes.Count; j++)
                    {
                        arr[(int)indexes[j]] = arr[(int)indexes[j]] / x;
                    }

                    res = res * x;
                }
                else
                {
                    x++;
                }
            }

            // Then multiply all reduced  
            // array elements  
            for (int i = 0; i < n; i++)
            {
                res = res * arr[i];
            }

            return res;
        }

        public static long lcmUsingOptimalComplexity(int[] arr)
        {
            if (arr.Length > 1)
            {
                int start_index = arr.Length / 2;

                long result1 = lcmUsingBestTimeComplexity(arr, start_index);
                long result2 = lcmUsingBestSpaceComplexity(arr, start_index);
                return lcmOfTwoNumbers(result1, result2);
            }
            return arr[0];
        }

        public static long gcd(long a, long b)
        {

            // Everything divides 0  
            if (a == 0 || b == 0)
                return 0;

            // base case 
            if (a == b)
                return a;

            // a is greater 
            if (a > b)
                return gcd(a - b, b);
            return gcd(a, b - a);
        }

        public static long lcmOfTwoNumbers(long a, long b)
        {
            return gcd(a, b) != 0 ? (a * b) / gcd(a, b) : 0;
        }
    }
}