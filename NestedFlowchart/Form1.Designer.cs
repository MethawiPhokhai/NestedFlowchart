namespace NestedFlowchart
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_InputPreview = new System.Windows.Forms.TextBox();
            this.txt_InputPath = new System.Windows.Forms.TextBox();
            this.lbl_Input = new System.Windows.Forms.Label();
            this.btn_ImportXML = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dg_ElementPreview = new System.Windows.Forms.DataGridView();
            this.lnk_CPNTools = new System.Windows.Forms.LinkLabel();
            this.label5 = new System.Windows.Forms.Label();
            this.lbl_CPNTools = new System.Windows.Forms.Label();
            this.dg_RouteTable = new System.Windows.Forms.DataGridView();
            this.label6 = new System.Windows.Forms.Label();
            this.btn_Transfrom = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lbl_ResultLabel = new System.Windows.Forms.Label();
            this.txt_ReultPath = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_close = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg_ElementPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_RouteTable)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txt_InputPreview);
            this.panel1.Controls.Add(this.txt_InputPath);
            this.panel1.Location = new System.Drawing.Point(41, 137);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(568, 679);
            this.panel1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 597);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Input Path";
            // 
            // txt_InputPreview
            // 
            this.txt_InputPreview.Location = new System.Drawing.Point(22, 31);
            this.txt_InputPreview.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_InputPreview.Multiline = true;
            this.txt_InputPreview.Name = "txt_InputPreview";
            this.txt_InputPreview.Size = new System.Drawing.Size(526, 561);
            this.txt_InputPreview.TabIndex = 4;
            // 
            // txt_InputPath
            // 
            this.txt_InputPath.Location = new System.Drawing.Point(22, 621);
            this.txt_InputPath.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_InputPath.Name = "txt_InputPath";
            this.txt_InputPath.Size = new System.Drawing.Size(526, 27);
            this.txt_InputPath.TabIndex = 3;
            // 
            // lbl_Input
            // 
            this.lbl_Input.AutoSize = true;
            this.lbl_Input.BackColor = System.Drawing.Color.Transparent;
            this.lbl_Input.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lbl_Input.Location = new System.Drawing.Point(41, 104);
            this.lbl_Input.Name = "lbl_Input";
            this.lbl_Input.Size = new System.Drawing.Size(144, 23);
            this.lbl_Input.TabIndex = 1;
            this.lbl_Input.Text = "Input Preview";
            // 
            // btn_ImportXML
            // 
            this.btn_ImportXML.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btn_ImportXML.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveBorder;
            this.btn_ImportXML.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_ImportXML.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btn_ImportXML.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btn_ImportXML.Location = new System.Drawing.Point(240, 824);
            this.btn_ImportXML.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_ImportXML.Name = "btn_ImportXML";
            this.btn_ImportXML.Size = new System.Drawing.Size(170, 59);
            this.btn_ImportXML.TabIndex = 2;
            this.btn_ImportXML.Text = "1. Import XML";
            this.btn_ImportXML.UseVisualStyleBackColor = false;
            this.btn_ImportXML.Click += new System.EventHandler(this.btn_ImportXML_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dg_ElementPreview);
            this.panel2.Controls.Add(this.lnk_CPNTools);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.lbl_CPNTools);
            this.panel2.Location = new System.Drawing.Point(627, 137);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(544, 679);
            this.panel2.TabIndex = 3;
            // 
            // dg_ElementPreview
            // 
            this.dg_ElementPreview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_ElementPreview.Location = new System.Drawing.Point(22, 71);
            this.dg_ElementPreview.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dg_ElementPreview.Name = "dg_ElementPreview";
            this.dg_ElementPreview.RowHeadersWidth = 51;
            this.dg_ElementPreview.RowTemplate.Height = 25;
            this.dg_ElementPreview.Size = new System.Drawing.Size(499, 523);
            this.dg_ElementPreview.TabIndex = 11;
            // 
            // lnk_CPNTools
            // 
            this.lnk_CPNTools.AutoSize = true;
            this.lnk_CPNTools.LinkVisited = true;
            this.lnk_CPNTools.Location = new System.Drawing.Point(319, 625);
            this.lnk_CPNTools.Name = "lnk_CPNTools";
            this.lnk_CPNTools.Size = new System.Drawing.Size(38, 20);
            this.lnk_CPNTools.TabIndex = 9;
            this.lnk_CPNTools.TabStop = true;
            this.lnk_CPNTools.Text = "here";
            this.lnk_CPNTools.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnk_CPNTools_LinkClicked);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 31);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(118, 20);
            this.label5.TabIndex = 9;
            this.label5.Text = "Element Preview";
            // 
            // lbl_CPNTools
            // 
            this.lbl_CPNTools.AutoSize = true;
            this.lbl_CPNTools.Location = new System.Drawing.Point(22, 625);
            this.lbl_CPNTools.Name = "lbl_CPNTools";
            this.lbl_CPNTools.Size = new System.Drawing.Size(333, 20);
            this.lbl_CPNTools.TabIndex = 8;
            this.lbl_CPNTools.Text = "If don\'t have CPN Tools software, Download free ";
            // 
            // dg_RouteTable
            // 
            this.dg_RouteTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_RouteTable.Location = new System.Drawing.Point(25, 71);
            this.dg_RouteTable.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dg_RouteTable.Name = "dg_RouteTable";
            this.dg_RouteTable.RowHeadersWidth = 51;
            this.dg_RouteTable.RowTemplate.Height = 25;
            this.dg_RouteTable.Size = new System.Drawing.Size(510, 523);
            this.dg_RouteTable.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(25, 31);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(128, 20);
            this.label6.TabIndex = 10;
            this.label6.Text = "Flowchart Preview";
            // 
            // btn_Transfrom
            // 
            this.btn_Transfrom.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btn_Transfrom.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveBorder;
            this.btn_Transfrom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Transfrom.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btn_Transfrom.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btn_Transfrom.Location = new System.Drawing.Point(812, 824);
            this.btn_Transfrom.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_Transfrom.Name = "btn_Transfrom";
            this.btn_Transfrom.Size = new System.Drawing.Size(170, 59);
            this.btn_Transfrom.TabIndex = 4;
            this.btn_Transfrom.Text = "2. Transform XML";
            this.btn_Transfrom.UseVisualStyleBackColor = false;
            this.btn_Transfrom.Click += new System.EventHandler(this.btn_Transfrom_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.dg_RouteTable);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.lbl_ResultLabel);
            this.panel3.Controls.Add(this.txt_ReultPath);
            this.panel3.Location = new System.Drawing.Point(1189, 137);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(566, 679);
            this.panel3.TabIndex = 5;
            // 
            // lbl_ResultLabel
            // 
            this.lbl_ResultLabel.AutoSize = true;
            this.lbl_ResultLabel.Location = new System.Drawing.Point(22, 597);
            this.lbl_ResultLabel.Name = "lbl_ResultLabel";
            this.lbl_ResultLabel.Size = new System.Drawing.Size(81, 20);
            this.lbl_ResultLabel.TabIndex = 7;
            this.lbl_ResultLabel.Text = "Result Path";
            // 
            // txt_ReultPath
            // 
            this.txt_ReultPath.Location = new System.Drawing.Point(22, 621);
            this.txt_ReultPath.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_ReultPath.Name = "txt_ReultPath";
            this.txt_ReultPath.Size = new System.Drawing.Size(513, 27);
            this.txt_ReultPath.TabIndex = 6;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.SystemColors.ControlLight;
            this.button3.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveBorder;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.button3.Location = new System.Drawing.Point(1382, 824);
            this.button3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(170, 59);
            this.button3.TabIndex = 6;
            this.button3.Text = "3. Export to CPN";
            this.button3.UseVisualStyleBackColor = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(627, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 23);
            this.label3.TabIndex = 7;
            this.label3.Text = "Transform";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(1189, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 23);
            this.label4.TabIndex = 8;
            this.label4.Text = "Output";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(41, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(672, 32);
            this.label1.TabIndex = 9;
            this.label1.Text = "Nested Flowchart into Colured Petri Nets Converter";
            // 
            // btn_close
            // 
            this.btn_close.AutoSize = true;
            this.btn_close.BackColor = System.Drawing.Color.Transparent;
            this.btn_close.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btn_close.Location = new System.Drawing.Point(1751, 9);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(22, 23);
            this.btn_close.TabIndex = 10;
            this.btn_close.Text = "X";
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            this.btn_close.MouseLeave += new System.EventHandler(this.btn_close_MouseLeave);
            this.btn_close.MouseHover += new System.EventHandler(this.btn_close_MouseHover);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::NestedFlowchart.Properties.Resources.background;
            this.ClientSize = new System.Drawing.Size(1785, 897);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.btn_Transfrom);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.btn_ImportXML);
            this.Controls.Add(this.lbl_Input);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Nested Flowchart Converter";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg_ElementPreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_RouteTable)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Panel panel1;
        private Label lbl_Input;
        private Label label2;
        private TextBox txt_InputPreview;
        private TextBox txt_InputPath;
        private Button btn_ImportXML;
        private Panel panel2;
        private Button btn_Transfrom;
        private Panel panel3;
        private Label lbl_ResultLabel;
        private TextBox txt_ReultPath;
        private Button button3;
        private Label label6;
        private Label label5;
        private Label label3;
        private Label label4;
        private Label label1;
        private DataGridView dg_RouteTable;
        private DataGridView dg_ElementPreview;
        private LinkLabel lnk_CPNTools;
        private Label lbl_CPNTools;
        private Label btn_close;
    }
}