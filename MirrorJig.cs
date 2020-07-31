using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.GraphicsInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoCad
{
    class MirrorJig : DrawJig
    {

        public Point3d ptMirror1, ptMirror2;
        public Entity[] selectEntity;
        public Entity[] resultEntity;
        public Matrix3d mtBack = Matrix3d.Identity;



        protected override SamplerStatus Sampler(JigPrompts prompts)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Matrix3d mt = ed.CurrentUserCoordinateSystem;
            JigPromptPointOptions optJip = new JigPromptPointOptions("\n请指定镜像线第二点:");
            optJip.Cursor = CursorType.RubberBand;
            Point3d p2 = ptMirror1.TransformBy(mt);
            optJip.BasePoint = p2;
            optJip.UseBasePoint = true;
            PromptPointResult ppr = prompts.AcquirePoint(optJip);
            Point3d cpt = ppr.Value;
            if (cpt != ptMirror2)
            {
                ptMirror2 = cpt;
                Line3d ml = new Line3d(ptMirror2, ptMirror1);
                Matrix3d mirrorMt = Matrix3d.Mirroring(ml);
                for (int i = 0; i < resultEntity.Length; i++)
                {
                    resultEntity[i].TransformBy(mtBack);
                    resultEntity[i].TransformBy(mirrorMt);

                }
                mtBack = mirrorMt.Inverse();
                return SamplerStatus.OK;
            }
            else
                return SamplerStatus.NoChange;
            

        }

        protected override bool WorldDraw(WorldDraw draw)
        {
            for (int i = 0; i < resultEntity.Length; i++)
            {
                draw.Geometry.Draw(resultEntity[i]);
            }
            return true;
        }


        public MirrorJig(Point3d pt1 ,Entity[] ent1,Entity[] ent2)
        {
            ptMirror1 = ptMirror2 = pt1;
            selectEntity = ent1;
            resultEntity = ent2;

        }

     
    }
}
