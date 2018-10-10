﻿using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace AirshipsModMaker
{
    public partial class FrmTemplate : Form
    {
        Template[] Templates;

        public FrmTemplate(Template[] templates)
        {
            InitializeComponent();
            Templates = templates;
            Rels();
        }

        public void Rels()
        {
            listView1.Items.Clear();
            foreach (Template tmp in Templates)
            {
                listView1.Items.Add(tmp.Name);
                listView1.Items[listView1.Items.Count - 1].SubItems.Add(tmp.Author);
                if (!new DirectoryInfo(tmp.path).Exists)
                {
                    listView1.Items[listView1.Items.Count - 1].BackColor = Color.LightSalmon;
                    listView1.Items[listView1.Items.Count - 1].SubItems.Add("be removed");
                }
                else if(tmp.Error != null)
                {
                    listView1.Items[listView1.Items.Count - 1].BackColor = Color.LightCoral;
                    listView1.Items[listView1.Items.Count - 1].SubItems.Add("Error:"+tmp.Error.Message);
                }              
                else
                {
                    listView1.Items[listView1.Items.Count - 1].SubItems.Add(tmp.Info);
                }
            }
        }

        //move templates or template folder to Software Use.
        public static void UpdateTemplates(DirectoryInfo Path)
        {
            if (!Path.Exists)
                return;
            FileInfo fi = new FileInfo(Path.FullName + @"\info.lps");
            if (fi.Exists)//copy
            {
                DirectoryInfo di = new DirectoryInfo(Program.PathMain + @"\" + Path.Name);
                if (di.Exists)
                {
                    di.Delete(true);
                    Thread.Sleep(10);//if Delete it, it will have bug
                }
            }
            else//copy all
            {
                foreach (DirectoryInfo di in Path.GetDirectories())
                    UpdateTemplates(di);
            }
        }

        private void addTemplatesToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                UpdateTemplates(new DirectoryInfo(folderBrowserDialog1.SelectedPath));
            }
        }

        private void reMoveTemplateToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (listView1.SelectedIndices.Count == 0)
                return;

            if (MessageBox.Show("Operation cannot be cancelled\n Are you Sure Delete This Template", "You are Try to Delete '" +
               listView1.SelectedItems[0].Text + "'", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                new DirectoryInfo(Templates[listView1.SelectedIndices[0]].path).Delete(true);
                Thread.Sleep(10);//if Delete it, it will have bug
                listView1.SelectedItems[0].BackColor = Color.LightSalmon;
                listView1.SelectedItems[0].SubItems[2].Text = "be removed";
            }
        }

        private void showOnExplorerToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (listView1.SelectedIndices.Count == 0)
                return;

            System.Diagnostics.Process.Start(Templates[listView1.SelectedIndices[0]].path);
        }
    }
}
