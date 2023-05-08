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
            panel1 = new Panel();
            label2 = new Label();
            txt_InputPreview = new TextBox();
            txt_InputPath = new TextBox();
            lbl_Input = new Label();
            btn_ImportXML = new Button();
            panel2 = new Panel();
            dg_ElementPreview = new DataGridView();
            lnk_CPNTools = new LinkLabel();
            label5 = new Label();
            lbl_CPNTools = new Label();
            btn_Transfrom = new Button();
            panel3 = new Panel();
            btn_browse = new Button();
            lbl_ResultLabel = new Label();
            txt_ReultPath = new TextBox();
            btn_ExportToCPN = new Button();
            label3 = new Label();
            label4 = new Label();
            label1 = new Label();
            btn_close = new Label();
            folderBrowserDialog1 = new FolderBrowserDialog();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dg_ElementPreview).BeginInit();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(label2);
            panel1.Controls.Add(txt_InputPreview);
            panel1.Controls.Add(txt_InputPath);
            panel1.Location = new Point(36, 103);
            panel1.Name = "panel1";
            panel1.Size = new Size(497, 509);
            panel1.TabIndex = 0;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(19, 448);
            label2.Name = "label2";
            label2.Size = new Size(62, 15);
            label2.TabIndex = 5;
            label2.Text = "Input Path";
            // 
            // txt_InputPreview
            // 
            txt_InputPreview.Location = new Point(19, 23);
            txt_InputPreview.Multiline = true;
            txt_InputPreview.Name = "txt_InputPreview";
            txt_InputPreview.Size = new Size(461, 422);
            txt_InputPreview.TabIndex = 4;
            // 
            // txt_InputPath
            // 
            txt_InputPath.Location = new Point(19, 466);
            txt_InputPath.Name = "txt_InputPath";
            txt_InputPath.Size = new Size(461, 23);
            txt_InputPath.TabIndex = 3;
            // 
            // lbl_Input
            // 
            lbl_Input.AutoSize = true;
            lbl_Input.BackColor = Color.Transparent;
            lbl_Input.Font = new Font("Century Gothic", 12F, FontStyle.Regular, GraphicsUnit.Point);
            lbl_Input.Location = new Point(36, 78);
            lbl_Input.Name = "lbl_Input";
            lbl_Input.Size = new Size(118, 21);
            lbl_Input.TabIndex = 1;
            lbl_Input.Text = "Input Preview";
            // 
            // btn_ImportXML
            // 
            btn_ImportXML.BackColor = SystemColors.ControlLight;
            btn_ImportXML.FlatAppearance.BorderColor = SystemColors.ActiveBorder;
            btn_ImportXML.FlatStyle = FlatStyle.Flat;
            btn_ImportXML.Font = new Font("Century Gothic", 12F, FontStyle.Regular, GraphicsUnit.Point);
            btn_ImportXML.ForeColor = SystemColors.ActiveCaptionText;
            btn_ImportXML.Location = new Point(210, 618);
            btn_ImportXML.Name = "btn_ImportXML";
            btn_ImportXML.Size = new Size(149, 44);
            btn_ImportXML.TabIndex = 2;
            btn_ImportXML.Text = "1. Import XML";
            btn_ImportXML.UseVisualStyleBackColor = false;
            btn_ImportXML.Click += btn_ImportXML_Click;
            // 
            // panel2
            // 
            panel2.Controls.Add(dg_ElementPreview);
            panel2.Controls.Add(lnk_CPNTools);
            panel2.Controls.Add(label5);
            panel2.Controls.Add(lbl_CPNTools);
            panel2.Location = new Point(549, 103);
            panel2.Name = "panel2";
            panel2.Size = new Size(476, 509);
            panel2.TabIndex = 3;
            // 
            // dg_ElementPreview
            // 
            dg_ElementPreview.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dg_ElementPreview.Location = new Point(19, 53);
            dg_ElementPreview.Name = "dg_ElementPreview";
            dg_ElementPreview.RowHeadersWidth = 51;
            dg_ElementPreview.RowTemplate.Height = 25;
            dg_ElementPreview.Size = new Size(437, 392);
            dg_ElementPreview.TabIndex = 11;
            // 
            // lnk_CPNTools
            // 
            lnk_CPNTools.AutoSize = true;
            lnk_CPNTools.LinkVisited = true;
            lnk_CPNTools.Location = new Point(279, 469);
            lnk_CPNTools.Name = "lnk_CPNTools";
            lnk_CPNTools.Size = new Size(30, 15);
            lnk_CPNTools.TabIndex = 9;
            lnk_CPNTools.TabStop = true;
            lnk_CPNTools.Text = "here";
            lnk_CPNTools.LinkClicked += lnk_CPNTools_LinkClicked;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(19, 23);
            label5.Name = "label5";
            label5.Size = new Size(94, 15);
            label5.TabIndex = 9;
            label5.Text = "Element Preview";
            // 
            // lbl_CPNTools
            // 
            lbl_CPNTools.AutoSize = true;
            lbl_CPNTools.Location = new Point(19, 469);
            lbl_CPNTools.Name = "lbl_CPNTools";
            lbl_CPNTools.Size = new Size(264, 15);
            lbl_CPNTools.TabIndex = 8;
            lbl_CPNTools.Text = "If don't have CPN Tools software, Download free ";
            // 
            // btn_Transfrom
            // 
            btn_Transfrom.BackColor = SystemColors.ControlLight;
            btn_Transfrom.FlatAppearance.BorderColor = SystemColors.ActiveBorder;
            btn_Transfrom.FlatStyle = FlatStyle.Flat;
            btn_Transfrom.Font = new Font("Century Gothic", 12F, FontStyle.Regular, GraphicsUnit.Point);
            btn_Transfrom.ForeColor = SystemColors.ActiveCaptionText;
            btn_Transfrom.Location = new Point(710, 618);
            btn_Transfrom.Name = "btn_Transfrom";
            btn_Transfrom.Size = new Size(149, 44);
            btn_Transfrom.TabIndex = 4;
            btn_Transfrom.Text = "2. Transform XML";
            btn_Transfrom.UseVisualStyleBackColor = false;
            btn_Transfrom.Click += btn_Transfrom_Click;
            // 
            // panel3
            // 
            panel3.Controls.Add(btn_browse);
            panel3.Controls.Add(lbl_ResultLabel);
            panel3.Controls.Add(txt_ReultPath);
            panel3.Location = new Point(1040, 103);
            panel3.Name = "panel3";
            panel3.Size = new Size(495, 509);
            panel3.TabIndex = 5;
            // 
            // btn_browse
            // 
            btn_browse.Location = new Point(415, 203);
            btn_browse.Name = "btn_browse";
            btn_browse.Size = new Size(64, 23);
            btn_browse.TabIndex = 8;
            btn_browse.Text = "browse";
            btn_browse.UseVisualStyleBackColor = true;
            btn_browse.Click += btn_browse_Click;
            // 
            // lbl_ResultLabel
            // 
            lbl_ResultLabel.AutoSize = true;
            lbl_ResultLabel.Location = new Point(16, 185);
            lbl_ResultLabel.Name = "lbl_ResultLabel";
            lbl_ResultLabel.Size = new Size(66, 15);
            lbl_ResultLabel.TabIndex = 7;
            lbl_ResultLabel.Text = "Result Path";
            // 
            // txt_ReultPath
            // 
            txt_ReultPath.Location = new Point(16, 203);
            txt_ReultPath.Name = "txt_ReultPath";
            txt_ReultPath.Size = new Size(393, 23);
            txt_ReultPath.TabIndex = 6;
            // 
            // btn_ExportToCPN
            // 
            btn_ExportToCPN.BackColor = SystemColors.ControlLight;
            btn_ExportToCPN.FlatAppearance.BorderColor = SystemColors.ActiveBorder;
            btn_ExportToCPN.FlatStyle = FlatStyle.Flat;
            btn_ExportToCPN.Font = new Font("Century Gothic", 12F, FontStyle.Regular, GraphicsUnit.Point);
            btn_ExportToCPN.ForeColor = SystemColors.ActiveCaptionText;
            btn_ExportToCPN.Location = new Point(1209, 618);
            btn_ExportToCPN.Name = "btn_ExportToCPN";
            btn_ExportToCPN.Size = new Size(149, 44);
            btn_ExportToCPN.TabIndex = 6;
            btn_ExportToCPN.Text = "3. Export to CPN";
            btn_ExportToCPN.UseVisualStyleBackColor = false;
            btn_ExportToCPN.Click += btn_ExportToCPN_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Century Gothic", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(549, 78);
            label3.Name = "label3";
            label3.Size = new Size(85, 21);
            label3.TabIndex = 7;
            label3.Text = "Transform";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.Font = new Font("Century Gothic", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label4.Location = new Point(1040, 78);
            label4.Name = "label4";
            label4.Size = new Size(69, 21);
            label4.TabIndex = 8;
            label4.Text = "Output";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Century Gothic", 15.75F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(36, 22);
            label1.Name = "label1";
            label1.Size = new Size(522, 25);
            label1.TabIndex = 9;
            label1.Text = "Nested Flowchart into Colured Petri Nets Converter";
            // 
            // btn_close
            // 
            btn_close.AutoSize = true;
            btn_close.BackColor = Color.Transparent;
            btn_close.Font = new Font("Century Gothic", 12F, FontStyle.Regular, GraphicsUnit.Point);
            btn_close.Location = new Point(1532, 7);
            btn_close.Name = "btn_close";
            btn_close.Size = new Size(19, 21);
            btn_close.TabIndex = 10;
            btn_close.Text = "X";
            btn_close.Click += btn_close_Click;
            btn_close.MouseLeave += btn_close_MouseLeave;
            btn_close.MouseHover += btn_close_MouseHover;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.background;
            ClientSize = new Size(1562, 673);
            Controls.Add(btn_close);
            Controls.Add(label1);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(btn_ExportToCPN);
            Controls.Add(panel3);
            Controls.Add(btn_Transfrom);
            Controls.Add(panel2);
            Controls.Add(btn_ImportXML);
            Controls.Add(lbl_Input);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Nested Flowchart Converter";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dg_ElementPreview).EndInit();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
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
        private Button btn_ExportToCPN;
        private Label label5;
        private Label label3;
        private Label label4;
        private Label label1;
        private DataGridView dg_ElementPreview;
        private LinkLabel lnk_CPNTools;
        private Label lbl_CPNTools;
        private Label btn_close;
        private Button btn_browse;
        private FolderBrowserDialog folderBrowserDialog1;
    }
}