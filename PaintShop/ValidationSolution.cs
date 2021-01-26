using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaintShop
{
    public class ValidationSolution
    {
        bool IsSatisfyingAllPreferences(string[] result, IEnumerable<IEnumerable<CustomerPreference>> customerPreferences)
        {
            return customerPreferences.All(preferences =>
            {
                return preferences.Any(x => result[x.Color - 1] == x.Finish);
            });
        }

        public string ValidateAndGetSolution(int numColors, List<CustomerPreference> candidate,
             IEnumerable<IEnumerable<CustomerPreference>> customerPreferences)
        {
            // colors is the working array, is created with numColors cells initialized to null
            var colors = new CustomerPreference[numColors];
            for (var i = 0; i < numColors; i++)
                colors[i] = null;

            bool found = false;
            for (var i = 0; i < candidate.Count; i++)
            {
                var preference = candidate[i];
                if (
                    colors[preference.Color - 1] != null &&
                    (colors[preference.Color - 1].IsReadOnly || preference.IsReadOnly) &&
                    colors[preference.Color - 1].Finish != preference.Finish)
                {
                    found = false; // stop iteration
                    break;
                }

                colors[preference.Color - 1] = preference; 
                found = true;
            }

            if (!found)
            {
                return string.Empty;
            }

            var result = new string[numColors];

            for (int i = 0; i < colors.Length; i++)
            {
                var c = colors[i];

                if (c != null)
                {
                    result[i] = c.Finish;
                }
                else
                    result[i] = "G";
            }

            // verifies that the current solution is satisfying all customers
            if (!IsSatisfyingAllPreferences(result, customerPreferences))
            {
                return string.Empty;
            }

            return string.Join(" ", result);
        }
    }
}
