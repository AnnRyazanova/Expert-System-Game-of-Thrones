using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ExpertSystem
{
    public partial class Form1 : Form
    {
        private ISet<string> facts;
        private ISet<string> sellectedFacts = new HashSet<string>();
        private IList<Rule> rules;

        public Form1()
        {
            InitializeComponent();
        }

        private static IList<Rule> Parse(string[] text)
        {
            var rules = new List<Rule>(text.Length);
            foreach (var line in text)
            {
                int arrowIndex = line.IndexOf("->");
                var left = line.Substring(0, arrowIndex).Split(',').Select(s => s.Trim()).ToList();
                var right = line.Substring(arrowIndex + 2).Trim();
                rules.Add(new Rule(left, right));
            }
            return rules;
        }

        private void OpenFile(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (DialogResult.OK != dialog.ShowDialog())
                return;
            rules = Parse(File.ReadAllLines(dialog.FileName));
            facts = new HashSet<string>();
            foreach (var rule in rules)
            {
                foreach (var antecedent in rule.Antecedents)
                    if (Char.IsLower(antecedent[0]))
                        facts.Add(antecedent);

                if (Char.IsLower(rule.Consequent[0]))
                    facts.Add(rule.Consequent);
            }

            var factsList = facts.ToList();
            factsList.Sort();
            listBox1.DataSource = factsList;
            listBox2.DataSource = rules;
            comboBox1.DataSource = factsList;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (null == listBox1.SelectedItem)
                return;
            var fact = (string)listBox1.SelectedItem;
            facts.Remove(fact);
            sellectedFacts.Add(fact);
            var factsList = facts.ToList();
            factsList.Sort();
            listBox1.DataSource = factsList;
            listBox3.DataSource = sellectedFacts.ToList();
            comboBox1.DataSource = factsList;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (null == listBox3.SelectedItem)
                return;
            var fact = (string)listBox3.SelectedItem;
            sellectedFacts.Remove(fact);
            facts.Add(fact);
            var factsList = facts.ToList();
            factsList.Sort();
            listBox1.DataSource = factsList;
            listBox3.DataSource = sellectedFacts.ToList();
            comboBox1.DataSource = factsList;
        }


        private class Node : IEquatable<Node>
        {
            

            public ISet<string> Set { get; set; }
            public Node Parent { get; set; }
            public Rule Rule { get; set; }

            public Node(ISet<string> set, Node parent, Rule rule)
            {
                Set = set;
                Parent = parent;
                Rule = rule;
            }

            public override bool Equals(object obj)
            {
                if (null == obj || !(obj is Node))
                    return false;
                var node = (Node)obj;
                return Equals(node);
            }

            public override int GetHashCode()
            {
                if (Set == null)
                    return 0;

                int hash = 0;

                foreach (var s in Set)
                    hash ^= s.GetHashCode();
                return hash;
            }

            public bool Equals(Node other)
            {
                if (null == other)
                    return false;
                bool t = (Set == null && other.Set == null) || 
                    ((Set != null && other.Set != null) && Set.SetEquals(other.Set));
                return t;
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            ISet<Node> current = new HashSet<Node>();
            current.Add(new Node(sellectedFacts, null, null));
            
            Node targetNode = null;
            while (targetNode == null)
            {
                ISet<Node> next = new HashSet<Node>();
                
                foreach (var node in current)
                {
                    foreach (var rule in rules)
                    {
                        bool skip = false;

                        foreach (var elem in rule.Antecedents)
                            if (!node.Set.Contains(elem))
                            {
                                skip = true;
                                break;
                            }

                        if (!skip)
                        {
                            
                            Node addNode = new Node(new HashSet<string>(node.Set), node, rule);
                            addNode.Set.Add(rule.Consequent);
                            
                            next.Add(addNode);
                            
                            if (addNode.Set.Contains(comboBox1.SelectedItem))
                                targetNode = addNode;
                            
                        }
                    }
                }
                
                if (!current.SetEquals(next))
                    current = next;
                else
                {
                    MessageBox.Show("Life is pain :(");
                    return;
                }
            }

            IList<Rule> inference = new List<Rule>();
            while(targetNode.Parent != null)
            {
                inference.Add(targetNode.Rule);
                targetNode = targetNode.Parent;
            }
            inference.Reverse();
            listBox4.DataSource = inference;

            MessageBox.Show("It's ok :)");

        }
    }
}
