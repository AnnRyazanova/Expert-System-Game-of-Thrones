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
        private ISet<string> selectedFacts = new HashSet<string>();
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
                    facts.Add(antecedent);
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
            selectedFacts.Add(fact);
            var factsList = facts.ToList();
            factsList.Sort();
            listBox1.DataSource = factsList;
            listBox3.DataSource = selectedFacts.ToList();
            comboBox1.DataSource = factsList;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (null == listBox3.SelectedItem)
                return;
            var fact = (string)listBox3.SelectedItem;
            selectedFacts.Remove(fact);
            facts.Add(fact);
            var factsList = facts.ToList();
            factsList.Sort();
            listBox1.DataSource = factsList;
            listBox3.DataSource = selectedFacts.ToList();
            comboBox1.DataSource = factsList;
        }

        private class Node
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
                var node = obj as Node;
                if (null == node) return false;
                return Set.SetEquals(node.Set);
            }

            public override int GetHashCode()
            {
                return Set
                    .Select(s => s.GetHashCode())
                    .Aggregate((a, b) => a ^ b);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var currentSet = new HashSet<Node>();
            currentSet.Add(new Node(selectedFacts, null, null));
            Node targetNode = null;
            while (targetNode == null)
            {
                var nextSet = new HashSet<Node>();
                foreach (var node in currentSet)
                    foreach (var rule in rules)
                    {
                        if (rule.Antecedents.Any(fact => !node.Set.Contains(fact)))
                            continue;
                        var addNode = new Node(new HashSet<string>(node.Set), node, rule);
                        addNode.Set.Add(rule.Consequent);
                        nextSet.Add(addNode);
                        if (addNode.Set.Contains(comboBox1.SelectedItem))
                            targetNode = addNode;
                    }
                if (currentSet.SetEquals(nextSet))
                    break;
                currentSet = nextSet;
            }
            if (null == targetNode)
            {
                MessageBox.Show("Вывод невозможен");
                return;
            }
            var inference = new List<Rule>();
            while (null != targetNode.Parent)
            {
                inference.Add(targetNode.Rule);
                targetNode = targetNode.Parent;
            }
            inference.Reverse();
            listBox4.DataSource = inference;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var currentSet = new HashSet<Node>();
            var initial= new HashSet<string>();
            initial.Add((string)comboBox1.SelectedItem);
            currentSet.Add(new Node(initial, null, null));
            Node sourceNode = null;
            while (true)
            {
                var nextSet = new HashSet<Node>();
                foreach (var node in currentSet)
                {
                    var activeRules = rules
                        .Where(r =>
                            !selectedFacts.Contains(r.Consequent) && 
                            node.Set.Contains(r.Consequent));
                    foreach (var r in activeRules)
                    {
                        var set = new HashSet<string>(node.Set);
                        set.Remove(r.Consequent);
                        foreach (var f in r.Antecedents)
                            set.Add(f);
                        var newNode = new Node(set, node, r);
                        nextSet.Add(newNode);
                        if (selectedFacts.All(f => set.Contains(f)))
                        {
                            sourceNode = newNode;
                            goto found;
                        }
                    }
                }
                if (currentSet.SetEquals(nextSet))
                    break;
                currentSet = nextSet;
            }
        found:
            if (null == sourceNode)
            {
                MessageBox.Show("Вывод невозможен");
                return;
            }
            var inference = new List<Rule>();
            while (null != sourceNode.Parent)
            {
                inference.Add(sourceNode.Rule);
                sourceNode = sourceNode.Parent;
            }
            listBox4.DataSource = inference;
        }
    }
}
