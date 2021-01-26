using System;
using System.Collections.Generic;
using System.Linq;

namespace PaintShop
{
    public class Solver
    {
        public List<List<CustomerPreference>> PrepareCustomerPreferences(List<List<CustomerPreference>> customerPreferences)
        {
            foreach (var item in customerPreferences)
            {
                item.Sort(SortColor);
                foreach (var item1 in customerPreferences)
                {
                    if (item1.Count == 1)
                    {
                        item1[0].IsReadOnly = true;
                    }
                }
            }
            return customerPreferences;
        }

        IEnumerable<CustomerPreference> GetCandidateSolution(int i, List<List<CustomerPreference>> customerPreferences)
        {
            List<CustomerPreference> customerPreferences1 = new List<CustomerPreference>();
            List<int> lengths = new List<int>();
            List<int> candidateSolutionIndexes = new List<int>();
            foreach (var item in customerPreferences)
            {
                lengths.Add(item.Count);
            }
            int x = i;

            lengths.Reverse();

            foreach (var length in lengths)
            {
                int index = x % length;
                decimal temp = x / length;
                x = (int)Math.Floor(temp);
                candidateSolutionIndexes.Add(index);
            }

            candidateSolutionIndexes.Reverse();
            for (int index = 0; index < candidateSolutionIndexes.Count; index++)
            {
                customerPreferences1.Add(customerPreferences[index][candidateSolutionIndexes[index]]);
            }

            return customerPreferences1;
        }

        public IEnumerable<IEnumerable<CustomerPreference>> CandidateSolutions(List<List<CustomerPreference>> customerPreferences)
        {
            int combinations = 1;

            foreach (var item in customerPreferences)
            {
                combinations = combinations * item.Count;
            }

            return Func(0, combinations, customerPreferences);
        }


        IEnumerable<IEnumerable<CustomerPreference>> Func(int start, int end, List<List<CustomerPreference>> customerPreferences)
        {
            for (int i = start; i < end; i++)
            {
                yield return GetCandidateSolution(i, customerPreferences);
            }
        }

        int SortColor(CustomerPreference a, CustomerPreference b)
        {
            if (a.Finish == "G" && b.Finish == "M")
            {
                return -1;
            }
            if (a.Finish == "M" && b.Finish == "G")
            {
                return 1;
            }

            return a.Color - b.Color;
        }

        public string Solve(ParsedFileObject parsedFileObject)
        {
            ValidationSolution validationSolution = new ValidationSolution();
            List<List<CustomerPreference>> customerPreferences = PrepareCustomerPreferences(parsedFileObject.CustomerPreferences);
            var candidates = CandidateSolutions(customerPreferences);

            foreach (var item in candidates)
            {
                string solution = validationSolution.ValidateAndGetSolution(parsedFileObject.NumberColors,
                    item.ToList(), customerPreferences);

                if (!string.IsNullOrEmpty(solution))
                    return solution;
            }
            return "No solution exists";
        }
    }
}
