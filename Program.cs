using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
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
                    pol.ColorIndex = 2;
                    btr.AppendEntity(pol);
                    trans.AddNewlyCreatedDBObject(pol,true);
                    Move2(pol.ObjectId,new Point3d(100,100,100),new Point3d(0,0,0));
                    Move3(pol.ObjectId, new Point3d(100, 100, 100), new Point3d(0, 0, 0),Math.PI/2);
                    Move4(pol.ObjectId, new Point3d(100, 100, 100), new Point3d(0, 0, 0),false);

                }
               
                trans.Commit();
            }
        }
        [CommandMethod("drawArc")]
        public void drawArc()
        {

            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                using (BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord)
                {
                    Point3d cPt= new Point3d(1000, 100, 100);
                    Point3d tPt = new Point3d(1100,100,100);
                    Point3d tPt2 = new Point3d(1000, 200, 100);
                    Point3d tPt3 = new Point3d(1000, 100, 200);
                    Vector3d vt = cPt.GetVectorTo(tPt);
                    Vector3d vt2 = cPt.GetVectorTo(tPt2);
                    Vector3d vt3 = cPt.GetVectorTo(tPt3);
                    Circle c1 = new Circle(cPt,vt,100);
                    c1.ColorIndex = 1;
                    Circle c2 = new Circle(cPt, vt2, 100);
                    c2.ColorIndex = 2;
                    Circle c3 = new Circle(cPt, vt3, 100);
                    c3.ColorIndex = 3;
                    btr.AppendEntity(c1);
                    trans.AddNewlyCreatedDBObject(c1,true);
                    btr.AppendEntity(c2);
                    trans.AddNewlyCreatedDBObject(c2, true);
                    btr.AppendEntity(c3);
                    trans.AddNewlyCreatedDBObject(c3, true);
                    Point3dCollection pts = new Point3dCollection();
                    for(int i = 0; i < 10; i++)
                    {
                        pts.Add(new Point3d(i*100+50, i % 2 * 100, 0));
                    }
                    Spline spline = new Spline(pts,4,0);
                    btr.AppendEntity(spline);
                    trans.AddNewlyCreatedDBObject(spline,true);
                }
                trans.Commit();
            }
        }

        [CommandMethod("drawtext")]
        public void drawText()
        {

            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                using (BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord)
                {
                    DBText text = new DBText();
                    text.Position = new Point3d(1000,250,100);
                    text.Height = 5;
                    text.TextString = "面积=_2000_m";
                    text.HorizontalMode = TextHorizontalMode.TextCenter;
                    text.VerticalMode = TextVerticalMode.TextVerticalMid;
                    text.AlignmentPoint = text.Position;
                    btr.AppendEntity(text);
                    trans.AddNewlyCreatedDBObject(text,true);
                }
                trans.Commit();
            }
         }


        public static  void Move(ObjectId objectId,Point3d sourcePt,Point3d targetPt)
        {
            Vector3d vt = targetPt.GetVectorTo(sourcePt);
            Matrix3d mt = Matrix3d.Displacement(vt);
            Entity ent = objectId.GetObject(OpenMode.ForWrite) as Entity;
            ent.ColorIndex = 1;
            ent.TransformBy(mt);
            ent.DowngradeOpen();
        }
        /// <summary>
        /// 平移
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="sourcePt"></param>
        /// <param name="targetPt"></param>
        /// <returns></returns>
        public static ObjectId  Move2(ObjectId objectId, Point3d sourcePt, Point3d targetPt)
        {
            Vector3d vt = targetPt.GetVectorTo(sourcePt);
            Matrix3d mt = Matrix3d.Displacement(vt);
            Entity ent = objectId.GetObject(OpenMode.ForWrite) as Entity;
           
            Entity ent2 = ent.GetTransformedCopy(mt);
            ent2.ColorIndex = 1;
            ObjectId objectId1 =  AddToModelSpace(objectId.Database, ent2);
            return objectId1;

        }
        /// <summary>
        /// 选择
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="sourcePt"></param>
        /// <param name="targetPt"></param>
        /// <param name="angle">弧度,逆时针为正</param>
        /// <returns></returns>
        public static ObjectId Move3(ObjectId objectId, Point3d sourcePt, Point3d targetPt, double angle)
        {
            Vector3d vt = targetPt.GetVectorTo(sourcePt);
            Matrix3d mt = Matrix3d.Rotation(angle,vt, sourcePt);
            Entity ent = objectId.GetObject(OpenMode.ForRead) as Entity;

            Entity ent2 = ent.GetTransformedCopy(mt);
            ent2.ColorIndex = 3;
            ObjectId objectId1 = AddToModelSpace(objectId.Database, ent2);
            return objectId1;

        }
        /// <summary>
        /// 镜像,以两点组成的线为镜像线
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="sourcePt"></param>
        /// <param name="targetPt"></param>
        /// <param name="dlt"></param>
        /// <returns></returns>
        public static ObjectId Move4(ObjectId objectId, Point3d sourcePt, Point3d targetPt, Boolean dlt)
        {
            Line3d line = new Line3d(sourcePt,targetPt);
            Matrix3d mt = Matrix3d.Mirroring(line);
            Entity ent = objectId.GetObject(OpenMode.ForWrite) as Entity;
            if (dlt)
            {
                ent.TransformBy(mt);
            }
            else
            {
                Entity ent2 = ent.GetTransformedCopy(mt);
                ent2.ColorIndex = 4;
                objectId = AddToModelSpace(objectId.Database, ent2);
            }
            return objectId;

        }















        public static ObjectId AddToModelSpace(Database db,Entity ent)
        {
            ObjectId objectId;

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                using (BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord)
                {
                    objectId = btr.AppendEntity(ent);
                    trans.AddNewlyCreatedDBObject(ent, true);
                }
                trans.Commit();
            }
                return objectId;
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
