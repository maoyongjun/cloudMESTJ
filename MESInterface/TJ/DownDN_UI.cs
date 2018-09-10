using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MESInterface.TJ
{
    public partial class DownDN_UI : UserControl
    {
        DownDN downDN;
        delegate void AddDataGridDelegate(DataGridView dgv, DataTable dt);
        public DownDN_UI(DownDN downDN)
        {
            InitializeComponent();
            this.downDN = downDN;
        }

        private void DownDN_UI_Load(object sender, EventArgs e)
        {
   
            downDN.addDataGridDelegate = new DownDN.AddDataGridDelegate(AddDataGrid);
    

        }

        void AddDataGrid(string DataGridViewName, DataTable dt)
        {
            Control cl = GetControlByName(this.Controls, DataGridViewName);
            if (cl != null)
            {
                AddDataGridDelegate dgdelegete = delegate (DataGridView dg, DataTable ta)
                {
                    dg.DataSource = ta;
                };
                this.Invoke(dgdelegete, new object[] { ((DataGridView)cl), dt });
            }
        }

        private Control GetControlByName(Control.ControlCollection Controls, string ControlName)
        {
            Control FindCl = null;
            foreach (Control cl in Controls)
            {
                if (cl.Name == ControlName)
                {
                    FindCl = cl;
                    break;
                }
                else
                {
                    if (cl.HasChildren)
                    {
                        FindCl = GetControlByName(cl.Controls, ControlName);
                        if (FindCl != null)
                        {
                            break;
                        }
                    }
                }
            }
            return FindCl;
        }
    }
}
