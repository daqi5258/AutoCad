using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoCad
{
    class TriangleCircleJig : EntityJig
    {
        public Point3d ptCenter;
        public Point3d ptMouse;
        public Point2d[] pts = new Point2d[3];
        public Matrix3d mt = Matrix3d.Identity;
        public ObjectId circleId = ObjectId.Null;

        protected override SamplerStatus Sampler(JigPrompts prompts)
        {
            JigPromptPointOptions optJip = new JigPromptPointOptions("\n 请选择等边三角形顶点");
            optJip.Cursor = CursorType.RubberBand;
            optJip.UserInputControls = UserInputControls.Accept3dCoordinates;
            optJip.BasePoint = ptCenter.TransformBy(mt);
            optJip.UseBasePoint = true;

            PromptPointResult ppr = prompts.AcquirePoint(optJip);
            Point3d curPt = ppr.Value;
            if (ppr.Status==PromptStatus.Error)
            {
                return SamplerStatus.Cancel;

            }
            if (ptMouse!=curPt)
            {
                ptMouse = curPt;
                
                Point2d cpt = new Point2d(ptCenter.X,ptCenter.Y);
                pts[0] = new Point2d(ptMouse.TransformBy(mt.Inverse()).X, ptMouse.TransformBy(mt.Inverse()).Y);
                double dis = pts[0].GetDistanceTo(cpt);
                Vector2d vec = pts[0] - cpt;
                double ang = vec.Angle;
                Double ang1 = 120.0 / 180.0 * Math.PI;// 除法不加小数点默认为整数相除,不会产生小数的. 要么改数要么强制转换结果类型
                Double ang2 = 240.0/ 180.0 * Math.PI;
                double pyX1 = dis * Math.Cos(ang  + ang1);
                double pyY1 = dis * Math.Sin(ang + ang1);
                double pyX2 = dis * Math.Cos(ang  + ang2);
                double pyY2 = dis * Math.Sin(ang + ang2);
                pts[1] = new Point2d(cpt.X+ pyX1, cpt.Y+ pyY1);
                pts[2] = new Point2d(cpt.X+ pyX2, cpt.Y+pyY2);
                return SamplerStatus.OK;

            }
            else
            {
                return SamplerStatus.NoChange;
            }


            throw new NotImplementedException();
        }

        protected override bool Update()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            ((Polyline)Entity).SetPointAt(0, pts[0]);
            ((Polyline)Entity).SetPointAt(1, pts[1]);
            ((Polyline)Entity).SetPointAt(2, pts[2]);
            ((Polyline)Entity).Normal = Vector3d.ZAxis;
            ((Polyline)Entity).Elevation = 0.0;
            ((Polyline)Entity).TransformBy(mt);
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                Circle circle = trans.GetObject(circleId,OpenMode.ForWrite) as Circle;
                circle.Center = ptCenter.TransformBy(mt);
                circle.Normal = mt.CoordinateSystem3d.Zaxis;
                double rad = 0.5 * pts[0].GetDistanceTo(pts[1]) * Math.Tan(30.0/180.0*Math.PI);
                if (rad>0)
                {
                    circle.Radius = rad;
                }
                trans.Commit();
            }
            return true;  
            //throw new NotImplementedException();

        }


        public TriangleCircleJig(Point3d pCenter,ObjectId cirID):base(new Polyline()) 
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            mt = ed.CurrentUserCoordinateSystem;
            ((Polyline)Entity).AddVertexAt(0,Point2d.Origin,0,0,0);
            ((Polyline)Entity).AddVertexAt(1, Point2d.Origin, 0, 0, 0);
            ((Polyline)Entity).AddVertexAt(2, Point2d.Origin, 0, 0, 0);

            ((Polyline)Entity).Closed = true;
            ptCenter = pCenter;
            circleId = cirID;
        }

        public Entity GetEntity()
        {
            return Entity;
        }
    }
}
