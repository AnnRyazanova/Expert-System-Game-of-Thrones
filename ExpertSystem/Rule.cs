using System.Collections.Generic;

namespace ExpertSystem
{
    // Antecedents -> Consequent
    class Rule
    {
        public IList<string> Antecedents; 
        public string Consequent;
        public double K;

        public Rule(IList<string> antecedents, string consequent)
        {
            Antecedents = antecedents;
            Consequent = consequent;
        }

        public Rule(IList<string> antecedents, string consequent, double k)
        {
            Antecedents = antecedents;
            Consequent = consequent;
            K = k;
        }

        public override string ToString()
        {
            return string.Join(", ", Antecedents) + " -> " + Consequent;
        }
    }
}
