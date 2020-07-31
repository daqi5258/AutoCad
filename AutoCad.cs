using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.ApplicationServices;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;

namespace AutoCad
{
    public partial class AutoCad : UserControl
    {
        public AutoCad()
        {
            InitializeComponent();
            GetMousePoint();
        }


        public  void GetMousePoint()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ed.PointFilter += new PointFilterEventHandler(m_GetMousePoint); 
        }
        public  void m_GetMousePoint(object sender, PointFilterEventArgs e)
        {
            //throw new System.Exception("The method or operation is not implemented.");
            locationT.Text = e.Context.ComputedPoint.ToString();
            Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(locationT.Text+","+ e.Context.ComputedPoint.ToString());

        }
    }
}
