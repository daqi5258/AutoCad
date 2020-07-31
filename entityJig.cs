using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoCad
{
    class EntJig : EntityJig
    {
        public Point3d ptCenter;
        public double radius = 100.0;

        protected override SamplerStatus Sampler(JigPrompts prompts)
        {
            JigPromptPointOptions optJig = new JigPromptPointOptions("\n请指定圆心或用右键修改半径");
            optJig.Keywords.Add("100");
            optJig.Keywords.Add("200");
            optJig.Keywords.Add("300");
            optJig.UserInputControls = UserInputControls.Accept3dCoordinates;
            PromptPointResult ppr = prompts.AcquirePoint(optJig);
            Point3d curCenter = ppr.Value;
            if (ppr.Status==PromptStatus.Cancel)
            {
                return SamplerStatus.Cancel;
            }
            if (ppr.Status == PromptStatus.Keyword)
            {
                switch (ppr.StringResult)
                {
                    case "100":
                        radius = 100;
                        return SamplerStatus.NoChange;
                    case "200":
                        radius = 200;
                        return SamplerStatus.NoChange;
                    case "300":
                        radius = 300;
                        return SamplerStatus.NoChange;
                }
            }

            if (curCenter != ptCenter)
            {
                ptCenter = curCenter;
                return SamplerStatus.OK;
            }
            else
                return SamplerStatus.NoChange;


           // throw new NotImplementedException();
        }

        protected override bool Update()
        {
            ((Circle)Entity).Center = ptCenter;
            ((Circle)Entity).Radius = radius;
            return true;
            //throw new NotImplementedException();
        }


        public EntJig(Vector3d normal):base(new Circle())
        {
            ((Circle)Entity).Center = ptCenter;
            ((Circle)Entity).Normal = normal;
            ((Circle)Entity).Radius = radius;

        }

        public Entity GetEntity()
        {
            return Entity;
        }
    }
}
