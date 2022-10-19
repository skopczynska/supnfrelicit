using Accord.MachineLearning.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportingElicitation.Lib
{
    public static class ClassifierExtenstions
    {
        public static List<AssociationRule<int>> GetHigestRule(this AssociationRuleMatcher<int> ruleMatcher, List<int> elementsToGenererateFrom )
        {
            var rulesWithElements = GetRulesWithElements(ruleMatcher.Rules, elementsToGenererateFrom);
            
            var sortedRules = rulesWithElements.OrderBy(r => r.Support).OrderBy(rr => rr.Confidence);
      

            return GetTopRules(sortedRules);
        }

        private static List<AssociationRule<int>> GetTopRules(IOrderedEnumerable<AssociationRule<int>> sortedRules)
        {
            if (sortedRules.Count() == 0)
                return new List<AssociationRule<int>>();
            if (sortedRules.Count() == 1)
                return new List<AssociationRule<int>>() { sortedRules.FirstOrDefault() };
            var firstElement = sortedRules.First();
            return sortedRules.Where(s => s.Support == firstElement.Support && s.Confidence == firstElement.Confidence).ToList();

        }

        private static List<AssociationRule<int>> GetRulesWithElements(AssociationRule<int>[] rules, List<int> elementsToGenererateFrom)
        {
            return rules.Where(r => RuleContainsOnlyElements(r, elementsToGenererateFrom)).ToList();  
        }

        private static bool RuleContainsOnlyElements(AssociationRule<int> r, List<int> elementsToGenererateFrom)
        {
            foreach(var element in r.X)
                if (!elementsToGenererateFrom.Contains(element))
                    return false;
            return true; 
        }
    }
}
