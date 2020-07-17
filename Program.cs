using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
[assembly: CommandClass(typeof(AutoCad.MainFunction))]
namespace AutoCad
{
 
    class MainFunction
    {
        
        [CommandMethod("drawLine")]
        public void drawLine()
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId,OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace],OpenMode.ForRead) as BlockTableRecord;
                btr.UpgradeOpen();
                Point3d startPoint = new Point3d(100,100,0);
                Point3d endPoint = new Point3d(200, 200, 0);
                Line line = new Line(startPoint,endPoint);
                btr.AppendEntity(line);
                trans.AddNewlyCreatedDBObject(line,true);
                doc.Editor.WriteMessage("hello CAD");
                //doc.SendStringToExecute("._break 150,150,100 160,160,100 ",true,false,false);
                doc.SendStringToExecute("._zoom all ", true, false, false);
                trans.Commit();
            }          
        }

        [CommandMethod("drawPoly")]
        public void DrawPoyLine()
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId,OpenMode.ForRead) as BlockTable;
                using (BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord)
                {
                    Point3dCollection p = new Point3dCollection();
                    Point3d p1 = new Point3d(100, 100, 100);
                    Point3d p2 = new Point3d(150, 150, 100);
                    Point3d p3 = new Point3d(200, 100, 100);
                    Point3d p4 = new Point3d(200, 100, 200);
                    Point3d p5 = new Point3d(150, 150, 200);
                    Point3d p6 = new Point3d(100, 100, 200);
                    Point3d p7 = new Point3d(100, 100, 150);
                    p.Add(p1); p.Add(p2); p.Add(p3); p.Add(p4); p.Add(p5); p.Add(p6); p.Add(p7);
                    Polyline3d pol = new Polyline3d(Poly3dType.SimplePoly, p, true);
                    btr.AppendEntity(pol);
                    trans.AddNewlyCreatedDBObject(pol,true);
                }
               
                trans.Commit();
            }

        }
























        [CommandMethod("getL")]
        public void getLine()
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                //以读模式打开块表 
                BlockTable acBlkTbl; acBlkTbl = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                //以写模式打开块表记录模型空间 
                BlockTableRecord acBlkTblRec; acBlkTblRec = trans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                //创建多段线 
                Line line = new Line(new Point3d(1000, 100, 0), new Point3d(1000, 800, 0));
                //添加新对象到块表记录和事务 

                List<Line> lines = new List<Line>();
                lines.Add(line);
                line = new Line(new Point3d(600, 550, 0), new Point3d(1250, 550, 0));
                lines.Add(line);
                /*
                foreach (Line l in lines)
                {
                    acBlkTblRec.AppendEntity(l);
                    trans.AddNewlyCreatedDBObject(l, true);
                    //偏移0.25距离 
                    DBObjectCollection acDbObjColl = l.GetOffsetCurves(-25);
                    //遍历得到的新对象 
                    LinetypeTable linetypeTbl = trans.GetObject(db.LinetypeTableId, OpenMode.ForRead) as LinetypeTable;
                    foreach (Entity acEnt in acDbObjColl)
                    {
                        using (acEnt)
                        {
                            String linetypeName = "JIS_02_1.0";
                            if (!linetypeTbl.Has(linetypeName))
                            {
                                db.LoadLineTypeFile(linetypeName, "acadiso.lin");
                            }
                            if (linetypeTbl.Has(linetypeName))
                            {
                                acEnt.Linetype = linetypeName;
                            }
                            //添加每个对象
                            acBlkTblRec.AppendEntity(acEnt);
                            trans.AddNewlyCreatedDBObject(acEnt, true);
                            //释放DBObject对象 
                        }
                    }
                    //释放DBObject对象
                }
                */
                foreach (Line l in lines)
                {
                    DBObjectCollection acDbObjColl = l.GetOffsetCurves(-50);
                    //遍历得到的新对象 
                    LinetypeTable linetypeTbl = trans.GetObject(db.LinetypeTableId, OpenMode.ForRead) as LinetypeTable;
                    foreach (Entity acEnt in acDbObjColl)
                    {
                        using (acEnt)
                        {
                            acBlkTblRec.AppendEntity(acEnt);
                            trans.AddNewlyCreatedDBObject(acEnt, true);
                            //释放DBObject对象 
                        }
                    }
                    //释放DBObject对象
                }
                //保存新对象到数据库
                trans.Commit();
            }
            
        }

    }
}
