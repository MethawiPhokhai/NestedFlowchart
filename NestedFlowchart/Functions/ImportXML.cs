using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NestedFlowchart.Functions
{
    public class ImportXml
    {
        public OpenFileDialog LoadXMLFile()
        {
            return new OpenFileDialog
            {
                InitialDirectory = @"D:\",
                Title = "Browse Flowchart.xml Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "xml",
                Filter = "XML files (*.xml)|*.xml",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };
        }

        public void WriteInputPath(TextBox txt_InputPath, OpenFileDialog openFileDialog1)
        {
            txt_InputPath.Text = openFileDialog1.FileName;
        }

        public void WriteInputPreview(TextBox txt_InputPath, TextBox txt_InputPreview)
        {
            StreamReader strRead = File.OpenText(txt_InputPath.Text);
            txt_InputPreview.Text = strRead.ReadToEnd();
        }
    }
}
