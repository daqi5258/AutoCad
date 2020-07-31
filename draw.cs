using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Text;

namespace AutoCad
{
    public static class draw
    {
        public static ObjectId Draw(this Document doc,Point3dCollection pts,Double Arg,String  type)
        {
            Database db = doc.Database;
            ObjectId objId;

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId,OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace],OpenMode.ForWrite) as BlockTableRecord;
                if (type=="line")
                {
                    Line l = new Line(pts[0],pts[1]);
                    objId = btr.AppendEntity(l);
                    trans.AddNewlyCreatedDBObject(l,true);
                }else if (type== "Circle")
                {
                    Circle cc = new Circle(pts[0],pts[1].GetVectorTo(pts[0]), Arg);
                    objId = btr.AppendEntity(cc);
                    trans.AddNewlyCreatedDBObject(cc, true);
                }
                else// if (type == "PolyLine")
                {
                    Polyline3d pl = new Polyline3d(Poly3dType.SimplePoly, pts, true);
                    objId = btr.AppendEntity(pl);
                    trans.AddNewlyCreatedDBObject(pl, true);
                }

                trans.Commit();
            }
            return objId;
        }
    }
}
