using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AttendanceSystemPrototype
{
    public partial class DataScreen : Form
    {
        public DataScreen()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void InitializeComboBox()
        {
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            string[] employees = new string[]{"Hamilton, David", "Hensien, Kari",
            "Hammond, Maria", "Harris, Keith", "Henshaw, Jeff D.",
            "Hanson, Mark", "Harnpadoungsataya, Sariya",
            "Harrington, Mark", "Harris, Keith", "Hartwig, Doris",
            "Harui, Roger", "Hassall, Mark", "Hasselberg, Jonas",
            "Harnpadoungsataya, Sariya", "Henshaw, Jeff D.",
            "Henshaw, Jeff D.", "Hensien, Kari", "Harris, Keith",
            "Henshaw, Jeff D.", "Hensien, Kari", "Hasselberg, Jonas",
            "Harrington, Mark", "Hedlund, Magnus", "Hay, Jeff",
            "Heidepriem, Brandon D."};

            comboBox1.Items.AddRange(employees);
            this.comboBox1.Location = new System.Drawing.Point(136, 32);
            this.comboBox1.IntegralHeight = false;
            this.comboBox1.MaxDropDownItems = 5;
            this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox1.Name = "ComboBox1";
            this.comboBox1.Size = new System.Drawing.Size(136, 81);
            this.comboBox1.TabIndex = 0;
            this.Controls.Add(this.comboBox1);

            // Associate the event-handling method with the 
            // SelectedIndexChanged event.
            this.comboBox1.SelectedIndexChanged +=
                new System.EventHandler(comboBox1_SelectedIndexChanged);
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
