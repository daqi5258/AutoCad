using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.GraphicsInterface;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;

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
        [CommandMethod("show")]
        public void Initialize()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
               // PaletteSet  

            }

            AutoCad acad = new AutoCad();
            PaletteSet ps = new PaletteSet("面板");
            ps.Size = new System.Drawing.Size(120, 100);
            ps.Style = PaletteSetStyles.ShowCloseButton;
            ps.Add("用户控件", acad);
            ps.Visible = true;

        }
        [CommandMethod("myform")]
        public void MyForm()
        {
             
        }

        [CommandMethod("pyL")]
        public void pyLine()
        {
            //获取当前文档及数据库 
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;
            //启动事务
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                //以读模式打开块表 
                BlockTable acBlkTbl; acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForRead) as BlockTable;
                //以写模式打开块表记录模型空间 
                BlockTableRecord acBlkTblRec; acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                //创建多段线 
                Line line = new Line(new Point3d(1000, 100, 0), new Point3d(1000, 800, 0));
                //添加新对象到块表记录和事务 

                List<Line> lines = new List<Line>();
                lines.Add(line);
                line = new Line(new Point3d(600, 550, 0), new Point3d(1250, 550, 0));
                lines.Add(line);
                foreach (Line l in lines)
                {
                    acBlkTblRec.AppendEntity(l);
                    acTrans.AddNewlyCreatedDBObject(l, true);
                    /*
                    //偏移0.25距离 
                    DBObjectCollection acDbObjColl = l.GetOffsetCurves(-25);
                    //遍历得到的新对象 
                    LinetypeTable linetypeTbl = acTrans.GetObject(acCurDb.LinetypeTableId, OpenMode.ForRead) as LinetypeTable;
                    foreach (Entity acEnt in acDbObjColl)
                    {
                        using (acEnt)
                        {
                            String linetypeName = "JIS_02_1.0";
                            if (!linetypeTbl.Has(linetypeName))
                            {
                                acCurDb.LoadLineTypeFile(linetypeName, "acadiso.lin");
                            }
                            if (linetypeTbl.Has(linetypeName))
                            {
                                acEnt.Linetype = linetypeName;
                            }
                            //添加每个对象
                            acBlkTblRec.AppendEntity(acEnt);
                            acTrans.AddNewlyCreatedDBObject(acEnt, true);
                            //释放DBObject对象 
                        }
                    }
                    //释放DBObject对象
                    */
                }
                foreach (Line l in lines)
                {
                    DBObjectCollection acDbObjColl = l.GetOffsetCurves(-50);
                    //遍历得到的新对象 
                    LinetypeTable linetypeTbl = acTrans.GetObject(acCurDb.LinetypeTableId, OpenMode.ForRead) as LinetypeTable;
                    foreach (Entity acEnt in acDbObjColl)
                    {
                        using (acEnt)
                        {
                            acBlkTblRec.AppendEntity(acEnt);
                            acTrans.AddNewlyCreatedDBObject(acEnt, true);
                            //释放DBObject对象 
                        }
                    }
                    //释放DBObject对象
                }
                //保存新对象到数据库

                acTrans.Commit();
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
                    /*
                      Polyline3d pol = new Polyline3d(Poly3dType.SimplePoly, p, true);
                      pol.ColorIndex = 2;
                      btr.AppendEntity(pol);
                      trans.AddNewlyCreatedDBObject(pol,true);
                    */
                    ObjectId objID = doc.Draw(p,0,"");
                    
                    Move2(objID, new Point3d(100,100,100),new Point3d(0,0,0));
                    Move3(objID, new Point3d(100, 100, 100), new Point3d(0, 0, 0),Math.PI/2);
                    Move4(objID, new Point3d(100, 100, 100), new Point3d(0, 0, 0),false);

                   
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
            //ent2.ColorIndex = 1;
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






        [CommandMethod("ckx")]
        public void getCKL()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            using (Transaction trans= db.TransactionManager.StartTransaction())
            {

                Point3dCollection pts = new Point3dCollection();
                PromptPointOptions ppo = new PromptPointOptions("请选择初始点:");
                PromptPointResult ppr ;
                ppr = doc.Editor.GetPoint(ppo);
                pts.Add(new Point3d(ppr.Value.X-300, ppr.Value.Y, ppr.Value.Z));
                pts.Add(new Point3d(ppr.Value.X,ppr.Value.Y,ppr.Value.Z));
                if (ppr.Status == PromptStatus.Cancel) return;
                ppo.Message="请选择終点:";
                ppr = doc.Editor.GetPoint(ppo);
                pts.Add(new Point3d(ppr.Value.X, ppr.Value.Y, ppr.Value.Z));
                pts.Add(new Point3d(ppr.Value.X + 300, ppr.Value.Y, ppr.Value.Z));
                if (ppr.Status == PromptStatus.Cancel) return;
                PromptDoubleOptions pdo = new PromptDoubleOptions("请输入高度:");
                pdo.DefaultValue = 2300;
                double pyY = pdo.DefaultValue; ;
                PromptDoubleResult pdr = doc.Editor.GetDouble(pdo);

                if (pdr.Status == PromptStatus.OK)
                    pyY = pdr.Value;
              
                doc.Editor.WriteMessage("y=" + pyY+",s="+pdr.Status);
                BlockTable bt = trans.GetObject(db.BlockTableId,OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                Polyline3d pl = new Polyline3d(Poly3dType.SimplePoly, pts, false);

                //List<LayerTableRecord> ltrs = db.GetLayerTableRecords();
                //var noPrintLayer = (from layer in ltrs where layer.IsPlottable==false select layer.Name).ToList();
                //doc.Editor.WriteMessage("c="+noPrintLayer.Count);
                LayerTable lt = db.LayerTableId.GetObject(OpenMode.ForRead) as LayerTable;
                if (!lt.Has("参考线不打印"))
                {
                    LayerTableRecord ltr = new LayerTableRecord();
                    ltr.Name = "参考线不打印";
                    ltr.Color = Color.FromColorIndex(ColorMethod.ByAci,195);
                    ltr.IsPlottable = false;

                    LinetypeTable  ltt= db.LinetypeTableId.GetObject(OpenMode.ForRead) as LinetypeTable;
                    if (!ltt.Has("DASHED"))
                    {
                        ltt.UpgradeOpen();
                        db.LoadLineTypeFile("DASHED", "acadiso.lin");
                    }
                    if (ltt.Has("DASHED"))
                    {
                        ltr.LinetypeObjectId = ltt["DASHED"];
                    }
                    lt.UpgradeOpen();
                    lt.Add(ltr);
                    trans.AddNewlyCreatedDBObject(ltr,true);
                    ltr.DowngradeOpen();
                    lt.DowngradeOpen();
                }
                
                db.Clayer = lt["参考线不打印"];


                ObjectId objId = btr.AppendEntity(pl);
                trans.AddNewlyCreatedDBObject(pl, false);

                Move2(objId, new Point3d(0,0,0),new Point3d(0, -pyY, 0));



                trans.Commit();
            }


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
                
                doc.SendStringToExecute("._trim f 1000,500,0 1000,550,0 " + Environment.NewLine, true,false,true);
                //保存新对象到数据库
                trans.Commit();
            }
            
        }
        public Point3d? getPoint(Line l1, Line l2, ref Point3d point, ref Boolean flag)
        {

            Point3d p1 = l1.StartPoint;
            Point3d p2 = l1.EndPoint;
            Point3d p3 = l2.StartPoint;
            Point3d p4 = l2.EndPoint;
            double a1, b1, c1, a2, b2, c2;
            a1 = p2.Y - p1.Y;
            b1 = p1.X - p2.X;
            c1 = p2.X * p1.Y - p1.X * p2.Y;
            a2 = p4.Y - p3.Y;
            b2 = p3.X - p4.X;
            c2 = p4.X * p3.Y - p3.X * p4.Y;
            double x = (b1 * c2 - b2 * c1) / (a1 * b2 - a2 * b1);
            double y = (a2 * c1 - a1 * c2) / (a1 * b2 - a2 * b1);
            double minX = Math.Min(Math.Min(p1.X, p2.X), Math.Min(p3.X, p4.X));
            double minY = Math.Min(Math.Min(p1.Y, p2.Y), Math.Min(p3.Y, p4.Y));
            double maxX = Math.Max(Math.Max(p1.X, p2.X), Math.Max(p3.X, p4.X));
            double maxY = Math.Max(Math.Max(p1.Y, p2.Y), Math.Max(p3.Y, p4.Y));
            if (x < minX || x > maxX || y < minY || y > maxY)
            {
                flag = false;
                return point;
            }
            else
            {
                flag = true;
                point = new Point3d(x, y, 0);
                return point;
            }

        }


        [CommandMethod("jig")]
        public void JigTest()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Matrix3d mt = ed.CurrentUserCoordinateSystem;
            Vector3d vt = mt.CoordinateSystem3d.Zaxis;
            EntJig entJig = new EntJig(vt);
            for (; ; )
            {
                PromptResult pr = ed.Drag(entJig);
                if (pr.Status==PromptStatus.Cancel)
                {
                    return;
                }
                if (pr.Status==PromptStatus.OK)
                {
                    AddToModelSpace(db,entJig.GetEntity());
                    break;
                }
            }
        }


        [CommandMethod("TriangleCircleJig")]
        public void TriangleCircleJigTest()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            PromptPointOptions ppo = new PromptPointOptions("\n 请指定等边三角形中心");
            PromptPointResult ppr = ed.GetPoint(ppo);
            if (ppr.Status != PromptStatus.OK) return;

            Point3d pcenter = ppr.Value;

            Circle circle = new Circle();
            ObjectId cirId = AddToModelSpace(db,circle);
            TriangleCircleJig triangleCircleJig = new TriangleCircleJig(pcenter,cirId);
            PromptResult pr = ed.Drag(triangleCircleJig);
            if (pr.Status==PromptStatus.Cancel)
            {
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    circle = trans.GetObject(triangleCircleJig.circleId,OpenMode.ForWrite) as Circle;
                    circle.Erase();
                    trans.Commit();
                }
            }
            if (pr.Status==PromptStatus.OK)
            {
                AddToModelSpace(db, triangleCircleJig.GetEntity());
            }
        }

        [CommandMethod("changeEntity")]
        public void ChangEntity()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            PromptSelectionOptions pso = new PromptSelectionOptions();
            pso.MessageForAdding = "\n选择对象";
            PromptSelectionResult psr = ed.GetSelection(pso);
            if (psr.Status != PromptStatus.OK) return;
            SelectionSet ss = psr.Value;
            ObjectId[] ids = ss.GetObjectIds();
            Entity[] selectEnt = new Entity[ids.Length];
            Entity[] resultEnt = new Entity[ids.Length];
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                for (int i = 0; i < ids.Length; i++)
                {
                    selectEnt[i] = trans.GetObject(ids[i], OpenMode.ForWrite) as Entity;
                    resultEnt[i] = selectEnt[i].Clone() as Entity;
                }
                PromptKeywordOptions pko = new PromptKeywordOptions("\n请选择操作类型");
                pko.Keywords.Add("J", "J", "镜像(J)", true,true);
                pko.Keywords.Add("Y", "Y", "移动(Y)", true, true);
                pko.Keywords.Add("X", "X", "旋转(X)", true, true);
                pko.Keywords.Add("S", "S", "缩放(S)", true, true);
                pko.Keywords.Default = "J";
                PromptResult prmt = ed.GetKeywords(pko);
                if (prmt.Status != PromptStatus.OK) return;
                if (prmt.Status== PromptStatus.OK) {
                    String mType = prmt.StringResult;
                    PromptPointResult ppr2 = ed.GetPoint("\n请第一点:");
                    if (ppr2.Status != PromptStatus.OK) return;
                    Point3d p = ppr2.Value;
                    EntityChangeTools entChangeJig = new EntityChangeTools(selectEnt, resultEnt, p, mType);
                    PromptResult pr = ed.Drag(entChangeJig);
                    if (pr.Status == PromptStatus.OK)
                    {
                        PromptKeywordOptions pko2 = new PromptKeywordOptions("\n是否删除源对象");
                        pko2.Keywords.Add("Y", "Y", "是(Y)", true, true);
                        pko2.Keywords.Add("N", "N", "否(N)", true, true);
                        pko2.Keywords.Default = "N";
                        PromptResult pr2 = ed.GetKeywords(pko2);
                        if (pr2.Status==PromptStatus.OK &&　pr2.StringResult=="Y")
                        {
                            foreach (Entity ent in selectEnt)
                            {
                                ent.Erase();
                            }
                        }
                        foreach (Entity ent in resultEnt)
                            {
                                AddToModelSpace(db, ent);
                            }
                        }
                    }
                trans.Commit();
                }
            }

        [CommandMethod("mirrorEntity")]
        public void JigMirror()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            PromptSelectionOptions pso = new PromptSelectionOptions();
            pso.MessageForAdding="选择对象";
            PromptSelectionResult psr = ed.GetSelection(pso);
            if (psr.Status != PromptStatus.OK) return;
            SelectionSet ss = psr.Value;
            ObjectId[] ids = ss.GetObjectIds();
            Entity[] selectEnt = new Entity[ids.Length];
            Entity[] resultEnt = new Entity[ids.Length];
            using (Transaction trans  = db.TransactionManager.StartTransaction())
            {
                for (int i = 0; i < ids.Length; i++)
                {
                    selectEnt[i] = trans.GetObject(ids[i],OpenMode.ForWrite) as Entity;
                    resultEnt[i] = selectEnt[i].Clone() as Entity;
                }
                PromptPointResult ppr2 = ed.GetPoint("\n请指定镜像线第一点:") ;
                if (ppr2.Status != PromptStatus.OK) return;
                Point3d p = ppr2.Value;
                MirrorJig mirrorJig = new MirrorJig(p,selectEnt,resultEnt);
                PromptResult pr = ed.Drag(mirrorJig);
                if (pr.Status==PromptStatus.OK)
                {
                    foreach (Entity ent in resultEnt)
                    {
                        AddToModelSpace(db, ent);
                    }
                }
                trans.Commit();
            }
              
        }












    }
}
