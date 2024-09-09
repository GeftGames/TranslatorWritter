using System.Collections.Generic;
using System.Windows.Forms;

namespace TranslatorWritter {
    public partial class SimpleTo : UserControl {
        List<Button> ListButtonRemove=new List<Button>();
        List<TextBox> ListTextBoxsBase=new List<TextBox>();
        List<Label> ListLabelsComment=new List<Label>();
        List<TextBox> ListTextBoxsComment=new List<TextBox>();
        List<Label> ListLabelsSource=new List<Label>();
        List<TextBox> ListTextBoxsSource=new List<TextBox>();

        List<Button> ListButtonUp=new List<Button>();
        List<Button> ListButtonDown=new List<Button>();

        public SimpleTo() {
            InitializeComponent();
        }

        public void Add(string text, string comment, string source) {
            int id=ListTextBoxsBase.Count;
            int posY=ListTextBoxsBase.Count*40+40;

            AnchorStyles anchorBasic=AnchorStyles.Left | AnchorStyles.Top;

            // pole pro překlad
            TextBox textBoxNounTo = new TextBox {
                Location = new System.Drawing.Point(41+14, 6 + posY),
                Margin = new Padding(5, 6, 5, 6),
                Size = new System.Drawing.Size(249, 23),
                Anchor=anchorBasic,
                Text=text
            };

           // labelNounInputPatternTo
           Label labelNounInputPatternTo = new Label {
               AutoSize = true,
               Location = new System.Drawing.Point(299+14, 9 + posY),
               Margin = new Padding(4, 0, 4, 0),
               Size = new System.Drawing.Size(37, 17),
               Anchor = anchorBasic,
               Text = "Komentář"
           };

            // pole komentář
            TextBox textBoxComment = new TextBox {
                Location = new System.Drawing.Point(394+14, 6 + posY),
                Margin = new Padding(5, 6, 5, 6),
                Size = new System.Drawing.Size(249, 23),
                Anchor = anchorBasic,
                Text = comment
            };
            textBoxComment.Text=comment;

            // source label
            Label labelSource = new Label {
               AutoSize = true,
               Location = new System.Drawing.Point(645+14, 9 + posY),
               Margin = new Padding(4, 0, 4, 0),
               Size = new System.Drawing.Size(37, 17),
               Anchor = anchorBasic,
               Text = "Zdroj"
            };

            // source textbox
            TextBox textBoxSource = new TextBox {
                Location = new System.Drawing.Point(695+14, 6 + posY),
                Margin = new Padding(5, 6, 5, 6),
                Size = new System.Drawing.Size(100, 23),
                Anchor = anchorBasic,
                Text = source
            };
            textBoxSource.Text=source;


            // Tlačítko smazat
            Button buttonRemove = new Button {
                Location = new System.Drawing.Point(6+15+5, 5+posY),
                Name = "buttonRemove",
                Size = new System.Drawing.Size(27, 26),
                Anchor = anchorBasic,
                Text = "-",
                TabStop=false,
                UseVisualStyleBackColor = true
            };
            buttonRemove.Click += (object sender, System.EventArgs e)=>{
                var a=MessageBox.Show(text: "Vážně chcete smazat?", caption: "Smatat", buttons: MessageBoxButtons.YesNo);
                if (a == DialogResult.Yes) Remove(GetIndexOfRow(textBoxNounTo));
            };
                        
            // Tlařítko up
            Button buttonUp = new Button {
                Location = new System.Drawing.Point(0, 5+posY-5),
                Name = "buttonUp",
                Size = new System.Drawing.Size(20, 20),
                Anchor = anchorBasic,
                Text = "🠉",
                Font = new System.Drawing.Font(Font.FontFamily, 8f),
                UseVisualStyleBackColor = true,
                TextAlign=System.Drawing.ContentAlignment.MiddleCenter,
                TabStop = false
            };
            buttonUp.Click += (object sender, System.EventArgs e) => {
                MoveUp(GetIndexOfRow(textBoxNounTo));
            };
                        
            // Tlařítko down
            Button buttonDown = new Button {
                Location = new System.Drawing.Point(0, 5+posY+16),
                Name = "buttonDown",
                Size = new System.Drawing.Size(20, 20),
                Anchor = anchorBasic,
                Text = "🠋",
                Font = new System.Drawing.Font(Font.FontFamily, 8f),
                UseVisualStyleBackColor = true,
                TabStop=false,
                TextAlign=System.Drawing.ContentAlignment.BottomCenter,
            };
            buttonDown.Click += (object sender, System.EventArgs e) => {
                MoveDown(GetIndexOfRow(textBoxNounTo));
            };

            ListTextBoxsBase.Add(textBoxNounTo);
            ListTextBoxsComment.Add(textBoxComment);
            ListLabelsSource.Add(labelSource);
            ListButtonUp.Add(buttonUp);
            ListButtonDown.Add(buttonDown);
            ListButtonRemove.Add(buttonRemove);
            ListLabelsComment.Add(labelNounInputPatternTo);
            ListTextBoxsSource.Add(textBoxSource);

            Controls.Add(buttonUp);
            Controls.Add(buttonDown);
            Controls.Add(buttonRemove);
            Controls.Add(textBoxNounTo);
            Controls.Add(textBoxComment);
            Controls.Add(textBoxSource);
            Controls.Add(labelSource);
            Controls.Add(labelNounInputPatternTo);
        }

        public void Remove(int indexOfRow) {
            if (indexOfRow<0)return;

            TextBox textBox=ListTextBoxsBase[indexOfRow];
            Controls.Remove(textBox);
            ListTextBoxsBase.RemoveAt(indexOfRow);
            textBox.Dispose();

            Button buttonRemove=ListButtonRemove[indexOfRow];
            Controls.Remove(buttonRemove);
            ListButtonRemove.RemoveAt(indexOfRow);
            buttonRemove.Dispose();

            Label labelComment = ListLabelsComment[indexOfRow];
            Controls.Remove(labelComment);
            ListLabelsComment.RemoveAt(indexOfRow);
            labelComment.Dispose();

            TextBox textBoxComment=ListTextBoxsComment[indexOfRow];
            Controls.Remove(textBoxComment);
            ListTextBoxsComment.RemoveAt(indexOfRow);
            textBoxComment.Dispose();

            TextBox textBoxSource=ListTextBoxsSource[indexOfRow];
            Controls.Remove(textBoxSource);
            ListTextBoxsSource.RemoveAt(indexOfRow);
            textBoxSource.Dispose();
                        
            Button buttonDown=ListButtonDown[indexOfRow];
            Controls.Remove(buttonDown);
            ListButtonDown.RemoveAt(indexOfRow);
            buttonDown.Dispose();
                        
            Button buttonUp=ListButtonUp[indexOfRow];
            Controls.Remove(buttonUp);
            ListButtonUp.RemoveAt(indexOfRow);
            buttonUp.Dispose();
                        
            Label source=ListLabelsSource[indexOfRow];
            Controls.Remove(source);
            ListLabelsSource.RemoveAt(indexOfRow);
            source.Dispose();

            for (int i=0; i<ListTextBoxsBase.Count; i++) {
                int posY=i*40+5+40;
                ListTextBoxsBase[i].Location=new System.Drawing.Point(ListTextBoxsBase[i].Location.X, posY);
                ListButtonRemove[i].Location=new System.Drawing.Point(ListButtonRemove[i].Location.X, posY);
                ListLabelsComment[i].Location=new System.Drawing.Point(ListLabelsComment[i].Location.X, posY);
                ListTextBoxsComment[i].Location=new System.Drawing.Point(ListTextBoxsComment[i].Location.X, posY);
                ListTextBoxsSource[i].Location=new System.Drawing.Point(ListTextBoxsSource[i].Location.X, posY);
                ListButtonDown[i].Location=new System.Drawing.Point(ListButtonDown[i].Location.X, posY);
                ListButtonUp[i].Location=new System.Drawing.Point(ListButtonUp[i].Location.X, posY);
                ListLabelsSource[i].Location=new System.Drawing.Point(ListLabelsSource[i].Location.X, posY);
            }
        }

        public int GetIndexOfRow(TextBox btn) {
            for (int i=0; i<ListTextBoxsBase.Count; i++){
                if (ListTextBoxsBase[i].Equals(btn)) return i;
            }
            return -1;
        }

        public TranslatingToData[] GetData() {
            TranslatingToData[] data=new TranslatingToData[ListTextBoxsBase.Count];
            for (int i=0; i<ListTextBoxsBase.Count; i++) {
                data[i]=new TranslatingToData{
                    Text=ListTextBoxsBase[i].Text,
                    Comment=ListTextBoxsComment[i].Text,
                    Source=ListTextBoxsSource[i].Text
                };
            }
            return data;
        }

        public void SetData(TranslatingToData[] data) {
            Clear();
            for (int i=0; i<data.Length; i++){
                TranslatingToData row = data[i];
                Add(row.Text, row.Comment, row.Source);
            }
        }

        internal void Clear() {
            foreach (var i in ListTextBoxsBase) Controls.Remove(i);
            ListTextBoxsBase.Clear();
            
            foreach (var i in ListButtonRemove) Controls.Remove(i);
            ListButtonRemove.Clear();
            
            foreach (var i in ListLabelsComment) Controls.Remove(i);
            ListLabelsComment.Clear();
            
            foreach (var i in ListTextBoxsComment) Controls.Remove(i);
            ListTextBoxsComment.Clear();
            
            foreach (var i in ListTextBoxsSource) Controls.Remove(i);
            ListTextBoxsSource.Clear();
            
            foreach (var i in ListLabelsSource) Controls.Remove(i);
            ListLabelsSource.Clear();
                        
            foreach (var i in ListButtonUp) Controls.Remove(i);
            ListButtonUp.Clear();
                        
            foreach (var i in ListButtonDown) Controls.Remove(i);
            ListButtonDown.Clear();
        }

        private void buttonAdd_Click(object sender, System.EventArgs e) {
            Add("","","");
        }

        void MoveUp(int indexOfRow) { 
            if (indexOfRow>0) { 
                int up=indexOfRow-1;
                ChangePos(indexOfRow, up);
                //// temp
                //var BoxsBase = ListTextBoxsBase[indexOfRow];
                //var BoxsBaseText = ListTextBoxsBase[indexOfRow];
                //var ButtonRemove = ListButtonRemove[indexOfRow];
                //var LabelsComment = ListLabelsComment[indexOfRow];
                //var BoxsComment = ListTextBoxsComment[indexOfRow];
                //var BoxsTextBoxsSource = ListTextBoxsSource[indexOfRow];
                //var ButtonUp = ListButtonUp[indexOfRow];
                //var buttonDown = ListButtonDown[indexOfRow];
                //var source = ListLabelsSource[indexOfRow];

                //// set current
                //ListTextBoxsBase[indexOfRow] = ListTextBoxsBase[up];
                //ListButtonRemove[indexOfRow] = ListButtonRemove[up];
                //ListLabelsComment[indexOfRow] = ListLabelsComment[up];
                //ListTextBoxsComment[indexOfRow] = ListTextBoxsComment[up];
                //ListTextBoxsSource[indexOfRow] = ListTextBoxsSource[up];
                //ListButtonDown[indexOfRow] = ListButtonDown[up];
                //ListButtonUp[indexOfRow] = ListButtonUp[up];
                //ListLabelsSource[indexOfRow] = ListLabelsSource[up];

                //// set up
                //ListTextBoxsBase[up] = BoxsBase;
                //ListButtonRemove[up]=ButtonRemove;
                //ListLabelsComment[up]=LabelsComment;
                //ListTextBoxsComment[up]=BoxsComment;
                //ListTextBoxsSource[up]=BoxsTextBoxsSource;
                //ListButtonUp[up]=ButtonUp;
                //ListButtonDown[up]=buttonDown;
                //ListLabelsSource[up]=source;
            }
        }

        void MoveDown(int indexOfRow) { 
            if (indexOfRow+1<ListTextBoxsBase.Count) {
                int down=indexOfRow+1;
                ChangePos(indexOfRow, down);
                //// temp
                //var BoxsBase = ListTextBoxsBase[indexOfRow];
                //var ButtonRemove = ListButtonRemove[indexOfRow];
                //var LabelsComment = ListLabelsComment[indexOfRow];
                //var BoxsComment = ListTextBoxsComment[indexOfRow];
                //var BoxsTextBoxsSource = ListTextBoxsSource[indexOfRow];
                //var ButtonUp = ListButtonUp[indexOfRow];
                //var buttonDown = ListButtonDown[indexOfRow];
                //var source = ListLabelsSource[indexOfRow];

                //// set current
                //ListTextBoxsBase[indexOfRow] = ListTextBoxsBase[down];
                //ListButtonRemove[indexOfRow] = ListButtonRemove[down];
                //ListLabelsComment[indexOfRow] = ListLabelsComment[down];
                //ListTextBoxsComment[indexOfRow] = ListTextBoxsComment[down];
                //ListTextBoxsSource[indexOfRow] = ListTextBoxsSource[down];
                //ListButtonDown[indexOfRow] = ListButtonDown[down];
                //ListButtonUp[indexOfRow] = ListButtonUp[down];
                //ListLabelsSource[indexOfRow] = ListLabelsSource[down];

                //// set up
                //ListTextBoxsBase[down] = BoxsBase;
                //ListButtonRemove[down]=ButtonRemove;
                //ListLabelsComment[down]=LabelsComment;
                //ListTextBoxsComment[down]=BoxsComment;
                //ListTextBoxsSource[down]=BoxsTextBoxsSource;
                //ListButtonUp[down]=ButtonUp;
                //ListButtonDown[down]=buttonDown;
                //ListLabelsSource[down]=source;
            }
        }

        void ChangePos(int a, int b) { 
            // temp
            var BoxsBase = ListTextBoxsBase[a].Text;
            var BoxsComment = ListTextBoxsComment[a].Text;
            var BoxsTextBoxsSource = ListTextBoxsSource[a].Text;

            // set current
            ListTextBoxsBase[a].Text = ListTextBoxsBase[b].Text;
            ListTextBoxsComment[a].Text = ListTextBoxsComment[b].Text;
            ListTextBoxsSource[a].Text = ListTextBoxsSource[b].Text;

            // set b
            ListTextBoxsBase[b].Text = BoxsBase;
            ListTextBoxsComment[b].Text=BoxsComment;
            ListTextBoxsSource[b].Text=BoxsTextBoxsSource;
        }
    }
}