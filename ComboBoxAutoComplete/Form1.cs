using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;

namespace ComboBoxAutoComplete
{
    public partial class Form1 : Form
    {
        private List<Person> _list = new List<Person>();
        private List<Person> _list2 = new List<Person>();
        private System.Windows.Forms.KeyEventArgs _LastKeyDown;

        public Form1()
        {
            InitializeComponent();

            InitializeList(_list);
            InitializeList(_list2);

            ConfigurinBuildInComboBox();
            ConfiguringCustomComboBox();
        }

        private void ConfiguringCustomComboBox()
        {
            _list2 = _list2.OrderBy(x => x.Name).ToList();
            comboBox2.DataSource = _list2;
            comboBox2.DisplayMember = "Name";
            comboBox2.ValueMember = "SSN";
            comboBox2.SelectedIndex = -1;
        }

        private void ConfigurinBuildInComboBox()
        {           
            _list = _list.OrderBy(x => x.Name).ToList();
            //AutoCompleteStringCollection collection = new AutoCompleteStringCollection();
            //string[] arr = _list.Select(x => x.Name).ToArray();
            //collection.AddRange(arr);
            //comboBox1.AutoCompleteCustomSource = collection;

            comboBox1.DataSource = _list;
            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "SSN";
            comboBox1.SelectedIndex = -1;
            
            comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        }

        private void InitializeList(List<Person> list)
        {
            list.Add(new Person { Name = "Anna Gunna", Email = "anna@gunna", SSN = "1" });
            list.Add(new Person { Name = "Baldur Siggi", Email = "baldur@siggi", SSN = "2" });
            list.Add(new Person { Name = "Daníel Einar", Email = "daniel@einar", SSN = "3" });
            list.Add(new Person { Name = "Elvar Gauti", Email = "elvar@gauti", SSN = "4" });
            list.Add(new Person { Name = "Fannar Smári", Email = "fannar@smari", SSN = "5" });
            list.Add(new Person { Name = "Gunnar Ari", Email = "gunnar@ari", SSN = "6" });
            list.Add(new Person { Name = "Baldur Siggi", Email = "baldur@siggi", SSN = "7" });
            list.Add(new Person { Name = "Anna Gunna", Email = "anna@gunna", SSN = "8" });
            list.Add(new Person { Name = "Hallgrímur Sæm", Email = "hallgrimur@saem", SSN = "9" });
            list.Add(new Person { Name = "Inga Jóna", Email = "inga@jona", SSN = "10" });
            list.Add(new Person { Name = "Jón Jóns", Email = "jon@jons", SSN = "11" });
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                Person person = comboBox1.SelectedItem as Person;

                label2.Text = person.Name;
                label4.Text = person.Email;
                label6.Text = person.SSN;
            }
        }

        private void comboBox2_Leave(object sender, EventArgs e)
        {
            if (comboBox2.Focused == true)
            {
                int iFoundIndex = 0;
                iFoundIndex = comboBox2.FindStringExact(comboBox2.Text);
                comboBox2.SelectedIndex = iFoundIndex;
            }
        }

        private void comboBox2_KeyUp(object sender, KeyEventArgs e)
        {
            if ((_LastKeyDown == null) == true || (e.KeyCode == _LastKeyDown.KeyCode) == false)
                return;

            AutoComplete_KeyUp(comboBox2, e);
        }

        private void AutoComplete_KeyUp(ComboBox comboBox2, KeyEventArgs e)
        {
            string sTypedText = null;
            int iFoundIndex = 0;
            object oFoundItem = null;
            string sFoundText = null;
            string sAppendText = null;

            //Allow select keys without Autocompleting
            switch (e.KeyCode)
            {
                case Keys.Back:
                case Keys.Left:
                case Keys.Right:
                case Keys.Up:
                case Keys.Delete:
                case Keys.Down:
                case Keys.ShiftKey:
                case Keys.Shift:
                case Keys.RShiftKey:
                case Keys.LShiftKey:
                case Keys.Oem1:
                case Keys.Oem102:
                case Keys.Oem2:
                case Keys.Oem3:
                case Keys.Oem4:
                case Keys.Oem5:
                case Keys.Oem6:
                case Keys.Oem7:
                case Keys.Oem8:
                    return;
            }

            //Get the Typed Text and Find it in the list
            sTypedText = comboBox2.Text;
            iFoundIndex = comboBox2.FindString(sTypedText);

            //If we found the Typed Text in the list then Autocomplete

            if (iFoundIndex >= 0)
            {
                //Get the Item from the list (Return Type depends if Datasource was bound or List Created)
                oFoundItem = comboBox2.Items[iFoundIndex];

                //Use the ListControl.GetItemText to resolve the Name in case the Combo was Data bound
                sFoundText = comboBox2.GetItemText(oFoundItem);

                //Append then found text to the typed text to preserve case
                sAppendText = sFoundText.Substring(sTypedText.Length);
                comboBox2.Text = sTypedText + sAppendText;

                //Select the Appended Text
                comboBox2.SelectionStart = sTypedText.Length;
                comboBox2.SelectionLength = sAppendText.Length;

                label10.Text = ((Person)oFoundItem).Name;
                label11.Text = ((Person)oFoundItem).Email;
                label12.Text = ((Person)oFoundItem).SSN;
            }
        }

        private void comboBox2_KeyDown(object sender, KeyEventArgs e)
        {
            _LastKeyDown = e;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem != null)
            {
                Person person = comboBox2.SelectedItem as Person;

                label10.Text = person.Name;
                label11.Text = person.Email;
                label12.Text = person.SSN;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Person person = comboBox2.SelectedItem as Person;
            MessageBox.Show("Name: " + person.Name + "\r\n" + "Email: " + person.Email + "\r\n" + "SSN: " + person.SSN);
            comboBox2.SelectedIndex = -1;
            label10.Text = string.Empty;
            label11.Text = string.Empty;
            label12.Text = string.Empty;
            comboBox2.Focus();
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string SSN { get; set; }
    }
}
