using System;
using System.Linq;
using System.Windows.Forms;

namespace TranslatorWritter {
    public partial class FormComboBox : Form {
        public string Input;
        public string LabelText;
        public string ReturnString;

        public FormComboBox(string[] vars) {
            InitializeComponent();
            KeyPreview = true;
            comboBox1.Items.AddRange(vars);
        }

        public void RefreshInp(){ 
            label1.Text=LabelText;
            comboBox1.Text=Input;
            ReturnString=Input;
        }

        void buttonOK_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.OK;
            if (comboBox1.SelectedIndex>=0) ReturnString=comboBox1.Text;
            Close();
        }

        void buttonCancel_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
            comboBox1.Text=Input;
            ReturnString=Input;
            Close();
        }

        void FormString_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                buttonOK_Click(null,null);
                return;
            }
            if (e.KeyCode == Keys.Escape) {
                buttonCancel_Click(null,null);
                return;
            }
        }
    }
}
