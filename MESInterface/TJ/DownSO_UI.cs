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
    public partial class DownSO_UI : UserControl
    {
        DownSO downSO;
        delegate void AddDataGridDelegate(DataGridView dgv, DataTable dt);
        public DownSO_UI(DownSO downSO)
        {
            InitializeComponent();
            this.downSO = downSO;
        }

        private void DownSO_UI_Load(object sender, EventArgs e)
        {

            downSO.addDataGridDelegate = new DownSO.AddDataGridDelegate(AddDataGrid);


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

        private void SOHeaderData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
