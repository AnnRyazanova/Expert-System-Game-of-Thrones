using System.Collections.Generic;

namespace ExpertSystem
{
    // Antecedents -> Consequent
    class Rule
    {
        public IList<string> Antecedents; 
        public string Consequent;

        public Rule(IList<string> antecedents, string consequent)
        {
            Antecedents = antecedents;
            Consequent = consequent;
        }

        public override string ToString()
        {
            return string.Join(", ", Antecedents) + " -> " + Consequent;
        }
    }
}
